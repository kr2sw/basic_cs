# 00 개발환경 설정

## 필수 도구

- **GCC (MinGW-w64)** - C 컴파일러
- **VS Code** (권장) 또는 다른 텍스트 에디터
- **CMake** (선택 사항)

## Windows 환경

### MinGW-w64 설치

1. [MSYS2](https://www.msys2.org/) 다운로드 및 설치
2. MSYS2 터미널에서 패키지 업데이트 및 GCC 설치:
```bash
pacman -Syu
pacman -S mingw-w64-ucrt-x86_64-gcc
```
3. `C:\msys64\ucrt64\bin`을 시스템 PATH에 추가
4. 확인: `gcc --version`

### 직접 설치 (scoop)
```bash
scoop install gcc
```

## macOS 환경

```bash
xcode-select --install
# 또는 Homebrew로 최신 GCC 설치
brew install gcc
```

## Linux 환경

```bash
sudo apt update
sudo apt install build-essential gcc gdb
```

## VS Code 확장

- **C/C++** (Microsoft) - IntelliSense, 디버깅
- **Code Runner** - 빠른 실행

## 컴파일 및 실행

```bash
# 컴파일
gcc main.c -o main

# 실행 (Windows)
main
# 또는
./main

# 실행 (macOS/Linux)
./main

# 디버그 정보 포함
gcc -g main.c -o main

# 경고 메시지 활성화
gcc -Wall -Wextra main.c -o main
```

## 디버깅 (GDB)

```bash
gcc -g main.c -o main
gdb ./main
```
