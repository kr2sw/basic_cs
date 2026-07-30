# 00 개발환경 설정

## 필수 도구

- **Python** 3.8 이상
- **pip** (Python 패키지 매니저)
- **가상 환경** (venv) 권장

## 설치

```bash
# 가상 환경 생성 및 활성화
python -m venv venv
source venv/bin/activate  # macOS/Linux
.\venv\Scripts\Activate.ps1  # Windows PowerShell

# FastAPI 및 Uvicorn 설치
pip install fastapi uvicorn
```

## 실행

```bash
cd 01-introduction
uvicorn main:app --reload
# http://127.0.0.1:8000
# API 문서: http://127.0.0.1:8000/docs
```

## 추가 패키지

```bash
# 필요한 패키지는 각 챕터의 requirements.txt 참고
pip install -r requirements.txt
```
