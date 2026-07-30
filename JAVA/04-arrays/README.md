# 04: Arrays — 배열

## 배열 선언과 초기화

```java
int[] arr = new int[5];          // 크기만 지정
int[] arr2 = {1, 2, 3, 4, 5};    // 선언과 동시에 초기화
int[] arr3 = new int[]{1, 2, 3}; // new와 함께 초기화
```

## 다차원 배열

```java
int[][] matrix = new int[3][4];
int[][] matrix2 = {{1,2}, {3,4}, {5,6}};
```

## Arrays 클래스

`java.util.Arrays`는 배열 조작을 위한 유틸리티 메서드를 제공합니다:
- `sort()`, `binarySearch()`, `copyOf()`, `fill()`, `equals()`, `toString()`, `asList()`
