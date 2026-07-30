Imports System
Imports System.IO

Module Program
    Sub Main()
        Dim filePath As String = "sample.txt"
        Dim csvPath As String = "data.csv"
        Dim copyPath As String = "sample_copy.txt"

        ' 텍스트 파일 쓰기
        Dim lines As String() = {
            "첫 번째 줄",
            "두 번째 줄",
            "세 번째 줄"
        }
        File.WriteAllLines(filePath, lines)
        Console.WriteLine($"파일 생성: {filePath}")

        ' 텍스트 파일 읽기
        Console.WriteLine("--- 파일 읽기 (ReadAllLines) ---")
        Dim readLines = File.ReadAllLines(filePath)
        For Each line In readLines
            Console.WriteLine($"  {line}")
        Next

        ' StreamWriter (추가 모드)
        Using writer As New StreamWriter(filePath, True)
            writer.WriteLine("네 번째 줄 (추가)")
        End Using
        Console.WriteLine("--- 파일 읽기 (StreamReader) ---")
        Using reader As New StreamReader(filePath)
            Dim line As String
            Do
                line = reader.ReadLine()
                If line Is Nothing Then Exit Do
                Console.WriteLine($"  {line}")
            Loop
        End Using

        ' CSV 파일 처리
        Dim data As String() = {
            "이름,나이,도시",
            "Alice,25,Seoul",
            "Bob,30,Busan",
            "Charlie,22,Incheon"
        }
        File.WriteAllLines(csvPath, data)

        Console.WriteLine("--- CSV 파싱 ---")
        Dim csvLines = File.ReadAllLines(csvPath)
        For i As Integer = 1 To csvLines.Length - 1
            Dim fields = csvLines(i).Split(","c)
            Console.WriteLine($"  이름: {fields(0)}, 나이: {fields(1)}, 도시: {fields(2)}")
        Next

        ' 파일 정보
        Dim info As New FileInfo(filePath)
        Console.WriteLine($"파일 크기: {info.Length} bytes")
        Console.WriteLine($"수정 시간: {info.LastWriteTime}")

        ' 파일 복사/삭제
        File.Copy(filePath, copyPath, True)
        Console.WriteLine($"복사 완료: {copyPath}")
        File.Delete(copyPath)
        Console.WriteLine($"삭제 완료: {copyPath}")

        ' 임시 파일 정리
        File.Delete(filePath)
        File.Delete(csvPath)
    End Sub
End Module
