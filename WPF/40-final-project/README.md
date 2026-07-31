# 40: 종합 프로젝트 — MVVM + DI + 테마의 완성된 앱

## 학습 목표
- 이 과정에서 배운 패턴을 하나의 앱에 통합
- MVVM(커맨드·바인딩) + DI(35장) + 테마(31장) + 필터(29장) + 영속화
- "메모 앱"을 완성하고 확장 포인트 확인

## 아키텍처

```
NotesViewModel ──▶ INoteStore ──▶ JsonNoteStore (JSON 파일)
     │
     ├──▶ ObservableCollection<Note>  (목록)
     ├──▶ ICollectionView.Filter      (검색)
     └──▶ App.ApplyTheme()            (라이트/다크)
```

DI 컨테이너가 `NotesViewModel`을 만들 때 `INoteStore`를 주입합니다.

```csharp
var services = new ServiceCollection();
services.AddSingleton<INoteStore, JsonNoteStore>();
services.AddSingleton<NotesViewModel>();
var provider = services.BuildServiceProvider();
DataContext = provider.GetService<NotesViewModel>();
```

VB.NET:

```vb
Dim services As New ServiceCollection()
services.AddSingleton(Of INoteStore, JsonNoteStore)()
services.AddSingleton(Of NotesViewModel)()
Dim provider = services.BuildServiceProvider()
DataContext = provider.GetService(Of NotesViewModel)()
```

## 모델: 수정 시각 자동 갱신

`Note`는 값이 바뀔 때 `UpdatedAt`을 함께 갱신해 목록에 반영합니다.

```csharp
public string Title
{
    get => _title;
    set
    {
        if (_title != value)
        {
            _title = value;
            _updatedAt = DateTime.Now;
            OnPropertyChanged();
            OnPropertyChanged(nameof(UpdatedAt));
        }
    }
}
```

## 저장소: JSON 영속화

`System.Text.Json`(BCL)로 `LocalApplicationData` 폴더에 저장합니다.

```csharp
public class JsonNoteStore : INoteStore
{
    private readonly string _path = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "BasicCs", "notes.json");

    public List<Note> Load()
    {
        if (!File.Exists(_path)) return new List<Note>();
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
    }

    public void Save(IEnumerable<Note> notes)
    {
        var dir = Path.GetDirectoryName(_path);
        if (dir is not null) Directory.CreateDirectory(dir);
        File.WriteAllText(_path,
            JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true }));
    }
}
```

## 검색 필터

`ICollectionView.Filter`로 목록을 걸러냅니다. 검색어가 바뀔 때마다
`Refresh()`를 호출합니다.

```csharp
_view = CollectionViewSource.GetDefaultView(Notes);
_view.Filter = o =>
{
    if (SearchText.Length == 0) return true;
    var note = (Note)o;
    return note.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
           note.Body.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
};
```

## 테마 토글

```csharp
private void ToggleTheme()
{
    IsDark = !IsDark;
    App.ApplyTheme(IsDark ? "Themes/Dark.xaml" : "Themes/Light.xaml");
}
```

## 확장 포인트

- `INoteStore`를 SQLite/EF Core 구현으로 교체 (DI만 바꾸면 됨)
- `INavigationService`(33장)를 붙여 메모 상세 페이지 분리
- `AsyncRelayCommand`(26장)로 저장 시 자동 저장/동기화
- `AutomationId`(39장)를 추가해 UI 자동화 테스트 작성

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

## 정리

- 한 프로젝트에서 MVVM·DI·테마·필터·영속화를 모두 활용
- 인터페이스 분리 덕분에 저장소 교체가 쉬움
- 이 패턴들을 조합하면 실무 WPF 앱의 기본 골격이 완성
