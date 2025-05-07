using System.ComponentModel;
using System.Data.SqlTypes;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Reflection.Emit;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;
using System.Collections;

namespace OrdersManagement.Common.Helper
{
    public static class DataHelper
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> listObject) where T : class
        {
            if (listObject == null)
            {
                return true;
            }
            else if (listObject.Count() == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrEmpty(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.IsNullOrEmpty(str);
            }
            else
            {
                return string.IsNullOrEmpty(str.Trim());
            }
        }

        public static bool IsPhoneNumber(this string str)
        {
            if (!str.IsNullOrEmpty())
            {
                str = str.Replace(" ", "");
                Regex reg = new Regex(@"^[0-9]{10,11}$");
                return reg.IsMatch(str);
            }
            return false;
        }

        public static string RemoveSpace(this string str)
        {
            return str.Replace(" ", "");
        }

        public static bool AnyNull<T>(this IEnumerable<T> listObject)
        {
            if (listObject == null)
            {
                return true;
            }
            else if (listObject.Count() == 0)
            {
                return true;
            }
            else if (listObject.Any(s => s == null))
            {
                return true;
            }
            return false;
        }

        public static string Join(this IEnumerable<int> listObjet, string separator = ",")
        {
            if (listObjet == null || listObjet.Count() == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(",", listObjet);
            }
        }

        public static string Join(this IEnumerable<long> listObjet, string separator = ",")
        {
            if (listObjet == null || listObjet.Count() == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(",", listObjet);
            }
        }

        public static string Join(this IEnumerable<string> listObject, string separator = ",")
        {
            if (listObject == null || listObject.Count() == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(separator, listObject);
            }
        }

        public static List<List<T>> ChunkBy<T>(this List<T> locations, int nSize = 30)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < locations.Count(); i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count() - i)));
            }
            return list;
        }

        public static DataTable ToUpperColumnName(this DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Columns[i].ColumnName = table.Columns[i].ColumnName.ToUpper();
            }
            return table;
        }

        #region DynamicTypeBuilder

        public class DynamicTypeBuilder
        {
            #region Properties

            private string _dynamicTypeName = string.Empty;
            private string _dynamicAssemblyName = string.Empty;
            private string _dynamicModuleName = string.Empty;
            Dictionary<string, Type> dynamicProperties;

            private Dictionary<string, Type> DynamicProperties
            {
                get
                {
                    if (dynamicProperties == null)
                    {
                        dynamicProperties = new Dictionary<string, Type>();
                    }

                    return dynamicProperties;
                }
                set { dynamicProperties = value; }
            }

            public string DynamicTypeName
            {
                get
                {
                    if (string.IsNullOrEmpty(_dynamicTypeName))
                    {
                        _dynamicTypeName = "DynamicTypeName";
                    }
                    return _dynamicTypeName;
                }
                set { _dynamicTypeName = value; }
            }

            public string DynamicAssemblyName
            {
                get
                {
                    if (string.IsNullOrEmpty(_dynamicAssemblyName))
                    {
                        _dynamicAssemblyName = "DynamicAssembly";
                    }
                    return _dynamicAssemblyName;
                }
                set { _dynamicAssemblyName = value; }
            }

            public string DynamicModuleName
            {
                get
                {
                    if (string.IsNullOrEmpty(_dynamicModuleName))
                    {
                        _dynamicModuleName = "DynamicModule.dll";
                    }
                    return _dynamicModuleName;
                }
                set { _dynamicModuleName = value; }
            }

            #endregion

            #region Constructors

            public DynamicTypeBuilder(string typeName)
            {
                _dynamicTypeName = typeName;
            }

            #endregion

            #region Methods

            public void AddProperty(string propertyName, Type propertyType)
            {
                DynamicProperties.Add(propertyName, propertyType);
            }

            public Type BuildType()
            {
                return BuildType(dynamicProperties);
            }

            public Type BuildType(Dictionary<string, Type> dynamicProperties)
            {
                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(DynamicAssemblyName), AssemblyBuilderAccess.Run);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(DynamicModuleName);
                TypeBuilder typeBuilder = moduleBuilder.DefineType(DynamicTypeName, TypeAttributes.Public);
                typeBuilder.SetParent(typeof(DynamicObject));

                foreach (var property in dynamicProperties)
                {
                    FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + property.Key, property.Value, FieldAttributes.Private);
                    PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(property.Key, PropertyAttributes.HasDefault, property.Value, null);

                    MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
                    MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + property.Key, getSetAttr, property.Value, Type.EmptyTypes);
                    ILGenerator getter = getMethodBuilder.GetILGenerator();

                    getter.Emit(OpCodes.Ldarg_0);
                    getter.Emit(OpCodes.Ldfld, fieldBuilder);
                    getter.Emit(OpCodes.Ret);

                    MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + property.Key, getSetAttr, null, new Type[] { property.Value });
                    ILGenerator setter = setMethodBuilder.GetILGenerator();

                    setter.Emit(OpCodes.Ldarg_0);
                    setter.Emit(OpCodes.Ldarg_1);
                    setter.Emit(OpCodes.Stfld, fieldBuilder);
                    setter.Emit(OpCodes.Ret);

                    propertyBuilder.SetGetMethod(getMethodBuilder);
                    propertyBuilder.SetSetMethod(setMethodBuilder);
                }

                return typeBuilder.CreateType();
            }

            public Type BuildType(params string[] propertyNames)
            {
                return BuildType(null, propertyNames);
            }

            /// <summary>
            /// Tạo dynamic type có các thuộc tính như entityType
            /// và thêm vào 1 số thuộc tính như propertyNames.
            /// </summary>
            /// <param name="entityType"></param>
            /// <param name="propertyNames"></param>
            /// <returns></returns>
            public Type BuildType(Type entityType, params string[] propertyNames)
            {
                var dynamicProperties = new Dictionary<string, Type>();

                if (entityType != null)
                {
                    var listProperty = entityType.GetProperties();

                    foreach (var property in listProperty)
                    {
                        dynamicProperties.Add(property.Name, property.PropertyType);
                    }
                }

                foreach (var propertyName in propertyNames)
                {
                    dynamicProperties.Add(propertyName, typeof(object));
                }

                return BuildType(dynamicProperties);
            }
            #endregion
        }

        #endregion

        #region Translate methods

        public static List<TEntity> Translate<TEntity>(this IDataReader reader, params string[] excludedFields)
        {
            IList listEntity = reader.Translate(typeof(TEntity), excludedFields);
            return listEntity.OfType<TEntity>().ToList();
        }

        /// <summary>
        /// Convert data reader thành danh sách objectType.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static IList Translate(this IDataReader reader, Type objectType, params string[] excludedFields)
        {
            IList listEntity = objectType.CreateList();

            // Get all property names and create a dictionary for quick lookup
            var propertyDict = objectType.GetProperties()
                                         .Where(p => p.CanWrite)
                                         .ToDictionary(p => p.Name.ToLower(), p => p);

            while (reader != null && reader.Read())
            {
                var entity = objectType.CreateInstance();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string propertyName = reader.GetName(i).ToLower(); // reader column name

                    if (excludedFields == null || !excludedFields.Contains(propertyName, StringComparer.OrdinalIgnoreCase))
                    {
                        if (propertyDict.TryGetValue(propertyName, out PropertyInfo propertyInfo) && !reader.IsDBNull(i))
                        {
                            entity.SetPropertyValue(propertyInfo, reader.GetValue(i));
                        }
                    }
                }

                listEntity.Add(entity);
            }

            return listEntity;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Khởi tạo danh sách cho một kiểu dữ liệu elementType.
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static IList CreateList(this Type elementType)
        {
            Type listGenericType = typeof(List<>).MakeGenericType(elementType);
            return (IList)Activator.CreateInstance(listGenericType);
        }

        /// <summary>
        /// Tạo mới đối tượng có kiểu elementType.
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type elementType)
        {
            return Activator.CreateInstance(elementType);
        }

        /// <summary>
        /// Tạo mới đối tượng có kiểu elementType và các tham số của nó.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type elementType, params object[] args)
        {
            return Activator.CreateInstance(elementType, args);
        }

        /// <summary>
        /// Kiểm tra 1 đối tượng có phải được thừa kế từ 1 type nào đó?
        /// </summary>
        /// <param name="elementType">Đối tượng cần kiểm tra.</param>
        /// <param name="interfaceType">Type để kiểm tra.</param>
        /// <returns></returns>
        public static bool IsMemberOf(this Type elementType, Type parentType)
        {
            return elementType.GetInterface(parentType.Name) != null
                || elementType.IsSubclassOf(parentType);
        }

        /// <summary>
        /// Kiểm tra 1 đối tượng có phải là kiểu dữ liệu nào đó hay không?
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static bool IsTypeOf(this Type elementType, Type objectType)
        {
            return elementType == objectType || elementType.IsMemberOf(objectType);
        }

        public static bool IsTypeOf(this object value, Type objectType)
        {
            return value != null ? value.GetType().IsTypeOf(objectType) : false;
        }

        /// <summary>
        /// Kiểu entity thực tế khi dùng entity framework có DynamicProxies.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static Type GetRealEntityType(this Type entityType)
        {
            if (entityType != null)
            {
                if (entityType.Namespace.EndsWith("DynamicProxies"))
                {
                    entityType = entityType.BaseType;
                }
            }

            return entityType;
        }

        #endregion

        #region Property methods

        /// <summary>
        /// Kiểm tra một type có chứa thuộc tính propertyName?
        /// propertyName có thể là 2 thuộc tính: Parent.Field.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private static bool HasProperty(this Type entityType,
            string propertyName, out Type propertyType)
        {
            bool result = false;
            propertyType = null;

            if (entityType != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                string[] listPropertyName = propertyName.Split('.');

                foreach (string fieldName in listPropertyName)
                {
                    PropertyInfo propertyInFo = entityType.GetProperty(fieldName);
                    propertyType = propertyInFo != null ? propertyInFo.PropertyType : null;
                    entityType = propertyInFo != null ? propertyInFo.PropertyType : null;
                    result = propertyInFo != null;

                    if (!result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra một type có chứa thuộc tính propertyName?
        /// propertyName có thể là 2 thuộc tính: Parent.Field.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Type entityType, string propertyName)
        {
            Type propertyType = null;//Kiểu dữ liệu của propertyName
            return entityType.HasProperty(propertyName, out propertyType);
        }

        /// <summary>
        /// Kiểm tra một đối tượng có chứa thuộc tính propertyName?
        /// propertyName có thể là 2 thuộc tính: Parent.Field
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this object entity, string propertyName)
        {
            if (entity != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                return entity.GetType().HasProperty(propertyName);
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra kiểu dữ liệu của thuộc tính propertyName.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type GetPropertyType(this Type entityType, string propertyName)
        {
            Type propertyType = null;//Kiểu dữ liệu của propertyName
            entityType.HasProperty(propertyName, out propertyType);
            return propertyType;
        }

        /// <summary>
        /// Kiểm tra kiểu dữ liệu của thuộc tính propertyName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type GetPropertyType(this object entity, string propertyName)
        {
            if (entity != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                Type propertyType = null;//Kiểu dữ liệu của propertyName
                entity.GetType().HasProperty(propertyName, out propertyType);
                return propertyType;
            }

            return null;
        }

        /// <summary>
        /// Kiểm tra kiểu dữ liệu thực của đối tượng propertyName.
        /// Ví dụ: propertyName là DateTime? => return DateTime.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type GetRealPropertyType(this object entity, string propertyName)
        {
            return entity.GetPropertyType(propertyName).GetRealPropertyType();
        }

        /// <summary>
        /// Kiểm tra kiểu dữ liệu thực của đối tượng propetyType.
        /// Ví dụ: propetyType là DateTime? => return DateTime.
        /// </summary>
        /// <param name="propetyType"></param>
        /// <returns></returns>
        public static Type GetRealPropertyType(this Type propetyType)
        {
            if (propetyType.IsGuid())
            {
                return typeof(Guid);
            }
            else if (propetyType.IsInteger())
            {
                return typeof(int);
            }
            else if (propetyType.IsShort())
            {
                return typeof(short);
            }
            else if (propetyType.IsLong())
            {
                return typeof(long);
            }
            else if (propetyType.IsDouble())
            {
                return typeof(double);
            }
            else if (propetyType.IsDecimal())
            {
                return typeof(decimal);
            }
            else if (propetyType.IsFloat())
            {
                return typeof(float);
            }
            else if (propetyType.IsDateTime())
            {
                return typeof(DateTime);
            }
            else if (propetyType.IsBoolean())
            {
                return typeof(bool);
            }
            else
            {
                return propetyType;
            }
        }

        public static Type GetNullablePropertyType(this object entity, string propertyName)
        {
            return entity.GetPropertyType(propertyName).GetNullablePropertyType();
        }

        /// <summary>
        /// Trả về kiểu cho phép null.
        /// </summary>
        /// <param name="propetyType"></param>
        /// <returns></returns>
        public static Type GetNullablePropertyType(this Type propetyType)
        {
            if (propetyType.IsGuid())
            {
                return typeof(Guid?);
            }
            else if (propetyType.IsInteger())
            {
                return typeof(int?);
            }
            else if (propetyType.IsShort())
            {
                return typeof(short?);
            }
            else if (propetyType.IsLong())
            {
                return typeof(long?);
            }
            else if (propetyType.IsDouble())
            {
                return typeof(double?);
            }
            else if (propetyType.IsDecimal())
            {
                return typeof(decimal?);
            }
            else if (propetyType.IsFloat())
            {
                return typeof(float?);
            }
            else if (propetyType.IsDateTime())
            {
                return typeof(DateTime?);
            }
            else if (propetyType.IsBoolean())
            {
                return typeof(bool?);
            }
            else
            {
                return propetyType;
            }
        }

        /// <summary>
        /// Lấy giá trị của thuộc tính fieldName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue(this object entity, string fieldName)
        {
            return entity.GetFieldValue(fieldName, BindingFlags.Default);
        }

        /// <summary>
        /// Lấy giá trị của thuộc tính fieldName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static object GetFieldValue(this object entity, string fieldName, BindingFlags bindingAttr)
        {
            object result = null;

            if (entity != null && !string.IsNullOrWhiteSpace(fieldName))
            {
                FieldInfo fieldInfo = null;

                if (bindingAttr == BindingFlags.Default)
                {
                    fieldInfo = entity.GetType().GetField(fieldName);
                }
                else
                {
                    fieldInfo = entity.GetType().GetField(fieldName, bindingAttr);
                }

                if (fieldInfo != null)
                {
                    result = fieldInfo.GetValue(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy giá trị của thuộc tính propertyName.
        /// propertyName có thể là 2 thuộc tính: Parent.Field
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object entity, string propertyName)
        {
            return entity.GetPropertyValue(propertyName, BindingFlags.Default);
        }

        public static object GetPropertyValue(this object entity, string propertyName, BindingFlags bindingAttr)
        {
            object result = null;

            if (entity != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                string[] listPropertyName = propertyName.Split('.');

                foreach (string fieldName in listPropertyName)
                {
                    PropertyInfo propertyInFo = null;

                    if (bindingAttr == BindingFlags.Default)
                    {
                        propertyInFo = entity.GetType().GetProperty(fieldName);
                    }
                    else
                    {
                        propertyInFo = entity.GetType().GetProperty(fieldName, bindingAttr);
                    }

                    if (propertyInFo != null && propertyInFo.CanRead == true)
                    {
                        result = entity = propertyInFo.GetValue(entity, null);
                    }
                    else
                    {
                        result = null;
                        break;
                    }

                    if (entity == null)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gán giá trị cho thuộc tính propertyName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object entity, string propertyName, object value)
        {
            entity.SetPropertyValue(propertyName, value, BindingFlags.Default);
        }

        /// <summary>
        /// Gán giá trị cho thuộc tính propertyName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="bindingAttr"></param>
        public static void SetPropertyValue(this object entity, string propertyName, object value, BindingFlags bindingAttr)
        {
            if (entity != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyInfo propertyInfo = null;

                if (bindingAttr == BindingFlags.Default)
                {
                    propertyInfo = entity.GetType().GetProperty(propertyName);
                }
                else
                {
                    propertyInfo = entity.GetType().GetProperty(propertyName, bindingAttr);
                }

                entity.SetPropertyValue(propertyInfo, value);
            }
        }

        /// <summary>
        /// Gán giá trị cho thuộc tính propertyName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object entity, PropertyInfo propertyInfo, object value)
        {
            if (entity != null && propertyInfo != null && propertyInfo.CanWrite)
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString())
                    && propertyInfo.PropertyType == typeof(XElement))
                {
                    value = XElement.Parse(value.ToString());
                }

                if (propertyInfo.PropertyType.IsEnum)
                {
                    value = Enum.ToObject(propertyInfo.PropertyType, value);
                }

                value = value == DBNull.Value ? null : value;
                propertyInfo.SetValue(entity, value, null);
            }
        }

        /// <summary>
        /// Gán giá trị cho thuộc tính fieldName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue(this object entity, string fieldName, object value)
        {
            entity.SetFieldValue(fieldName, value, BindingFlags.Default);
        }

        /// <summary>
        /// Gán giá trị cho thuộc tính fieldName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="bindingAttr"></param>
        public static void SetFieldValue(this object entity, string fieldName, object value, BindingFlags bindingAttr)
        {
            if (entity != null && !string.IsNullOrWhiteSpace(fieldName))
            {
                FieldInfo fieldInfo = null;

                if (bindingAttr == BindingFlags.Default)
                {
                    fieldInfo = entity.GetType().GetField(fieldName);
                }
                else
                {
                    fieldInfo = entity.GetType().GetField(fieldName, bindingAttr);
                }

                entity.SetFieldValue(fieldInfo, value);
            }
        }

        /// <summary>
        /// Gán giá trị cho thuộc tính fieldName.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="value"></param>
        public static void SetFieldValue(this object entity, FieldInfo fieldInfo, object value)
        {
            if (entity != null && fieldInfo != null)
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString())
                    && fieldInfo.FieldType == typeof(XElement))
                {
                    value = XElement.Parse(value.ToString());
                }

                if (fieldInfo.FieldType.IsEnum)
                {
                    value = Enum.ToObject(fieldInfo.FieldType, value);
                }

                value = value == DBNull.Value ? null : value;
                fieldInfo.SetValue(entity, value);
            }
        }

        #endregion

        #region Validate Methods

        /// <summary>
        /// Kiểm tra giá trị value là null hay không?
        /// Nếu value là null, thì trả về true; Ngược lại, trả về false.
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra.</param>
        /// <returns>Nếu value là null, thì trả về true; Ngược lại, trả về false.</returns>
        public static bool IsNull(this object value)
        {
            return value == null || value == DBNull.Value;
        }

        /// <summary>
        /// Kiểm tra một đối tượng có bị null hoặc empty?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object value)
        {
            return value == null || string.IsNullOrWhiteSpace(value.ToString());
        }

        public static object NullIfEmpty(this object value)
        {
            return value.IsNullOrEmpty() ? null : value;
        }

        public static bool IsBoolean(this Type value)
        {
            return value != null ? value == typeof(bool) || value == typeof(bool?) : false;
        }

        public static bool IsInteger(this Type value)
        {
            return value != null ? value == typeof(int) || value == typeof(int?) : false;
        }

        public static bool IsShort(this Type value)
        {
            return value != null ? value == typeof(short) || value == typeof(short?)
                || value == typeof(short) || value == typeof(short?) : false;
        }

        public static bool IsLong(this Type value)
        {
            return value != null ? value == typeof(long) || value == typeof(long?)
                || value == typeof(long) || value == typeof(long?) : false;
        }

        public static bool IsDecimal(this Type value)
        {
            return value != null ? value == typeof(decimal) || value == typeof(decimal?) : false;
        }

        public static bool IsDouble(this Type value)
        {
            return value != null ? value == typeof(double) || value == typeof(double?) : false;
        }

        public static bool IsFloat(this Type value)
        {
            return value != null ? value == typeof(float) || value == typeof(float?) : false;
        }

        public static bool IsGuid(this Type value)
        {
            return value != null ? value == typeof(Guid) || value == typeof(Guid?) : false;
        }

        public static bool IsDateTime(this Type value)
        {
            return value != null ? value == typeof(DateTime) || value == typeof(DateTime?) : false;
        }

        public static bool IsTimeSpan(this Type value)
        {
            return value != null ? value == typeof(TimeSpan) || value == typeof(TimeSpan?) : false;
        }

        public static bool IsNullable(this Type value)
        {
            try
            {
                return value.IsGenericType && value.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsOnlyNumeric(this object value)
        {
            //Khai báo mẫu dùng để so khớp
            string pattern = "^[0-9]+$";

            //Trả về kết quả sau khi so khớp
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// Kiểm tra giá trị đầu vào là toàn số nguyên hay không?
        /// Nếu đúng, thì trả về true; Ngược lại, trả về false.
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra.</param>
        /// <returns>c</returns>
        public static bool IsOnlyInt(this object value)
        {
            //Khai báo mẫu dùng để so khớp
            string pattern = "^([+]|[-])?([0-9])+$";

            //Trả về kết quả sau khi so khớp
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// Kiểm tra giá trị đầu vào là khớp với mẫu pattern hay không?
        /// Nếu đúng, thì trả về true; Ngược lại, trả về false.
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra.</param>
        /// <param name="pattern">Mẫu dùng để so khớp.</param>
        ///  /// <param name="isCheckNull">Cho phép null với mẫu hay không. return true nếu cho phép.</param>
        /// <returns>Nếu đúng, thì trả về true; Ngược lại, trả về false.</returns>
        private static bool IsMatch(this object value, string pattern, bool isCheckNull = false)
        {
            //Nếu value là null, empty, hoặc khoảng trắng thì trả về false
            if (value.IsNullOrEmpty())
            {
                if (isCheckNull)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //Khai báo mẫu dùng để so khớp
            Regex regex = new Regex(pattern);

            //Trả về kết quả sau khi so khớp
            return regex.IsMatch(value.ToString().Trim());
        }
        #endregion

        #region Object methods

        /// <summary>
        /// ToString một đối tượng không cần kiểm tra null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetString(this object value)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        /// <summary>
        /// Trả về false nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetBoolean(this bool? value)
        {
            return value != null ? value.Value : false;
        }

        /// <summary>
        /// Trả về 0 nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetInteger(this int? value)
        {
            return value != null ? value.Value : 0;
        }

        /// <summary>
        /// Dùng cho kiểu Int16 và short.
        /// Trả về 0 nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short GetShort(this short? value)
        {
            return value != null ? value.Value : (short)0;
        }

        /// <summary>
        /// Dùng cho kiểu Int64 và long.
        /// Trả về 0 nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetLong(this long? value)
        {
            return value != null ? value.Value : 0;
        }

        /// <summary>
        /// Trả về 0 nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetDecimal(this decimal? value)
        {
            return value != null ? value.Value : 0M;
        }

        /// <summary>
        /// Trả về 0 nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetDouble(this double? value)
        {
            return value != null ? value.Value : 0D;
        }

        /// <summary>
        /// Trả về 0 nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetFloat(this float? value)
        {
            return value != null ? value.Value : 0F;
        }

        /// <summary>
        /// Trả về Guid.Empty nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid GetGuid(this Guid? value)
        {
            return value != null ? value.Value : Guid.Empty;
        }

        /// <summary>
        /// Chuyển .net guid thành oracle guid hoặc ngược lại.
        /// Vì .net và oracle có cách đọc giá trị guid khác nhau.
        /// </summary>
        /// <param name="guidValue"></param>
        /// <returns></returns>
        public static Guid ReverseGuid(this Guid guidValue)
        {
            var guidBytes = guidValue.ToByteArray();
            var result = BitConverter.ToString(guidBytes);
            result = result.Replace("-", string.Empty);
            return new Guid(result);
        }

        /// <summary>
        /// Chuyển .net guid thành oracle guid hoặc ngược lại.
        /// Vì .net và oracle có cách đọc giá trị guid khác nhau.
        /// </summary>
        /// <param name="guidString"></param>
        /// <returns></returns>
        public static Guid ReverseGuid(this string guidString)
        {
            var guidValue = guidString.TryGetValue<Guid>();
            return guidValue.ReverseGuid();
        }

        public static TimeSpan GetTimeSpan(this TimeSpan? value)
        {
            return value != null ? value.Value : TimeSpan.Zero;
        }

        /// <summary>
        /// Trả về ngày hiện tại nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this DateTime? value)
        {
            return value != null ? value.Value : GetEmptyDate();
        }

        /// <summary>
        /// Trả về ngày hiện tại nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeOrNow(this DateTime? value)
        {
            return value != null ? value.Value : DateTime.Now;
        }

        /// <summary>
        /// Trả về ngày nhỏ nhất nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeOrMin(this DateTime? value)
        {
            return value != null ? value.Value : SqlDateTime.MinValue.Value;
        }

        /// <summary>
        /// Trả về ngày lớn nhất nếu value bị null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeOrMax(this DateTime? value)
        {
            return value != null ? value.Value : SqlDateTime.MaxValue.Value;
        }

        public static DateTime GetEmptyDate()
        {
            return new DateTime(0);
        }

        /// <summary>
        /// Lấy dánh sách DayOfWeek  tồn tại trong khoảng thời gian.
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        public static List<DayOfWeek> GetDayOfWeeksBeween(DateTime dtFrom, DateTime dtTo)
        {
            dtFrom = dtFrom.Date;
            dtTo = dtTo.Date;
            if (dtFrom > dtTo)
            {
                return new List<DayOfWeek>();
            }
            else if (dtTo.Subtract(dtFrom).TotalDays >= 7)
            {
                return new List<DayOfWeek>()
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
            }
            else
            {
                var lstTemp = new List<DayOfWeek>();
                dtFrom = dtFrom.Date;
                for (DateTime dtTemp = dtFrom; dtTemp <= dtTo;)
                {
                    lstTemp.Add(dtTemp.DayOfWeek);
                    dtTemp = dtTemp.AddDays(1);
                }
                return lstTemp;
            }
        }

        /// <summary>
        /// Check DayOfWeek có tồn tại trong khoảng thời gian.
        /// </summary>
        /// <param name="eDayOfWeek"></param>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        public static bool IsContains(this DayOfWeek eDayOfWeek, DateTime dtFrom, DateTime dtTo)
        {
            List<DayOfWeek> lstDayOfWeek = GetDayOfWeeksBeween(dtFrom, dtTo);
            return lstDayOfWeek.Any(s => s == eDayOfWeek);
        }


        public static object RandomValue(this Type propertyType)
        {
            object result = null;

            if (propertyType.IsInteger() || propertyType.IsShort()
                || propertyType.IsLong() || propertyType.IsFloat()
                || propertyType.IsDouble() || propertyType.IsDecimal())
            {
                Random random = new Random();
                result = random.Next(0, int.MaxValue);
            }
            else if (propertyType.IsGuid())
            {
                result = Guid.NewGuid();
            }
            else if (propertyType.IsDateTime())
            {
                result = DateTime.Now;
            }
            else
            {
                result = TryGetValue(null, propertyType);
            }

            return result;
        }

        public static T TryGetValue<T>(this object value)
        {
            object resultValue = value.TryGetValue(typeof(T));
            return resultValue != null ? (T)resultValue : default;
        }

        /// <summary>
        /// Validate dữ liệu theo T để có dữ liệu đúng trước khi sử dụng.
        /// Ví dụ: value là chuỗi ký tự 13, T là int => result = số nguyên 13
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T TryGetValue<T>(this object value, out bool invalidFormat)
        {
            object resultValue = value.TryGetValue(typeof(T), out invalidFormat);
            return resultValue != null ? (T)resultValue : default;
        }

        /// <summary>
        /// Validate dữ liệu theo propertyType để có dữ liệu đúng trước khi sử dụng.
        /// Ví dụ: value là chuỗi ký tự 13, propertyType là int => result = số nguyên 13
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static object TryGetValue(this object value, Type propertyType)
        {
            bool invalidFormat = false;//kiểm tra đúng định dạng
            return value.TryGetValue(propertyType, out invalidFormat);
        }

        /// <summary>
        /// Validate dữ liệu theo propertyType để có dữ liệu đúng trước khi sử dụng.
        /// Ví dụ: value là chuỗi ký tự 13, propertyType là int => result = số nguyên 13
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyType"></param>
        /// <param name="invalidFormat"></param>
        /// <returns></returns>
        public static object TryGetValue(this object value,
            Type propertyType, out bool invalidFormat)
        {
            object resultValue = null;
            invalidFormat = false;

            if (value.GetString().ToLower().Equals("null"))
            {
                resultValue = null;//Dữ liệu bằng null
            }
            else if (propertyType != null)
            {
                if (value != null && value.GetType().GetRealPropertyType()
                    == propertyType.GetRealPropertyType())
                {
                    resultValue = value;//Cùng kiểu dữ liệu
                }
                else if (propertyType.IsEnum)
                {
                    resultValue = Enum.Parse(propertyType, value.GetString());
                }
                else if (propertyType.IsBoolean())
                {
                    if (value.GetString().TrimAll() == "1")
                    {
                        value = "true";
                    }
                    else if (value.GetString().TrimAll() == "0")
                    {
                        value = "false";
                    }

                    bool currentValue = false;//value không phải bool hoặc type là bool
                    if (bool.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(bool))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsInteger())
                {
                    int currentValue = 0;//value không phải int hoặc type là int
                    if (int.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(int))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsShort())
                {
                    short currentValue = 0;//value không phải int hoặc type là int
                    if (short.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(short))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsLong())
                {
                    long currentValue = 0;//value không phải int hoặc type là int
                    if (long.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(long))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsDecimal())
                {
                    decimal currentValue = 0;//value không phải decimal hoặc type là decimal
                    if (decimal.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(decimal))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsFloat())
                {
                    float currentValue = 0;//value không phải float hoặc type là float
                    if (float.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(float))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsDouble())
                {
                    double currentValue = 0;//value không phải double hoặc type là double
                    if (double.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(double))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsGuid())
                {
                    Guid currentValue = Guid.Empty;//value không phải Guid hoặc type là Guid
                    if (Guid.TryParse(value.GetString().TrimAll(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(Guid))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsDateTime())
                {
                    //value không phải DateTime hoặc type là DateTime
                    DateTime currentValue = DateTime.MinValue;

                    if (DateTime.TryParse(value.GetString(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(DateTime))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsTimeSpan())
                {
                    //value không phải TimeSpan hoặc type là TimeSpan
                    TimeSpan currentValue = DateTime.Now.TimeOfDay;

                    if (TimeSpan.TryParse(value.GetString(), out currentValue))
                    {
                        resultValue = currentValue;
                    }
                    else
                    {
                        if (propertyType == typeof(TimeSpan))
                        {
                            resultValue = currentValue;
                        }

                        invalidFormat = true;
                    }
                }
                else if (propertyType == typeof(string))
                {
                    if (value != null)
                    {
                        resultValue = value.GetString();
                    }
                }
                else
                {
                    resultValue = value;
                }
            }
            else
            {
                resultValue = value;
            }

            return resultValue;
        }

        public static DateTime TryGetValue(this object value, string dateFormat)
        {
            bool invalidFormat = false;//kiểm tra đúng định dạng
            return value.TryGetValue(dateFormat, out invalidFormat);
        }

        public static DateTime TryGetValue(this object value, string dateFormat, out bool invalidFormat)
        {
            DateTime resultValue = DateTime.Now;

            if (string.IsNullOrWhiteSpace(dateFormat))
            {
                resultValue = value.TryGetValue<DateTime>(out invalidFormat);
            }
            else
            {
                invalidFormat = !DateTime.TryParseExact(value.GetString(), dateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out resultValue);
            }

            return resultValue;
        }

        /// <summary>
        /// Kiểm tra hai giá trị có khác nhau hay không?
        /// </summary>
        /// <param name="originalValue">Gí trị trước đó.</param>
        /// <param name="currentValue">Giá trị hiện tại.</param>
        /// <returns></returns>
        public static bool HasChanged(this object originalValue, object currentValue)
        {
            return originalValue.HasChanged(currentValue, true);
        }

        /// <summary>
        /// Kiểm tra hai giá trị có khác nhau hay không?
        /// </summary>
        /// <param name="originalValue"></param>
        /// <param name="currentValue"></param>
        /// <param name="nullToZero">Bao gồm null to zero</param>
        /// <returns></returns>
        public static bool HasChanged(this object originalValue, object currentValue, bool nullToZero)
        {
            bool result = false;

            if (originalValue == null && (currentValue == null
                || !nullToZero && currentValue.TryGetValue<decimal>() == 0M))
            {
                result = false;
            }
            else if (originalValue == null && currentValue != null)
            {
                result = currentValue.GetType() == typeof(string) ?
                    !string.IsNullOrWhiteSpace(currentValue.GetString()) : true;
            }
            else if (originalValue != null && currentValue == null)
            {
                result = originalValue.GetType() == typeof(string) ?
                    !string.IsNullOrWhiteSpace(originalValue.GetString()) : true;
            }
            else if (!originalValue.Equals(currentValue) &&
                originalValue.GetHashCode() != currentValue.GetHashCode())
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// So sánh 2 mảng đối tượng
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool IsEquals(this Array array1, Array array2)
        {
            if (array1 == null && array2 == null)
                return true;

            if (array1 == null || array2 == null)
                return false;

            if (array1.GetType() != array2.GetType())
                return false;

            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1.GetValue(i).HasChanged(array2.GetValue(i)))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsOverlap(DateTime? dateFrom1, DateTime? dateTo1, DateTime? dateFrom2, DateTime? dateTo2)
        {
            dateFrom1 = dateFrom1.HasValue ? dateFrom1 : SqlDateTime.MinValue.Value;
            dateFrom2 = dateFrom2.HasValue ? dateFrom2 : SqlDateTime.MinValue.Value;

            dateTo1 = dateTo1.HasValue ? dateTo1 : SqlDateTime.MaxValue.Value;
            dateTo2 = dateTo2.HasValue ? dateTo2 : SqlDateTime.MaxValue.Value;

            return dateFrom1.Value <= dateTo2.Value && dateTo1.Value >= dateFrom2.Value;
        }

        /// <summary>
        /// So sánh hai đối tượng bất kỳ với nhau.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>Nhỏ hơn 0 nếu value1 nhỏ hơn value2</returns>
        public static int Compare(this object value1, object value2)
        {
            if (value1 == value2)
            {
                return 0;//value1 = value2
            }
            else if (value1 == null && value2 != null)
            {
                return -1;//value1 < value2
            }
            else if (value1 != null && value2 == null)
            {
                return 1;//value1 > value2
            }
            else if (value1 != null && value2 != null)
            {
                Type propertyType = value1.GetType();

                if (propertyType == typeof(string) || propertyType == typeof(char))
                {
                    return value1.TryGetValue<string>().CompareTo(value2);
                }
                else if (propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    return value1.TryGetValue<int>().CompareTo(value2);
                }
                else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                {
                    return value1.TryGetValue<decimal>().CompareTo(value2);
                }
                else if (propertyType == typeof(double) || propertyType == typeof(double?))
                {
                    return value1.TryGetValue<double>().CompareTo(value2);
                }
                else if (propertyType == typeof(float) || propertyType == typeof(float?))
                {
                    return value1.TryGetValue<float>().CompareTo(value2);
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    return value1.TryGetValue<DateTime>().CompareTo(value2);
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    return value1.TryGetValue<bool>().CompareTo(value2);
                }
                else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    return value1.TryGetValue<Guid>().CompareTo(value2);
                }
                else if (propertyType == typeof(byte) || propertyType == typeof(byte?))
                {
                    return Convert.ToByte(value1.ToString()).CompareTo(value2);
                }
                else if (propertyType == typeof(sbyte) || propertyType == typeof(sbyte?))
                {
                    return Convert.ToSByte(value1.ToString()).CompareTo(value2);
                }
            }

            return 0;
        }

        /// <summary>
        /// Hàm làm tròn quy cách
        /// </summary>
        /// <param name="dobOptimalQuantityProduct">Số cần làm tròn</param>
        /// <param name="dobPackingQuantity">Quy cách làm tròn</param>
        /// <param name="dobRoundPercent">Làm tròn bao nhiêu % (vd: 20 là làm tròn 20%)</param>
        /// <param name="bolIsRoundInt">Có làm tròn thành số nguyên hay không</param>
        /// <returns></returns>
        public static double RoundNumberByPacking(double dobOptimalQuantityProduct, double dobPackingQuantity, double dobRoundPercent, bool bolIsRoundInt)
        {
            if (dobPackingQuantity != 0)
            {
                double dobProportion = dobOptimalQuantityProduct / dobPackingQuantity;
                double dobRound = 0;

                double dobMod = dobOptimalQuantityProduct % dobPackingQuantity;
                if (dobMod >= dobPackingQuantity * dobRoundPercent / 100)
                {
                    dobRound = Math.Ceiling(dobProportion);
                }
                else
                {
                    dobRound = Math.Floor(dobProportion);
                }
                dobOptimalQuantityProduct = dobPackingQuantity * dobRound;
            }
            else
            {
                dobOptimalQuantityProduct = Math.Round(dobOptimalQuantityProduct, 0, MidpointRounding.AwayFromZero);
            }
            if (bolIsRoundInt)
            {
                dobOptimalQuantityProduct = Math.Round(dobOptimalQuantityProduct, 0, MidpointRounding.AwayFromZero);
            }
            return dobOptimalQuantityProduct;
        }

        #endregion

        #region String methods
        /// <summary>
        /// Xóa tất cả khoảng trắng trong một chuỗi.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimAll(this string value)
        {
            string result = string.Empty;

            if (value != null)
            {
                result = value;

                while (result.Contains(" "))
                {
                    result = result.Replace(" ", "");
                }
            }

            return result;
        }

        /// <summary>
        /// Chuẩn hóa chuỗi lỗi dạng [+ strMessage +]
        /// </summary>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static string ErrorMessage(this string strMessage)
        {
            if (!strMessage.IsNullOrEmpty())
                return $"[+ {strMessage} +]{Environment.NewLine}";
            return strMessage;
        }
        #endregion

        #region GetDayOfWeek

        /// <summary>
        /// Xác định ngày thứ hai cùng tuần với ngày đang xét.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            DateTime result = date;

            while (result.DayOfWeek != DayOfWeek.Monday)
            {
                result = result.AddDays(-1);
            }

            return result;
        }

        /// <summary>
        /// Xác định ngày chủ nhật cùng tuần với ngày đang xét.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            DateTime result = date;

            while (result.DayOfWeek != DayOfWeek.Sunday)
            {
                result = result.AddDays(1);
            }

            return result;
        }

        /// <summary>
        /// Xác định ngày DayOfWeek tiếp theo của date ngay cả khi date = DayOfWeek.
        /// Ví dụ tìm ngày chủ nhật tiếp theo của date ngay cả khi date = chủ nhật.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetNextDay(this DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime result = date;

            while (result == date || result.DayOfWeek != dayOfWeek)
            {
                result = result.AddDays(1);
            }

            return result;
        }

        /// <summary>
        /// Xác định ngày DayOfWeek phía trước của date ngay cả khi date = DayOfWeek.
        /// Ví dụ tìm ngày chủ nhật phía trước của date ngay cả khi date = chủ nhật.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetPreviousDay(this DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime result = date;

            while (result == date || result.DayOfWeek != dayOfWeek)
            {
                result = result.AddDays(-1);
            }

            return result;
        }
        #endregion


        public static DateTime RoundMins(this DateTime value, int intMinute)
        {
            var ticksInMins = TimeSpan.FromMinutes(intMinute).Ticks;

            return value.Ticks % ticksInMins == 0 ? value : new DateTime((value.Ticks / ticksInMins + 1) * ticksInMins);
        }

        public static string ToMD5Hash(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" means hexadecimal with two characters
                }

                return sb.ToString();
            }
        }

        public static DataTable ToDataTable(List<Dictionary<string, object>> list)
        {
            var dataTable = new DataTable();

            if (list.Count == 0)
            {
                return dataTable;
            }

            foreach (var key in list[0].Keys)
            {
                dataTable.Columns.Add(key, typeof(object));
            }

            foreach (var dict in list)
            {
                var row = dataTable.NewRow();
                foreach (var kvp in dict)
                {
                    row[kvp.Key] = kvp.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.
                       GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attributes != null) && (attributes.Length > 0))
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }

        public static TEnum ToEnum<TEnum>(this string valEnum) where TEnum : struct, IConvertible
        {
            if (Enum.TryParse(valEnum, true, out TEnum ret))
                return ret;
            throw new ArgumentException($"'{valEnum}' is not a valid {typeof(TEnum).Name} enum value.", nameof(valEnum));
        }

        public static string GetEnumDescription<TEnum>(this string valEnum) where TEnum : struct, IConvertible
        {
            return GetEnumDescription(ToEnum<TEnum>(valEnum));
        }

        public static DateTime FisrtOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Kiểm tra email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9_\.-]{2,32}@[a-z0-9-]{2,}(\.[a-z0-9]{2,4}){1,2}$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        #region Làm tròn tiền
        /// <summary>
        /// Hàm làm tròn số tiền
        /// Mặc định là làm tròn từ 0.5
        /// Vd: 1.5 -> 2
        /// </summary>
        public static decimal RoundMoney(decimal value)
        {
            return Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Hàm làm tròn số tiền
        /// Mặc định là làm tròn từ 0.5
        /// Vd: 1.5 -> 2
        /// </summary>
        public static double RoundMoney(double value)
        {
            return Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Hàm làm tròn số
        /// Mặc định làm tròn 4 chữ số
        /// </summary>
        public static decimal RoundNumber(decimal value, int digits = 4)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Hàm làm tròn số
        /// Mặc định làm tròn 4 chữ số
        /// </summary>
        public static double RoundNumber(double value, int digits = 4)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region Convert List object nhiều lớp thành datatable
        public static DataTable ConvertListToDataTable<T>(List<T> objectList)
        {
            DataTable dataTable = new DataTable();
            AddColumnsToDataTable(dataTable, typeof(T));

            // Populate DataTable with data from the list
            foreach (var item in objectList)
            {
                DataRow row = dataTable.NewRow();
                PopulateDataRow(row, item);
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public static void AddColumnsToDataTable(DataTable dataTable, Type type)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (IsSimpleType(propertyType))
                {
                    dataTable.Columns.Add(property.Name, propertyType);
                }
                else if (IsEnumerableType(propertyType))
                {
                    // Handle collections (lists, arrays, etc.)
                    Type elementType = propertyType.GetElementType() ?? propertyType.GetGenericArguments().FirstOrDefault();
                    Type listType = typeof(List<>).MakeGenericType(elementType);
                    IList dummyList = (IList)Activator.CreateInstance(listType);
                    dataTable.Columns.Add(property.Name, dummyList.GetType());

                    if (elementType != null && !IsSimpleType(elementType))
                    {
                        AddColumnsToDataTable(dataTable, elementType);
                    }
                }
                else
                {
                    // Handle nested objects
                    dataTable.Columns.Add(property.Name, propertyType);
                    AddColumnsToDataTable(dataTable, propertyType);
                }
            }
        }

        public static void PopulateDataRow(DataRow row, object obj)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                object value = property.GetValue(obj);

                if (value != null)
                {
                    Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    if (IsSimpleType(propertyType))
                    {
                        row[property.Name] = value;
                    }
                    else if (IsEnumerableType(propertyType))
                    {
                        // Handle collections (lists, arrays, etc.)
                        Type elementType = propertyType.GetElementType() ?? propertyType.GetGenericArguments().FirstOrDefault();
                        Type listType = typeof(List<>).MakeGenericType(elementType);
                        IList dummyList = (IList)Activator.CreateInstance(listType);
                        PopulateDataRow(row, value);
                    }
                    else
                    {
                        // Handle nested objects
                        PopulateDataRow(row, value);
                    }
                }
            }
        }
        public static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type.IsValueType || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime);
        }

        public static bool IsEnumerableType(Type type)
        {
            return type.GetInterface(nameof(IEnumerable)) != null;
        }

        #endregion

        #region Giải mã phiếu mua hàng
        //truyền vào cipherString là 532BDC3D0BB606D97B15FCFA51E3BFB37A836BBDCCE6F5FC7BA4BF6A785D0FA6470EF054EF07E85D (lấy từ PinCodeEncryptByKeySecret trả ra ở hàm GetGiftVoucherIssueByOVID)         
        public static string DecryptCouponPincode(string cipherString, string strKey, string outputvoucherid)
        {
            if (outputvoucherid.IsNullOrEmpty())
                throw new Exception("Vui lòng cung cấp mã phiếu nhập. ");
            if (strKey.IsNullOrEmpty())
                throw new Exception("Vui lòng cung cấp mã bí mật. ");

            byte[] toEncryptArray = HexStringToByteArray(cipherString);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = createKey(strKey);
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            var result = UTF8Encoding.UTF8.GetString(resultArray);
            if (!outputvoucherid.IsNullOrEmpty())
                result = result.Replace(outputvoucherid.Trim() + "╬", string.Empty);
            if (!result.IsNullOrEmpty())
                result.Trim();
            return result;
        }
        private static byte[] HexStringToByteArray(string hex)
        {
            hex = hex.ToUpper();
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static byte[] createKey(String secretKey)
        {
            SHA1 sha1 = SHA1Managed.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] keyArray = sha1.ComputeHash(inputBytes);
            byte[] trimmedBytes = new byte[24];
            // convert to correct byte[] that the same with Java
            int length = keyArray.Length;
            for (int i = 0; i < 24; i++)
            {
                if (i >= length)
                {
                    trimmedBytes[i] = 0;
                }
                else
                {
                    int result = Convert.ToInt32(keyArray[i]);
                    if (result > 127)
                    {
                        trimmedBytes[i] = (byte)(result - 256);
                    }
                    else
                    {
                        trimmedBytes[i] = keyArray[i];
                    }
                }
            }
            return trimmedBytes;
        }
        #endregion
    }
}
