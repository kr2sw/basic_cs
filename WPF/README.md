# WPF (Windows Presentation Foundation) 완벽 강좌

## WPF란?

WPF(Windows Presentation Foundation)는 Microsoft가 .NET Framework의 일부로 도입한 최신 UI 프레임워크입니다. 2006년 .NET Framework 3.0과 함께 처음 출시되었으며, 데스크톱 애플리케이션 개발을 위한 강력한 플랫폼을 제공합니다.

### 주요 특징

- **XAML 기반 UI**: 선언적 마크업 언어인 XAML(eXtensible Application Markup Language)을 사용하여 UI를 정의합니다.
- **데이터 바인딩**: UI와 데이터를 쉽게 연결할 수 있는 강력한 데이터 바인딩 시스템을 제공합니다.
- **스타일 및 템플릿**: CSS와 유사한 스타일 시스템과 컨트롤 템플릿을 통해 UI를 완벽하게 사용자 정의할 수 있습니다.
- **MVVM 패턴**: Model-View-ViewModel 패턴을 자연스럽게 지원합니다.
- **벡터 기반 그래픽**: 해상도 독립적인 벡터 기반 렌더링을 사용합니다.
- **애니메이션 및 미디어**: 풍부한 애니메이션 시스템과 멀티미디어 지원을 제공합니다.
- **하드웨어 가속**: DirectX를 통한 하드웨어 가속 렌더링을 지원합니다.

### 역사

| 버전 | .NET 버전 | 출시일 | 주요 기능 |
|------|-----------|--------|-----------|
| 3.0 | 3.0 | 2006-11 | 최초 출시 |
| 3.5 | 3.5 | 2007-11 | 성능 향상, 편집기 개선 |
| 4.0 | 4.0 | 2010-04 | 멀티터치, 리본 컨트롤 |
| 4.5 | 4.5 | 2012-08 | 성능 개선, 데이터 바인딩 향상 |
| 4.6 | 4.6 | 2015-07 | 투명도 지원 개선 |
| 4.7 | 4.7 | 2017-04 | 터치/스타일러스 개선 |
| 4.8 | 4.8 | 2019-04 | 고해상도 DPI 개선 |
| 5.0 | 5.0 | 2020-11 | .NET 5 통합 |
| 6.0 | 6.0 | 2021-11 | .NET 6 LTS |
| 7.0 | 7.0 | 2022-11 | .NET 7 |
| 8.0 | 8.0 | 2023-11 | .NET 8 LTS |
| 10.0 | 10.0 | 2025-11 | .NET 10 (현재) |

## 목차

| 장 | 제목 | 설명 |
|----|------|------|
| 00 | 개발 환경 설정 | Visual Studio 설치, .NET SDK, WPF 워크로드 |
| 01 | Hello, WPF! | 첫 번째 WPF 애플리케이션 만들기 |
| 02 | 레이아웃 | Grid, StackPanel, WrapPanel, DockPanel |
| 03 | 컨트롤 | Button, TextBox, Slider, ProgressBar 등 |
| 04 | 이벤트 | 라우티드 이벤트, 버블링, 터널링 |
| 05 | 데이터 바인딩 | {Binding}, DataContext, INotifyPropertyChanged |
| 06 | 커맨드 | ICommand, RelayCommand, CommandBinding |
| 07 | 스타일 | Style, Setter, TargetType, BasedOn |
| 08 | 템플릿 | ControlTemplate, DataTemplate |
| 09 | 트리거 | PropertyTrigger, EventTrigger, MultiTrigger |
| 10 | 리소스 | Resources, ResourceDictionary |
| 11 | MVVM 패턴 | Model-View-ViewModel 아키텍처 |
| 12 | 컬렉션 | ObservableCollection, ListBox, ListView |
| 13 | 데이터 그리드 | DataGrid, 열 템플릿 |
| 14 | 대화상자 | MessageBox, OpenFileDialog, 사용자 정의 대화상자 |
| 15 | 내비게이션 | Frame, Page, NavigationService |
| 16 | 멀티스레딩 | Dispatcher, async/await, Task |
| 17 | 사용자 정의 컨트롤 | UserControl, DependencyProperty |
| 18 | 애니메이션 | Storyboard, DoubleAnimation, ColorAnimation |
| 19 | 스타일 및 테마 | ResourceDictionary, 테마 전환 |
| 20 | 배포 | ClickOnce, 단일 파일 게시 |

## 시작하기

```bash
# .NET 8 SDK 확인
dotnet --version

# 새 WPF 프로젝트 생성
dotnet new wpf -n MyWpfApp

# 실행
cd MyWpfApp
dotnet run
```

## 기술 스택

- .NET 8.0 (C#) / 10.0 (VB.NET)
- Visual Studio 2022 이상 (권장)
- Windows 10/11 (필수)
- C# 12 / VB.NET 16
