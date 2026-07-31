#include <stdio.h>
#include <stdlib.h>

// --- 이진 탐색 트리 노드 ---
typedef struct Node {
    int key;
    struct Node* left;
    struct Node* right;
    int height;          // AVL 높이
} Node;

Node* createNode(int key) {
    Node* n = (Node*)malloc(sizeof(Node));
    n->key = key;
    n->left = NULL;
    n->right = NULL;
    n->height = 1;
    return n;
}

int max(int a, int b) { return a > b ? a : b; }
int getHeight(Node* n) { return n ? n->height : 0; }
int getBalance(Node* n) {
    return n ? getHeight(n->left) - getHeight(n->right) : 0;
}

Node* rotateRight(Node* y) {
    Node* x = y->left;
    Node* T2 = x->right;
    x->right = y;
    y->left = T2;
    y->height = 1 + max(getHeight(y->left), getHeight(y->right));
    x->height = 1 + max(getHeight(x->left), getHeight(x->right));
    return x;
}

Node* rotateLeft(Node* x) {
    Node* y = x->right;
    Node* T2 = y->left;
    y->left = x;
    x->right = T2;
    x->height = 1 + max(getHeight(x->left), getHeight(x->right));
    y->height = 1 + max(getHeight(y->left), getHeight(y->right));
    return y;
}

// AVL 삽입 (균형 유지 포함)
Node* insertAVL(Node* node, int key) {
    if (!node) return createNode(key);
    if (key < node->key)      node->left = insertAVL(node->left, key);
    else if (key > node->key) node->right = insertAVL(node->right, key);
    else return node;   // 중복 무시

    node->height = 1 + max(getHeight(node->left), getHeight(node->right));

    int bal = getBalance(node);
    if (bal > 1 && key < node->left->key)       return rotateRight(node);   // LL
    if (bal < -1 && key > node->right->key)     return rotateLeft(node);    // RR
    if (bal > 1 && key > node->left->key) {                                 // LR
        node->left = rotateLeft(node->left);
        return rotateRight(node);
    }
    if (bal < -1 && key < node->right->key) {                               // RL
        node->right = rotateRight(node->right);
        return rotateLeft(node);
    }
    return node;
}

// 최소값 노드 찾기
Node* findMin(Node* node) {
    while (node->left) node = node->left;
    return node;
}

// AVL 삭제 (균형 유지 포함)
Node* deleteAVL(Node* root, int key) {
    if (!root) return root;
    if (key < root->key)      root->left = deleteAVL(root->left, key);
    else if (key > root->key) root->right = deleteAVL(root->right, key);
    else {
        if (!root->left || !root->right) {
            Node* temp = root->left ? root->left : root->right;
            if (!temp) { free(root); return NULL; }
            *root = *temp;
            free(temp);
        } else {
            Node* min = findMin(root->right);
            root->key = min->key;
            root->right = deleteAVL(root->right, min->key);
        }
    }
    root->height = 1 + max(getHeight(root->left), getHeight(root->right));
    int bal = getBalance(root);

    if (bal > 1 && getBalance(root->left) >= 0) return rotateRight(root);
    if (bal > 1 && getBalance(root->left) < 0) {
        root->left = rotateLeft(root->left);
        return rotateRight(root);
    }
    if (bal < -1 && getBalance(root->right) <= 0) return rotateLeft(root);
    if (bal < -1 && getBalance(root->right) > 0) {
        root->right = rotateRight(root->right);
        return rotateLeft(root);
    }
    return root;
}

Node* search(Node* root, int key) {
    if (!root || root->key == key) return root;
    if (key < root->key) return search(root->left, key);
    return search(root->right, key);
}

void inorder(Node* n) {
    if (!n) return;
    inorder(n->left);
    printf("%d ", n->key);
    inorder(n->right);
}

void preorder(Node* n) {
    if (!n) return;
    printf("%d ", n->key);
    preorder(n->left);
    preorder(n->right);
}

void postorder(Node* n) {
    if (!n) return;
    postorder(n->left);
    postorder(n->right);
    printf("%d ", n->key);
}

void freeTree(Node* n) {
    if (!n) return;
    freeTree(n->left);
    freeTree(n->right);
    free(n);
}

int main() {
    printf("=== AVL 트리 (자가 균형 BST) ===\n\n");

    Node* root = NULL;
    // 정렬된 순서로 삽입해도 AVL이 균형을 유지하는지 확인
    int keys[] = {10, 20, 30, 40, 50, 25};
    for (int i = 0; i < 6; i++) {
        root = insertAVL(root, keys[i]);
    }

    printf("중위 순회 (정렬 확인): ");
    inorder(root);
    printf("\n전위 순회 (루트 우선): ");
    preorder(root);
    printf("\n후위 순회 (자식 우선): ");
    postorder(root);
    printf("\n");
    printf("트리 높이: %d (노드 6개 → 최적 높이 ≈ 2~3)\n", getHeight(root));

    printf("\n탐색: %s\n", search(root, 40) ? "40 존재" : "40 없음");
    printf("탐색: %s\n", search(root, 99) ? "99 존재" : "99 없음");

    printf("\n삭제 (30 삭제 후 중위 순회): ");
    root = deleteAVL(root, 30);
    inorder(root);
    printf("\n높이: %d\n", getHeight(root));

    freeTree(root);
    printf("\n※ BST와 비교: AVL은 삽입/삭제 후 회전으로 항상 O(log n)을 보장합니다.\n");
    return 0;
}
