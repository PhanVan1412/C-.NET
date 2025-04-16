using LINQ.Controller;

public class Program
{
    static void Main(string[] args)
    {
        // Bài tập cơ bản
        BasicController basicController = new BasicController();
        basicController.FilterNameByKey("A");
        basicController.SortList();
        basicController.ToUpperCase();

        // Bài tập trung bình
        MediumController mediumController = new MediumController();
        mediumController.FilterAgeStudent();
        mediumController.CountTotalStudentClass();
        mediumController.IsFindExistStudent("Lan");

        // Bài tập nâng cao
        AdvancedController advancedController = new AdvancedController();
        advancedController.GetAllStudent();
        advancedController.FindOldestAge();
        advancedController.FindTwoStudentOldestAge();
        advancedController.GetFirstStudentAge17To18();
    }
}