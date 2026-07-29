using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicCS.Chapter10
{
    internal class LinqDemo
    {
        static void Main()
        {
            var students = GetStudents();

            // ---- LINQ 쿼리 (Query syntax) ----
            var honorRoll = from s in students
                            where s.Grade >= 3.5
                            orderby s.Grade descending
                            select $"{s.Name} ({s.Grade:F2})";

            Console.WriteLine("Honor Roll (query syntax):");
            foreach (var entry in honorRoll)
                Console.WriteLine($"  {entry}");

            // ---- LINQ 메서드 (Method syntax) ----
            var topStudents = students
                .Where(s => s.Grade >= 3.0)
                .OrderBy(s => s.Name)
                .Select(s => new { s.Name, s.Grade });

            Console.WriteLine("\nAll passing students (method syntax):");
            foreach (var s in topStudents)
                Console.WriteLine($"  {s.Name}: {s.Grade:F2}");

            // ---- 집계 (Aggregation) ----
            Console.WriteLine($"\nTotal students: {students.Count}");
            Console.WriteLine($"Average grade: {students.Average(s => s.Grade):F2}");
            Console.WriteLine($"Max grade: {students.Max(s => s.Grade):F2}");
            Console.WriteLine($"Min grade: {students.Min(s => s.Grade):F2}");

            // ---- GroupBy ----
            var byMajor = students.GroupBy(s => s.Major);
            Console.WriteLine("\nStudents by Major:");
            foreach (var group in byMajor)
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} students");
                foreach (var s in group)
                    Console.WriteLine($"    - {s.Name}");
            }

            // ---- Any / All / Contains ----
            Console.WriteLine($"\nAny student with grade >= 4.0? {students.Any(s => s.Grade >= 4.0)}");
            Console.WriteLine($"All students have grade >= 2.0? {students.All(s => s.Grade >= 2.0)}");

            // ---- First / FirstOrDefault ----
            var firstHigh = students.FirstOrDefault(s => s.Grade >= 4.0);
            Console.WriteLine($"First student with grade >= 4.0: {firstHigh?.Name ?? "None"}");

            // ---- SelectMany ----
            var allSubjects = students.SelectMany(s => s.Subjects).Distinct();
            Console.WriteLine($"\nAll subjects offered: {string.Join(", ", allSubjects)}");

            // ---- LINQ to Objects with IEnumerable ----
            var numbers = Enumerable.Range(1, 20);
            var evenSquares = numbers
                .Where(n => n % 2 == 0)
                .Select(n => n * n);
            Console.WriteLine($"\nEven squares: {string.Join(", ", evenSquares)}");

            // ---- Zip (C# 3.0+) ----
            var names = new[] { "Alice", "Bob", "Charlie" };
            var scores = new[] { 95, 87, 91 };
            var zipped = names.Zip(scores, (name, score) => $"{name}: {score}");
            Console.WriteLine($"\nZipped: {string.Join(", ", zipped)}");
        }

        static List<Student> GetStudents() => new()
        {
            new Student("Alice", "CS", 4.0, new[] {"Algorithms", "OS", "DB"}),
            new Student("Bob", "Math", 3.7, new[] {"Algebra", "Calculus"}),
            new Student("Charlie", "CS", 3.2, new[] {"Algorithms", "Networks"}),
            new Student("Diana", "Physics", 3.9, new[] {"Mechanics", "EM"}),
            new Student("Eve", "Math", 2.8, new[] {"Statistics", "Algebra"}),
            new Student("Frank", "CS", 3.5, new[] {"OS", "DB", "AI"}),
        };
    }

    record Student(string Name, string Major, double Grade, string[] Subjects);
}
