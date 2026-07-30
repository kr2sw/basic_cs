# 15 - 내비게이션 (Navigation)

## 학습 목표
- Frame 컨트롤과 Page 사용법
- NavigationService로 페이지 간 이동
- 저널(뒤로/앞으로) 관리
- 페이지 간 데이터 전달

## 내비게이션 구조

```
MainWindow (Frame 포함)
    ├── Page1 (홈)
    ├── Page2 (설정)
    └── ... 
```

Frame은 Page 컨텐츠를 호스팅하고 NavigationService를 통해 페이지 간 이동을 관리합니다.
