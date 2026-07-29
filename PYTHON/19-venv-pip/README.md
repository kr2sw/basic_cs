# 19: 가상 환경(Venv)과 패키지 관리(Pip)

## 가상 환경 (Virtual Environment)
프로젝트마다 독립된 Python 환경을 만들어 의존성 충돌을 방지합니다.

```bash
python -m venv venv          # 생성
venv\Scripts\activate        # 활성화 (Windows)
source venv/bin/activate     # 활성화 (macOS/Linux)
deactivate                   # 비활성화
```

## pip
Python 패키지를 설치/관리하는 패키지 매니저입니다.

```bash
pip install requests          # 패키지 설치
pip install requests==2.31.0  # 특정 버전 설치
pip list                      # 설치된 패키지 목록
pip freeze > requirements.txt # 현재 환경을 파일로 저장
```

## requirements.txt
의존성 목록을 파일로 저장하여 공유합니다. `pip install -r requirements.txt`로 동일한 환경을 재현할 수 있습니다.

## 주요 패키지
- `requests`: HTTP 통신
- `numpy`/`pandas`: 데이터 분석
- `django`/`flask`: 웹 프레임워크
- `beautifulsoup4`/`scrapy`: 웹 스크래핑
