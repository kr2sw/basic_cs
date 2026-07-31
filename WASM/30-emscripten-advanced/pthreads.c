#include <stdio.h>
#include <stdint.h>
#include <pthread.h>
#include <emscripten/emscripten.h>

// 공유 카운터 (스레드 동기화 데모)
static int counter = 0;
static pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

#define THREAD_COUNT 4
#define ITER_PER_THREAD 100000

// 각 스레드가 실행할 작업
static void *worker(void *arg) {
  int id = (int)(intptr_t)arg;

  for (int i = 0; i < ITER_PER_THREAD; i++) {
    pthread_mutex_lock(&mutex);
    counter++;
    pthread_mutex_unlock(&mutex);
  }

  printf("워커 %d 완료 (%d회 증가)\n", id, ITER_PER_THREAD);
  return NULL;
}

// 4개 스레드를 생성하고 모두 끝날 때까지 대기
EMSCRIPTEN_KEEPALIVE
int runThreads(void) {
  pthread_t threads[THREAD_COUNT];

  counter = 0;
  printf("스레드 %d개 생성 시작\n", THREAD_COUNT);

  for (int i = 0; i < THREAD_COUNT; i++) {
    pthread_create(&threads[i], NULL, worker, (void *)(intptr_t)i);
  }
  for (int i = 0; i < THREAD_COUNT; i++) {
    pthread_join(threads[i], NULL);
  }

  printf("최종 카운터: %d (기대값: %d)\n", counter, THREAD_COUNT * ITER_PER_THREAD);
  return counter;
}

// CPU 작업량 비교용: 소수 개수 세기
EMSCRIPTEN_KEEPALIVE
int countPrimes(int limit) {
  int count = 0;
  for (int n = 2; n <= limit; n++) {
    int isPrime = 1;
    for (int d = 2; d * d <= n; d++) {
      if (n % d == 0) { isPrime = 0; break; }
    }
    if (isPrime) count++;
  }
  return count;
}

int main(void) {
  printf("=== Emscripten 고급 (pthreads) 데모 ===\n");
  return 0;
}
