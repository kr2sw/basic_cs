# 31: 프로세스 — fork/exec 개념, exit, 환경 변수 (Windows 참고)

## 프로세스란?

실행 중인 프로그램입니다. 각 프로세스는 독립된 메모리 공간(코드/데이터/스택/힙)을 가집니다.

## 프로세스 생성: fork / exec (POSIX)

```c
pid_t pid = fork();        // 현재 프로세스를 복제
if (pid == 0) {
    // 자식 프로세스
    execlp("ls", "ls", "-l", NULL);   // 완전히 다른 프로그램으로 교체
} else {
    wait(NULL);            // 부모는 자식 종료 대기
}
```

- `fork()`: 부모/자식으로 나뉘며 자식은 부모의 메모리 복사본을 가짐
- `exec`: 자식 프로세스의 이미지를 새 프로그램으로 교체
- **Windows에서는 fork 대신 `CreateProcess`(MSVC) 또는 MinGW의 `_spawn` 계열을 사용**

## 종료와 종료 코드

```c
exit(0);                 // 정상 종료 (atexit 콜백 실행, 버퍼 flush)
_EXIT(1);                // 즉시 종료 (정리 없음)
return EXIT_SUCCESS;     // 성공 0
return EXIT_FAILURE;     // 실패 1
```

- 부모는 `wait()`로 자식의 종료 상태를 얻을 수 있음

## 환경 변수

```c
getenv("PATH");   // 표준 C: 환경 변수 읽기
```

## Windows에서의 참고

- fork/exec 대신 `CreateProcess`, `WaitForSingleObject`
- 프로세스 대신 스레드가 가볍고 일반적 (`_beginthreadex`)
- 본 강의 main.c는 표준 C만 사용 (POSIX 예제는 주석으로 제공)

## 실행

```bash
gcc main.c -o main && ./main
```
