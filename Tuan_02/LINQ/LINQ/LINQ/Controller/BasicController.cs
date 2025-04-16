namespace LINQ.Controller
{
    public class BasicController
    {
        List<string> names = new List<string>  { "An", "Bình", "Chi", "Dung", "Anh", "Bảo" };

        // Bài 1: Lọc ra các tên bắt đầu bằng chữ "A"
        public void FilterNameByKey(string key)
        {
            var listFilter = names.Where(x => x.ToUpper().StartsWith(key.ToUpper())).ToList();
            Console.WriteLine($"Danh sách các tên bắt đầu bằng {key.ToUpper()} : {string.Join(',', listFilter)}");
        }

        // Bài 2: Sắp xếp danh sách theo thứ tự bảng chữ cái
        public void SortList()
        {
            var listOrderBy = names.OrderBy(x => x).ToList();
            Console.WriteLine($"Sắp xếp theo thứ tự bảng chứ cái: {string.Join(',', listOrderBy)}");
        }

        // Bài 3: Chuyển tất cả thành chữ hoa

        public void ToUpperCase()
        {
            var listToUpperCase = names.Select(x => x.ToUpper()).ToList();
            Console.WriteLine($"Chuyển tất cả thành chữ hoa:  {string.Join(',', listToUpperCase)}");
        }
    }
}
