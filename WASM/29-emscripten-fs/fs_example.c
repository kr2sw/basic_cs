#include <stdio.h>
#include <string.h>
#include <emscripten/emscripten.h>

// MEMFS에 파일을 쓰고 다시 읽어 길이를 반환
EMSCRIPTEN_KEEPALIVE
int demoMemfs(void) {
  FILE *f = fopen("/hello.txt", "w");
  if (!f) return -1;

  fputs("Hello from MEMFS!\n", f);
  fclose(f);

  f = fopen("/hello.txt", "r");
  if (!f) return -2;

  char buf[128] = {0};
  char *got = fgets(buf, sizeof(buf), f);
  fclose(f);

  if (!got) return -3;
  return (int)strlen(buf);
}

// IDBFS 마운트 + 메모리의 /hello.txt를 IndexedDB로 저장
EMSCRIPTEN_KEEPALIVE
int persistToIdb(void) {
  EM_ASM({
    FS.mkdir('/persistent');
    FS.mount(IDBFS, {}, '/persistent');
    // /hello.txt를 /persistent/hello.txt로 복사
    FS.copyFile('/hello.txt', '/persistent/hello.txt');
    FS.syncfs(true, function(err) {
      if (err) {
        console.error('syncfs 저장 실패:', err);
      } else {
        console.log('IDBFS에 저장됨');
      }
    });
  });
  return 0;
}

// IDBFS에서 /persistent/hello.txt를 불러와 콘솔에 출력
EMSCRIPTEN_KEEPALIVE
int loadFromIdb(void) {
  EM_ASM({
    FS.mkdir('/persistent');
    FS.mount(IDBFS, {}, '/persistent');
    FS.syncfs(true, function(err) {
      if (err) {
        console.error('syncfs 복원 실패:', err);
        return;
      }
      try {
        var data = FS.readFile('/persistent/hello.txt', { encoding: 'utf8' });
        console.log('IDBFS에서 복원:', data);
      } catch (e) {
        console.log('저장된 파일 없음:', e.message);
      }
    });
  });
  return 0;
}

// 루트 디렉터리 목록을 콘솔에 출력
EMSCRIPTEN_KEEPALIVE
int listRoot(void) {
  EM_ASM({
    var names = FS.readdir('/');
    console.log('루트 디렉터리:', names.join(', '));
  });
  return 0;
}

int main(void) {
  printf("=== Emscripten FS 데모 시작 ===\n");
  int len = demoMemfs();
  printf("MEMFS로 쓰고 읽은 문자열 길이: %d\n", len);
  return 0;
}
