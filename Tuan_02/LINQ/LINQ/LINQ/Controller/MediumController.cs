using LINQ.BO;

namespace LINQ.Controller
{
    public class MediumController
    {
        List<StudentBO> students = new List<StudentBO> {
            new StudentBO { Name = "An", Age = 18, Class = "12A" },
            new StudentBO { Name = "Bình", Age = 17, Class = "12A" },
            new StudentBO { Name = "Chi", Age = 18, Class = "12B" },
            new StudentBO { Name = "Dung", Age = 16, Class = "12B" },
            new StudentBO { Name = "Hà", Age = 18, Class = "12A" }
        };

        // Bài 4: Lọc ra những học sinh trên 17 tuổi
        public void FilterAgeStudent()
        {
            var listStudentFilter = students.Where(x => x.Age > 17).ToList();
            Console.WriteLine($"Danh sách học sinh trên 17 tuổi: {string.Join(", ", listStudentFilter.Select(x => x.Name).ToList())}");
        }

        // Bài 5: Đếm số học sinh mỗi lớp
        public void CountTotalStudentClass()
        {
            var totalStudent = students.GroupBy(x => x.Class).Select(y => new TotalStudentByClassBO
            {
                Class = y.Key,
                Total = y.Count()
            }).ToList();

            if (totalStudent != null && totalStudent.Count > 0)
            {
                foreach(var item in totalStudent)
                {
                    Console.WriteLine($"Lớp {item.Class}: có tổng {item.Total} học sinh");
                }
            }
        }

        // Bài 6: Kiểm tra học sinh nào tên "Lan" hay không?
        public bool IsFindExistStudent(string name)
        {
            bool isHasStudent = students.Any(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper());
            if (isHasStudent) 
            {
                Console.WriteLine($"Có học sinh tên {name.Trim()}");
                return isHasStudent;
            }
            Console.WriteLine($"Không có học sinh tên {name.Trim()}");
            return isHasStudent;
        }
    }
}
