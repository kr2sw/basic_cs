# 25: 트리 자료구조 — 이진 탐색 트리, 순회, AVL 균형

## 이진 탐색 트리 (BST)

왼쪽 자식 < 부모 < 오른쪽 자식 규칙을 만족하는 이진 트리입니다.

```c
typedef struct Node {
    int key;
    struct Node* left;
    struct Node* right;
} Node;
```

- 탐색/삽입/삭제 평균 O(log n)
- 균형이 무너지면 O(n)으로 퇴화 (→ AVL 필요)

## 순회 (Traversal)

```c
전위(preorder):  부모 → 왼쪽 → 오른쪽
중위(inorder):   왼쪽 → 부모 → 오른쪽   (정렬 순서 출력)
후위(postorder): 왼쪽 → 오른쪽 → 부모
```

## AVL 트리

높이 균형을 유지하는 자가 균형 BST입니다. 모든 노드의 균형 인자가 ±1 이내여야 합니다.

```c
int balance = height(node->left) - height(node->right);  // 2 이상이면 회전
```

- **LL** (오른쪽 회전), **RR** (왼쪽 회전), **LR**, **RL** 네 가지 회전
- 삽입/삭제 후 균형이 깨지면 회전으로 복구 → 항상 O(log n) 보장
- 트리 높이를 노드에 저장해 균형 인자를 O(1)로 계산

## 실행

```bash
gcc main.c -o main && ./main
```
