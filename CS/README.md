# C# 기초 강의 (20개 챕터)

C#(C Sharp) 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

C#은 2000년 Microsoft의 Anders Hejlsberg가 .NET Framework의 주요 언어로 설계했습니다. C/C++와 Java의 강점을 결합하면서도 현대적인 기능(가비지 컬렉션, LINQ, async/await)을 추가했습니다. 2002년 .NET 1.0과 함께 첫 공개되었고, 2005년 C# 2.0(제네릭), 2007년 C# 3.0(LINQ), 2012년 C# 5.0(async/await), 2019년 C# 8.0(nullable reference types) 등으로 발전했습니다. 2014년 .NET이 오픈소스화되면서 크로스 플랫폼으로 확장되었습니다.

## 특징

- **객체 지향 + 함수형**: 클래스, 인터페이스, 람다, 패턴 매칭 등 멀티 패러다임
- **Type Safety**: 강력한 정적 타입 시스템, nullable reference types
- **LINQ (Language Integrated Query)**: 컬렉션, XML, 데이터베이스에 대한 일관된 질의 언어
- **async/await**: 비동기 프로그래밍을 언어 수준에서 지원
- **.NET 생태계**: 방대한 표준 라이브러리, NuGet 패키지 매니저
- **크로스 플랫폼**: .NET Core 이후 Windows, macOS, Linux 모두 지원
- **다양한 용도**: 웹(ASP.NET), 데스크톱(WPF/WinUI), 게임(Unity), 모바일(Xamarin/MAUI)

## 실행

```bash
cd CS/01_hello_world && dotnet run --project Ch01_HelloWorld.csproj
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 입출력, Main 메서드, 주석 |
| 02 | Variables | 변수, 데이터 타입, 형변환, var, 상수 |
| 03 | Control Flow | if/else, switch, for/foreach/while, break/continue |
| 04 | Arrays & Collections | 배열, List, Dictionary, Queue, Stack |
| 05 | Methods | 메서드, ref/out, 오버로딩, 선택적 매개변수 |
| 06 | Classes & Objects | 클래스, 생성자, this, static, 접근 제한자 |
| 07 | Inheritance | 상속, virtual/override, sealed, base |
| 08 | Interfaces | 인터페이스, 명시적 구현, 다중 상속 |
| 09 | Exceptions | try-catch-finally, using, 사용자 정의 예외 |
| 10 | LINQ | LINQ 쿼리, 메서드 체이닝, IEnumerable |
| 11 | Delegates & Events | delegate, event, Action/Func, lambda |
| 12 | Generics | 제네릭 클래스/메서드, where 제약 |
| 13 | async/await | Task, async/await, 비동기 패턴 |
| 14 | Strings | String, StringBuilder, 문자열 조작 |
| 15 | Date & Time | DateTime, TimeSpan, TimeOnly |
| 16 | File I/O & Streams | File, StreamReader/Writer, Serialization |
| 17 | Serialization | JSON, XML, BinarySerializer |
| 18 | Reflection & Attributes | Attribute, Reflection, 사용자 정의 Attribute |
| 19 | Networking | HttpClient, Socket, TCP/UDP |
| 20 | Unit Testing | xUnit/NUnit, Assert, Mock, TDD |
