using Xunit;

public class GradeAnalyzerTests
{
    [Fact]
    public void CalculateFinalGrade_Returns0ForStudentWithNoGrades()
    {
        // Arrange
        var student = new Student("1", "John Doe");
        var analyzer = new GradeAnalyzer(student);
        
        // Act
        var finalGrade = analyzer.CalculateFinalGrade();
        
        // Assert
        Assert.Equal(0, finalGrade);
    }

    [Fact]
    public void CalculateFinalGrade_HandlesLateSubmissionsWithPenalty()
    {
        // Arrange
        var student = new Student("1", "John Doe");
        var analyzer = new GradeAnalyzer(student);
        var lateGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 90,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2024, 1, 5)
        };
        var onTimeGrade = new StudentGrade {
            AssignmentId = "hw2",
            Score = 80,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(lateGrade);
        student.AddGrade(onTimeGrade);
        
        // Act
        var finalGrade = analyzer.CalculateFinalGrade();
        
        // Assert
        var expectedScore = ((90 * 0.9 + 80) / 200) * 100;
        Assert.Equal(expectedScore, finalGrade);
    }

    [Fact]
    public void CalculateFinalGrade_HandlesSubmissionExactlyOnDueDate()
    {
        // Arrange
        var student = new Student("1", "John Doe");
        var analyzer = new GradeAnalyzer(student);
        var exactlyOnTimeGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 90,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2024, 1, 1)
        };
        student.AddGrade(exactlyOnTimeGrade);
        
        // Act
        var finalGrade = analyzer.CalculateFinalGrade();
        
        // Assert
        Assert.Equal(90, finalGrade);
    }

    [Fact]
    public void CalculateFinalGrade_HandlesPerfectScores()
    {
        // Arrange
        var student = new Student("1", "John Doe");
        var analyzer = new GradeAnalyzer(student);
        var perfectGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 100,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(perfectGrade);
        
        // Act
        var finalGrade = analyzer.CalculateFinalGrade();
        
        // Assert
        Assert.Equal(100, finalGrade);
    }

    [Fact]
    public void CalculateFinalGrade_HandlesZeroScores()
    {
        // Arrange
        var student = new Student("1", "John Doe");
        var analyzer = new GradeAnalyzer(student);
        var zeroGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 0,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(zeroGrade);
        
        // Act
        var finalGrade = analyzer.CalculateFinalGrade();
        
        // Assert
        Assert.Equal(0, finalGrade);
    }

    [Fact]
    public void GetFailedAssignments_ReturnsEmptyArrayWhenNoFailures()
    {
        // Arrange
        var student = new Student("2", "Jane Smith");
        var analyzer = new GradeAnalyzer(student);
        var passingGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 80,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(passingGrade);
        
        // Act
        var failedAssignments = analyzer.GetFailedAssignments();
        
        // Assert
        Assert.Empty(failedAssignments);
    }

    [Fact]
    public void GetFailedAssignments_ReturnsAssignmentsBelow60Percent()
    {
        // Arrange
        var student = new Student("2", "Jane Smith");
        var analyzer = new GradeAnalyzer(student);
        var failingGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 50,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        var passingGrade = new StudentGrade {
            AssignmentId = "hw2",
            Score = 75,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 26)
        };
        student.AddGrade(failingGrade);
        student.AddGrade(passingGrade);
        
        // Act
        var failedAssignments = analyzer.GetFailedAssignments();
        
        // Assert
        Assert.Single(failedAssignments);
        Assert.Equal("hw1", failedAssignments[0].AssignmentId);
    }

    [Fact]
    public void GetFailedAssignments_HandlesExactly60PercentGrades()
    {
        // Arrange
        var student = new Student("2", "Jane Smith");
        var analyzer = new GradeAnalyzer(student);
        var borderlineGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 60,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(borderlineGrade);
        
        // Act
        var failedAssignments = analyzer.GetFailedAssignments();
        
        // Assert
        Assert.Empty(failedAssignments);
    }

    [Fact]
    public void GetFailedAssignments_HandlesDifferentTotalPointValues()
    {
        // Arrange
        var student = new Student("2", "Jane Smith");
        var analyzer = new GradeAnalyzer(student);
        var failingGrade = new StudentGrade {
            AssignmentId = "hw1",
            Score = 15,
            TotalPoints = 50,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(failingGrade);
        
        // Act
        var failedAssignments = analyzer.GetFailedAssignments();
        
        // Assert
        Assert.Single(failedAssignments);
    }

    [Fact]
    public void CalculateFinalGrade_HandlesAssignmentsWithDifferentTotalPoints()
    {
        // Arrange
        var student = new Student("1", "John Doe");
        var analyzer = new GradeAnalyzer(student);
        var fiftyPointAssignment = new StudentGrade {
            AssignmentId = "hw1",
            Score = 45,
            TotalPoints = 50,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        var hundredPointAssignment = new StudentGrade {
            AssignmentId = "hw2",
            Score = 80,
            TotalPoints = 100,
            SubmittedDate = new DateTime(2023, 12, 25)
        };
        student.AddGrade(fiftyPointAssignment);
        student.AddGrade(hundredPointAssignment);
        
        // Act
        var finalGrade = analyzer.CalculateFinalGrade();
        
        // Assert
        var expectedScore = ((45.0 + 80.0) / (50.0 + 100.0)) * 100.0;
        Assert.Equal(expectedScore, finalGrade);
    }
}