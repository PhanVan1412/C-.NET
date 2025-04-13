//🧠 Bài 3: Tìm phần tử xuất hiện nhiều nhất trong danh sách
//🎯 Yêu cầu:
//Nhập một dãy số nguyên từ bàn phím (cách nhau bằng dấu ,)
//Đếm số lần xuất hiện của từng phần tử
//In ra phần tử có số lần xuất hiện nhiều nhất cùng với số lần xuất hiện đó


class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Nhập dãy danh sách (cách nhau dấu ,): ");
            string input = Console.ReadLine();
            List<int> listNumber = input.Split(",").Select(x => int.Parse(x.Trim())).ToList();
            CountDisplayNumber(listNumber);
        }
        catch(Exception)
        {
            Console.WriteLine("Lỗi nhập danh sách từ bàn phím.");
        }
    }

    static void CountDisplayNumber(List<int> listNumber)
    {
        try
        {
            if (listNumber == null || listNumber.Count == 0) {
                Console.WriteLine("Lỗi không thể tìm được phần tử xuất hiện nhiều nhất trong danh sách");
                return;
            }

            Dictionary<int, int> myDict = new Dictionary<int, int>();
            // Đếm số lần xuất hiện của phần tử theo key và value
            foreach (var item in listNumber)
            {
                if (myDict.ContainsKey(item))
                {
                    myDict[item]++;
                }
                else
                {
                    myDict[item] = 1;
                }
            }

            //// Xác định phần tử xuất hiện nhiều nhất và số lần xuất hiện
            //int mostFrequentNumber = myDict.First().Key;
            //int maxCount = myDict.First().Value;

            //foreach(var item in myDict) 
            //{ 
            //    if (item.Value > maxCount)
            //    {
            //        mostFrequentNumber = item.Key;
            //        maxCount = item.Value;
            //    }
            //}

            //Console.WriteLine($"Phần tử xuất hiện nhiều nhất là: {mostFrequentNumber} (xuất hiện {maxCount} lần)");

            var maxEntry = myDict.OrderByDescending(x => x.Value).First();
            Console.WriteLine($"Phần tử xuất hiện nhiều nhất là: {maxEntry.Key} (xuất hiện {maxEntry.Value} lần)");
        }
        catch(Exception)
        {
            Console.WriteLine("Lỗi không thể tìm được phần tử xuất hiện nhiều nhất trong danh sách");
        }
    }
}