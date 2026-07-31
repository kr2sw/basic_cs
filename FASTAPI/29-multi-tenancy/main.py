import time
from typing import Literal

from fastapi import Depends, FastAPI, HTTPException, Request
from pydantic import BaseModel

app = FastAPI(title="멀티테넌시 - 헤더 기반 테넌트 분리")

# 테넌트 헤더 이름 (클라이언트가 항상 전달)
TENANT_HEADER = "X-Tenant-ID"


class TenantStore:
    """테넌트별 저장소를 분리하는 인메모리 스토어 (데모)"""

    def __init__(self):
        self._stores: dict[str, dict[int, dict]] = {}
        self._seq: dict[str, int] = {}

    def get_store(self, tenant: str) -> dict[int, dict]:
        """테넌트별 데이터 공간을 분리해서 반환"""
        return self._stores.setdefault(tenant, {})

    def create(self, tenant: str, content: str) -> dict:
        store = self.get_store(tenant)
        seq = self._seq.get(tenant, 0) + 1
        self._seq[tenant] = seq
        note = {"id": seq, "content": content, "created_at": time.time()}
        store[seq] = note
        return note

    def list(self, tenant: str) -> list[dict]:
        return list(self.get_store(tenant).values())

    def delete(self, tenant: str, note_id: int) -> bool:
        store = self.get_store(tenant)
        return store.pop(note_id, None) is not None


store = TenantStore()


class NoteCreate(BaseModel):
    content: str


class NoteOut(BaseModel):
    id: int
    content: str
    created_at: float


def get_tenant(request: Request) -> str:
    """헤더에서 테넌트 ID를 꺼내고, 없으면 400 반환"""
    tenant = request.headers.get(TENANT_HEADER)
    if not tenant:
        raise HTTPException(status_code=400, detail=f"{TENANT_HEADER} 헤더가 필요합니다")
    return tenant


@app.post("/notes", response_model=NoteOut)
def create_note(data: NoteCreate, tenant: str = Depends(get_tenant)):
    """요청한 테넌트의 저장소에만 노트를 저장 (격리)"""
    return store.create(tenant, data.content)


@app.get("/notes", response_model=list[NoteOut])
def list_notes(tenant: str = Depends(get_tenant)):
    """해당 테넌트의 데이터만 반환"""
    return store.list(tenant)


@app.delete("/notes/{note_id}")
def delete_note(note_id: int, tenant: str = Depends(get_tenant)):
    if not store.delete(tenant, note_id):
        raise HTTPException(status_code=404, detail="노트를 찾을 수 없습니다")
    return {"message": "삭제 완료"}


@app.get("/tenants")
def list_tenants(tenant: str = Depends(get_tenant)):
    """운영/디버그용: 각 테넌트의 데이터 개수를 확인"""
    counts = {name: len(s) for name, s in store._stores.items()}
    return {"tenants": counts}
