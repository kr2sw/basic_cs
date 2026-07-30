# 00 개발환경 설정

## 필수 도구

- **JDK** (Java Development Kit) 17 이상
- **IDE**: IntelliJ IDEA, VS Code, 또는 Eclipse

## JDK 설치

### Windows (scoop)
```bash
scoop install openjdk
```

### Windows (직접)
1. [Adoptium](https://adoptium.net/) 방문
2. JDK 17(LTS) 또는 21(LTS) 설치 관리자 다운로드 및 실행
3. `JAVA_HOME` 환경 변수 설정

### macOS
```bash
brew install openjdk@17
```

### Linux
```bash
sudo apt update
sudo apt install openjdk-17-jdk
```

### 설치 확인
```bash
java -version
javac -version
```

## 컴파일 및 실행

```bash
# 컴파일
javac Main.java

# 실행
java Main

# 한 번에 컴파일 + 실행
java Main.java  # Java 11+ 지원
```

## VS Code 확장

- **Extension Pack for Java** (Microsoft)
- **Debugger for Java**

## IntelliJ IDEA

1. https://www.jetbrains.com/idea/download/ - Community Edition 무료
2. 설치 후 프로젝트 열기 (Open) 또는 새 프로젝트 생성
3. JDK 경로 자동 인식
