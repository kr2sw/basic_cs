# 20: MVC Pattern — MVC 패턴

## MVC (Model-View-Controller)

애플리케이션을 세 가지 역할로 분리하는 디자인 패턴입니다.

| 구성 요소 | 설명 |
|-----------|------|
| **Model** | 데이터와 비즈니스 로직 |
| **View** | 사용자 인터페이스 (표시) |
| **Controller** | 요청 처리, Model-View 연결 |

## 처리 흐름

```
Request → Router → Controller → Model → View → Response
```

## 간단한 라우터 (Front Controller)

모든 요청을 `index.php`에서 받아 URL에 따라 적절한 컨트롤러로 전달합니다.

### .htaccess (Apache)

```
RewriteEngine On
RewriteCond %{REQUEST_FILENAME} !-f
RewriteRule ^(.*)$ index.php [QSA,L]
```

### Nginx

```nginx
location / {
    try_files $uri $uri/ /index.php?$query_string;
}
```
