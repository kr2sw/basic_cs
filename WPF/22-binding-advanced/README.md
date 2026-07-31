# 22: 고급 데이터 바인딩 — MultiBinding, PriorityBinding, UpdateSourceTrigger

## 학습 목표
- 여러 소스를 하나로 합치는 `MultiBinding` / `IMultiValueConverter`
- 우선순위에 따라 소스를 선택하는 `PriorityBinding`
- `UpdateSourceTrigger`(PropertyChanged / LostFocus / Explicit) 차이
- `StringFormat`과 컨버터의 적절한 활용

## MultiBinding

한 속성에 여러 바인딩을 연결하고 컨버터로 합칩니다.

```xml
<TextBlock>
  <TextBlock.Text>
    <MultiBinding Converter="{StaticResource FullNameConverter}">
      <Binding Path="Person.LastName"/>
      <Binding Path="Person.FirstName"/>
    </MultiBinding>
  </TextBlock.Text>
</TextBlock>
```

```csharp
public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    => $"{values[0]} {values[1]}"; // ["홍", "길동"] → "홍 길동"
```

단순 조합이면 컨버터 없이 `StringFormat`만으로 충분합니다.

```xml
<MultiBinding StringFormat="전체 이름: {0} {1}">
  <Binding Path="Person.LastName"/>
  <Binding Path="Person.FirstName"/>
</MultiBinding>
```

## PriorityBinding

같은 타깃에 여러 소스를 **우선순위 순서**로 등록하고,
가장 먼저 "값이 있는" 바인딩이 표시됩니다. 로딩 지연, 캐시 폴백에 유용합니다.

```xml
<PriorityBinding>
  <Binding Path="Person.NicknameFast"/>      <!-- 1초 후 채워짐 -->
  <Binding Path="Person.NicknameSlow"/>      <!-- 3초 후 채워짐 -->
  <Binding Path="Person.NicknameFallback"/>  <!-- 기본값 -->
</PriorityBinding>
```

초기에는 모든 소스가 `null`이라 폴백("닉네임 없음")이 보이고,
빠른 값이 도착하면 즉시 교체됩니다.

```csharp
var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
timer.Tick += (_, _) => { Person.NicknameFast = "번개"; timer.Stop(); };
timer.Start();
```

## UpdateSourceTrigger

`TextBox.Text`처럼 기본적으로 `LostFocus`에 소스가 갱신되는 속성이 있습니다.
이때 갱신 시점을 조정합니다.

```xml
<!-- 글자를 칠 때마다 소스 갱신 -->
<TextBox Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}"/>

<!-- 명시적으로만 갱신 -->
<TextBox x:Name="noteBox" Text="{Binding Note, UpdateSourceTrigger=Explicit}"/>
```

```csharp
// Explicit 바인딩은 코드에서 직접 갱신
noteBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
```

- `PropertyChanged`: 즉시 반영 (목록 UI에 적합)
- `LostFocus`: 포커스를 잃을 때 갱신 (기본값, 검증과 조합 가능)
- `Explicit`: `UpdateSource()`를 호출한 순간에만 갱신 (저장 버튼이 있는 폼)

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

## 정리

| 도구 | 용도 |
|------|------|
| `MultiBinding` | 여러 속성 → 하나의 표시 값 |
| `IMultiValueConverter` | 조합/조건 로직이 필요할 때 |
| `PriorityBinding` | 지연 로딩 + 폴백 값 |
| `UpdateSourceTrigger` | 소스 갱신 시점 제어 |
| `BindingExpression.UpdateSource()` | Explicit 강제 갱신 |
