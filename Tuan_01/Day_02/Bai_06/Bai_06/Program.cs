//👉 Bài 4: Kiểm tra xem danh sách có phần tử nào là số nguyên tố không?

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Vui lòng nhập số dãy (cách nhau bằng dấu ,)");
            string input = Console.ReadLine();
            List<int> listNumber = input.Split(",").Select(x => int.Parse(x.Trim())).ToList();
            bool hasPrime = listNumber.Any(x => IsPrime(x));
            if (hasPrime)
            {
                Console.WriteLine("Danh sách có ít nhất một số nguyên tố.");
                return;
            }
            Console.WriteLine("Danh sách không có số nguyên tố nào.");
        }
        catch (Exception objEx)
        {
            Console.WriteLine("Lỗi nhập dữ liệu từ bàn phím.");
        }
    }

    // Hàm kiểm tra số nguyên có phải số nguyên tố hay không.
    static bool IsPrime (int number)
    {
        if (number < 2)
        {
            return false;
        }
        var isPrime = true;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        return isPrime;
    }
}
