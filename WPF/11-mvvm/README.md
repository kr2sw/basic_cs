# 11 - MVVM 패턴

## 학습 목표
- MVVM(Model-View-ViewModel) 아키텍처 이해
- ViewModel에서 INotifyPropertyChanged 구현
- RelayCommand를 사용한 커맨드 바인딩
- View와 ViewModel의 분리

## MVVM 구조

```
View (XAML) ← 데이터 바인딩 → ViewModel (C#/VB) ← 모델 → Model
   (UI만 담당)                (상태와 로직)             (데이터)
```

MVVM은 WPF 애플리케이션의 표준 아키텍처 패턴으로, 관심사 분리와 테스트 용이성을 제공합니다.
