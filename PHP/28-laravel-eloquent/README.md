# 28: Eloquent ORM — 모델, 관계(1:N, N:M) 개념

## 모델 (Model)

Eloquent는 테이블 하나를 하나의 모델 클래스에 매핑합니다.

```php
class User extends Model {
    protected $fillable = ['name', 'email'];  // 대량 할당 허용 필드
}
```

```bash
php artisan make:model User -m   # 모델 + 마이그레이션 생성
```

## 관계 (Relationships)

### 1:N — hasMany / belongsTo

```php
// User
public function posts() {
    return $this->hasMany(Post::class);          // user_id FK
}

// Post
public function user() {
    return $this->belongsTo(User::class);
}

$user->posts;  // Collection
$post->user;   // User
```

### N:M — belongsToMany

중간(피벗) 테이블 `role_user`를 거칩니다.

```php
public function roles() {
    return $this->belongsToMany(Role::class);    // role_user
}

$user->roles;
$user->roles()->attach($roleId);   // 관계 추가
```

## 접근자 / 뮤테이터

```php
public function getFullNameAttribute() { ... }   // $user->full_name
public function setPasswordAttribute($v) { ... } // 저장 시 해시 처리
```

## 지역 스코프

```php
public function scopeActive($query) {
    return $query->where('status', 'active');
}
User::active()->get();
```

## 컬렉션과 findOrFail

```php
User::all();          // 전체
User::find(1);        // 단일 (없으면 null)
User::findOrFail(1);  // 없으면 404 예외
User::where('name', 'Alice')->first();
```

## 실행

```bash
php index.php
```
