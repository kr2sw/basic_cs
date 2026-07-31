#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>

#define V 6   // 정점 수

// --- 인접 행렬 기반 그래프 ---
int adjMat[V][V];

// --- 인접 리스트 기반 그래프 ---
typedef struct GNode {
    int vertex;
    int weight;
    struct GNode* next;
} GNode;

GNode* adjList[V];

void addEdgeList(int u, int v, int w) {
    GNode* n = (GNode*)malloc(sizeof(GNode));
    n->vertex = v;
    n->weight = w;
    n->next = adjList[u];
    adjList[u] = n;
}

// --- DFS (인접 리스트, 재귀) ---
void dfs(int v, int visited[]) {
    visited[v] = 1;
    printf("%d ", v);
    for (GNode* p = adjList[v]; p; p = p->next) {
        if (!visited[p->vertex]) {
            dfs(p->vertex, visited);
        }
    }
}

// --- BFS (인접 리스트, 큐) ---
typedef struct {
    int data[V];
    int head, tail;
} Queue;

void enqueue(Queue* q, int x) { q->data[q->tail++] = x; }
int dequeue(Queue* q) { return q->data[q->head++]; }
int queueEmpty(Queue* q) { return q->head == q->tail; }

void bfs(int start, int visited[]) {
    Queue q = {{0}, 0, 0};
    visited[start] = 1;
    enqueue(&q, start);
    while (!queueEmpty(&q)) {
        int v = dequeue(&q);
        printf("%d ", v);
        for (GNode* p = adjList[v]; p; p = p->next) {
            if (!visited[p->vertex]) {
                visited[p->vertex] = 1;
                enqueue(&q, p->vertex);
            }
        }
    }
}

// --- 다익스트라 (인접 행렬) ---
void dijkstra(int src) {
    int dist[V];
    int done[V] = {0};
    for (int i = 0; i < V; i++) dist[i] = INT_MAX;
    dist[src] = 0;

    for (int step = 0; step < V - 1; step++) {
        // 미방문 중 dist 최소인 정점 선택
        int u = -1, best = INT_MAX;
        for (int i = 0; i < V; i++) {
            if (!done[i] && dist[i] < best) { best = dist[i]; u = i; }
        }
        if (u == -1) break;
        done[u] = 1;
        for (int w = 0; w < V; w++) {           // 간선 완화
            if (adjMat[u][w] > 0 && !done[w] &&
                dist[u] + adjMat[u][w] < dist[w]) {
                dist[w] = dist[u] + adjMat[u][w];
            }
        }
    }

    printf("다익스트라 최단 경로 (시작: %d)\n", src);
    for (int i = 0; i < V; i++) {
        printf("  → %d: %s\n", i, dist[i] == INT_MAX ? "도달 불가" :
               (dist[i] == 0 ? "시작" : "거리"));
        if (dist[i] != INT_MAX && dist[i] != 0) {
            printf("          최단 거리: %d\n", dist[i]);
        }
    }
}

int main() {
    // 그래프 구성 (무방향 가중 그래프)
    //       1
    //   0 ----- 1
    //   | \4    |
    // 2 |  \    | 3
    //   |   3   |
    //   3 ----- 2
    int edges[][3] = {
        {0, 1, 1}, {0, 2, 2}, {0, 3, 4},
        {1, 2, 3}, {2, 3, 3}, {3, 4, 1}, {4, 5, 5}
    };
    int numEdges = sizeof(edges) / sizeof(edges[0]);

    for (int i = 0; i < numEdges; i++) {
        int u = edges[i][0], v = edges[i][1], w = edges[i][2];
        adjMat[u][v] = adjMat[v][u] = w;          // 인접 행렬
        addEdgeList(u, v, w);                      // 인접 리스트
        addEdgeList(v, u, w);
    }

    printf("=== 인접 행렬 ===\n");
    for (int i = 0; i < V; i++) {
        for (int j = 0; j < V; j++) {
            printf("%2d ", adjMat[i][j]);
        }
        printf("\n");
    }

    printf("\n=== DFS (시작 0) ===\n");
    int visited[V] = {0};
    dfs(0, visited);
    printf("\n");

    printf("\n=== BFS (시작 0) ===\n");
    memset(visited, 0, sizeof(visited));
    bfs(0, visited);
    printf("\n");

    printf("\n=== 최단 경로 ===\n");
    dijkstra(0);

    // 메모리 해제
    for (int i = 0; i < V; i++) {
        GNode* p = adjList[i];
        while (p) { GNode* t = p; p = p->next; free(t); }
    }
    return 0;
}
