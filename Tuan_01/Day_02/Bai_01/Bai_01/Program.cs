// Bài 1: Nhập vào 5 số nguyên và in ra tổng các phần tử:

class Program
{
    static void Main(string[] args)
    {
        int[] myArray = new int[5];
        int i = 0;
        while (i < myArray.Length)
        {
            try
            {
                Console.WriteLine($"Nhập số tự nhiên thứ {i + 1} : ");
                string value = Console.ReadLine();
                int number = int.Parse(value);
                myArray[i] = number;
                i++; // Chỉ tăng nếu nhập hợp lệ
            }
            catch(Exception objEx)
            {
                Console.WriteLine("Giá trị không hợp lệ, vui lòng nhập lại một số nguyên.");
            }
        }
        TotalNumber(myArray);
    }

    static void TotalNumber(int[] myArray)
    {
        int total = myArray.ToList().Sum();
        Console.WriteLine($"Tổng các phần tử trong mảng là: {total}");
    }
}