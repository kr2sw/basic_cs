# 20 - 배포 (Deployment)

## 학습 목표
- ClickOnce 배포 이해
- 단일 파일 게시 (Single-file publish)
- app.config 설정
- MSIX 패키징

## 배포 방법

| 방법 | 설명 |
|------|------|
| ClickOnce | 웹/네트워크 공유를 통한 자동 업데이트 배포 |
| 단일 파일 게시 | 모든 의존성을 하나의 .exe로 패키징 |
| MSIX | 최신 Windows 패키징 형식, 스토어 배포 |
| Installer (WiX) | MSI 설치 프로그램 |

## 단일 파일 게시 명령어

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## ClickOnce 배포 (Visual Studio)
1. 프로젝트 우클릭 → 속성 → 게시
2. 게시 대상 선택 (폴더, FTP, 웹)
3. 버전 및 업데이트 설정
4. 게시 실행
