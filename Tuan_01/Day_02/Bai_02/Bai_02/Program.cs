
//Bài 2: Tìm giá trị lớn nhất và nhỏ nhất trong mảng số nguyên bạn đã nhập.

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Vui lòng nhập dãy số tự nhiên cách nhau bởi dấu ',' : ");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) {
                var listNumber = input.Split(",").Select(x => int.Parse(x)).ToList();
                FindMinMax(listNumber);
            }
            else
            {
                Console.WriteLine("Danh sách nhập vào không hợp lệ, vui lòng thử lại!");
            }
        }
        catch (Exception) { 
            Console.WriteLine("Danh sách nhập vào không hợp lệ, vui lòng thử lại!");
        }
    }

    static void FindMinMax(List<int> listNumber)
    {
        Console.WriteLine($"Phần tử có giá trị nhỏ nhất là: {listNumber.Min()}");
        Console.WriteLine($"Phần tử có giá trị lớn nhất là: {listNumber.Max()}");
    }
}