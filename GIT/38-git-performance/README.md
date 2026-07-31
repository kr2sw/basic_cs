# 38: 성능 — LFS, shallow clone, partial clone, 대형 저장소

대형 파일·저장소의 성능 문제를 해결하는 방법입니다.

## Git LFS (Large File Storage)

```bash
git lfs install
git lfs track "*.psd"
git add .gitattributes
```

바이너리 파일은 포인터로 대체되고 실제 데이터는 LFS 서버에 저장됩니다.

## Shallow Clone

```bash
git clone --depth 1 <url>      # 최근 1개 커밋만
git fetch --depth 50           # 추가 이력
```

## Partial Clone

```bash
git clone --filter=blob:none <url>   # blob 지연 다운로드
git clone --filter=tree:0 <url>      # 필요한 시점에 다운로드
```

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
