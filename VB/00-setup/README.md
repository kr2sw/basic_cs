# 00 개발환경 설정

## 필수 도구

- **.NET SDK** 8.0 이상 (https://dotnet.microsoft.com/download)
- **IDE**: Visual Studio 2022+, VS Code, 또는 JetBrains Rider
- **VS Code 확장**: VB.NET ( 또는 .NET Extension Pack )

## .NET SDK 설치

```bash
# Windows (scoop)
scoop install dotnet-sdk

# macOS
brew install dotnet-sdk

# 설치 확인
dotnet --version
dotnet --list-sdks
```

## 프로젝트 생성

```bash
dotnet new console -lang vb -o MyApp
cd MyApp
dotnet run
```

## 실행

```bash
cd VB/01-hello-world
dotnet run
```

## VS Code 확장

- **VB.NET** (k--kato) - 문법 강조
- **.NET Extension Pack**
