namespace LINQ.BO
{
    public class StudentBO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Class { get; set; }
    }

    public class TotalStudentByClassBO
    {
        public string Class { get; set; }
        public int Total { get; set; }
    }
}
