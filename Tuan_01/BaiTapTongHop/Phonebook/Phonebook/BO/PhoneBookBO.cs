using Phonebook;

namespace Phonebook.BO
{
    public class PhoneBookBO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        public string PhoneNumber { get; set; }

        public string Mail { get; set; }
        public string Address { get; set; }

        public PhoneBookGroup Group { get; set; }
    }
}
