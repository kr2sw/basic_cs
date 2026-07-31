# 37: 파서 — 토크나이저, 표현식 계산기, 재귀 하강 파서

## 토크나이저 (Tokenizer)

입력 문자열을 의미 있는 토큰 단위로 쪼갭니다.

```c
typedef enum { TOK_NUM, TOK_PLUS, TOK_MINUS, TOK_MUL, TOK_DIV, TOK_LPAREN, TOK_RPAREN, TOK_END } TokenType;
typedef struct { TokenType type; double value; } Token;
```

- 공백은 건너뛰고, 숫자/연산자/괄호를 인식
- 파서의 입력이 되는 "단어"를 만드는 역할

## 재귀 하강 파서 (Recursive Descent)

문법 규칙에 따라 함수를 재귀 호출하는 하향식 파서입니다. 문법과 구조가 1:1로 대응되어 구현하기 쉽습니다.

```
expr   := term   (('+'|'-') term)*
term   := factor (('*'|'/') factor)*
factor := NUMBER | '(' expr ')'
```

- **expr**: 덧셈/뺄셈 (우선순위 낮음)
- **term**: 곱셈/나눗셈 (우선순위 높음)
- **factor**: 숫자 또는 괄호 (재귀)

덧셈보다 곱셈을 더 안쪽 함수에서 파싱하므로 자연스럽게 연산자 우선순위가 적용됩니다.

- 재귀 호출 깊이가 깊어지면 스택 오버플로 위험 → 반복 버전도 고려
- 오류 복구(error recovery)를 추가하면 실전용 파서가 됩니다

## 계산기 동작

`2 + 3 * 4` → 토큰화 → 파싱 → `14` (괄호/우선순위 처리)

## 실행

```bash
gcc main.c -o main && ./main
```
