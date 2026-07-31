# 36: 성능 최적화 — opcache, 지연 로딩, 프로파일링

## 측정부터 (프로파일링)

최적화 전에 반드시 **측정**해야 합니다. 추측으로 고치면 오히려 나빠질 수 있습니다.

```php
$start = hrtime(true);
// 작업 실행
$ms = (hrtime(true) - $start) / 1e6;
```

PHPUnit 벤치마크용 도구로 `phpbench/phpbench`가 있습니다. Xdebug + PHPStorm 등으로 함수 단위 프로파일도 가능합니다.

## OPcache

컴파일된 바이트코드를 메모리에 캐시합니다. PHP 성능의 첫 번째 기반입니다.

```ini
opcache.enable=1
opcache.memory_consumption=128
opcache.max_accelerated_files=10000
opcache.validate_timestamps=0   ; 배포 후 캐시 재시작
```

## 지연 로딩 (Lazy Loading)

비싼 객체는 **사용하는 시점에** 생성합니다.

```php
class App {
    private ?HeavyService $service = null;

    public function getService(): HeavyService {
        return $this->service ??= new HeavyService();
    }
}
```

## 메모리 관리

- 큰 배열은 `unset()`으로 참조를 해제
- `memory_get_usage()`, `memory_get_peak_usage()`로 확인
- 대용량 루프는 한 번에 모으지 말고 스트리밍/배치 처리

## 빠른 패턴 몇 가지

| 상황 | 더 빠른 방법 |
|------|-------------|
| 문자열 반복 결합 | 배열에 담았다가 `implode()` |
| 배열에 추가 | `$arr[] = $x` (array_push보다 빠름) |
| 리스트 포함 검사 | `in_array` 대신 키 검색(`isset`) |
| 반복 쿼리 | N+1 대신 한 번에 조회 + 관계 매핑 |

## 실행

```bash
php index.php
```
