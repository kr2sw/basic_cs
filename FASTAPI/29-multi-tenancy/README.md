# 29: 멀티테넌시 — 테넌트 분리, 헤더 기반 라우팅

하나의 서비스로 여러 고객사(테넌트)의 데이터를 각각 안전하게 제공하는 설계를 **멀티테넌시**라고 합니다. 이번 챕터에서는 헤더 기반으로 테넌트를 식별하고 데이터를 격리합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

`X-Tenant-ID: acme` 헤더로 테넌트를 구분합니다. 테넌트가 다르면 같은 API여도 다른 데이터가 보입니다.

```bash
curl -H "X-Tenant-ID: acme" http://localhost:8000/notes
curl -H "X-Tenant-ID: globex" http://localhost:8000/notes
```

## 주요 개념

### 테넌트 식별 — 헤더 기반 라우팅

테넌트를 요청 헤더에 넣어 중앙에서 검증하는 패턴입니다. 의존성 주입으로 `get_tenant`를 만들어 모든 엔드포인트에서 사용합니다.

```python
def get_tenant(request: Request) -> str:
    tenant = request.headers.get("X-Tenant-ID")
    if not tenant:
        raise HTTPException(400, "X-Tenant-ID 헤더가 필요합니다")
    return tenant

@app.get("/notes")
def list_notes(tenant: str = Depends(get_tenant)):
    return store.list(tenant)   # 테넌트별로만 조회
```

테넌트 ID는 검증(화이트리스트/JWT 클레임)을 통과한 값만 사용해야 합니다.

### 데이터 격리 방식 3가지

| 방식 | 설명 | 장단점 |
|------|------|--------|
| **공유 DB + 테넌트 컬럼** | 모든 데이터에 `tenant_id` 컬럼 추가 | 관리 단순, 격리 위험(쿼리 누락 시 섞임) |
| **공유 DB + 별도 스키마** | 테넌트마다 스키마 분리 | 중간 수준 격리, migration 복잡 |
| **테넌트별 DB** | 테넌트마다 데이터베이스 분리 | 최고 격리, 비용/운영 부담 큼 |

이번 챕터 데모는 저장소를 테넌트 키로 분리한 구조입니다. SQLite 사용 시 테넌트별 파일(`tenant_acme.db`)로 동일한 패턴을 구현할 수 있습니다.

```python
def get_tenant_engine(tenant: str):
    return create_engine(f"sqlite:///./tenant_{tenant}.db")
```

### 고려할 점

- **인증과 결합**: JWT에 `tenant_id` 클레임을 넣어, 헤더 테넌트와 토큰 테넌트가 다르면 거부.
- **인덱스**: 공유 테이블 방식은 모든 쿼리에 `tenant_id` 조건 + 복합 인덱스.
- **프록시 문제**: Nginx 등에서 `X-Tenant-ID` 헤더를 수정하지 않도록 프록시 헤더 정리.

## 연습

1. `get_tenant`에 화이트리스트(`{"acme", "globex"}`) 검증을 추가해 보세요.
2. 테넌트별 SQLite 파일(`tenant_<id>.db`)을 사용하도록 `TenantStore`를 바꿔 보세요.
