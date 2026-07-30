Imports System
Imports System.Globalization

Module Program
    Sub Main()
        ' 현재 날짜/시간
        Dim now As Date = Date.Now
        Console.WriteLine($"Now: {now}")
        Console.WriteLine($"Today: {Date.Today}")
        Console.WriteLine($"UtcNow: {Date.UtcNow}")

        ' 날짜 구성 요소
        Console.WriteLine($"Year: {now.Year}")
        Console.WriteLine($"Month: {now.Month}")
        Console.WriteLine($"Day: {now.Day}")
        Console.WriteLine($"Hour: {now.Hour}")
        Console.WriteLine($"Minute: {now.Minute}")
        Console.WriteLine($"Second: {now.Second}")
        Console.WriteLine($"DayOfWeek: {now.DayOfWeek}")

        ' 날짜 생성
        Dim specific As New Date(2024, 12, 25, 10, 30, 0)
        Console.WriteLine($"Specific: {specific}")

        ' 날짜 연산
        Console.WriteLine($"내일: {now.AddDays(1)}")
        Console.WriteLine($"한 달 후: {now.AddMonths(1)}")
        Console.WriteLine($"1년 전: {now.AddYears(-1)}")

        ' TimeSpan
        Dim start As New Date(2024, 1, 1)
        Dim [end] As New Date(2024, 12, 31)
        Dim diff As TimeSpan = [end] - start
        Console.WriteLine($"총 일수: {diff.TotalDays}일")
        Console.WriteLine($"총 시간: {diff.TotalHours}시간")

        ' 서식 출력
        Console.WriteLine($"yyyy-MM-dd: {now.ToString("yyyy-MM-dd")}")
        Console.WriteLine($"yyyy-MM-dd HH:mm: {now:yyyy-MM-dd HH:mm}")
        Console.WriteLine($"long date: {now.ToString("D")}")
        Console.WriteLine($"short date: {now.ToString("d")}")

        ' 문자열 → 날짜 변환
        Dim dateStr As String = "2024-03-15"
        Dim parsed As Date = Date.Parse(dateStr)
        Console.WriteLine($"Parsed: {parsed}")

        ' TryParse
        Dim input As String = "2024/13/01"
        Dim result As Date
        If Date.TryParse(input, result) Then
            Console.WriteLine($"변환 성공: {result}")
        Else
            Console.WriteLine($"변환 실패: {input}")
        End If

        ' Exact 파싱
        Dim exact = Date.ParseExact("15/03/2024", "dd/MM/yyyy", Nothing)
        Console.WriteLine($"Exact: {exact}")
    End Sub
End Module
