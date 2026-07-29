namespace BasicCS.Chapter17;

using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

// ──────────────────────────────────────────────
// 데이터 계약(DataContract) 예제용 클래스
// ──────────────────────────────────────────────
[DataContract(Name = "PersonData", Namespace = "http://basiccs.ch17")]
public class PersonData
{
    [DataMember(Name = "FullName", Order = 1)]
    public string? Name { get; set; }

    [DataMember(Order = 2)]
    public int Age { get; set; }

    [DataMember(Order = 3)]
    public string? Email { get; set; }

    [IgnoreDataMember]
    public string? InternalNotes { get; set; }
}

// ──────────────────────────────────────────────
// XML 직렬화 예제용 클래스 (public 기본 생성자 필요)
// ──────────────────────────────────────────────
public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    // XmlSerializer는 기본 생성자가 필요
    public Product() { }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

// ──────────────────────────────────────────────
// 순환 참조 예제용 클래스
// ──────────────────────────────────────────────
public class Parent
{
    public string? Name { get; set; }
    public Child? Child { get; set; }
}

public class Child
{
    public string? Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Parent? Parent { get; set; }
}

// ──────────────────────────────────────────────
// System.Text.Json용 일반 POCO
// ──────────────────────────────────────────────
public class Student
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<string> Subjects { get; set; } = new();
}

internal class Serialization
{
    static void Main()
    {
        Console.WriteLine("=== 직렬화(Serialization) 예제 ===");
        Console.WriteLine("기본 인코딩: UTF-8\n");

        // ──────────────────────────────────────────────
        // 1. System.Text.Json — JsonSerializer
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 1. System.Text.Json 기본 직렬화/역직렬화 ───");

        var student = new Student
        {
            Id = 101,
            Name = "김철수",
            Subjects = new List<string> { "수학", "영어", "과학" }
        };

        // 직렬화 (기본 옵션)
        string jsonDefault = JsonSerializer.Serialize(student);
        Console.WriteLine($"기본 JSON:\n{jsonDefault}\n");

        // 역직렬화
        Student? deserialized = JsonSerializer.Deserialize<Student>(jsonDefault);
        Console.WriteLine($"역직렬화: Id={deserialized?.Id}, Name={deserialized?.Name}, 과목 수={deserialized?.Subjects.Count}\n");

        // ──────────────────────────────────────────────
        // 2. JsonSerializerOptions (WriteIndented, PropertyNamingPolicy)
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 2. JsonSerializerOptions 활용 ───");

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        string jsonPretty = JsonSerializer.Serialize(student, options);
        Console.WriteLine($"들여쓰기 + camelCase JSON:\n{jsonPretty}");

        // camelCase JSON을 대소문자 구분 없이 역직렬화
        string camelJson = """{"id":201,"name":"이영희","subjects":["역사","지리"]}""";
        Student? student2 = JsonSerializer.Deserialize<Student>(camelJson, options);
        Console.WriteLine($"역직렬화(camelCase): Id={student2?.Id}, Name={student2?.Name}\n");

        // ──────────────────────────────────────────────
        // 3. XML Serialization (XmlSerializer)
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 3. XML Serialization ───");

        var product = new Product(1, "노트북", 1500000m);

        var xmlSerializer = new XmlSerializer(typeof(Product));
        using (var ms = new MemoryStream())
        using (var writer = new StreamWriter(ms, Encoding.UTF8))
        {
            xmlSerializer.Serialize(writer, product);
            string xml = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine($"XML 출력:\n{xml}");
        }

        // XML 역직렬화
        string xmlData = """<Product><Id>2</Id><Name>마우스</Name><Price>25000</Price></Product>""";
        using (var reader = new StringReader(xmlData))
        {
            Product? product2 = xmlSerializer.Deserialize(reader) as Product;
            Console.WriteLine($"XML 역직렬화: Id={product2?.Id}, Name={product2?.Name}, Price={product2?.Price}\n");
        }

        // ──────────────────────────────────────────────
        // 4. DataContract / DataMember 특성
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 4. DataContract / DataMember ───");

        var person = new PersonData
        {
            Name = "홍길동",
            Age = 30,
            Email = "hong@example.com",
            InternalNotes = "내부 메모 (직렬화 제외)"
        };

        // DataContractSerializer를 사용한 직렬화
        var dcs = new DataContractSerializer(typeof(PersonData));
        using (var ms = new MemoryStream())
        {
            dcs.WriteObject(ms, person);
            string dataContractXml = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine($"DataContract XML:\n{dataContractXml}");

            // 역직렬화
            ms.Position = 0;
            PersonData? person2 = dcs.ReadObject(ms) as PersonData;
            Console.WriteLine($"역직렬화: Name={person2?.Name}, Age={person2?.Age}");
            Console.WriteLine($"InternalNotes (null 예상): {person2?.InternalNotes ?? "(null)"}\n");
        }

        // ──────────────────────────────────────────────
        // 5. 순환 참조 처리
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 5. 순환 참조(Circular Reference) 처리 ───");

        var parent = new Parent { Name = "아버지" };
        var child = new Child { Name = "아들" };
        parent.Child = child;
        child.Parent = parent; // ← 순환 참조 발생

        try
        {
            // 기본 옵션으로 직렬화 → 예외 발생
            string cyclicJson = JsonSerializer.Serialize(parent);
            Console.WriteLine($"순환참조 직렬화: {cyclicJson}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"순환참조 오류 (예상됨): {ex.Message}");
        }

        // ReferenceHandler.IgnoreCycles 로 순환 참조 무시
        var safeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        string safeJson = JsonSerializer.Serialize(parent, safeOptions);
        Console.WriteLine($"순환참조 무시 후 직렬화:\n{safeJson}\n");

        Console.WriteLine("=== 직렬화 예제 종료 ===");
    }
}
