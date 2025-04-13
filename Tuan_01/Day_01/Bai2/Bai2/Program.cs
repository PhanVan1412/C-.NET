using System;

class Program
{
    static void Main(string[] args)
    {
        PrintBangCuuChuong();
    }

    static void PrintBangCuuChuong()
    {
        for (int i = 2; i <= 9; i++)
        {
            Console.WriteLine($"\n👉 Bảng cửu chương {i}:");

            for (int j = 1; j <= 10; j++)
            {
                Console.WriteLine($"{i} x {j} = {i * j}");
            }
        }
    }
}
