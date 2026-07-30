#include <stdio.h>
#include <string.h>

// 구조체 정의
struct Student {
    char name[50];
    int age;
    float gpa;
};

// typedef 사용
typedef struct {
    char title[100];
    char author[50];
    int year;
    int pages;
} Book;

// Point 구조체
typedef struct {
    int x;
    int y;
} Point;

// 구조체 반환 함수
Point createPoint(int x, int y) {
    Point p = {x, y};
    return p;
}

// 구조체 포인터 파라미터
void movePoint(Point* p, int dx, int dy) {
    p->x += dx;
    p->y += dy;
}

int main() {
    // 구조체 변수 선언 및 초기화
    struct Student s1 = {"Alice", 20, 3.8f};
    struct Student s2;
    strcpy(s2.name, "Bob");
    s2.age = 22;
    s2.gpa = 3.5f;

    printf("=== Student ===\n");
    printf("s1: %s, %d세, GPA %.2f\n", s1.name, s1.age, s1.gpa);
    printf("s2: %s, %d세, GPA %.2f\n", s2.name, s2.age, s2.gpa);

    // typedef 구조체
    Book books[2] = {
        {"C Programming", "Brian Kernighan", 1978, 272},
        {"The C Programming Language", "Dennis Ritchie", 1988, 288}
    };

    printf("\n=== Books ===\n");
    for (int i = 0; i < 2; i++) {
        printf("\"%s\" by %s (%d) - %d pages\n",
               books[i].title, books[i].author,
               books[i].year, books[i].pages);
    }

    // 구조체 포인터
    struct Student* ptr = &s1;
    printf("\n=== 구조체 포인터 ===\n");
    printf("name: %s (-> 연산자)\n", ptr->name);
    printf("age: %d\n", ptr->age);

    // 구조체 반환
    Point p1 = createPoint(3, 5);
    printf("\n=== Point ===\n");
    printf("p1: (%d, %d)\n", p1.x, p1.y);

    // 구조체 포인터로 수정
    movePoint(&p1, 2, -1);
    printf("after move: (%d, %d)\n", p1.x, p1.y);

    // 구조체 배열
    Point polygon[] = {{0, 0}, {10, 0}, {10, 10}, {0, 10}};
    int n = sizeof(polygon) / sizeof(polygon[0]);

    printf("\n=== Polygon ===\n");
    for (int i = 0; i < n; i++) {
        printf("  (%d, %d)\n", polygon[i].x, polygon[i].y);
    }

    // 중첩 구조체
    typedef struct {
        Point topLeft;
        Point bottomRight;
    } Rectangle;

    Rectangle rect = {{0, 0}, {100, 50}};
    printf("\n=== Rectangle ===\n");
    printf("TL: (%d, %d), BR: (%d, %d)\n",
           rect.topLeft.x, rect.topLeft.y,
           rect.bottomRight.x, rect.bottomRight.y);

    // 구조체 크기 (패딩 주의)
    printf("\n=== sizeof ===\n");
    printf("sizeof(Student): %zu\n", sizeof(struct Student));
    printf("sizeof(Book): %zu\n", sizeof(Book));
    printf("sizeof(Point): %zu\n", sizeof(Point));

    return 0;
}
