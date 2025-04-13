//Nhập vào một dãy số nguyên (phân cách bởi dấu ,) và in ra phần tử có giá trị lớn thứ 2 trong dãy.
//👉 Lưu ý: Nếu không tồn tại phần tử lớn thứ 2 (ví dụ chỉ có 1 số hoặc tất cả số giống nhau) thì in ra "Không tồn tại phần tử lớn thứ 2".

//🧩 Gợi ý xử lý:
//Nhập chuỗi và chuyển thành List<int>
//Dùng Distinct() để loại bỏ các số trùng nhau
//Kiểm tra xem danh sách còn ≥ 2 phần tử không
//Nếu có: list.OrderByDescending().Skip(1).First() → ra số lớn thứ 2
//Nếu không có: in thông báo lỗi


class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Nhập vào một dãy số nguyên (phân cách bởi dấu ,)");
            string input = Console.ReadLine();
            List<int> listNumber = input.Split(",").Select(x => int.Parse(x.Trim())).ToList();
            FindSecondLargestNumber(listNumber);
        }
        catch (Exception)
        {
            Console.WriteLine("Lỗi nhập danh sách từ bàn phím");
        }
    }

    static void FindSecondLargestNumber(List<int> listNumber)
    {
        try
        {
            List<int> listDistinct = listNumber.Distinct().ToList();
            if (listDistinct.Count <= 1)
            {
                Console.WriteLine("không tồn tại phần tử lớn thứ 2");
                return;
            }
            // Sort theo thứ tự giảm dần và bỏ đi phần tử đầu tiên, sau đó lấy phần tử thứ 2
            int resultNumber = listDistinct.OrderByDescending(x => x).Skip(1).FirstOrDefault();
            Console.WriteLine($"kết quả: {resultNumber}");
        }
        catch (Exception)
        {
            Console.WriteLine("Lỗi tìm số lớn thứ 2 trong danh sách");
        }
    }
}