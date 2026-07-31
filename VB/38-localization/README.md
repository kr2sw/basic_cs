# 38: 지역화 — 리소스 파일, CultureInfo, 다국어

## 소개

애플리케이션을 여러 언어/문화권에서 사용할 수 있게 하는 **지역화(Localization)**를 다룹니다. `CultureInfo`로 문화권을 다루고, 메시지·날짜·숫자 형식이 문화권에 따라 달라지는 것을 확인합니다.

## 주요 개념

### 1. CultureInfo — 문화권 객체

`CurrentCulture`(숫자/날짜 형식)와 `CurrentUICulture`(메시지 선택)로 나뉩니다.

```vb
Thread.CurrentThread.CurrentCulture = New CultureInfo("ko-KR")
Thread.CurrentThread.CurrentUICulture = New CultureInfo("ko")
```

### 2. 리소스 파일(.resx)과 ResourceManager

실제 프로젝트에서는 문자열을 리소스 파일로 분리하고 `ResourceManager`로 읽습니다.

```
Resources.resx        (기본)
Resources.ko.resx     (한국어)
Resources.en.resx     (영어)
```

```vb
Dim rm As New ResourceManager("MyApp.Resources", GetType(Program).Assembly)
Dim msg = rm.GetString("Welcome")     ' CurrentUICulture 기준 선택
```

예제에서는 이 구조를 딕셔너리로 재현했습니다.

### 3. 문화권별 날짜/숫자/통화 형식

같은 값도 문화권에 따라 표기가 달라집니다.

```vb
price.ToString("C", culture)      ' 통화
now.ToString("d", culture)        ' 짧은 날짜
now.ToString("T", culture)        ' 시간
```

### 4. 문화권 인식 문자열 비교

문화권에 따라 정렬 순서도 달라집니다. `StringComparer.Create(culture, ignoreCase)`를 사용합니다.

```vb
Dim comparer = StringComparer.Create(New CultureInfo("sv-SE"), True)
words.OrderBy(Function(w) w, comparer)
```

## 실행

```bash
dotnet run
```

## 정리

- `CurrentCulture` = 형식, `CurrentUICulture` = 언어 선택.
- 문자열은 .resx 리소스 + ResourceManager로 분리합니다.
- 통화/날짜/숫자 형식은 `ToString("C"/"d"/"T", culture)`로 현지화합니다.
- 정렬/비교도 문화권을 고려해야 합니다.
