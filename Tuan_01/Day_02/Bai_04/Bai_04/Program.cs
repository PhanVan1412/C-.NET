//🧩 Bài 1: Tính tổng số chẵn trong List
//Nhập vào dãy số nguyên từ bàn phím (phân cách bằng dấu ,)

//Tính và in ra tổng các số chẵn

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Nhập dãy số nguyên (cách nhau bởi dấu , ): ");
            string input = Console.ReadLine();
            List<int> listNumber = input.Split(",").Select(x => int.Parse(x.Trim())).ToList();
            CalSumNumberEven(listNumber);
        }
        catch (Exception)
        {
            Console.WriteLine("Lỗi nhập danh sách từ bàn phím.");
        }
    }

    static void CalSumNumberEven(List<int> listNumber)
    {
        try
        {
            if (listNumber == null || listNumber.Count == 0)
            {
                Console.WriteLine("Danh sách không hợp lệ.");
                return;
            }
            int sumNumberEven = listNumber.Where(x => x%2 ==0).Sum();
            Console.WriteLine($"Tổng số chẵn của danh sách: {sumNumberEven}");
        }
        catch(Exception)
        {
            Console.WriteLine("Lỗi tính tổng số chẵn của danh sách");
        }
    }
}