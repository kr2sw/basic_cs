# 35: Docker 배포 — Dockerfile and Compose Concepts

Node.js 애플리케이션을 Docker로 컨테이너화하는 방법을 학습합니다.

## Dockerfile

```dockerfile
FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm ci --only=production

COPY . .

ENV NODE_ENV=production
ENV PORT=3000

EXPOSE 3000

CMD ["node", "index.js"]
```

## 빌드와 실행

```bash
docker build -t my-app .
docker run -p 3000:3000 --env-file .env my-app
```

## .dockerignore

불필요한 파일이 이미지에 들어가지 않도록 제외합니다.

```text
node_modules
.git
.env
*.log
```

## Docker Compose

여러 컨테이너를 한 번에 관리합니다.

```yaml
services:
  app:
    build: .
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
    depends_on:
      - db
  db:
    image: mongo:7
    volumes:
      - mongo-data:/data/db

volumes:
  mongo-data:
```

```bash
docker compose up -d
docker compose down
```

## 레이어 최적화

`COPY package*.json` → `npm ci`를 먼저 수행하면 의존성 레이어가 캐시되어 이후 빌드가 빨라집니다.

## 예제 실행

```bash
# 로컬에서 바로 실행
node index.js

# 컨테이너 실행
docker build -t basic-njs-35 .
docker run -p 3000:3000 basic-njs-35
```
