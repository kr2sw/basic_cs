# 00 개발환경 설정

## 필수 도구

- **Visual Studio 2022** 이상 (Community 무료) — WPF 워크로드 필요
- **.NET SDK** 8.0 / 10.0 이상 (https://dotnet.microsoft.com/download)
  - C#: .NET 8.0 대상 / VB.NET: .NET 10.0 대상

## Visual Studio 설치

Visual Studio Installer 실행 후 **.NET 데스크톱 개발** 워크로드 선택 → WPF 자동 포함

## CLI로 프로젝트 생성

```bash
# C# WPF 앱
dotnet new wpf -lang c# -n MyApp

# VB.NET WPF 앱
dotnet new wpf -lang vb -n MyApp
```

## 프로젝트 구조

```
MyApp/
├── MyApp.csproj    # 프로젝트 파일 (.vbproj for VB)
├── App.xaml         # 애플리케이션 정의
├── App.xaml.cs      # App 코드-비하인드 (.vb)
└── MainWindow.xaml  # 메인 윈도우 XAML
└── MainWindow.xaml.cs  # 윈도우 코드-비하인드 (.vb)
```

## 실행

```bash
cd WPF/01-hello-world/csharp
dotnet run
```
