#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

// --- 1. 문자열 빌더 (StringBuilder) ---
typedef struct {
    char* buf;
    size_t len;
    size_t cap;
} StringBuilder;

void sbInit(StringBuilder* sb, size_t cap) {
    sb->buf = (char*)malloc(cap);
    sb->buf[0] = '\0';
    sb->len = 0;
    sb->cap = cap;
}

void sbDestroy(StringBuilder* sb) {
    free(sb->buf);
    sb->buf = NULL;
    sb->len = sb->cap = 0;
}

void sbAppend(StringBuilder* sb, const char* s) {
    size_t need = sb->len + strlen(s) + 1;
    if (need > sb->cap) {                    // 용량 부족 → 2배씩 확장
        while (sb->cap < need) sb->cap *= 2;
        sb->buf = (char*)realloc(sb->buf, sb->cap);
    }
    strcpy(sb->buf + sb->len, s);
    sb->len += strlen(s);
}

void sbAppendChar(StringBuilder* sb, char c) {
    char tmp[2] = {c, '\0'};
    sbAppend(sb, tmp);
}

// --- 2. 토크나이저 (delimiter 기반 분리) ---
// strtok는 원본을 수정하므로 복사본을 만들어 사용
void tokenize(const char* input, const char* delims) {
    char* copy = (char*)malloc(strlen(input) + 1);
    strcpy(copy, input);

    int count = 0;
    char* tok = strtok(copy, delims);
    while (tok) {
        printf("  토큰[%d]: \"%s\"\n", count++, tok);
        tok = strtok(NULL, delims);
    }
    free(copy);
}

// --- 3. 정규화 ---
char* trim(char* s) {
    while (*s && isspace((unsigned char)*s)) s++;   // 앞 공백 제거
    char* end = s + strlen(s);
    while (end > s && isspace((unsigned char)end[-1])) end--;  // 뒤 공백 제거
    *end = '\0';
    return s;
}

void normalize(char* out, size_t cap, const char* s) {
    size_t w = 0;
    int inSpace = 0;
    for (const char* p = s; *p && w + 1 < cap; p++) {
        if (isspace((unsigned char)*p)) {
            if (!inSpace) { out[w++] = ' '; inSpace = 1; }  // 연속 공백은 1개로
        } else {
            out[w++] = (char)tolower((unsigned char)*p);    // 소문자 통일
            inSpace = 0;
        }
    }
    if (w > 0 && out[w - 1] == ' ') w--;   // 끝 공백 제거
    out[w] = '\0';
}

int main() {
    printf("=== 1. 문자열 빌더 ===\n");
    StringBuilder sb;
    sbInit(&sb, 8);   // 작게 시작
    sbAppend(&sb, "C 언어");
    sbAppend(&sb, " 동적 문자열");
    for (int i = 0; i < 3; i++) sbAppendChar(&sb, '!');
    printf("결과: %s (길이=%zu, 용량=%zu)\n", sb.buf, sb.len, sb.cap);
    sbDestroy(&sb);

    printf("\n=== 2. 토큰화 ===\n");
    const char* line = "name=alice,age=30,city=seoul";
    printf("입력: %s\n", line);
    tokenize(line, ",=");

    printf("\n=== 3. 정규화 ===\n");
    char raw[] = "   Hello   World,   C Programming   ";
    printf("원본: \"%s\"\n", raw);
    printf("trim: \"%s\"\n", trim(raw));

    char norm[128];
    normalize(norm, sizeof(norm), "   Hello   WORLD   C Prog  ");
    printf("정규화: \"%s\"\n", norm);

    printf("\n※ strtok는 정적 상태를 쓰므로 스레드 안전하지 않습니다.\n");
    printf("  스레드 환경에서는 strtok_r(POSIX) 또는 strtok_s(C11)를 사용하세요.\n");
    return 0;
}
