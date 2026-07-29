using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicCS.Chapter04
{
    internal class ArraysCollections
    {
        static void Main()
        {
            // ---- 배열 (Arrays) ----
            int[] numbers = new int[5] { 10, 20, 30, 40, 50 };
            numbers[0] = 99;

            Console.WriteLine($"Array length: {numbers.Length}");
            Console.WriteLine($"First element: {numbers[0]}");
            Console.WriteLine($"Last element: {numbers[^1]}");  // C# 8.0 index from end

            // 2차원 배열 (Multi-dimensional array)
            int[,] matrix = new int[2, 3]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };
            Console.WriteLine($"matrix[1,2] = {matrix[1, 2]}");

            // 가변 배열 (Jagged array)
            int[][] jagged = new int[3][];
            jagged[0] = new int[] { 1, 2 };
            jagged[1] = new int[] { 3, 4, 5 };
            jagged[2] = new int[] { 6 };
            Console.WriteLine($"jagged[1][2] = {jagged[1][2]}");

            // ---- 리스트 (List<T>) ----
            List<string> fruits = new List<string> { "Apple", "Banana", "Cherry" };
            fruits.Add("Durian");
            fruits.Remove("Banana");

            Console.WriteLine($"\nList count: {fruits.Count}");
            foreach (var fruit in fruits)
                Console.WriteLine($"  - {fruit}");

            // ---- 딕셔너리 (Dictionary<TKey, TValue>) ----
            Dictionary<string, int> ages = new()
            {
                { "Alice", 30 },
                { "Bob", 25 },
                { "Charlie", 35 }
            };
            ages["Diana"] = 28;

            Console.WriteLine($"\nDictionary entries:");
            foreach (var kvp in ages)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");

            // ---- HashSet ----
            HashSet<int> unique = new() { 1, 2, 3, 2, 1 };
            Console.WriteLine($"\nHashSet count (duplicates removed): {unique.Count}");

            // ---- Queue / Stack ----
            Queue<string> queue = new();
            queue.Enqueue("First");
            queue.Enqueue("Second");
            Console.WriteLine($"\nQueue dequeue: {queue.Dequeue()}");

            Stack<int> stack = new();
            stack.Push(1);
            stack.Push(2);
            Console.WriteLine($"Stack pop: {stack.Pop()}");
        }
    }
}
