#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define DB_FILE "database.txt"
#define INIT_CAP 4

// --- 레코드 및 데이터베이스 구조 ---
typedef struct {
    int id;
    char name[32];
    double score;
} Record;

typedef struct {
    Record* records;
    int count;
    int capacity;
} Database;

// --- 초기화/정리 ---
void dbInit(Database* db) {
    db->capacity = INIT_CAP;
    db->count = 0;
    db->records = (Record*)malloc(db->capacity * sizeof(Record));
}

void dbFree(Database* db) {
    free(db->records);
    db->records = NULL;
    db->count = db->capacity = 0;
}

// --- 용량 확장 (필요 시 2배) ---
void dbEnsure(Database* db) {
    if (db->count >= db->capacity) {
        db->capacity *= 2;
        db->records = (Record*)realloc(db->records, db->capacity * sizeof(Record));
        printf("  [DB] 용량 확장 → %d\n", db->capacity);
    }
}

// --- CRUD ---
int dbInsert(Database* db, int id, const char* name, double score) {
    for (int i = 0; i < db->count; i++) {
        if (db->records[i].id == id) return 0;   // 중복 id 거부
    }
    dbEnsure(db);
    Record* r = &db->records[db->count++];
    r->id = id;
    strncpy(r->name, name, 31);
    r->name[31] = '\0';
    r->score = score;
    return 1;
}

Record* dbSelect(Database* db, int id) {
    for (int i = 0; i < db->count; i++) {
        if (db->records[i].id == id) return &db->records[i];
    }
    return NULL;
}

int dbUpdate(Database* db, int id, const char* name, double score) {
    Record* r = dbSelect(db, id);
    if (!r) return 0;
    strncpy(r->name, name, 31);
    r->name[31] = '\0';
    r->score = score;
    return 1;
}

int dbDelete(Database* db, int id) {
    for (int i = 0; i < db->count; i++) {
        if (db->records[i].id == id) {
            db->records[i] = db->records[db->count - 1];  // 맨 뒤와 교환
            db->count--;
            return 1;
        }
    }
    return 0;
}

void dbList(Database* db) {
    printf("  [레코드 %d개]\n", db->count);
    printf("  %-4s %-12s %s\n", "ID", "이름", "점수");
    for (int i = 0; i < db->count; i++) {
        printf("  %-4d %-12s %.1f\n",
               db->records[i].id, db->records[i].name, db->records[i].score);
    }
}

// --- 파일 저장/로드 (텍스트 형식) ---
int dbSave(Database* db, const char* path) {
    FILE* fp = fopen(path, "w");
    if (!fp) { perror("save 실패"); return 0; }
    for (int i = 0; i < db->count; i++) {
        fprintf(fp, "%d,%s,%.1f\n",
                db->records[i].id, db->records[i].name, db->records[i].score);
    }
    fclose(fp);
    return 1;
}

int dbLoad(Database* db, const char* path) {
    FILE* fp = fopen(path, "r");
    if (!fp) { perror("load 실패"); return 0; }

    char line[128];
    while (fgets(line, sizeof(line), fp)) {
        int id;
        char name[32];
        double score;
        if (sscanf(line, "%d,%31[^,],%lf", &id, name, &score) == 3) {
            dbInsert(db, id, name, score);
        }
    }
    fclose(fp);
    return 1;
}

int main(void) {
    printf("=== 미니 메모리 기반 DB (파일 저장) ===\n\n");

    Database db;
    dbInit(&db);

    printf("--- insert ---\n");
    dbInsert(&db, 1, "Alice", 95.5);
    dbInsert(&db, 2, "Bob", 82.0);
    dbInsert(&db, 3, "Carol", 88.7);
    dbInsert(&db, 4, "Dave", 76.3);
    dbInsert(&db, 5, "Eve", 91.2);   // 용량 확장 발생
    dbList(&db);

    printf("\n--- select (id=3) ---\n");
    Record* r = dbSelect(&db, 3);
    if (r) printf("  찾음: id=%d name=%s score=%.1f\n", r->id, r->name, r->score);

    printf("\n--- update (id=2 → Carol? 아니면 이름 변경) ---\n");
    dbUpdate(&db, 2, "Bobby", 99.9);
    dbList(&db);

    printf("\n--- delete (id=4) ---\n");
    printf("  삭제 결과: %s\n", dbDelete(&db, 4) ? "성공" : "실패");
    dbList(&db);

    printf("\n--- save → 파일 저장 ---\n");
    dbSave(&db, DB_FILE);
    printf("  %s 저장 완료\n", DB_FILE);

    printf("\n--- 파일 내용 확인 ---\n");
    FILE* fp = fopen(DB_FILE, "r");
    char line[128];
    while (fgets(line, sizeof(line), fp)) printf("  %s", line);
    fclose(fp);

    printf("\n--- load → 새 DB로 복원 ---\n");
    Database restored;
    dbInit(&restored);
    dbLoad(&restored, DB_FILE);
    dbList(&restored);

    dbFree(&db);
    dbFree(&restored);
    remove(DB_FILE);

    printf("\n※ 구조체 + 동적 배열 + 파일 I/O를 종합한 CRUD 시스템입니다.\n");
    return 0;
}
