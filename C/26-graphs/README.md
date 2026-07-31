# 26: 그래프 — 인접 리스트/행렬, DFS, BFS, 최단경로

## 그래프 표현

```c
// 인접 행렬: n x n bool 배열
int adjMat[5][5] = {0};          // adjMat[u][v] = 1 이면 간선 존재

// 인접 리스트: 각 정점의 연결 목록
typedef struct GNode {
    int vertex, weight;
    struct GNode* next;
} GNode;
```

| 표현 | 장점 | 단점 |
|------|------|------|
| 인접 행렬 | 간선 확인 O(1), 구현 쉬움 | 메모리 O(V²) |
| 인접 리스트 | 메모리 O(V+E), 희소 그래프에 유리 | 간선 확인 O(degree) |

## 탐색

- **DFS (깊이 우선)**: 재귀/스택. 한 길로 끝까지 간 뒤 되돌아옴
- **BFS (너비 우선)**: 큐. 시작점에서 가까운 정점부터 방문 → 최단 거리 계산에 유용

```c
void dfs(int v) { visited[v] = 1; for (각 인접 정점) dfs(u); }
void bfs(int s) { 큐에 넣고, while(!큐.empty) { 빼서 방문, 인접 정점 추가 } }
```

## 최단 경로 (다익스트라)

가중치가 있는 그래프에서 한 정점에서 모든 정점까지의 최단 거리를 구합니다. 음수 가중치가 없을 때 사용합니다.

- `dist[]` 초기값: 시작점 0, 나머지 무한대
- 미방문 정점 중 dist가 최소인 정점을 골라 간선 완화(relaxation)
- 복잡도 O(V²) (우선순위 큐 사용 시 O(E log V))

## 실행

```bash
gcc main.c -o main && ./main
```
