# 15: 백그라운드 작업 — BackgroundTasks

## 실행

```bash
uvicorn main:app --reload
```

## 주요 개념

- **BackgroundTasks**: 응답 후 백그라운드 실행
- **이메일 발송, 로그 기록, 알림**: 응답 지연 없이 처리
- **의존성 주입**: BackgroundTasks를 의존성으로 주입 가능
- **Celery와의 차이**: 단순한 작업은 BackgroundTasks, 복잡한 작업은 Celery
