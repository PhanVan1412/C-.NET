// Viết chương trình nhập vào tuổi → in ra "Đủ tuổi lái xe" hay "Chưa đủ tuổi".


class Program
{
    static void Main(string[] agrs)
    {
        Console.WriteLine("Nhập độ tuổi của bạn: ");
        int age = int.Parse(Console.ReadLine());

        CheckDrivingEligibitity(age);

    }

    static void CheckDrivingEligibitity(int age)
    {
        if (age < 18)
        {
            Console.WriteLine("❌ Bạn chưa đủ tuổi lái xe!");
        }
        else
        {
            Console.WriteLine("✅ Bạn đã đủ tuổi lái xe!");
        }
    }
}