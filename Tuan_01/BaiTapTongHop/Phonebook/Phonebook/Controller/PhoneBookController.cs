using Phonebook.BLL;

namespace Phonebook.Controller
{
    public class PhoneBookController
    {

        private PhoneBookBLL _phoneBookBLL;
        private string filePath = "contacts.json"; // đường dẫn file

        public PhoneBookController()
        {
            _phoneBookBLL = new PhoneBookBLL();
            _phoneBookBLL.LoadFromFile(filePath); // Load dữ liệu khi chương trình khởi động
        }

        public void AddContact(string firstName, string lastName, string phoneNumber, string mail, string address, string group)
        {
            bool result = _phoneBookBLL.AddContact(firstName, lastName, phoneNumber, mail, address, group);
            if (result)
            {
                if (result)
                {
                    _phoneBookBLL.SaveToFile(filePath); // Lưu lại mỗi khi thêm liên hệ
                    Console.WriteLine("✅ Đã thêm liên hệ thành công!");
                }
            }
        }

        public void DisplayAllContacts()
        {
            _phoneBookBLL.DisplayAllContacts();
        }
    }
}
