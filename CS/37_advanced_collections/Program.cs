using System.Collections.Immutable;
using System.Threading.Channels;

namespace BasicCS.Chapter37;

static class Program
{
    static void Main()
    {
        // ---- 1) 불변 컬렉션: 변경은 새 컬렉션 반환 ----
        Console.WriteLine("== 불변 컬렉션 ==");
        ImmutableArray<int> arr = ImmutableArray.Create(1, 2, 3);
        ImmutableArray<int> arr2 = arr.Add(4).Add(5);

        Console.WriteLine($"  원본: [{string.Join(",", arr)}] (불변)");
        Console.WriteLine($"  변경본: [{string.Join(",", arr2)}]");

        ImmutableDictionary<string, int> scores =
            ImmutableDictionary.Create<string, int>()
                .Add("철수", 90)
                .Add("영희", 85);
        var updated = scores.SetItem("철수", 95);
        Console.WriteLine($"  딕셔너리 원본 철수={scores["철수"]} -> 변경본 철수={updated["철수"]}");

        // ---- 2) PriorityQueue ----
        Console.WriteLine("\n== PriorityQueue ==");
        var tasks = new PriorityQueue<string, int>(); // (작업, 우선순위)
        tasks.Enqueue("일반 점검", 3);
        tasks.Enqueue("긴급 패치", 1);
        tasks.Enqueue("백업", 5);
        tasks.Enqueue("로그 정리", 2);

        Console.WriteLine("  처리 순서:");
        while (tasks.TryDequeue(out string? task, out int priority))
            Console.WriteLine($"    [우선순위 {priority}] {task}");

        // ---- 3) Channel: 생산자-소비자 ----
        Console.WriteLine("\n== Channel ==");
        var channel = Channel.CreateUnbounded<int>();

        var producer = Task.Run(async () =>
        {
            for (int i = 1; i <= 5; i++)
            {
                await channel.Writer.WriteAsync(i);
                await Task.Delay(50);
            }
            channel.Writer.TryComplete();
        });

        var consumer = Task.Run(async () =>
        {
            var list = new List<int>();
            await foreach (var item in channel.Reader.ReadAllAsync())
                list.Add(item);
            return list;
        });

        Task.WaitAll(producer);
        Console.WriteLine($"  소비 결과: [{string.Join(",", consumer.Result)}]");

        // ---- 4) 기타 유용한 컬렉션 ----
        Console.WriteLine("\n== 기타 ==");
        var deq = new LinkedList<int>();
        deq.AddFirst(1);
        deq.AddLast(2);
        deq.AddFirst(0);
        Console.WriteLine($"  LinkedList(양방향): [{string.Join(",", deq)}]");

        var bag = new HashSet<int> { 1, 2, 2, 3 }; // 중복 제거
        Console.WriteLine($"  HashSet(중복 제거): [{string.Join(",", bag)}]");
    }
}
