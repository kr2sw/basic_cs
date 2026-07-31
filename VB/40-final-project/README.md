# 40: 종합 프로젝트 — 콘솔 할일 관리 앱 (파일 저장)

## 소개

기초·중급 과정에서 배운 내용을 종합해 **할일(Todo) 관리 콘솔 앱**을 만듭니다. 모델 클래스, 저장소(파일 I/O + JSON), 메뉴 기반 콘솔 UI를 분리해 구현합니다. 학습한 기술: 컬렉션, LINQ, 예외 처리, 파일 I/O, System.Text.Json.

## 요구사항

- 할일 추가 / 목록 보기 / 완료 처리 / 삭제 / 종료
- 프로그램을 닫아도 데이터 유지 → JSON 파일로 저장/로드
- 손상된 저장 파일에 대비한 예외 처리

## 설계

### 1. 모델 — TodoItem

```vb
Public Class TodoItem
    Public Property Id As Integer
    Public Property Text As String
    Public Property IsDone As Boolean
    Public Property CreatedAt As DateTime
End Class
```

### 2. 저장소 — TodoRepository

파일 로드/저장을 담당합니다. `System.Text.Json`으로 직렬화하고, 파일이 없거나 손상되면 빈 목록을 반환합니다.

```vb
Public Class TodoRepository
    Public Function Load() As List(Of TodoItem)
        If Not File.Exists(_filePath) Then Return New List(Of TodoItem)()
        ...
    End Function

    Public Sub Save(items As List(Of TodoItem))
        File.WriteAllText(_filePath, JsonSerializer.Serialize(items, options))
    End Sub
End Class
```

### 3. UI — 메뉴 루프

`While` 루프로 메뉴를 반복 표시하고 `Select Case`로 동작을 분기합니다.

```vb
While True
    ShowMenu()
    Dim choice = Console.ReadLine()
    Select Case choice
        Case "1" : AddTodo(...)
        Case "2" : ListItems(...)
        ...
        Case "5" : Exit While
    End Select
End While
```

### 4. 사용자 입력 안전 처리

`Integer.TryParse`로 잘못된 입력에 대비합니다.

```vb
Dim idx = 0
Integer.TryParse(Console.ReadLine(), idx)
If idx >= 1 AndAlso idx <= items.Count Then ...
```

## 실행

```bash
dotnet run
```

## 정리

- 관심사를 모델 / 저장소 / UI로 분리했습니다.
- JSON 직렬화로 데이터를 파일에 저장·복원합니다.
- 예외와 잘못된 입력을 안전하게 처리했습니다.
- 이 구조는 이후 GUI(WinForms/WPF)나 웹 버전으로 확장할 수 있습니다.
