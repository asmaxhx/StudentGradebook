using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public string StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CourseCode { get; set; }

    public Dictionary<string, List<int>> Grades { get; set; }

    public Student(string id, string first, string last, string course)
    {
        StudentId = id;
        FirstName = first;
        LastName = last;
        CourseCode = course;

        Grades = new Dictionary<string, List<int>>()
        {
            { "Programming", new List<int>() },
            { "Databases", new List<int>() },
            { "Testing", new List<int>() },
            { "DevOps", new List<int>() }
        };
    }

    public string FullName => $"{FirstName} {LastName}";
}

class Program
{
    static List<Student> students = new List<Student>();

    static HashSet<string> courseCodes = new HashSet<string>();

    static Queue<Student> assessmentQueue = new Queue<Student>();

    static Stack<string> recentActions = new Stack<string>();

    static void Main()
    {
        SeedStudents();

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== Student Gradebook App =====");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View All Students");
            Console.WriteLine("3. Search Student by ID");
            Console.WriteLine("4. Remove Student");
            Console.WriteLine("5. Add Grade");
            Console.WriteLine("6. View Student Grades");
            Console.WriteLine("7. Calculate Average Grade");
            Console.WriteLine("8. View Unique Course Codes");
            Console.WriteLine("9. Add Student To Assessment Queue");
            Console.WriteLine("10. Process Assessment Queue");
            Console.WriteLine("11. View Recent Actions");
            Console.WriteLine("12. Sort Students By Last Name");
            Console.WriteLine("13. Exit");

            string choice = GetInput("Select option: ");

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;

                case "2":
                    ViewStudents();
                    break;

                case "3":
                    SearchStudent();
                    break;

                case "4":
                    RemoveStudent();
                    break;

                case "5":
                    AddGrade();
                    break;

                case "6":
                    ViewStudentGrades();
                    break;

                case "7":
                    CalculateAverage();
                    break;

                case "8":
                    ViewCourseCodes();
                    break;

                case "9":
                    AddToQueue();
                    break;

                case "10":
                    ProcessQueue();
                    break;

                case "11":
                    ViewRecentActions();
                    break;

                case "12":
                    SortStudents();
                    break;

                case "13":
                    running = false;
                    Console.WriteLine("Exiting application...");
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please select 1-13.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    static void SeedStudents()
    {
        students.Add(new Student("ST001", "Sarah", "Ahmed", "CSD101"));
        students.Add(new Student("ST002", "James", "Brown", "JAV101"));
        students.Add(new Student("ST003", "Priya", "Patel", "SDET202"));
        students.Add(new Student("ST004", "John", "Smith", "DEVOPS301"));
        students.Add(new Student("ST005", "Emma", "Wilson", "CSD101"));

        foreach (var student in students)
        {
            courseCodes.Add(student.CourseCode);
        }

        students[0].Grades["Programming"].AddRange(new List<int> { 85, 90, 78 });
        students[0].Grades["Databases"].AddRange(new List<int> { 72, 80 });

        students[1].Grades["Testing"].AddRange(new List<int> { 88, 91 });

        students[2].Grades["DevOps"].AddRange(new List<int> { 75, 82 });

        recentActions.Push("Loaded default students");
    }

