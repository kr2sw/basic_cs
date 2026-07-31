#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILE_NAME "records.dat"

typedef struct {
    int id;
    char name[20];
    double score;
} Record;

// 레코드 3개를 바이너리 파일에 쓰기
int writeRecords(void) {
    Record records[] = {
        {1, "Alice", 95.5},
        {2, "Bob",   82.0},
        {3, "Carol", 88.7}
    };
    FILE* fp = fopen(FILE_NAME, "wb");
    if (!fp) { perror("fopen(wb)"); return 0; }
    fwrite(records, sizeof(Record), 3, fp);
    fclose(fp);
    return 1;
}

// 파일 전체를 순서대로 읽기
void readAll(void) {
    FILE* fp = fopen(FILE_NAME, "rb");
    if (!fp) { perror("fopen(rb)"); return; }
    Record r;
    while (fread(&r, sizeof(Record), 1, fp) == 1) {
        printf("id=%d name=%s score=%.1f\n", r.id, r.name, r.score);
    }
    fclose(fp);
}

// n번째 레코드를 랜덤 접근으로 읽기
int readAt(int idx) {
    FILE* fp = fopen(FILE_NAME, "rb");
    if (!fp) { perror("fopen"); return 0; }
    // 0번째 레코드부터 idx * 레코드크기 만큼 건너뜀
    fseek(fp, idx * sizeof(Record), SEEK_SET);
    Record r;
    int ok = (fread(&r, sizeof(Record), 1, fp) == 1);
    if (ok) {
        printf("[랜덤 접근] 레코드[%d]: id=%d name=%s score=%.1f\n",
               idx, r.id, r.name, r.score);
    }
    fclose(fp);
    return ok;
}

// 파일 크기 구하기 (SEEK_END + ftell)
long fileSize(void) {
    FILE* fp = fopen(FILE_NAME, "rb");
    if (!fp) return -1;
    fseek(fp, 0, SEEK_END);
    long size = ftell(fp);
    fclose(fp);
    return size;
}

// 레코드 2번의 score 수정 (읽기-수정-쓰기)
void updateRecord(int idx, double newScore) {
    FILE* fp = fopen(FILE_NAME, "r+b");
    if (!fp) { perror("fopen(r+b)"); return; }
    fseek(fp, idx * sizeof(Record), SEEK_SET);
    Record r;
    if (fread(&r, sizeof(Record), 1, fp) == 1) {
        r.score = newScore;
        fseek(fp, idx * sizeof(Record), SEEK_SET);  // 다시 처음으로 이동
        fwrite(&r, sizeof(Record), 1, fp);
        printf("레코드[%d] score를 %.1f로 수정\n", idx, newScore);
    }
    fclose(fp);
}

int main() {
    printf("=== 1. 바이너리 쓰기/읽기 ===\n");
    if (!writeRecords()) return 1;
    readAll();

    printf("\n=== 2. 랜덤 접근 (fseek/ftell) ===\n");
    long size = fileSize();
    printf("파일 크기: %ld 바이트 (레코드 %ld개)\n", size, size / (long)sizeof(Record));
    readAt(0);
    readAt(2);

    printf("\n=== 3. 레코드 수정 ===\n");
    updateRecord(1, 99.9);
    readAt(1);

    printf("\n=== 4. 버퍼링 (setvbuf) ===\n");
    FILE* out = fopen("buf_test.txt", "w");
    if (out) {
        char buf[4096];
        setvbuf(out, buf, _IOFBF, sizeof(buf));  // 큰 버퍼로 성능 향상
        for (int i = 0; i < 10; i++) {
            fprintf(out, "라인 %d\n", i);
        }
        fclose(out);
        printf("buf_test.txt 작성 완료 (fclose 시 버퍼 flush)\n");
    }

    printf("\n※ 바이너리 파일은 구조체 패딩 때문에 이식성이 낮습니다.\n");
    printf("  크로스 플랫폼이 필요하면 명시적 직렬화를 사용하세요.\n");
    remove(FILE_NAME);
    remove("buf_test.txt");
    return 0;
}
