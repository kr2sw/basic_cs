#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/*
 * [POSIX 전용 예제 - 주석으로만 제공합니다]
 * 소켓은 표준 C가 아니므로 플랫폼 API를 사용합니다.
 * gcc (Linux):  gcc server.c -o server
 * Windows: Winsock2 + WSAStartup 필요 (ws2_32.lib)
 *
 * --- TCP 에코 서버 (POSIX) ---
 * #include <sys/socket.h>
 * #include <netinet/in.h>
 * #include <unistd.h>
 *
 * int main(void) {
 *     int sfd = socket(AF_INET, SOCK_STREAM, 0);
 *     struct sockaddr_in addr = {0};
 *     addr.sin_family = AF_INET;
 *     addr.sin_port = htons(9000);
 *     addr.sin_addr.s_addr = INADDR_ANY;
 *     bind(sfd, (struct sockaddr*)&addr, sizeof(addr));
 *     listen(sfd, 5);
 *     int cfd = accept(sfd, NULL, NULL);        // 클라이언트 대기
 *     char buf[256];
 *     int n = recv(cfd, buf, sizeof(buf), 0);   // 수신
 *     send(cfd, buf, n, 0);                      // 에코
 *     close(cfd);
 *     close(sfd);
 * }
 *
 * --- TCP 클라이언트 (POSIX) ---
 * socket(AF_INET, SOCK_STREAM, 0) → connect(...) → send/recv → close
 */

// --- TCP 스트림 프레이밍(Framing) 예제 (표준 C로 구현) ---
// TCP는 스트림이라 메시지 경계가 없습니다. 길이 접두사를 붙여 경계를 만듭니다.
// [길이 1바이트][페이로드 ...] 형태의 프로토콜을 시뮬레이션합니다.

#define MAX_MSG 255

// 버퍼에 [len][data...]로 메시지를 "캡슐화" (송신 측 시뮬레이션)
int encodeMessage(unsigned char* out, const char* data) {
    int len = (int)strlen(data);
    if (len > MAX_MSG) return 0;            // 1바이트 길이 한계
    out[0] = (unsigned char)len;            // 길이 접두사
    memcpy(out + 1, data, len);
    return len + 1;                          // 총 프레임 크기
}

// 스트림에서 프레임을 하나 추출 (수신 측 시뮬레이션)
// 스트림이 잘려서 도착하는 상황도 함께 재현합니다.
typedef struct {
    unsigned char stream[256];
    int size;
    int pos;
} Stream;

int readFrame(Stream* s, char* outMsg, int cap) {
    if (s->size - s->pos < 1) return 0;              // 길이 바이트 부족
    int len = s->stream[s->pos];
    if (s->size - s->pos < 1 + len) return 0;        // 본문이 아직 안 옴
    if (len >= cap) return 0;
    memcpy(outMsg, s->stream + s->pos + 1, len);
    outMsg[len] = '\0';
    s->pos += 1 + len;                                // 프레임 소비
    return len;
}

void demoFraming(void) {
    printf("=== TCP 스트림 프레이밍 (길이 접두사) ===\n");
    printf("'hello'와 'world' 두 메시지를 프레임으로 묶어 전송합니다.\n\n");

    unsigned char packet[256];
    int p = 0;
    p += encodeMessage(packet + p, "hello");   // 프레임 1
    p += encodeMessage(packet + p, "world");   // 프레임 2

    // 수신 버퍼에 일부만 먼저 도착하는 상황 시뮬레이션
    Stream s = {{0}, 0, 0};
    memcpy(s.stream, packet, 3);               // 3바이트만 먼저 도착
    s.size = 3;

    char msg[64];
    printf("첫 3바이트 도착 후 프레임 추출 시도: ");
    if (readFrame(&s, msg, sizeof(msg)) == 0) {
        printf("프레임 불완전 (대기)\n");
    }

    memcpy(s.stream + s.size, packet + 3, p - 3);   // 나머지 도착
    s.size = p;

    printf("나머지 도착 후:\n");
    int count = 0;
    while (readFrame(&s, msg, sizeof(msg)) > 0) {
        printf("  수신 프레임: \"%s\"\n", msg);
        count++;
    }
    printf("총 %d개 프레임 수신\n", count);
}

int main() {
    printf("=== 소켓 프로그래밍 개념 (표준 C 데모) ===\n\n");

    demoFraming();

    printf("\n--- TCP/UDP 구조 ---\n");
    printf("TCP: socket → bind → listen → accept → recv/send → close\n");
    printf("UDP: socket → bind → recvfrom/sendto (연결 없음)\n");

    printf("\n※ 실제 소켓 API 사용 예제는 main.c 상단 주석을 참고하세요.\n");
    printf("  소켓은 표준 C가 아니므로 POSIX/Winsock 같은 플랫폼 API가 필요합니다.\n");
    return 0;
}
