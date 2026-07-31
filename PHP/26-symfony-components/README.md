# 26: Symfony 컴포넌트 — 라우터, Console 컴포넌트 개념

## Symfony 컴포넌트

Symfony는 프레임워크인 동시에 **개별 컴포넌트 모음**입니다. 필요한 부품만 Composer로 가져올 수 있습니다.

```bash
composer require symfony/routing symfony/console symfony/http-foundation
```

## Console 컴포넌트

CLI 명령어를 표준화된 방식으로 만드는 도구입니다. Laravel Artisan, Drush(Drupal) 등이 이를 기반으로 합니다.

```php
class HelloCommand extends Command {
    protected string $name = 'app:hello';

    protected function execute(Input $input, Output $output): int {
        $output->writeln('안녕하세요!');
        return Command::SUCCESS;   // 0, 실패 시 1
    }
}
```

- `Input` — 인자/옵션 파싱
- `Output` — 텍스트 출력, 색상, 테이블
- `Application` — 명령어 등록·실행, `list` 명령어로 목록 표시

## Routing 컴포넌트

URL을 컨트롤러와 연결합니다.

```php
$routes->add('user_show', new Route('/users/{id}', 'App\Controller\UserController::show'));
$match = $matcher->match('GET', '/users/42');
// ['_controller' => '...', 'parameters' => ['id' => 42]]
```

- `{id}`처럼 중괄호로 **동적 파라미터**를 표현
- `requirements`로 정규식 제약 (`'id' => '\d+'`)
- `methods`로 HTTP 메서드 제한
- 매칭 실패 시 404 예외

## 기본 디렉토리 구조

```
bin/console          # CLI 진입점
src/Controller/      # 컨트롤러
src/Entity/          # 엔티티
config/routes.yaml   # 라우트 정의
```

## 실행

```bash
php index.php
```
