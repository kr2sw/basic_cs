# 30: 가상화와 성능 — VirtualizingStackPanel, 병렬 디자인

## 학습 목표
- UI 가상화(UI virtualization) 개념
- `VirtualizingStackPanel` 옵션: `IsVirtualizing`, `VirtualizationMode`, `ScrollUnit`
- `ObservableCollection` vs 읽기 전용 컬렉션의 성능 차이
- 10만 개 데이터로 직접 비교

## UI 가상화란

스크롤 영역에 **보이는 항목만** 컨테이너를 생성해 화면에 배치하고,
벗어난 항목은 재활용하는 기법입니다. 10만 개를 모두 만들지 않으므로
초기 로드가 빠르고 메모리도 절약됩니다.

```xml
<ListBox ItemsSource="{Binding Items}"
         VirtualizingPanel.IsVirtualizing="True"
         VirtualizingPanel.VirtualizationMode="Recycling"
         VirtualizingPanel.ScrollUnit="Pixel"/>
```

## 옵션 설명

| 옵션 | 값 | 의미 |
|------|----|------|
| `IsVirtualizing` | True/False | 가상화 켜기/끄기 |
| `VirtualizationMode` | `Standard` | 벗어난 컨테이너 파괴 |
| | `Recycling` | 벗어난 컨테이너 **재활용**(권장) |
| `ScrollUnit` | `Pixel` | 픽셀 단위 스크롤 (컨테이너 재사용) |
| | `Item` | 항목 단위 스크롤 (가장 빠름) |

`VirtualizationMode=Recycling`과 `ScrollUnit=Pixel`이 기본이며
가장 부드러운 스크롤과 재사용을 제공합니다.

## 컬렉션 선택

10만 개를 `ObservableCollection`에 넣으면 항목마다 변경 알림이
발생해 성능이 떨어집니다. **일괄 로드 후 변경이 없을 때는 `List<T>`**를 쓰고
전체 교체 시 `PropertyChanged` 하나만 발생시킵니다.

```csharp
public List<ItemModel> Items { get; private set; } = new();

private void Load()
{
    var sw = Stopwatch.StartNew();
    Items = Enumerable.Range(0, 100_000).Select(i => new ItemModel(i)).ToList();
    OnPropertyChanged(nameof(Items));
    LoadInfo = $"10만 개 로드: {sw.ElapsedMilliseconds} ms";
}
```

VB.NET:

```vb
Items = Enumerable.Range(0, 100000).Select(Function(i) New ItemModel(i)).ToList()
OnPropertyChanged(NameOf(Items))
```

## 토글로 직접 비교

가상화를 끄면 10만 개의 `ListBoxItem`이 모두 생성됩니다.
`ScrollIntoView`로 맨 끝으로 이동하는 시간도 함께 측정합니다.

```csharp
var sw = Stopwatch.StartNew();
itemList.ScrollIntoView(itemList.Items[itemList.Items.Count - 1]);
Dispatcher.BeginInvoke(
    () => scrollInfo.Text = $"맨 끝 스크롤: {sw.ElapsedMilliseconds} ms",
    DispatcherPriority.Loaded);
```

## 가상화가 깨지는 경우

- `ItemsPanel`을 `StackPanel`/`WrapPanel`로 바꾸면 가상화 해제
- `ItemContainerStyle`에서 행 높이를 자유롭게 바꾸면 성능 저하
- 픽셀 스크롤이 필요한 `ListBox`는 높이 고정 항목이어야 재사용 효율이 좋음

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

- 대량 데이터는 가상화 + `Recycling` + `Pixel` 스크롤
- 변경이 없는 컬렉션은 `List<T>` 전체 교체 방식
- 성능은 이론이 아니라 측정: `Stopwatch`로 로드/스크롤 시간 확인
