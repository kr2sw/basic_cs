Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text.Json

Module Program
    Sub Main()
        Console.WriteLine("=== 할일 관리 앱 (JSON 파일 저장) ===")

        Dim repository As New TodoRepository("todo.json")
        Dim items = repository.Load()

        While True
            ShowMenu()
            Console.Write("선택 > ")
            Dim choice = Console.ReadLine()

            Select Case choice
                Case "1"
                    Console.Write("할일 입력 > ")
                    Dim text = Console.ReadLine()
                    If Not String.IsNullOrWhiteSpace(text) Then
                        items.Add(New TodoItem() With {
                            .Text = text,
                            .CreatedAt = Date.Now
                        })
                        repository.Save(items)
                        Console.WriteLine("추가 완료!")
                    End If
                Case "2"
                    ListItems(items)
                Case "3"
                    ListItems(items, showIndex:=True)
                    Console.Write("완료 처리할 번호 > ")
                    Dim idx = 0
                    Integer.TryParse(Console.ReadLine(), idx)
                    If idx >= 1 AndAlso idx <= items.Count Then
                        items(idx - 1).IsDone = True
                        repository.Save(items)
                        Console.WriteLine("완료 처리!")
                    Else
                        Console.WriteLine("잘못된 번호입니다.")
                    End If
                Case "4"
                    ListItems(items, showIndex:=True)
                    Console.Write("삭제할 번호 > ")
                    Dim idx = 0
                    Integer.TryParse(Console.ReadLine(), idx)
                    If idx >= 1 AndAlso idx <= items.Count Then
                        items.RemoveAt(idx - 1)
                        repository.Save(items)
                        Console.WriteLine("삭제 완료!")
                    Else
                        Console.WriteLine("잘못된 번호입니다.")
                    End If
                Case "5"
                    Console.WriteLine("앱을 종료합니다.")
                    Exit While
                Case Else
                    Console.WriteLine("잘못된 입력입니다.")
            End Select
            Console.WriteLine()
        End While
    End Sub

    Sub ShowMenu()
        Console.WriteLine("1. 할일 추가")
        Console.WriteLine("2. 목록 보기")
        Console.WriteLine("3. 완료 처리")
        Console.WriteLine("4. 삭제")
        Console.WriteLine("5. 종료")
    End Sub

    Sub ListItems(items As List(Of TodoItem), Optional showIndex As Boolean = False)
        If items.Count = 0 Then
            Console.WriteLine("  (할일이 없습니다)")
            Return
        End If
        For i = 0 To items.Count - 1
            Dim item = items(i)
            Dim mark = If(item.IsDone, "[X]", "[ ]")
            Dim line = $"  {mark} {item.Text} ({item.CreatedAt:MM-dd})"
            If showIndex Then line = $"  {i + 1}. {line.Trim()}"
            Console.WriteLine(line)
        Next
    End Sub
End Module

' 모델: 할일 항목
Public Class TodoItem
    Public Property Id As Integer
    Public Property Text As String
    Public Property IsDone As Boolean
    Public Property CreatedAt As DateTime
End Class

' 저장소: JSON 파일로 저장/로드
Public Class TodoRepository
    Private ReadOnly _filePath As String

    Public Sub New(filePath As String)
        _filePath = filePath
    End Sub

    Public Function Load() As List(Of TodoItem)
        If Not File.Exists(_filePath) Then
            Return New List(Of TodoItem)()
        End If
        Try
            Dim json = File.ReadAllText(_filePath)
            Return JsonSerializer.Deserialize(Of List(Of TodoItem))(json) ?? New List(Of TodoItem)()
        Catch ex As JsonException
            Console.WriteLine($"저장 파일 손상: {ex.Message} → 새 목록으로 시작합니다")
            Return New List(Of TodoItem)()
        End Try
    End Function

    Public Sub Save(items As List(Of TodoItem))
        Dim options As New JsonSerializerOptions() With {.WriteIndented = True}
        File.WriteAllText(_filePath, JsonSerializer.Serialize(items, options))
        Console.WriteLine($"저장됨: {_filePath} ({items.Count}개 항목)")
    End Sub
End Class
