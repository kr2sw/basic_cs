# 23: Alembic — 마이그레이션 개념, revision, upgrade

프로젝트가 커지면 테이블 스키마가 계속 바뀝니다. 코드와 DB 스키마가 어긋나면 운영 장애로 이어지므로, 스키마 변경을 **버전 관리**하는 도구가 필요합니다. SQLAlchemy 공식 마이그레이션 도구인 **Alembic**를 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

## 주요 개념

### 마이그레이션이란?

DB 스키마(테이블/컬럼/인덱스) 변경 이력을 **revision(버전)** 으로 관리하고, 업그레이드/다운그레이드를 순차 적용하는 작업입니다. Git이 코드를 관리한다면 Alembic은 스키마를 관리합니다.

### 초기화와 설정

```bash
alembic init alembic
```

생성된 `alembic.ini`에서 DB URL을 설정합니다.

```ini
sqlalchemy.url = sqlite:///./alembic.db
```

### env.py에 모델 연결

`alembic/env.py`에서 모델 메타데이터를 연결합니다. 그래야 모델과 현재 DB를 비교해 마이그레이션을 자동 생성할 수 있습니다.

```python
import models  # noqa
from database import Base
target_metadata = Base.metadata
```

### revision 생성 (모델 -> 스키마)

```bash
alembic revision --autogenerate -m "create users table"
# produces: alembic/versions/xxxx_create_users_table.py
```

`--autogenerate`는 **모델과 현재 DB의 차이**를 읽어 변경 스크립트를 자동 생성합니다. 생성된 revision은 반드시 검토해야 합니다.

### 적용 / 되돌리기

```bash
alembic upgrade head      # 최신 버전까지 적용
alembic downgrade -1      # 한 단계 되돌림
alembic current           # 현재 적용된 버전 확인
alembic history           # 변경 이력 확인
```

### revision 파일 구조

```python
def upgrade():
    op.add_column("users", sa.Column("nickname", sa.String(50)))

def downgrade():
    op.drop_column("users", "nickname")
```

`upgrade`는 적용, `downgrade`는 롤백을 작성합니다. 데이터 삭제를 동반하는 변경은 반드시 `downgrade`까지 대칭으로 작성합니다.

## 운영 팁

- 커밋 전에 **반드시** `upgrade`/`downgrade`를 로컬에서 검증합니다.
- `alembic current`와 배포 스크립트가 항상 최신인지 확인합니다.
- CI에서 마이그레이션 적용 후 테스트를 실행하면 스키마 오류를 조기에 잡을 수 있습니다.

## 연습

1. `User`에 `age` 컬럼을 추가하고 `alembic revision --autogenerate`로 마이그레이션을 만들어 보세요.
2. `upgrade` 후 `downgrade -1`로 되돌리는 흐름을 연습해 보세요.
