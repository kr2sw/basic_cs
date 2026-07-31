#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include <string.h>

// --- 1. 토크나이저 ---
typedef enum {
    TOK_NUM, TOK_PLUS, TOK_MINUS, TOK_MUL, TOK_DIV,
    TOK_LPAREN, TOK_RPAREN, TOK_END, TOK_ERROR
} TokenType;

typedef struct {
    TokenType type;
    double value;   // TOK_NUM일 때만 유효
} Token;

typedef struct {
    const char* src;
    int pos;
} Lexer;

void lexerSkipSpace(Lexer* lx) {
    while (lx->src[lx->pos] && isspace((unsigned char)lx->src[lx->pos])) {
        lx->pos++;
    }
}

Token lexerNext(Lexer* lx) {
    lexerSkipSpace(lx);
    char c = lx->src[lx->pos];
    if (c == '\0') return (Token){TOK_END, 0};

    if (isdigit((unsigned char)c) || c == '.') {
        char* end;
        double v = strtod(lx->src + lx->pos, &end);
        lx->pos = (int)(end - lx->src);
        return (Token){TOK_NUM, v};
    }
    lx->pos++;
    switch (c) {
        case '+': return (Token){TOK_PLUS, 0};
        case '-': return (Token){TOK_MINUS, 0};
        case '*': return (Token){TOK_MUL, 0};
        case '/': return (Token){TOK_DIV, 0};
        case '(': return (Token){TOK_LPAREN, 0};
        case ')': return (Token){TOK_RPAREN, 0};
        default:  return (Token){TOK_ERROR, 0};
    }
}

// --- 2. 재귀 하강 파서 (평가 즉시 계산) ---
typedef struct {
    Lexer lx;
    Token lookahead;   // 현재 토큰
} Parser;

void parserAdvance(Parser* p) {
    p->lookahead = lexerNext(&p->lx);
}

void parserInit(Parser* p, const char* src) {
    p->lx.src = src;
    p->lx.pos = 0;
    parserAdvance(p);
}

// grammer:
//   expr   := term   (('+'|'-') term)*
//   term   := factor (('*'|'/') factor)*
//   factor := NUMBER | '(' expr ')'

double parseExpr(Parser* p);

double parseFactor(Parser* p) {
    if (p->lookahead.type == TOK_NUM) {
        double v = p->lookahead.value;
        parserAdvance(p);
        return v;
    }
    if (p->lookahead.type == TOK_LPAREN) {
        parserAdvance(p);                      // '(' 소비
        double v = parseExpr(p);               // 재귀: 괄호 안 표현식
        if (p->lookahead.type != TOK_RPAREN) {
            printf("오류: ')' 기대\n");
            return 0;
        }
        parserAdvance(p);                      // ')' 소비
        return v;
    }
    printf("오류: 숫자 또는 '(' 기대\n");
    return 0;
}

double parseTerm(Parser* p) {
    double left = parseFactor(p);
    while (p->lookahead.type == TOK_MUL || p->lookahead.type == TOK_DIV) {
        TokenType op = p->lookahead.type;
        parserAdvance(p);
        double right = parseFactor(p);
        if (op == TOK_MUL) left *= right;
        else left /= right;
    }
    return left;
}

double parseExpr(Parser* p) {
    double left = parseTerm(p);
    while (p->lookahead.type == TOK_PLUS || p->lookahead.type == TOK_MINUS) {
        TokenType op = p->lookahead.type;
        parserAdvance(p);
        double right = parseTerm(p);
        if (op == TOK_PLUS) left += right;
        else left -= right;
    }
    return left;
}

double evaluate(const char* expr) {
    Parser p;
    parserInit(&p, expr);
    double result = parseExpr(&p);
    if (p.lookahead.type != TOK_END) {
        printf("오류: 표현식 끝에 남은 토큰 있음\n");
    }
    return result;
}

// --- 3. 토큰 리스트 출력 (파싱 전 미리보기) ---
void printTokens(const char* expr) {
    Lexer lx = {expr, 0};
    printf("토큰 스트림: ");
    Token t;
    do {
        t = lexerNext(&lx);
        switch (t.type) {
            case TOK_NUM: printf("[수 %g] ", t.value); break;
            case TOK_PLUS: printf("[+] "); break;
            case TOK_MINUS: printf("[-] "); break;
            case TOK_MUL: printf("[*] "); break;
            case TOK_DIV: printf("[/] "); break;
            case TOK_LPAREN: printf("[(] "); break;
            case TOK_RPAREN: printf("[)] "); break;
            case TOK_END: printf("[END]"); break;
            default: printf("[?]"); break;
        }
    } while (t.type != TOK_END && t.type != TOK_ERROR);
    printf("\n");
}

int main(void) {
    printf("=== 표현식 파서 (토크나이저 + 재귀 하강) ===\n\n");

    const char* exprs[] = {
        "2 + 3 * 4",            // 14 (우선순위)
        "(2 + 3) * 4",          // 20 (괄호)
        "10 - 2 * 3 + 1",       // 5
        "100 / (2 * 5)",        // 10
        "3.5 * 2 + 1.5"         // 8.5
    };

    for (int i = 0; i < 5; i++) {
        printf("%-18s = %g\n", exprs[i], evaluate(exprs[i]));
    }

    printf("\n토크나이저 동작:\n");
    printTokens("(2 + 3) * 4");

    printf("\n※ 파서의 3단계: 문자 → 토큰(어휘 분석) → AST/값(구문 분석)\n");
    printf("  여기서는 구문 분석과 평가를 동시에 수행했습니다.\n");
    return 0;
}
