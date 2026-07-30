#include <stdio.h>
#include <string.h>
#include <stdlib.h>

typedef struct {
    char name[50];
    int age;
    float score;
} Student;

int main() {
    const char* filename = "students.txt";
    const char* binaryFile = "students.bin";

    // --- 텍스트 파일 쓰기 ---
    printf("=== 텍스트 파일 쓰기 ===\n");
    FILE* fp = fopen(filename, "w");
    if (!fp) {
        perror("파일 열기 실패");
        return 1;
    }

    fprintf(fp, "%s %d %.1f\n", "Alice", 20, 95.5);
    fprintf(fp, "%s %d %.1f\n", "Bob", 22, 88.0);
    fprintf(fp, "%s %d %.1f\n", "Charlie", 21, 92.3);
    fclose(fp);
    printf("파일 쓰기 완료: %s\n", filename);

    // --- 텍스트 파일 읽기 (fscanf) ---
    printf("\n=== 텍스트 파일 읽기 (fscanf) ===\n");
    fp = fopen(filename, "r");
    if (!fp) {
        perror("파일 열기 실패");
        return 1;
    }

    char name[50];
    int age;
    float score;
    while (fscanf(fp, "%s %d %f", name, &age, &score) == 3) {
        printf("%s (%d세): %.1f점\n", name, age, score);
    }
    fclose(fp);

    // --- 텍스트 파일 읽기 (fgets) ---
    printf("\n=== 텍스트 파일 읽기 (fgets) ===\n");
    fp = fopen(filename, "r");
    char line[256];
    while (fgets(line, sizeof(line), fp)) {
        line[strcspn(line, "\n")] = 0;  // 개행 제거
        printf("라인: %s\n", line);
    }
    fclose(fp);

    // --- 바이너리 파일 쓰기 ---
    printf("\n=== 바이너리 파일 쓰기 ===\n");
    Student students[] = {
        {"Alice", 20, 95.5f},
        {"Bob", 22, 88.0f},
        {"Charlie", 21, 92.3f}
    };
    int count = sizeof(students) / sizeof(students[0]);

    fp = fopen(binaryFile, "wb");
    if (!fp) {
        perror("파일 열기 실패");
        return 1;
    }
    fwrite(students, sizeof(Student), count, fp);
    fclose(fp);
    printf("바이너리 파일 쓰기 완료: %s\n", binaryFile);

    // --- 바이너리 파일 읽기 ---
    printf("\n=== 바이너리 파일 읽기 ===\n");
    Student loaded[10];
    fp = fopen(binaryFile, "rb");
    if (!fp) {
        perror("파일 열기 실패");
        return 1;
    }
    int loadedCount = fread(loaded, sizeof(Student), 10, fp);
    fclose(fp);

    for (int i = 0; i < loadedCount; i++) {
        printf("%s (%d세): %.1f점\n",
               loaded[i].name, loaded[i].age, loaded[i].score);
    }

    // --- 파일 위치 제어 ---
    printf("\n=== 파일 위치 제어 ===\n");
    fp = fopen(filename, "r");
    fseek(fp, 0, SEEK_END);  // 파일 끝
    long fileSize = ftell(fp);  // 현재 위치 = 파일 크기
    printf("파일 크기: %ld bytes\n", fileSize);

    rewind(fp);  // 파일 처음으로
    fgets(line, sizeof(line), fp);
    printf("첫 줄: %s", line);
    fclose(fp);

    // 임시 파일 정리
    remove(filename);
    remove(binaryFile);
    printf("\n임시 파일 정리 완료\n");

    return 0;
}
