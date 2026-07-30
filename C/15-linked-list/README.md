# 15: Linked List — 연결 리스트

## 단일 연결 리스트 (Singly Linked List)

각 노드는 데이터와 다음 노드를 가리키는 포인터로 구성됩니다.

```c
struct Node {
    int data;
    struct Node* next;
};
```

### 주요 연산

- 삽입 (처음/중간/끝)
- 삭제 (처음/중간/끝)
- 탐색
- 순회
- 역순
