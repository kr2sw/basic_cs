# 12: 파일 업로드 — File, UploadFile

## 실행

```bash
pip install python-multipart
uvicorn main:app --reload
```

## 테스트

```bash
curl -X POST http://localhost:8000/upload \
  -F "file=@test.txt" \
  -F "description=Test file"
```

## 주요 개념

- **UploadFile**: FastAPI의 파일 업로드 클래스
- **File()**: 파일 매개변수 데코레이터
- **다중 파일 업로드**: `list[UploadFile]`
- **파일 형식 검증**: Content-Type 확인
