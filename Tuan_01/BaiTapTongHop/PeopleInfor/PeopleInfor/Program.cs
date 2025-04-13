using PeopleInfor;

class Program
{
    static void Main(string[] args)
    {
        // Nhập số lượng và thông tin người, in ra màn hình.
        List<Person> listPerson = new List<Person>();
        Console.WriteLine("Nhập số lượng người: ");
        int numberPerson = int.Parse(Console.ReadLine());

        for (int i = 1; i <= numberPerson; i++) {

            Console.WriteLine($"\n👉 Nhập thông tin người thứ {i}:");
            Console.WriteLine("Họ tên: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Họ tên không hợp lệ, vui lòng nhập lại : ");
                Console.WriteLine("Họ tên: ");
                name = Console.ReadLine();
            }

            Console.WriteLine("Nhập tuổi: ");
            int age = int.Parse(Console.ReadLine());

            Console.WriteLine("Giới tính: ");
            string gender = Console.ReadLine();

            Person person = new Person(name, age, gender);
            listPerson.Add(person);
        }

        foreach (var item in listPerson) 
        { 
            item.DisplayInfor();
        }

        // Đếm số lượng nam/nữ
        int maleCount = listPerson.Where(x => x.Gender.Trim().ToUpper() == "NAM").Count();
        int femaleCount = listPerson.Where(x => x.Gender.Trim().ToUpper() == "NỮ").Count();
        Console.WriteLine($"\n👨 Số lượng nam: {maleCount}");
        Console.WriteLine($"👩 Số lượng nữ: {femaleCount}");

        // Tìm người lớn tuổi nhất
        Person maxAgePerson = listPerson.OrderByDescending(x => x.Age).FirstOrDefault();
        maxAgePerson.DisplayInfor();
    }
}