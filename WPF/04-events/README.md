# 04: 이벤트 — RoutedEvent, 버블링, 터널링

WPF의 라우티드 이벤트(RoutedEvent) 시스템을 학습합니다.

## 실행

```bash
cd csharp && dotnet run
cd vbnet && dotnet run
```

## 주요 개념

- **라우티드 이벤트**: 요소 트리를 따라 전파되는 이벤트
- **버블링(Bubbling)**: 자식 → 부모로 전파
- **터널링(Tunneling)**: 부모 → 자식으로 전파 (Preview 접두사)
- **Handled = true**: 이벤트 전파 중단
- **MouseEnter / MouseLeave**: 마우스 호버 이벤트
