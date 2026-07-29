# 20. 배포 (Deployment)

Node.js 애플리케이션을 프로덕션 환경에 배포하는 방법을 학습합니다.

## PM2 (Process Manager)

Node.js 애플리케이션의 프로세스 관리 도구입니다.

### 설치

```bash
npm install -g pm2
```

### 기본 명령어

```bash
pm2 start app.js           # 앱 시작
pm2 list                   # 프로세스 목록
pm2 logs                   # 로그 확인
pm2 restart app-name       # 재시작
pm2 stop app-name          # 중지
pm2 delete app-name        # 삭제
pm2 startup                # 서버 부팅 시 자동 시작
```

## 클러스터 모드

CPU 코어를 모두 활용하여 성능을 향상시킵니다.

```bash
pm2 start app.js -i max    # CPU 코어 수만큼 인스턴스 생성
```

## 환경 변수 설정

```bash
NODE_ENV=production pm2 start app.js
# 또는 ecosystem.config.js 사용
```

## Docker 기초

Dockerfile로 애플리케이션을 컨테이너화합니다.

```dockerfile
FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
EXPOSE 3000
CMD ["node", "app.js"]
```

## 예제 실행

```bash
# PM2
pm2 start index.js -i max --name "my-app"

# Docker
docker build -t my-app .
docker run -p 3000:3000 my-app
```
