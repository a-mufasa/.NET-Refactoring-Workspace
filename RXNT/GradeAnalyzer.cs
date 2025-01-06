// TODO: Convert this class to add properties
//       - add a DueDate property extracted from the code below
//       - add a ScoreWithLatePenalty property or method extracted from IsLateSubmission below
public class StudentGrade
{
    public string AssignmentId { get; set; }
    public double Score { get; set; }
    public double TotalPoints { get; set; }
    public DateTime SubmittedDate { get; set; }
}

public class Student
{
    private string Id;
    private string Name;
    private List<StudentGrade> Grades;

    public Student(string id, string name)
    {
        Id = id;
        Name = name;
        Grades = new List<StudentGrade>();
    }

    public string GetId() => Id;

    public string GetName() => Name;

    public List<StudentGrade> GetGrades() => Grades;

    public void AddGrade(StudentGrade grade) => Grades.Add(grade);
}

public class GradeAnalyzer
{
    private Student Student;

    public GradeAnalyzer(Student student)
    {
        Student = student;
    }

    public double CalculateFinalGrade()
    {
        var grades = Student.GetGrades();
        if (grades.Count == 0) return 0;

        double totalEarned = 0;
        double totalPossible = 0;

        foreach (var grade in grades)
        {
            // Feature Envy: Deeply manipulating data that belongs to StudentGrade
            // TODO: Extract this logic into the `StudentGrade` class
            if (IsLateSubmission(grade))
            {
                totalEarned += grade.Score * 0.9; // 10% penalty for late submissions
            }
            else
            {
                totalEarned += grade.Score;
            }
            totalPossible += grade.TotalPoints;
        }

        return (totalEarned / totalPossible) * 100;
    }

    // Feature Envy: This method only uses StudentGrade data
    // TODO: Extract this logic into the `StudentGrade` class
    private bool IsLateSubmission(StudentGrade grade)
    {
        var submissionDate = grade.SubmittedDate;
        var dueDate = new DateTime(2024, 1, 1); // Simplified for example
        return submissionDate > dueDate;
    }

    // Feature Envy: This method is all about StudentGrade data. We'll focus on this later
    public List<StudentGrade> GetFailedAssignments()
    {
        return Student.GetGrades().FindAll(grade => (grade.Score / grade.TotalPoints) < 0.6);
    }
}