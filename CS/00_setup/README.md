# 00 개발환경 설정

## 필수 도구

- **.NET SDK** (https://dotnet.microsoft.com/download)
- **IDE**: Visual Studio, Visual Studio Code, 또는 JetBrains Rider

## .NET SDK 설치

### Windows (scoop)
```bash
scoop install dotnet-sdk
```

### Windows (직접)
1. https://dotnet.microsoft.com/download 방문
2. .NET SDK 설치 관리자 다운로드 및 실행

### macOS
```bash
brew install dotnet-sdk
```

### Linux
```bash
# Ubuntu/Debian
sudo apt update && sudo apt install dotnet-sdk-8.0
```

### 설치 확인
```bash
dotnet --version
dotnet --list-sdks
```

## 프로젝트 생성 및 실행

```bash
# 콘솔 애플리케이션 생성
dotnet new console -n MyApp
cd MyApp

# 실행
dotnet run

# 빌드
dotnet build

# 게시 (릴리스)
dotnet publish -c Release
```

## VS Code 확장

- **C# Dev Kit** (Microsoft) - IntelliSense, 디버깅
- **.NET Extension Pack**

## 솔루션 파일 (.slnx)

이 저장소의 C# 프로젝트는 `basic_cs.slnx` 솔루션 파일로 관리됩니다.

```bash
# 전체 솔루션 빌드
dotnet build basic_cs.slnx

# 특정 프로젝트 실행
dotnet run --project 01_hello_world/Ch01_HelloWorld.csproj
```
