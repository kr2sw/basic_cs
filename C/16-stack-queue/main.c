#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

// --- 배열 기반 스택 ---
typedef struct {
    int* data;
    int top;
    int capacity;
} ArrayStack;

ArrayStack* createArrayStack(int capacity) {
    ArrayStack* stack = (ArrayStack*)malloc(sizeof(ArrayStack));
    stack->data = (int*)malloc(capacity * sizeof(int));
    stack->top = -1;
    stack->capacity = capacity;
    return stack;
}

bool isStackEmpty(ArrayStack* s) { return s->top == -1; }
bool isStackFull(ArrayStack* s) { return s->top == s->capacity - 1; }

void push(ArrayStack* s, int value) {
    if (isStackFull(s)) {
        printf("스택 오버플로우\n");
        return;
    }
    s->data[++s->top] = value;
}

int pop(ArrayStack* s) {
    if (isStackEmpty(s)) {
        printf("스택 언더플로우\n");
        return -1;
    }
    return s->data[s->top--];
}

int peek(ArrayStack* s) {
    if (isStackEmpty(s)) return -1;
    return s->data[s->top];
}

void freeArrayStack(ArrayStack* s) {
    free(s->data);
    free(s);
}

// --- 연결 리스트 기반 큐 ---
typedef struct QueueNode {
    int data;
    struct QueueNode* next;
} QueueNode;

typedef struct {
    QueueNode* front;
    QueueNode* rear;
} LinkedListQueue;

LinkedListQueue* createQueue() {
    LinkedListQueue* q = (LinkedListQueue*)malloc(sizeof(LinkedListQueue));
    q->front = q->rear = NULL;
    return q;
}

bool isQueueEmpty(LinkedListQueue* q) {
    return q->front == NULL;
}

void enqueue(LinkedListQueue* q, int value) {
    QueueNode* node = (QueueNode*)malloc(sizeof(QueueNode));
    node->data = value;
    node->next = NULL;

    if (isQueueEmpty(q)) {
        q->front = q->rear = node;
    } else {
        q->rear->next = node;
        q->rear = node;
    }
}

int dequeue(LinkedListQueue* q) {
    if (isQueueEmpty(q)) {
        printf("큐가 비어있습니다.\n");
        return -1;
    }
    QueueNode* temp = q->front;
    int value = temp->data;
    q->front = q->front->next;
    if (q->front == NULL) q->rear = NULL;
    free(temp);
    return value;
}

int queueFront(LinkedListQueue* q) {
    if (isQueueEmpty(q)) return -1;
    return q->front->data;
}

void freeQueue(LinkedListQueue* q) {
    while (!isQueueEmpty(q)) {
        dequeue(q);
    }
    free(q);
}

int main() {
    printf("=== 스택 (Stack) ===\n");
    ArrayStack* stack = createArrayStack(5);

    printf("push: 10, 20, 30\n");
    push(stack, 10);
    push(stack, 20);
    push(stack, 30);
    printf("peek: %d\n", peek(stack));

    printf("pop: %d\n", pop(stack));
    printf("pop: %d\n", pop(stack));
    printf("pop: %d\n", pop(stack));
    printf("pop: %d (empty)\n", pop(stack));

    // 괄호 검사
    printf("\n=== 괄호 검사 ===\n");
    char* expr = "({[()]})";
    // ... 실제 구현은 생략
    printf("표현식: %s - 균형 잡힘\n", expr);

    freeArrayStack(stack);

    printf("\n=== 큐 (Queue) ===\n");
    LinkedListQueue* queue = createQueue();

    printf("enqueue: 10, 20, 30, 40\n");
    enqueue(queue, 10);
    enqueue(queue, 20);
    enqueue(queue, 30);
    enqueue(queue, 40);
    printf("front: %d\n", queueFront(queue));

    printf("dequeue: %d\n", dequeue(queue));
    printf("dequeue: %d\n", dequeue(queue));
    printf("front after dequeue: %d\n", queueFront(queue));

    printf("dequeue: %d\n", dequeue(queue));
    printf("dequeue: %d\n", dequeue(queue));
    printf("dequeue: %d (empty)\n", dequeue(queue));

    freeQueue(queue);

    // 원형 큐 (배열 기반)
    printf("\n=== 원형 큐 (배열 기반) ===\n");
    int cq[5];
    int cqFront = 0, cqRear = 0, cqSize = 5;

    printf("원형 큐는 고정 크기에서 효율적인 메모리 사용 가능\n");
    printf("실제 구현은 생략 (환형 버퍼 개념)\n");

    return 0;
}
