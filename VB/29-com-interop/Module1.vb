Imports System
Imports System.Runtime.InteropServices
Imports System.Threading

Module Program
    ' Win32 API P/Invoke: 비관리 코드 연동의 기초
    Declare Function GetTickCount Lib "kernel32.dll" () As UInteger

    Sub Main()
        Console.WriteLine("=== 1. P/Invoke (Win32 API 호출) ===")
        Dim start = GetTickCount()
        Thread.Sleep(10)
        Dim elapsed = GetTickCount() - start
        Console.WriteLine($"GetTickCount 경과 (밀리초): {elapsed}")

        Console.WriteLine()
        Console.WriteLine("=== 2. Marshal: 비관리 메모리 ===")
        Dim str = "Hello COM"
        Dim unmanagedPtr = Marshal.StringToHGlobalAnsi(str)
        Try
            Dim back = Marshal.PtrToStringAnsi(unmanagedPtr)
            Console.WriteLine($"  비관리 메모리 왕복: {back}")
        Finally
            Marshal.FreeHGlobal(unmanagedPtr)
        End Try

        Console.WriteLine()
        Console.WriteLine("=== 3. COM 인터페이스 개념 ===")
        Console.WriteLine("  (RCW / CreateObject 설명은 README 참고)")

        ' 실제 Excel COM 자동화 (Excel이 설치된 환경에서만 동작):
        '   Dim excel As Object = CreateObject("Excel.Application")
        '   excel.Visible = True
        '   excel.Workbooks.Add()
        '   excel.Cells(1, 1) = "안녕하세요"
        '   excel.Quit()
        '   Marshal.ReleaseComObject(excel)

        ' COM에서 인터페이스/클래스는 GUID로 식별됨
        Dim excelClsid = New Guid("00024500-0000-0000-C000-000000000046")  ' Excel.Application
        Console.WriteLine($"  Excel.Application CLSID: {excelClsid}")
        Console.WriteLine($"  해당 CLSID는 레지스트리에서 확인할 수 있습니다")
    End Sub
End Module
