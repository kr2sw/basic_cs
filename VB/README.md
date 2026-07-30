# Visual Basic .NET 기초 강의 (20개 챕터)

VB.NET은 .NET 플랫폼을 위한 현대적인 객체 지향 프로그래밍 언어입니다.

## 역사

Visual Basic은 1991년 Microsoft가 Alan Cooper의 "Tripod" 프로토타입을 기반으로 출시한 Visual Basic 1.0에서 시작되었습니다. RAD(Rapid Application Development) 도구로서 Windows 애플리케이션 개발을 혁신했으며, 드래그 앤 드롭 방식의 GUI 디자인과 이벤트 기반 프로그래밍을 대중화했습니다. VB 3.0(1993), VB 6.0(1998)까지 독자적인 생태계를 유지하다가, 2002년 .NET 플랫폼의 등장과 함께 Visual Basic .NET(VB 7.0)으로 완전히 재탄생했습니다. VB.NET은 완전한 객체 지향 언어로, C#과 동일한 .NET 런타임 위에서 실행되며, 언어 수준에서 비동기(async/await), LINQ, 제네릭 등 현대적인 기능을 지속적으로 도입하고 있습니다.

## 특징

- **영어와 같은 문법**: `If...Then...Else`, `For Each...Next`, `With...End With` 등 가독성 높은 자연어 스타일
- **대소문자 구분 없음**: 식별자의 대소문자를 구분하지 않아 코드 일관성 유지
- **Option Strict / Option Explicit**: 타입 안전성과 변수 선언 강제 설정 가능
- **My 네임스페이스**: 파일 시스템, 네트워크, 설정 등에 쉽게 접근할 수 있는 VB 전용 기능
- **.NET 완전 통합**: C#과 동일한 .NET 라이브러리, 런타임, 도구 체인 공유
- **COM 상호 운용성**: 레거시 COM/ActiveX 컨트롤과의 쉬운 통합
- **Windows Forms / WPF**: 데스크톱 애플리케이션 개발에 강력한 지원

## 실행

```bash
cd VB/01-hello-world && dotnet run
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 입출력, Module, Sub Main, 주석 |
| 02 | Variables | 데이터 타입, Dim/Const, 형변환, Option Strict |
| 03 | Control Flow | If/ElseIf/Else, Select Case, For/While/Do/Loop |
| 04 | Arrays & Collections | Array, ReDim, List(Of T), Dictionary(Of K,V) |
| 05 | Functions & Subs | Sub, Function, ByVal/ByRef, Optional, ParamArray |
| 06 | Strings | 문자열 함수, StringBuilder, 보간, Format |
| 07 | Classes & Objects | Class, Property, Constructor(Sub New), Shared |
| 08 | Inheritance | Inherits, Overridable/Overrides, MustInherit |
| 09 | Interfaces | Interface, Implements, 다중 인터페이스 |
| 10 | Exceptions | Try/Catch/Finally, Throw, 사용자 정의 예외 |
| 11 | LINQ | LINQ 쿼리 구문, 메서드 구문, Aggregate |
| 12 | Generics | 제네릭 클래스/메서드, Of T, 제약 조건 |
| 13 | File I/O | StreamReader/Writer, File, My.Computer.FileSystem |
| 14 | Date & Time | Date, TimeSpan, 날짜 연산, 서식 |
| 15 | Delegates & Events | Delegate, Event, RaiseEvent, Handles, AddHandler |
| 16 | Async & Await | Async/Await, Task, 비동기 프로그래밍 |
| 17 | XML & JSON | LINQ to XML, System.Text.Json |
| 18 | Database | ADO.NET, SqlConnection, SqlCommand |
| 19 | Reflection & Attributes | Attribute, Reflection, CallByName |
| 20 | Unit Testing | MSTest, Assert, DataRow, TestInitialize |