    static void AddStudent()
    {
        string id = GetInput("Student ID: ");

        if (students.Any(s => s.StudentId.Equals(id, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Error: Student ID already exists.");
            return;
        }

        string first = GetInput("First Name: ");
        string last = GetInput("Last Name: ");
        string course = GetInput("Course Code: ");

        Student student = new Student(id, first, last, course);

        students.Add(student);

        courseCodes.Add(course);

        recentActions.Push($"Added student {id}");

        Console.WriteLine("Student added successfully.");
    }

    static void ViewStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        Console.WriteLine("\nStudents:");

        foreach (var s in students)
        {
            Console.WriteLine($"{s.StudentId} | {s.FullName} | {s.CourseCode}");
        }
    }

    static void SearchStudent()
    {
        Student student = FindStudentById();

        if (student == null)
            return;

        Console.WriteLine($"\n{student.StudentId} | {student.FullName} | {student.CourseCode}");
    }

    static void RemoveStudent()
    {
        Student student = FindStudentById();

        if (student == null)
            return;

        Console.Write($"Are you sure you want to remove {student.FullName}? (Y/N): ");

        string answer = Console.ReadLine();

        if (answer.ToUpper() != "Y")
        {
            Console.WriteLine("Removal cancelled.");
            return;
        }

        students.Remove(student);

        recentActions.Push($"Removed student {student.StudentId}");

        Console.WriteLine("Student removed successfully.");
    }

    static void SortStudents()
    {
        students = students.OrderBy(s => s.LastName).ToList();

        Console.WriteLine("Students sorted by last name.");
    }

    static void AddGrade()
    {
        Student student = FindStudentById();

        if (student == null)
            return;

        string subject = GetSubject();

        int grade = GetGrade();

        student.Grades[subject].Add(grade);

        recentActions.Push($"Added grade for {student.StudentId}");

        Console.WriteLine("Grade added successfully.");
    }

    static void ViewStudentGrades()
    {
        Student student = FindStudentById();

        if (student == null)
            return;

        Console.WriteLine($"\nGrades for {student.FullName}");

        foreach (var subject in student.Grades)
        {
            if (subject.Value.Count == 0)
            {
                Console.WriteLine($"{subject.Key}: No grades");
            }
            else
            {
                Console.WriteLine($"{subject.Key}: {string.Join(", ", subject.Value)}");
            }
        }
    }

    static void CalculateAverage()
    {
        Student student = FindStudentById();

        if (student == null)
            return;

        List<int> allGrades = new List<int>();

        Console.WriteLine($"\nAverage Grades for {student.FullName}");

        foreach (var subject in student.Grades)
        {
            if (subject.Value.Count > 0)
            {
                double average = subject.Value.Average();

                Console.WriteLine($"{subject.Key}: {average:F2}");

                allGrades.AddRange(subject.Value);
            }
        }

        if (allGrades.Count > 0)
        {
            Console.WriteLine($"\nOverall Average: {allGrades.Average():F2}");
        }
        else
        {
            Console.WriteLine("No grades available.");
        }
    }

    static void ViewCourseCodes()
    {
        Console.WriteLine("\nUnique Course Codes:");

        foreach (string code in courseCodes)
        {
            Console.WriteLine(code);
        }

        string searchCode = GetInput("\nEnter a course code to search: ");

        if (courseCodes.Contains(searchCode))
        {
            Console.WriteLine("Course code exists.");
        }
        else
        {
            Console.WriteLine("Course code not found.");
        }

        bool added = courseCodes.Add("CSD101");

        if (!added)
        {
            Console.WriteLine("Duplicate course code rejected by HashSet.");
        }
    }

    static void AddToQueue()
    {
        Student student = FindStudentById();

        if (student == null)
            return;

        if (assessmentQueue.Any(s => s.StudentId == student.StudentId))
        {
            Console.WriteLine("Student is already waiting for assessment.");
            return;
        }

        assessmentQueue.Enqueue(student);

        Console.WriteLine($"{student.FullName} added to assessment queue.");
    }

    static void ProcessQueue()
    {
        if (assessmentQueue.Count == 0)
        {
            Console.WriteLine("No students waiting for assessment.");
            return;
        }

        Console.WriteLine($"\nNext Student: {assessmentQueue.Peek().FullName}");

        Student processed = assessmentQueue.Dequeue();

        recentActions.Push($"Processed assessment for {processed.StudentId}");

        Console.WriteLine($"{processed.FullName} processed.");

        Console.WriteLine("\nRemaining Queue:");

        if (assessmentQueue.Count == 0)
        {
            Console.WriteLine("Queue is now empty.");
        }
        else
        {
            foreach (var student in assessmentQueue)
            {
                Console.WriteLine(student.FullName);
            }
        }
    }

    static void ViewRecentActions()
    {
        if (recentActions.Count == 0)
        {
            Console.WriteLine("No recent actions available.");
            return;
        }

        Console.WriteLine($"\nMost Recent Action: {recentActions.Peek()}");

        Console.WriteLine("\nAction History:");

        foreach (string action in recentActions)
        {
            Console.WriteLine(action);
        }

        Console.Write("\nUndo latest action? (Y/N): ");

        string answer = Console.ReadLine();

        if (answer.ToUpper() == "Y")
        {
            string removedAction = recentActions.Pop();

            Console.WriteLine($"Removed action: {removedAction}");
        }
    }

    static Student FindStudentById()
    {
        string id = GetInput("Enter Student ID: ");

        Student student = students
            .FirstOrDefault(s => s.StudentId.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (student == null)
        {
            Console.WriteLine("Error: Student not found.");
        }

        return student;
    }

    static string GetSubject()
    {
        while (true)
        {
            Console.WriteLine("\nAvailable Subjects:");
            Console.WriteLine("Programming");
            Console.WriteLine("Databases");
            Console.WriteLine("Testing");
            Console.WriteLine("DevOps");

            string subject = GetInput("Enter Subject: ");

            if (subject.Equals("Programming", StringComparison.OrdinalIgnoreCase))
                return "Programming";

            if (subject.Equals("Databases", StringComparison.OrdinalIgnoreCase))
                return "Databases";

            if (subject.Equals("Testing", StringComparison.OrdinalIgnoreCase))
                return "Testing";

            if (subject.Equals("DevOps", StringComparison.OrdinalIgnoreCase))
                return "DevOps";

            Console.WriteLine("Error: Invalid subject.");
        }
    }

    static int GetGrade()
    {
        while (true)
        {
            Console.Write("Enter Grade (0-100): ");

            if (!int.TryParse(Console.ReadLine(), out int grade))
            {
                Console.WriteLine("Error: Grade must be a number.");
                continue;
            }

            if (grade < 0 || grade > 100)
            {
                Console.WriteLine("Error: Grade must be between 0 and 100.");
                continue;
            }

            return grade;
        }
    }

    static string GetInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Error: Please enter a value.");
                continue;
            }

            return input.Trim();
        }
    }
}