//3: Duyệt mảng số, in ra số chẵn.

//4: Viết máy tính mini: nhập 2 số, nhập toán tử (+ - * /), in kết quả.


class Program
{
    static void Main(string[] args)
    {
        List<int> listNumber = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        CheckEvenNumber(listNumber);
    }

    static void CheckEvenNumber(List<int> list)
    {
        List<int> listResult = list.Where(x => x%2 == 0).ToList();
        Console.WriteLine(string.Join(',', listResult));
    }
}