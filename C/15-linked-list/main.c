#include <stdio.h>
#include <stdlib.h>

typedef struct Node {
    int data;
    struct Node* next;
} Node;

// 새 노드 생성
Node* createNode(int data) {
    Node* newNode = (Node*)malloc(sizeof(Node));
    if (!newNode) return NULL;
    newNode->data = data;
    newNode->next = NULL;
    return newNode;
}

// 맨 앞에 삽입
void insertFront(Node** head, int data) {
    Node* newNode = createNode(data);
    if (!newNode) return;
    newNode->next = *head;
    *head = newNode;
}

// 맨 뒤에 삽입
void insertBack(Node** head, int data) {
    Node* newNode = createNode(data);
    if (!newNode) return;

    if (*head == NULL) {
        *head = newNode;
        return;
    }

    Node* current = *head;
    while (current->next) {
        current = current->next;
    }
    current->next = newNode;
}

// 특정 값 삭제
void deleteValue(Node** head, int data) {
    if (*head == NULL) return;

    Node* current = *head;
    Node* prev = NULL;

    // 첫 노드가 대상
    if (current->data == data) {
        *head = current->next;
        free(current);
        return;
    }

    // 탐색
    while (current && current->data != data) {
        prev = current;
        current = current->next;
    }

    if (current) {
        prev->next = current->next;
        free(current);
    }
}

// 탐색
Node* search(Node* head, int data) {
    Node* current = head;
    while (current) {
        if (current->data == data) return current;
        current = current->next;
    }
    return NULL;
}

// 리스트 길이
int length(Node* head) {
    int count = 0;
    while (head) {
        count++;
        head = head->next;
    }
    return count;
}

// 리스트 출력
void printList(Node* head) {
    printf("Head");
    while (head) {
        printf(" -> %d", head->data);
        head = head->next;
    }
    printf(" -> NULL\n");
}

// 리스트 역순
void reverse(Node** head) {
    Node* prev = NULL;
    Node* current = *head;
    Node* next = NULL;

    while (current) {
        next = current->next;
        current->next = prev;
        prev = current;
        current = next;
    }
    *head = prev;
}

// 리스트 메모리 해제
void freeList(Node** head) {
    Node* current = *head;
    while (current) {
        Node* temp = current;
        current = current->next;
        free(temp);
    }
    *head = NULL;
}

int main() {
    Node* head = NULL;

    printf("=== 연결 리스트 ===\n\n");

    // 삽입
    printf("insertFront: 30, 20, 10\n");
    insertFront(&head, 30);
    insertFront(&head, 20);
    insertFront(&head, 10);
    printList(head);
    printf("길이: %d\n\n", length(head));

    printf("insertBack: 40, 50\n");
    insertBack(&head, 40);
    insertBack(&head, 50);
    printList(head);

    // 삭제
    printf("\ndeleteValue: 20\n");
    deleteValue(&head, 20);
    printList(head);

    printf("\ndeleteValue: 10 (처음)\n");
    deleteValue(&head, 10);
    printList(head);

    printf("\ndeleteValue: 50 (마지막)\n");
    deleteValue(&head, 50);
    printList(head);

    // 탐색
    printf("\nsearch(40): ");
    Node* found = search(head, 40);
    if (found) printf("찾음 (%d)\n", found->data);
    else printf("없음\n");

    printf("search(99): ");
    found = search(head, 99);
    if (found) printf("찾음\n");
    else printf("없음\n");

    // 역순
    printf("\n=== 역순 ===\n");
    insertBack(&head, 10);
    insertBack(&head, 20);
    insertBack(&head, 30);
    printf("원본: ");
    printList(head);

    reverse(&head);
    printf("역순: ");
    printList(head);

    // 정리
    freeList(&head);
    printf("\n리스트 메모리 해제 완료\n");

    return 0;
}
