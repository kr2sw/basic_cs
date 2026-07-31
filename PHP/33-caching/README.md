# 33: 캐싱 — OPcache, 파일 캐시, Redis 개념

## OPcache

PHP 소스의 **컴파일 결과(바이트코드)**를 메모리에 저장합니다. 매 요청마다 파싱/컴파일하지 않아 성능이 크게 향상됩니다.

```ini
opcache.enable=1
opcache.memory_consumption=128
opcache.max_accelerated_files=10000
opcache.validate_timestamps=1
opcache.revalidate_freq=2
```

프로덕션에서는 `validate_timestamps=0`으로 두고 배포 시 캐시를 재시작합니다.

## 파일 캐시

데이터를 파일로 저장하는 가장 간단한 캐시입니다.

```php
$cache->set('user:1', $user, 60);   // 60초 TTL
$user = $cache->get('user:1');
```

배포 환경이 단일 서버라면 파일 캐시만으로 충분한 경우가 많습니다.

## Redis (인메모리 캐시)

원격 메모리 기반 키-값 저장소입니다. 여러 서버가 공유하는 캐시에 적합합니다.

```bash
redis-cli SET user:1:name Alice
redis-cli GET user:1:name
redis-cli EXPIRE session:abc123 30
redis-cli INCR counter:visits
```

## Cache-Aside 패턴

1. 캐시 확인 → 있으면(HIT) 반환
2. 없으면(MISS) DB 조회
3. 결과를 캐시에 저장 후 반환

캐시가 없어도 DB 조회로 서비스가 유지되므로 장애 안전(fail-safe)합니다.

## 실행

```bash
php index.php
```
