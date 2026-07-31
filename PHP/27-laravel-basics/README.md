# 27: Laravel 기초 — 설치, 라우팅, 컨트롤러, 블레이드 개념

## 설치

```bash
composer create-project laravel/laravel my-app "^11.0"
cd my-app
php artisan serve   # http://127.0.0.1:8000
```

`php artisan`이 콘솔 진입점입니다. `list`로 모든 명령어를 볼 수 있습니다.

## 디렉토리 구조

| 경로 | 역할 |
|------|------|
| `routes/web.php` | 웹 라우트 정의 |
| `app/Http/Controllers` | 컨트롤러 |
| `app/Models` | Eloquent 모델 |
| `resources/views` | 블레이드 템플릿 |
| `config/` | 환경 설정 |
| `database/migrations` | 스키마 버전 관리 |

## 라우팅

```php
Route::get('/', fn () => 'Hello');
Route::get('/users/{id}', [UserController::class, 'show']);
Route::post('/users', [UserController::class, 'store']);
```

- `{id}` 경로 파라미터, `->where('id', '[0-9]+')` 제약
- `->name('users.show')`로 라우트 이름 부여
- `Route::resource('users', UserController::class)`로 RESTful 라우트 한 번에 등록

## 컨트롤러

```bash
php artisan make:controller UserController
```

```php
class UserController extends Controller {
    public function show(int $id) {
        return view('users.show', ['user' => User::findOrFail($id)]);
    }
}
```

## 블레이드

```blade
<h1>{{ $user->name }}</h1>          {{-- 이스케이프 출력 --}}
{!! $html !!}                        {{-- 이스케이프 없이 출력 --}}

@if ($user->isAdmin)
    관리자입니다.
@else
    일반 사용자입니다.
@endif

@foreach ($users as $user)
    <li>{{ $user->name }}</li>
@endforeach
```

레이아웃은 `@extends('layouts.app')` + `@section` / `@yield`으로 구성합니다.

## 실행

```bash
php index.php
```
