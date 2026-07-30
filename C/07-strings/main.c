#include <stdio.h>
#include <string.h>

int main() {
    // 문자열 초기화
    char str1[] = "Hello";        // 자동 크기 (6 bytes)
    char str2[20] = "World";
    char str3[] = {'C', '+', '+', '\0'};  // 수동 NULL 종료

    printf("str1: %s (len: %zu)\n", str1, strlen(str1));
    printf("str2: %s\n", str2);
    printf("str3: %s\n", str3);

    // 문자열 복사
    char copy[20];
    strcpy(copy, str1);
    printf("strcpy: %s\n", copy);

    // 문자열 연결
    char result[50] = "";
    strcat(result, str1);
    strcat(result, " ");
    strcat(result, str2);
    printf("strcat: %s\n", result);

    // 문자열 비교
    char* a = "apple";
    char* b = "banana";
    int cmp = strcmp(a, b);
    printf("strcmp(\"%s\", \"%s\") = %d\n", a, b, cmp);
    // 음수: a < b, 0: 같음, 양수: a > b

    // 문자열 길이
    printf("strlen(\"Hello\") = %zu\n", strlen("Hello"));
    printf("sizeof(\"Hello\") = %zu\n", sizeof("Hello"));  // +1 for \0

    // 문자 찾기
    char* pos = strchr(result, 'o');
    if (pos) {
        printf("첫 'o' 위치: %ld (pos=%s)\n", pos - result, pos);
    }

    // 부분 문자열 찾기
    pos = strstr(result, "lo");
    if (pos) {
        printf("\"lo\" 위치: %ld\n", pos - result);
    }

    // 서식 문자열 생성
    char formatted[100];
    sprintf(formatted, "%s - %d세", "Alice", 25);
    printf("sprintf: %s\n", formatted);

    // 문자열 입력
    char input[100];
    printf("\n문자열 입력 (fgets): ");
    fgets(input, sizeof(input), stdin);
    input[strcspn(input, "\n")] = 0;  // 개행 제거
    printf("입력: %s\n", input);

    // 문자열 토큰화
    char text[] = "apple,banana,cherry,date";
    char* token = strtok(text, ",");
    printf("\nstrtok 분할:\n");
    while (token) {
        printf("  %s\n", token);
        token = strtok(NULL, ",");
    }

    // 대소문자 변환 (직접 구현)
    char mixed[] = "Hello C World";
    for (int i = 0; mixed[i]; i++) {
        if (mixed[i] >= 'a' && mixed[i] <= 'z')
            mixed[i] -= 32;  // 대문자로
    }
    printf("대문자: %s\n", mixed);

    return 0;
}
