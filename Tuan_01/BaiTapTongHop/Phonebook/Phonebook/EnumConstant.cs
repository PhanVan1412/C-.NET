using System.ComponentModel;

namespace Phonebook
{
    public enum PhoneBookGroup
    {
        [Description("Gia đình")]
        Family,

        [Description("Bạn bè")]
        Friend,

        [Description("Công việc")]
        Work,

        [Description("Khác")]
        Other
    }
}