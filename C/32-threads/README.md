# 32: 스레드 — pthread 개념, 스레드 동기화 (Windows 스레드 참고 포함)

## 스레드란?

한 프로세스 안에서 동시에 실행되는 실행 흐름입니다. 스레드들은 **같은 메모리를 공유**하므로 데이터 동기화가 필수입니다.

## pthread (POSIX)

```c
#include <pthread.h>

void* worker(void* arg) { /* ... */ return NULL; }

pthread_t t;
pthread_create(&t, NULL, worker, &data);   // 스레드 생성
pthread_join(t, NULL);                     // 종료 대기
```

- **Windows**: `CreateThread` / `_beginthreadex` 사용. MinGW에서도 pthread는 별도 라이브러리 필요

## 동기화: 뮤텍스 (Mutex)

공유 자원을 보호하는 잠금입니다.

```c
pthread_mutex_t mtx = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_lock(&mtx);      // 임계 구역 진입
balance += amount;             // 공유 자원 수정
pthread_mutex_unlock(&mtx);    // 임계 구역 탈출
```

- **경쟁 상태(race condition)**: 잠금 없이 여러 스레드가 공유 데이터를 수정하면 값이 틀어짐

## 기타 동기화 도구

| 도구 | 용도 |
|------|------|
| mutex | 상호 배제 (임계 구역) |
| semaphore | 허용 개수 제한 (생산자-소비자) |
| condition variable | 조건 충족 대기/알림 |
| atomic | 개별 연산을 원자적으로 실행 |

## 생산자-소비자 패턴

- **생산자**: 데이터를 버퍼에 넣음
- **소비자**: 버퍼에서 데이터를 꺼냄
- 버퍼가 가득/비었을 때를 조건 변수로 처리

본 강의 main.c는 표준 C만 사용합니다 (pthread 예제는 주석으로 제공).

## 실행

```bash
gcc main.c -o main && ./main
```
