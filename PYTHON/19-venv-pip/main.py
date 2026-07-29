"""
이 스크립트는 가상 환경과 pip 사용법을 설명합니다.
실제로 실행하기보다는 아래 주석 처리된 안내를 따라주세요.

## 가상 환경 생성 및 활성화

### Windows:
    python -m venv venv
    venv\\Scripts\\activate

### macOS / Linux:
    python3 -m venv venv
    source venv/bin/activate

## 패키지 설치

    pip install requests
    pip install numpy pandas matplotlib
    pip install flask
    pip install beautifulsoup4

## requirements.txt 사용

    pip freeze > requirements.txt   # 현재 환경 저장
    pip install -r requirements.txt # 환경 복원

## 패키지 정보 확인

    pip list                  # 설치된 패키지 목록
    pip show requests         # 특정 패키지 상세 정보
    pip uninstall requests    # 패키지 제거

## 주요 패키지 개요
├── requests         → HTTP 요청 라이브러리
├── numpy            → 수치 계산 (배열, 행렬 연산)
├── pandas           → 데이터 분석 (DataFrame)
├── matplotlib       → 데이터 시각화 (그래프)
├── django / flask   → 웹 애플리케이션 프레임워크
├── beautifulsoup4   → HTML/XML 파싱 (웹 스크래핑)
├── pytest           → 테스트 프레임워크
└── pillow           → 이미지 처리
"""

import sys
import subprocess


def check_venv():
    return hasattr(sys, "real_prefix") or (
        hasattr(sys, "base_prefix") and sys.base_prefix != sys.prefix
    )


def list_installed():
    """pip list 실행 결과를 출력합니다."""
    result = subprocess.run([sys.executable, "-m", "pip", "list", "--format=columns"],
                            capture_output=True, text=True)
    print(result.stdout)


if __name__ == "__main__":
    print(f"Python: {sys.version}")
    print(f"Executable: {sys.executable}")
    print(f"In venv: {check_venv()}")
    print()

    print("설치된 패키지 목록:")
    list_installed()

    print("\n" + "=" * 50)
    print("참고: pip 명령어는 터미널에서 직접 실행해야 합니다.")
    print("예: pip install requests")
    print("    pip freeze > requirements.txt")
