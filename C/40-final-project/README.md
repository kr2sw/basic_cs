# 40: 종합 프로젝트 — 미니 메모리 기반 DB (파일 저장)

## 프로젝트 개요

지금까지 배운 **구조체, 동적 메모리, 파일 I/O, 문자열 처리**를 종합해 미니 DB를 만듭니다.

- 메모리에 레코드를 저장 (동적 배열)
- `insert`, `select`, `update`, `delete`, `list` 연산 제공
- `save`/`load`로 파일에 영속화

## 설계

```c
typedef struct {
    int id;
    char name[32];
    double score;
} Record;

typedef struct {
    Record* records;   // 동적 배열
    int count;
    int capacity;
} Database;
```

- 파일 형식: 한 줄에 레코드 하나 (`id,name,score`)
- 부족하면 `realloc`으로 용량을 2배 확장

## 연산

| 연산 | 설명 |
|------|------|
| `dbInsert` | 새 레코드 추가 |
| `dbSelect` | id로 검색 |
| `dbUpdate` | 이름/점수 수정 |
| `dbDelete` | id로 삭제 (맨 뒤와 교환 후 축소) |
| `dbList` | 전체 출력 |
| `dbSave` | 파일에 저장 |
| `dbLoad` | 파일에서 읽기 |

## 실행

```bash
gcc main.c -o main && ./main
```
