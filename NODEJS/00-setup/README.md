# 00 개발환경 설정

## 필수 도구

- **Node.js** (https://nodejs.org, LTS 권장)
- **npm** (Node.js 설치 시 함께 설치됨)
- **nvm** (선택 사항, Node.js 버전 관리)

## Node.js 설치

### Windows (scoop)
```bash
scoop install nodejs-lts
```

### Windows (직접)
1. https://nodejs.org 방문
2. LTS 버전 설치 관리자 다운로드 및 실행

### macOS
```bash
brew install node@20
```

### Linux
```bash
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt install -y nodejs
```

### 설치 확인
```bash
node --version
npm --version
```

## nvm (Node Version Manager)

여러 Node.js 버전을 전환하며 사용할 수 있습니다.

```bash
# Windows: nvm-windows
scoop install nvm

# macOS/Linux
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.7/install.sh | bash

# 버전 설치 및 전환
nvm install 20
nvm use 20
```

## 패키지 매니저

```bash
# npm (기본)
npm install <package>

# yarn
npm install -g yarn
yarn add <package>

# pnpm
npm install -g pnpm
pnpm add <package>
```

## 프로젝트 실행

```bash
cd 01-introduction
node index.js

# 또는 package.json scripts 사용
npm start
npm run dev
```
