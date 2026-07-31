# 26: 소스 생성기 — Source Generators

소스 생성기(Source Generator)는 컴파일 시점에 메타데이터(attribute, 타입 정의
등)를 분석해서 **추가 소스 코드를 자동 생성**하는 기술입니다. 반복 코드를
없애고 리플렉션 대비 성능도 좋습니다.

## partial과 소스 생성기

소스 생성기는 생성할 코드가 있는 대상에 **`partial` 클래스/메서드**가 선언되어
있어야 합니다. 생성기가 만든 코드는 `partial` 부분에 합쳐집니다.

```csharp
[AutoNotify]          // 어트리뷰트 -> 생성기가 감지
public partial string Name { get; set; }
```

## Incremental Generator 개념

- **IIncrementalGenerator** — 입력 변경분만 다시 계산하는 고성능 생성기 인터페이스
- `RegisterSourceProvider()` 로 분석 대상을 등록
- `GenerateCode()` 로 `AddSource("이름", sourceText)` 호출

실제 Roslyn 기반 소스 생성기는 별도 프로젝트(NuGet)가 필요하므로, 이 장에서는
**개념을 재현하는 시뮬레이터**를 만들어 동작 원리를 이해합니다.

## 실행

```bash
dotnet run
```

## 핵심 요약

- 소스 생성기는 컴파일 타임에 코드를 만들어 리플렉션 오버헤드를 줄입니다.
- `partial`은 생성된 코드와 손으로 쓴 코드를 합치는 장치입니다.
- 대표 사례: System.Text.Json(JSON 소스 생성), MediatR, AutoMapper.
