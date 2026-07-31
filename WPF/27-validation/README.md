# 27: 유효성 검사 — IDataErrorInfo, INotifyDataErrorInfo, ValidationRule

## 학습 목표
- WPF의 세 가지 유효성 검사 방식 비교
- `INotifyDataErrorInfo`(모델 기반, 실시간, 다중 오류)
- `ValidationRule`(바인딩 기반, 선언적)
- 공용 `Validation.ErrorTemplate`으로 오류 UI 통일

## 세 가지 방식

| 방식 | 검사 위치 | 오류 수 | 실시간 |
|------|-----------|---------|--------|
| `IDataErrorInfo` | 모델 | 1개/속성 | PropertyChanged 트리거 |
| `INotifyDataErrorInfo` | 모델 | 여러 개/속성 | `ErrorsChanged` 이벤트 |
| `ValidationRule` | 바인딩 파이프라인 | 1개 | 소스 갱신 시 |

## INotifyDataErrorInfo

속성 setter에서 오류를 판정하고 `ErrorsChanged`를 발생시킵니다.

```csharp
private void ValidateName()
{
    if (string.IsNullOrWhiteSpace(Name))
    {
        SetErrors(nameof(Name), ["이름은 필수 입력입니다."]);
    }
    else
    {
        ClearErrors(nameof(Name));
    }
}

private void SetErrors(string propertyName, IEnumerable<string> messages)
{
    if (_errors.TryGetValue(propertyName, out var list) && list.SequenceEqual(messages)) return;
    _errors[propertyName] = messages.ToList();
    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    OnPropertyChanged(nameof(HasErrors));
}

public IEnumerable GetErrors(string? propertyName)
    => propertyName != null && _errors.TryGetValue(propertyName, out var list)
        ? list
        : Enumerable.Empty<string>();
```

VB.NET 동일 구조:

```vb
Private Sub ValidateName()
    If String.IsNullOrWhiteSpace(Name) Then
        SetErrors(NameOf(Name), New String() {"이름은 필수 입력입니다."})
    Else
        ClearErrors(NameOf(Name))
    End If
End Sub
```

## ValidationRule

모델에 손대지 않고 바인딩 전환 시점에 검증합니다.

```csharp
public class AgeRangeRule : ValidationRule
{
    public int Min { get; set; }
    public int Max { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (int.TryParse(value?.ToString(), out int age) && age >= Min && age <= Max)
        {
            return ValidationResult.ValidResult;
        }
        return new ValidationResult(false, $"나이는 {Min}~{Max} 사이여야 합니다.");
    }
}
```

```xml
<TextBox.Text>
    <Binding Path="Age" UpdateSourceTrigger="PropertyChanged"
             ValidatesOnNotifyDataErrors="False">
        <Binding.ValidationRules>
            <local:AgeRangeRule Min="0" Max="120"/>
        </Binding.ValidationRules>
    </Binding>
</TextBox.Text>
```

## 공용 오류 표시 템플릿

`Validation.ErrorTemplate`을 스타일로 만들어 오류 UI를 통일합니다.

```xml
<Style TargetType="TextBox">
    <Setter Property="Validation.ErrorTemplate">
        <Setter.Value>
            <ControlTemplate>
                <DockPanel>
                    <TextBlock Foreground="Red" FontSize="11" DockPanel.Dock="Top"
                               Text="{Binding AdornedElement.(Validation.Errors)[0].ErrorContent,
                                      RelativeSource={RelativeSource TemplatedParent}}"/>
                    <Border BorderBrush="Red" BorderThickness="1">
                        <AdornedElementPlaceholder/>
                    </Border>
                </DockPanel>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

- `AdornedElementPlaceholder` = 원래 컨트롤이 놓일 자리
- `AdornedElement.(Validation.Errors)[0].ErrorContent` = 첫 번째 오류 메시지

## 주의점

- `IDataErrorInfo`는 `GetErrors`가 1개 오류만 반환 → `INotifyDataErrorInfo`가 상위 호환
- 숫자 바인딩에 문자를 넣으면 **형식 변환 오류**도 자동으로 검증 오류가 됨
- `ValidatesOnNotifyDataErrors="False"`로 규칙 검증만 사용 가능
- `HasErrors`는 UI 스레드에서 갱신해야 함

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

- 모델 규칙(비즈니스) → `INotifyDataErrorInfo`
- 바인딩 형식/범위 → `ValidationRule`
- 표시 방식 → 공용 `Validation.ErrorTemplate`
