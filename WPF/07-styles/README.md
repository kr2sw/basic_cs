# 07 - 스타일 (Styles)

## 학습 목표
- WPF Style 시스템 이해
- Setter를 사용한 속성 설정
- TargetType으로 특정 컨트롤 타입 스타일링
- BasedOn으로 스타일 상속
- Named 스타일과 TargetType 스타일 구분

## 스타일 구조

```xml
<Style x:Key="MyStyle" TargetType="Button">
    <Setter Property="Background" Value="Blue"/>
    <Setter Property="Foreground" Value="White"/>
</Style>
```
