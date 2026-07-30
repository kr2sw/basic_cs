#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main() {
    // 파일 생성
    FILE* fp = fopen("/output.txt", "w");
    if (fp) {
        fprintf(fp, "WASI 파일 입출력 예제\n");
        fprintf(fp, "현재 시간: %ld\n", time(NULL));
        fclose(fp);
        printf("파일이 생성되었습니다.\n");
    }

    // 파일 읽기
    fp = fopen("/output.txt", "r");
    if (fp) {
        char buffer[256];
        while (fgets(buffer, sizeof(buffer), fp)) {
            printf("%s", buffer);
        }
        fclose(fp);
    }

    printf("WASI 프로그램이 종료됩니다.\n");
    return 0;
}
