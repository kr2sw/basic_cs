<?php
// --- WebSocket: Ratchet 개념, 서버/클라이언트 예제 구조 ---

echo "=== 1. WebSocket 소개 ===\n";
echo "  HTTP 요청-응답이 끝나도 연결을 유지해 양방향 통신하는 프로토콜\n";
echo "  채팅, 알림, 실시간 데이터(주문/주가)에 사용\n";
echo "  HTTP 폴링(주기적 요청) 대비 지연과 트래픽이 감소\n\n";

echo "=== 2. 핸드셰이크 (Upgrade) ===\n";

// RFC 6455에 정의된 고정 GUID
const WEBSOCKET_GUID = '258EAFA5-E914-47DA-95CA-C5AB0DC85B11';

function computeAcceptKey(string $secWebSocketKey): string {
    return base64_encode(sha1($secWebSocketKey . WEBSOCKET_GUID, true));
}

// RFC 6455 문서의 예제 키
$clientKey = 'dGhlIHNhbXBsZSBub25jZQ==';
$accept = computeAcceptKey($clientKey);

echo "  요청 헤더:\n";
echo "    GET /chat HTTP/1.1\n";
echo "    Upgrade: websocket\n";
echo "    Connection: Upgrade\n";
echo "    Sec-WebSocket-Key: $clientKey\n";
echo "  응답 헤더:\n";
echo "    HTTP/1.1 101 Switching Protocols\n";
echo "    Sec-WebSocket-Accept: $accept\n";
echo "    (RFC 6455 예상값: s3pPLMBiTxaQ9kYGzzhZRbK+xOo=)\n\n";

echo "=== 3. Ratchet 구조 ===\n";
echo "  composer require cboden/ratchet\n";
echo "  MessageComponentInterface 4가지 콜백:\n";
echo "    onOpen()    연결 수립 시\n";
echo "    onMessage() 메시지 수신 시\n";
echo "    onClose()   연결 종료 시\n";
echo "    onError()   오류 발생 시\n\n";

echo "=== 4. 채팅 서버 시뮬레이션 ===\n\n";

class Connection {
    public function __construct(
        public string $id,
        public ?string $nickname = null
    ) {}

    public function send(string $message): void {
        echo "      [→ {$this->id}] $message\n";
    }
}

class ChatServer {
    private array $connections = [];

    public function onOpen(Connection $conn): void {
        $this->connections[$conn->id] = $conn;
        echo "  [onOpen] {$conn->id} 연결 수립 (접속자 " . count($this->connections) . "명)\n";
    }

    public function onMessage(Connection $from, string $message): void {
        // 첫 메시지는 닉네임 설정
        if ($from->nickname === null) {
            $from->nickname = $message;
            $from->send("닉네임 설정 완료: {$from->nickname}");
            return;
        }

        $line = "{$from->nickname}: $message";
        $this->broadcast($line, $from);   // 다른 사용자에게
        $from->send("(나) $line");       // 본인에게
    }

    public function onClose(Connection $conn): void {
        unset($this->connections[$conn->id]);
        $name = $conn->nickname ?? $conn->id;
        $this->broadcast("[$name 님이 퇴장했습니다.]");
        echo "  [onClose] {$conn->id} 연결 종료\n";
    }

    private function broadcast(string $message, ?Connection $except = null): void {
        foreach ($this->connections as $conn) {
            if ($except !== null && $conn->id === $except->id) {
                continue;
            }
            $conn->send($message);
        }
    }
}

$server = new ChatServer();

$c1 = new Connection('conn-1');
$c2 = new Connection('conn-2');
$c3 = new Connection('conn-3');

$server->onOpen($c1);
$server->onOpen($c2);
$server->onOpen($c3);

echo "\n  [클라이언트 conn-1] 닉네임 설정: Alice\n";
$server->onMessage($c1, 'Alice');
echo "  [클라이언트 conn-2] 닉네임 설정: Bob\n";
$server->onMessage($c2, 'Bob');

echo "\n  [클라이언트 conn-1] 메시지 전송: 안녕하세요!\n";
$server->onMessage($c1, '안녕하세요!');
echo "  [클라이언트 conn-3] 메시지 전송: 저도 참여할게요\n";
$server->onMessage($c3, 'Carol');

echo "\n  [클라이언트 conn-3] 연결 종료\n";
$server->onClose($c3);
echo "\n";

echo "=== 5. 실제 Ratchet 서버 파일 (개념 코드) ===\n";
$serverCode = <<<'PHP'
<?php
// chat-server.php — composer require cboden/ratchet 후 실행
// php chat-server.php
use Ratchet\MessageComponentInterface;
use Ratchet\ConnectionInterface;

class Chat implements MessageComponentInterface {
    protected $clients;

    public function __construct() {
        $this->clients = new \SplObjectStorage;
    }

    public function onOpen(ConnectionInterface $conn) {
        $this->clients->attach($conn);
        echo "새 연결: {$conn->resourceId}\n";
    }

    public function onMessage(ConnectionInterface $from, $msg) {
        foreach ($this->clients as $client) {
            if ($from !== $client) {
                $client->send($msg);
            }
        }
    }

    public function onClose(ConnectionInterface $conn) {
        $this->clients->detach($conn);
    }

    public function onError(ConnectionInterface $conn, \Exception $e) {
        echo "오류: {$e->getMessage()}\n";
        $conn->close();
    }
}

$app = new \Ratchet\App('localhost', 8080);
$app->route('/chat', new Chat);
$app->run();
PHP;
echo $serverCode . "\n\n";

echo "=== 6. 클라이언트 (브라우저 JS) 예제 ===\n";
$clientCode = <<<'JS'
// ws://localhost:8080/chat 연결
const ws = new WebSocket('ws://localhost:8080/chat');

ws.onopen = () => {
    ws.send('Alice');          // 닉네임
    ws.send('안녕하세요!');     // 채팅 메시지
};

ws.onmessage = (event) => {
    console.log('수신:', event.data);
};

ws.onclose = () => {
    console.log('연결 종료');
};
JS;
echo $clientCode . "\n";
