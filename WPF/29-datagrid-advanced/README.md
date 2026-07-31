# 29: DataGrid 고급 — 편집, 그룹핑, 열 템플릿

## 학습 목표
- `DataGridTextColumn` / `DataGridTemplateColumn` 차이
- `GroupDescription` 기반 그룹핑과 `GroupStyle`
- `SelectedItem` 바인딩과 커맨드 연동
- 컬렉션 편집(추가/삭제)이 즉시 반영되는 `ObservableCollection`

## 열 정의

```xml
<DataGrid AutoGenerateColumns="False" ItemsSource="{Binding Products}"
          SelectedItem="{Binding SelectedProduct}">
    <DataGrid.Columns>
        <!-- 텍스트 편집 가능 -->
        <DataGridTextColumn Header="이름" Width="*"
                            Binding="{Binding Name, UpdateSourceTrigger=PropertyChanged}"/>

        <!-- 템플릿 열: 체크박스 등 자유로운 셀 표현 -->
        <DataGridTemplateColumn Header="즐겨찾기" Width="90">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <CheckBox IsChecked="{Binding IsFavorite}"
                              HorizontalAlignment="Center"/>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
    </DataGrid.Columns>
</DataGrid>
```

## 그룹핑

`ICollectionView.GroupDescriptions`에 그룹 기준을 추가합니다.

```csharp
var view = CollectionViewSource.GetDefaultView(Products);
view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Category)));
```

```vb
Dim view = CollectionViewSource.GetDefaultView(Products)
view.GroupDescriptions.Add(New PropertyGroupDescription(NameOf(Product.Category)))
```

헤더 템플릿을 정의하면 그룹 이름이 표시됩니다.

```xml
<DataGrid.GroupStyle>
    <GroupStyle>
        <GroupStyle.HeaderTemplate>
            <DataTemplate>
                <TextBlock Text="{Binding Name}" FontWeight="Bold" Foreground="DodgerBlue"/>
            </DataTemplate>
        </GroupStyle.HeaderTemplate>
    </GroupStyle>
</DataGrid.GroupStyle>
```

## 선택 항목과 커맨드

`SelectedItem`을 VM에 바인딩하고, 삭제 커맨드의 `CanExecute`를 연결합니다.

```csharp
public Product? SelectedProduct
{
    get => _selectedProduct;
    set
    {
        _selectedProduct = value;
        OnPropertyChanged();
        DeleteCommand.RaiseCanExecuteChanged();
    }
}

DeleteCommand = new RelayCommand(_ => DeleteProduct(), _ => SelectedProduct is not null);

private void DeleteProduct()
{
    if (SelectedProduct is not null)
    {
        Products.Remove(SelectedProduct);
        SelectedProduct = null;
    }
}
```

## 컬렉션 변경 알림

`List<T>` 대신 `ObservableCollection<T>`를 사용해야 추가/삭제가
즉시 UI에 반영됩니다.

```csharp
public ObservableCollection<Product> Products { get; } = new();
```

## 팁

- `CanUserAddRows="True"`로 하단 빈 행 추가 활성화
- `AlternatingRowBackground`로 짝수 행 구분
- `StringFormat={}{0:C}`로 화폐 표시 (컬럼 템플릿과 결합 가능)
- 대량 데이터에서는 `DataGrid` 가상화 옵션 확인 (30장에서 상세)

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

- 편집: `UpdateSourceTrigger=PropertyChanged`로 입력 즉시 소스 반영
- 그룹핑: 뷰 수준에서 `GroupDescription` 추가/제거
- 템플릿 열: 셀마다 다른 컨트롤과 바인딩 가능
