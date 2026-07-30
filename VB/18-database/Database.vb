Imports System
Imports System.Data
Imports Microsoft.Data.SqlClient

Module Program
    Sub Main()
        ' 연결 문자열 (SQL Server LocalDB 예제)
        Dim connStr = "Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;"

        ' 연결 및 기본 작업
        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Console.WriteLine("데이터베이스 연결 성공!")

                ' 테이블 생성
                Dim createSql = "
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Students' AND xtype='U')
                    CREATE TABLE Students (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(50),
                        Score INT
                    )"
                Using cmd As New SqlCommand(createSql, conn)
                    cmd.ExecuteNonQuery()
                    Console.WriteLine("테이블 생성 완료")
                End Using

                ' 데이터 삽입 (매개변수화)
                Dim students = {
                    New With {.Name = "Alice", .Score = 95},
                    New With {.Name = "Bob", .Score = 87},
                    New With {.Name = "Charlie", .Score = 92}
                }

                For Each s In students
                    Using cmd As New SqlCommand(
                        "INSERT INTO Students (Name, Score) VALUES (@Name, @Score)", conn)
                        cmd.Parameters.AddWithValue("@Name", s.Name)
                        cmd.Parameters.AddWithValue("@Score", s.Score)
                        cmd.ExecuteNonQuery()
                    End Using
                Next
                Console.WriteLine("데이터 삽입 완료")

                ' 데이터 조회
                Using cmd As New SqlCommand("SELECT Id, Name, Score FROM Students", conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Console.WriteLine($"  #{reader("Id")} {reader("Name")}: {reader("Score")}")
                        End While
                    End Using
                End Using

                ' 데이터 정리
                Using cmd As New SqlCommand("DROP TABLE Students", conn)
                    cmd.ExecuteNonQuery()
                    Console.WriteLine("테이블 정리 완료")
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine($"데이터베이스 오류: {ex.Message}")
        End Try
    End Sub
End Module
