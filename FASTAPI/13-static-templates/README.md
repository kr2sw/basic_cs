# 13: 정적 파일과 템플릿

## 설치

```bash
pip install jinja2 aiofiles
```

## 실행

```bash
uvicorn main:app --reload
```

http://127.0.0.1:8000 - Jinja2 템플릿 페이지
http://127.0.0.1:8000/items/42 - 동적 템플릿
http://127.0.0.1:8000/static/style.css - 정적 파일

## 주요 개념

- **Jinja2Templates**: 템플릿 렌더링
- **StaticFiles**: 정적 파일 서빙
- **Mount**: 하위 애플리케이션 마운트
