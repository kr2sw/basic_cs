using System.Text.Json;

namespace BasicCS.Chapter40;

// ---- 도메인 모델: 레코드 + enum ----
public enum TodoPriority { 낮음, 보통, 높음 }

public record Todo(int Id, string Title, TodoPriority Priority, bool IsDone)
{
    public Todo MarkDone() => this with { IsDone = true };
}

// ---- 파일 저장소: JSON 직렬화로 영속화 ----
public class TodoRepository
{
    private readonly string _filePath;
    private readonly List<Todo> _todos = new();
    private int _nextId = 1;

    public TodoRepository(string filePath)
    {
        _filePath = filePath;
        Load();
    }

    // 시작 시 파일에서 불러오기
    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var loaded = JsonSerializer.Deserialize<List<Todo>>(File.ReadAllText(_filePath));
            if (loaded is null) return;
            _todos.AddRange(loaded);
            _nextId = _todos.Count > 0 ? _todos.Max(t => t.Id) + 1 : 1;
            Console.WriteLine($"(기존 저장 데이터 {_todos.Count}건 불러옴)\n");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"(저장 파일 손상: {ex.Message})");
        }
    }

    // save/exit 시 파일로 저장
    public void Save()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_todos,
            new JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine("저장 완료: " + _filePath);
    }

    public void Add(string title, TodoPriority priority)
        => _todos.Add(new Todo(_nextId++, title, priority, false));

    public List<Todo> GetAll() => _todos.ToList();

    // LINQ로 완료 처리/삭제
    public bool MarkDone(int id)
    {
        var index = _todos.FindIndex(t => t.Id == id);
        if (index < 0) return false;
        _todos[index] = _todos[index].MarkDone();
        return true;
    }

    public bool Delete(int id)
    {
        int removed = _todos.RemoveAll(t => t.Id == id);
        return removed > 0;
    }
}

static class Program
{
    private static void PrintTodos(IEnumerable<Todo> todos)
    {
        if (!todos.Any())
        {
            Console.WriteLine("  (할일이 없습니다)");
            return;
        }

        foreach (var todo in todos)
        {
            string check = todo.IsDone ? "[X]" : "[ ]";
            string prio = todo.Priority switch
            {
                TodoPriority.높음 => "!!",
                TodoPriority.보통 => "!",
                _ => " ",
            };
            Console.WriteLine($"  {todo.Id,3}. {check} {todo.Title} {prio}");
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("""
            사용 가능한 명령:
              add <내용> [낮음|보통|높음]   할일 추가 (우선순위 기본: 보통)
              list                      전체 조회
              done <번호>                완료 처리
              del <번호>                 삭제
              sort                      우선순위 순 정렬
              save                      파일 저장
              exit                      저장 후 종료
            """);
    }

    static void Main(string[] args)
    {
        // 저장 위치: 프로젝트 루트의 todos.json
        var repo = new TodoRepository(Path.Combine(AppContext.BaseDirectory, "todos.json"));

        Console.WriteLine("=== 할일 관리 앱 (종합 프로젝트) ===");
        Console.WriteLine("help 를 입력하면 명령 도움말을 보여줍니다.\n");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            var parts = input.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();

            try
            {
                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;

                    case "add":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("사용법: add <내용> [우선순위]");
                            break;
                        }
                        var priority = TodoPriority.보통;
                        if (parts.Length == 3 && Enum.TryParse(parts[2], true, out TodoPriority parsed))
                            priority = parsed;
                        repo.Add(parts[1], priority);
                        Console.WriteLine($"추가됨: {parts[1]} ({priority})");
                        break;

                    case "list":
                        PrintTodos(repo.GetAll());
                        break;

                    case "done":
                        if (parts.Length < 2 || !int.TryParse(parts[1], out int doneId))
                        {
                            Console.WriteLine("사용법: done <번호>");
                            break;
                        }
                        Console.WriteLine(repo.MarkDone(doneId) ? "완료 처리됨." : "번호를 찾을 수 없습니다.");
                        break;

                    case "del":
                        if (parts.Length < 2 || !int.TryParse(parts[1], out int delId))
                        {
                            Console.WriteLine("사용법: del <번호>");
                            break;
                        }
                        Console.WriteLine(repo.Delete(delId) ? "삭제됨." : "번호를 찾을 수 없습니다.");
                        break;

                    case "sort":
                        var sorted = repo.GetAll().OrderByDescending(t => t.Priority);
                        PrintTodos(sorted);
                        break;

                    case "save":
                        repo.Save();
                        break;

                    case "exit":
                        repo.Save();
                        Console.WriteLine("종료합니다.");
                        return;

                    default:
                        Console.WriteLine("알 수 없는 명령입니다. 'help' 를 입력하세요.");
                        break;
                }
            }
            catch (Exception ex)
            {
                // 입력/파일 오류를 우아하게 처리
                Console.WriteLine($"오류 발생: {ex.Message}");
            }
        }
    }
}
