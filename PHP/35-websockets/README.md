# 35: WebSocket — Ratchet 개념, 서버/클라이언트 예제 구조

## WebSocket이란

HTTP 요청-응답 후에도 **연결을 유지**해 서버가 클라이언트로 실시간 푸시할 수 있는 프로토콜입니다. 채팅, 알림, 실시간 주문에 사용합니다.

```
HTTP 요청 (Upgrade 헤더)
        ↓
WebSocket 연결 유지 (양방향 통신)
```

## 핸드셰이크

`Sec-WebSocket-Key` + 고정 GUID를 SHA1 → base64로 인코딩한 값을 `Sec-WebSocket-Accept`로 응답합니다.

```php
base64_encode(sha1($key . '258EAFA5-E914-47DA-95CA-C5AB0DC85B11', true));
```

## Ratchet

PHP 대표 WebSocket 라이브러리입니다.

```bash
composer require cboden/ratchet
```

```php
class Chat implements MessageComponentInterface {
    public function onOpen(ConnectionInterface $conn) { ... }
    public function onMessage(ConnectionInterface $from, $msg) { ... }
    public function onClose(ConnectionInterface $conn) { ... }
    public function onError(ConnectionInterface $conn, \Exception $e) { ... }
}

$app = new Ratchet\App('localhost', 8080);
$app->route('/chat', new Chat);
$app->run();
```

## 프레임 구조

```
0 1 2 3 4 5 6 7
|F|R S S S|opcode|   첫 바이트: FIN + opcode
|M|R R R R|       |
```
각 메시지는 **프레임** 단위로 전송되며, 클라이언트는 암호화를 위해 마스킹을 적용합니다.

## 클라이언트 (브라우저)

```js
const ws = new WebSocket('ws://localhost:8080/chat');
ws.onmessage = (e) => console.log(e.data);
ws.send('메시지');
```

## 실행

```bash
php index.php
```
