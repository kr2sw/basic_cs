# 10 Traits — 트레이트

공유 동작을 정의하는 트레이트: 정의, 구현, 기본 메서드, 트레이트 바운드, 트레이트 객체.

## 주요 개념
- 트레이트 정의: 공유 메서드 시그니처 선언
- 기본 구현: trait 내부에 기본 동작 정의 가능
- 트레이트 구현: `impl Trait for Type` — 외부 타입에도 구현 가능
- `impl Trait` 문법: 매개변수와 반환 타입 위치
- 트레이트 바운드: `where` 절로 제약 조건 명시
- 트레이트 객체: `Box<dyn Trait>` 동적 디스패치
- `#[derive]` 매크로로 공통 트레이트 자동 구현
- Supertrait: 트레이트 상속 (`TraitA: TraitB`)
- `From`/`Into` 트레이트: 타입 변환

```rust
trait Summary {
    fn summarize(&self) -> String;
    fn summarize_author(&self) -> String { "기본 구현".to_string() }
}

impl Summary for Article {
    fn summarize(&self) -> String {
        format!("{} - {}", self.headline, self.location)
    }
}

fn notify(item: &impl Summary) { println!("{}", item.summarize()); }
```

## 실행
```bash
cd RUST/10-traits && cargo run
```

## 핵심 요점
- 트레이트는 여러 타입 간의 공유 동작을 추상화
- `impl Trait`은 정적 디스패치, `dyn Trait`은 동적 디스패치
- 트레이트 객체 사용 시 `Box<dyn Trait>` 또는 `&dyn Trait`
- orphan rule: 외부 타입에 외부 트레이트 구현 불가 (둘 중 하나는 로컬)
