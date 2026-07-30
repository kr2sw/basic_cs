# 08: Structs — 구조체

## 구조체 정의

```c
struct Student {
    char name[50];
    int age;
    float gpa;
};
```

## typedef

```c
typedef struct {
    char name[50];
    int age;
} Person;
```

## 구조체 접근

- `.` (멤버 접근 연산자): 일반 변수
- `->` (화살표 연산자): 포인터 변수
