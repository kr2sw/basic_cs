#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// --- 1. 기본 캡슐화 (struct + 함수) ---
typedef struct {
    char name[50];
    int age;
    float balance;
} Account;

// 생성자
Account* accountCreate(const char* name, int age) {
    Account* acc = (Account*)malloc(sizeof(Account));
    strcpy(acc->name, name);
    acc->age = age;
    acc->balance = 0.0f;
    return acc;
}

// 소멸자
void accountDestroy(Account* acc) {
    free(acc);
}

// 메서드
void accountDeposit(Account* acc, float amount) {
    if (amount > 0) {
        acc->balance += amount;
        printf("%s: %.0f원 입금 (잔액: %.0f원)\n", acc->name, amount, acc->balance);
    }
}

void accountWithdraw(Account* acc, float amount) {
    if (amount > 0 && amount <= acc->balance) {
        acc->balance -= amount;
        printf("%s: %.0f원 출금 (잔액: %.0f원)\n", acc->name, amount, acc->balance);
    } else {
        printf("%s: 잔액 부족 (잔액: %.0f원)\n", acc->name, acc->balance);
    }
}

void accountPrint(Account* acc) {
    printf("[계좌] %s (%d세) - 잔액: %.0f원\n", acc->name, acc->age, acc->balance);
}

// --- 2. VTable 기반 다형성 ---
// Shape "클래스" (추상)
typedef struct Shape Shape;  // 전방 선언

typedef struct {
    double (*area)(const Shape*);
    void (*draw)(const Shape*);
    void (*destroy)(Shape*);
} ShapeVTable;

struct Shape {
    ShapeVTable* vtable;
    char color[20];
};

double shapeArea(const Shape* s) { return s->vtable->area(s); }
void shapeDraw(const Shape* s) { s->vtable->draw(s); }
void shapeDestroy(Shape* s) { s->vtable->destroy(s); }

// --- Circle "클래스" ---
typedef struct {
    Shape base;
    double radius;
} Circle;

double circleArea(const Shape* s) {
    const Circle* c = (const Circle*)s;
    return 3.14159 * c->radius * c->radius;
}

void circleDraw(const Shape* s) {
    const Circle* c = (const Circle*)s;
    printf("○ %s 원 (반지름: %.1f, 면적: %.2f)\n", c->base.color, c->radius, circleArea(s));
}

void circleDestroy(Shape* s) {
    printf("Circle 소멸\n");
    free(s);
}

ShapeVTable circleVTable = {circleArea, circleDraw, circleDestroy};

Circle* circleCreate(const char* color, double radius) {
    Circle* c = (Circle*)malloc(sizeof(Circle));
    strcpy(c->base.color, color);
    c->base.vtable = &circleVTable;
    c->radius = radius;
    return c;
}

// --- Rectangle "클래스" ---
typedef struct {
    Shape base;
    double width;
    double height;
} Rectangle;

double rectArea(const Shape* s) {
    const Rectangle* r = (const Rectangle*)s;
    return r->width * r->height;
}

void rectDraw(const Shape* s) {
    const Rectangle* r = (const Rectangle*)s;
    printf("▭ %s 직사각형 (%.1f x %.1f, 면적: %.2f)\n",
           r->base.color, r->width, r->height, rectArea(s));
}

void rectDestroy(Shape* s) {
    printf("Rectangle 소멸\n");
    free(s);
}

ShapeVTable rectVTable = {rectArea, rectDraw, rectDestroy};

Rectangle* rectCreate(const char* color, double width, double height) {
    Rectangle* r = (Rectangle*)malloc(sizeof(Rectangle));
    strcpy(r->base.color, color);
    r->base.vtable = &rectVTable;
    r->width = width;
    r->height = height;
    return r;
}

// --- 3. 상속 시뮬레이션 (구조체 포함) ---
typedef struct {
    Account base;  // Account를 포함 (상속과 유사)
    float interestRate;
} SavingsAccount;

SavingsAccount* savingsCreate(const char* name, int age, float rate) {
    SavingsAccount* sa = (SavingsAccount*)malloc(sizeof(SavingsAccount));
    strcpy(sa->base.name, name);
    sa->base.age = age;
    sa->base.balance = 0.0f;
    sa->interestRate = rate;
    return sa;
}

void savingsAddInterest(SavingsAccount* sa) {
    float interest = sa->base.balance * sa->interestRate / 100.0f;
    sa->base.balance += interest;
    printf("이자 %.0f원 추가 (이율: %.1f%%)\n", interest, sa->interestRate);
}

void savingsDestroy(SavingsAccount* sa) {
    free(sa);
}

int main() {
    printf("=== C에서 OOP 흉내내기 ===\n\n");

    // 1. 기본 캡슐화
    printf("--- 캡슐화 ---\n");
    Account* acc = accountCreate("Alice", 25);
    accountPrint(acc);
    accountDeposit(acc, 50000);
    accountWithdraw(acc, 20000);
    accountWithdraw(acc, 40000);  // 잔액 부족
    accountDestroy(acc);

    // 2. VTable 다형성
    printf("\n--- VTable 다형성 ---\n");
    Shape* shapes[] = {
        (Shape*)circleCreate("빨간", 5.0),
        (Shape*)rectCreate("파란", 4.0, 6.0),
        (Shape*)circleCreate("초록", 3.0)
    };
    int n = sizeof(shapes) / sizeof(shapes[0]);

    for (int i = 0; i < n; i++) {
        shapeDraw(shapes[i]);
        printf("  면적: %.2f\n", shapeArea(shapes[i]));
    }

    for (int i = 0; i < n; i++) {
        shapeDestroy(shapes[i]);
    }

    // 3. 상속 시뮬레이션
    printf("\n--- 상속 시뮬레이션 ---\n");
    SavingsAccount* sa = savingsCreate("Bob", 30, 3.5f);
    accountDeposit((Account*)sa, 100000);  // 부모 메서드 사용
    savingsAddInterest(sa);                // 자식 메서드
    accountPrint((Account*)sa);
    savingsDestroy(sa);

    printf("\n※ C에서 OOP를 완전히 구현하기는 어렵지만\n");
    printf("  캡슐화, 다형성, 상속을 흉내낼 수 있습니다.\n");

    return 0;
}
