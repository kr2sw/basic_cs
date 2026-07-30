# 16 - 멀티스레딩 (Multithreading)

## 학습 목표
- Dispatcher를 사용한 UI 스레드 마샬링
- async/await 패턴
- Task.Run으로 백그라운드 작업
- ProgressBar 업데이트
- UI 응답성 유지

## WPF 스레딩 모델
- UI 요소는 UI 스레드에서만 접근 가능
- 백그라운드 작업은 Task.Run 또는 async/await 사용
- Dispatcher.Invoke/InvokeAsync로 UI 업데이트
- Progress<T>와 IProgress<T> 패턴 권장
