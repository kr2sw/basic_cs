# 24: 고급 파일 I/O — 바이너리, 랜덤 접근(fseek), 버퍼링

## 바이너리 파일 입출력

구조체를 그대로 파일에 저장하고 읽습니다.

```c
fwrite(&rec, sizeof(Record), 1, fp);   // 쓰기
fread(&rec, sizeof(Record), 1, fp);    // 읽기
```

- `fread`/`fwrite`는 레코드 크기와 개수를 받음
- 반환값으로 실제 읽힌 개수를 확인해야 함
- 주의: 구조체 패딩(padding)이 저장되므로, 파일을 다른 프로그램과 공유하려면 직렬화가 필요

## 랜덤 접근 (fseek / ftell)

파일 포인터를 임의 위치로 이동합니다.

```c
fseek(fp, 0, SEEK_END);   // 파일 끝
long pos = ftell(fp);     // 현재 위치 (파일 크기)
fseek(fp, idx * sizeof(Record), SEEK_SET);  // idx번째 레코드로
```

## 버퍼링 (setvbuf)

```c
setvbuf(fp, NULL, _IOFBF, 8192);  // 전체 버퍼링
setvbuf(fp, NULL, _IOLBF, 1024);  // 줄 단위 버퍼링
setvbuf(fp, NULL, _IONBF, 0);     // 버퍼링 없음
```

- `_IOFBF`: 버퍼가 가득 찰 때 쓰기 (기본, 가장 빠름)
- `_IOLBF`: 개행 문자마다 쓰기 (터미널에 적합)
- `_IONBF`: 매번 즉시 쓰기 (느림)

## 오류 처리

```c
if (ferror(fp)) { perror("파일 오류"); clearerr(fp); }
```

## 실행

```bash
gcc main.c -o main && ./main
```
