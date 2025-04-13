using System.Reflection;

namespace PeopleInfor
{
    public class Person
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Person (string fullName, int age, string gender)
        {
            this.FullName = fullName;
            this.Age = age;
            this.Gender = gender;
        }

        public void DisplayInfor()
        {
            Console.WriteLine($"Họ tên: {FullName} - Tuổi: {Age} - Giới tính: {Gender}");
        }
    }
}
