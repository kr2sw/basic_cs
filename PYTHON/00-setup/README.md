# 00 개발환경 설정

## 필수 도구

- **Python** 3.x (https://www.python.org/downloads)
- **pip** (Python 패키지 관리자, Python 3.4+ 기본 포함)
- **pyenv** (선택 사항, Python 버전 관리)

## Python 설치

### Windows (scoop)
```bash
scoop install python
```

### Windows (직접)
1. https://www.python.org 방문
2. 최신 Python 3 다운로드 및 설치
3. 설치 시 **Add Python to PATH** 체크

### macOS
```bash
brew install python@3.12
```

### Linux
```bash
sudo apt update
sudo apt install python3 python3-pip python3-venv
```

### 설치 확인
```bash
python --version   # Windows
python3 --version  # macOS/Linux
pip --version
```

## 가상 환경 (venv)

```bash
# 가상 환경 생성
python -m venv venv

# 활성화 (Windows PowerShell)
.\venv\Scripts\Activate.ps1

# 활성화 (macOS/Linux)
source venv/bin/activate

# 비활성화
deactivate

# 의존성 저장/복원
pip freeze > requirements.txt
pip install -r requirements.txt
```

## pyenv (Python 버전 관리)

```bash
# Windows: pyenv-win
scoop install pyenv

# macOS/Linux
curl https://pyenv.run | bash

# 버전 설치 및 전환
pyenv install 3.12.0
pyenv global 3.12.0
```

## VS Code 확장

- **Python** (Microsoft)
- **Pylance**
- **Jupyter**
