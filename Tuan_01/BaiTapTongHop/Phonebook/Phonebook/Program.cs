using Phonebook.BLL;

public class Program
{
    static void Main(string[] args)
    {
        PhoneBookBLL phoneBook = new PhoneBookBLL();

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║      📱 CHƯƠNG TRÌNH QUẢN LÝ DANH BẠ ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 1. ➕ Thêm liên hệ mới                ║");
            Console.WriteLine("║ 2. 📋 Hiển thị tất cả liên hệ        ║");
            Console.WriteLine("║ 0. ❌ Thoát                           ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.ResetColor();

            Console.Write("👉 Chọn chức năng: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Họ: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Tên: ");
                    string lastName = Console.ReadLine();
                    Console.Write("SĐT: ");
                    string phone = Console.ReadLine();
                    Console.Write("Email: ");
                    string email = Console.ReadLine();
                    Console.Write("Địa chỉ: ");
                    string address = Console.ReadLine();
                    Console.Write("Nhóm (Gia đình/Bạn bè/Công việc/Khác): ");
                    string group = Console.ReadLine();

                    bool added = phoneBook.AddContact(firstName, lastName, phone, email, address, group);
                    if (added)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✅ Thêm thành công!");
                        Console.ResetColor();
                    }
                    break;

                case "2":
                    phoneBook.DisplayAllContacts();
                    break;

                case "0":
                    Console.WriteLine("👋 Tạm biệt!");
                    return;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Lựa chọn không hợp lệ.");
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}