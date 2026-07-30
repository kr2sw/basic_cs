# 00 개발환경 설정

## Git 설치

### Windows
```bash
# Chocolatey
choco install git

# 또는 winget
winget install --id Git.Git -e --source winget
```

### macOS
```bash
brew install git
```

### Linux (Ubuntu/Debian)
```bash
sudo apt update && sudo apt install git
```

## 기본 설정

```bash
git config --global user.name "Your Name"
git config --global user.email "your@email.com"
git config --global init.defaultBranch main
git config --global core.autocrlf input  # macOS/Linux
git config --global core.autocrlf true   # Windows
```

## GitHub 계정 생성

1. https://github.com 접속 → Sign up
2. 이메일 인증
3. SSH 키 등록 (선택):
```bash
ssh-keygen -t ed25519 -C "your@email.com"
cat ~/.ssh/id_ed25519.pub
# GitHub Settings → SSH and GPG keys → New SSH key
```

## 확인

```bash
git --version
git config --list
```
