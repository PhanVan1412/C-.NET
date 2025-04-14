
using LINQ.BO;

namespace LINQ.Controller
{
    public class AdvancedController
    {
        List<ClassRoomBO> classRooms = new List<ClassRoomBO> {
            new ClassRoomBO {
                ClassName = "12A",
                Students = new List<StudentBO> {
                    new StudentBO { Name = "An", Age = 18 },
                    new StudentBO { Name = "Bình", Age = 17 }
                }
            },
            new ClassRoomBO {
                ClassName = "12B",
                Students = new List<StudentBO> {
                    new StudentBO { Name = "Chi", Age = 18 },
                    new StudentBO { Name = "Dung", Age = 16 }
                }
            }
        };

        // Bài 7: Lấy toàn bộ danh sách học sinh của tất cả lớp thành 1 danh sách duy nhất (SelectMany).
        public List<StudentBO> GetAllStudent()
        {
            var listResult = classRooms.SelectMany(x => x.Students).ToList();
            return listResult;
        }

        // Bài 8: Tìm học sinh lớn tuổi nhất.
        public void FindOldestAge()
        {
            var listStudent = GetAllStudent();
            var student = new StudentBO();
            student = listStudent.OrderByDescending(x => x.Age).ToList().FirstOrDefault();
            if (student != null)
            {
                Console.WriteLine($"Học sinh có tuổi lớn nhất là {student.Name}: {student.Age}");
                return;
            }
            Console.WriteLine("Không có học sinh lớn tuổi nhất");
        }

        // Bài 9: Lấy ra 2 học sinh lớn tuổi nhất.
        public void FindTwoStudentOldestAge()
        {
            var listStudent = GetAllStudent();
            var top2 = listStudent.OrderByDescending(x => x.Age).Take(2).ToList();
            foreach (var student in top2)
            {
                Console.WriteLine($"Học sinh: {student.Name} - {student.Age}");
            }
        }

        // Bài 10: Lấy danh sách học sinh có độ tuổi từ 17 đến 18, sắp xếp theo tuổi giảm dần, lấy 1 học sinh đầu tiên.
        public void GetFirstStudentAge17To18()
        {
            var listStudent = GetAllStudent();
            var student = listStudent.Where(x => x.Age >= 17 && x.Age <= 18).OrderByDescending(x => x.Age).FirstOrDefault();
            if (student != null)
            {
                Console.WriteLine($"Học sinh: {student.Name} - {student.Age}");
            }
            else
            {
                Console.WriteLine("Không có học sinh từ 17 đến 18 tuổi");
            }
        } 
    }
}
