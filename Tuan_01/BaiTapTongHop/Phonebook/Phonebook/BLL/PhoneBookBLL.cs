using Newtonsoft.Json;
using Phonebook.BO;
using System.ComponentModel;

namespace Phonebook.BLL
{
    public class PhoneBookBLL
    {
        private List<PhoneBookBO> _contacts;

        public PhoneBookBLL()
        {
            _contacts = new List<PhoneBookBO>();
        }

        // Thêm liên hệ mới sau khi kiểm tra số điện thoại
        public bool AddContact(string firstName, string lastName, string phoneNumber, string mail, string address, string group)
        {
            if (IsPhoneNumberExists(phoneNumber))
            {
                Console.WriteLine("❌ Số điện thoại đã tồn tại trong danh bạ.");
                return false;
            }

            PhoneBookBO phoneBook = new PhoneBookBO
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Mail = mail,
                Address = address,
                Group = ConvertToGroup(group)
            };

            _contacts.Add(phoneBook);
            Console.WriteLine("✅ Đã thêm liên hệ thành công!");
            return true;
        }

        // Kiểm tra trùng số điện thoại
        private bool IsPhoneNumberExists(string phoneNumber)
        {
            return _contacts.Any(p => p.PhoneNumber == phoneNumber);
        }

        private PhoneBookGroup ConvertToGroup(string group)
        {
            return group.ToUpper() switch
            {
                "GIA ĐÌNH" => PhoneBookGroup.Family,
                "BẠN BÈ" => PhoneBookGroup.Friend,
                "CÔNG VIỆC" => PhoneBookGroup.Work,
                _ => PhoneBookGroup.Other
            };
        }

        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

        // Hàm xem danh sách
        public void DisplayAllContacts()
        {
            if (_contacts.Count == 0)
            {
                Console.WriteLine("📒 Danh bạ trống!");
                return;
            }

            Console.WriteLine("\n📒 Danh sách liên hệ:");
            foreach (var contact in _contacts)
            {
                string groupDescription = GetEnumDescription(contact.Group);
                Console.WriteLine($"{contact.FullName} - {contact.PhoneNumber} - {groupDescription}");
            }
        }

        // Lưu thông tin danh bạ thành file
        public void SaveToFile(string path)
        {
            var json = JsonConvert.SerializeObject(_contacts, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        //Đọc lại thông tin từ file
        public void LoadFromFile(string path)
        {
            if(File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _contacts = JsonConvert.DeserializeObject<List<PhoneBookBO>>(json);
            }
        }
    }
}
