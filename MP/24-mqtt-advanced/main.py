# 24: MQTT 고급 — QoS, Retained, TLS, Last Will
# 대상: ESP32 + Wi-Fi, Mosquitto 브로커 (테스트용) 또는 클라우드 브로커
from umqtt.simple import MQTTClient
import network
import time
import os

# --- 네트워크 설정 (실제 값으로 교체) -------------------------------
SSID = "YOUR_WIFI_SSID"
PASSWORD = "YOUR_WIFI_PASSWORD"
BROKER = "test.mosquitto.org"      # 공용 테스트 브로커
PORT = 8883                        # TLS 포트 (1883은 평문)
CLIENT_ID = "esp32-adv-001"

# TLS 사용 시 CA 인증서를 보드 플래시에 저장한 경로
CA_CERT = "/cert/ca.crt"
TOPIC_STATUS = "iot/adv/status"    # will(유언) 토픽
TOPIC_DATA = "iot/adv/data"

def connect_wifi():
    wlan = network.WLAN(network.STA_IF)
    wlan.active(True)
    wlan.connect(SSID, PASSWORD)
    while not wlan.isconnected():
        time.sleep(0.5)
    print("Wi-Fi 연결:", wlan.ifconfig())


def main():
    connect_wifi()

    # --- TLS 컨텍스트 (암호화 연결) ---------------------------------
    import ssl
    tls_ctx = None
    if os.path.exists(CA_CERT):
        tls_ctx = ssl.SSLContext(ssl.PROTOCOL_TLS_CLIENT)
        tls_ctx.load_verify_locations(CA_CERT)
        print("CA 인증서 로드 완료")
    else:
        print(f"경고: {CA_CERT} 없음 — 인증서 확인 생략(테스트용)")

    client = MQTTClient(
        CLIENT_ID,
        BROKER,
        port=PORT,
        ssl=tls_ctx,
    )

    # --- Last Will (유언): 갑작스러운 연결 종료 시 브로커가 전송 ----
    client.set_last_will(TOPIC_STATUS, b"offline", qos=1, retain=True)

    def on_msg(topic, msg):
        print(f"수신: {topic.decode()} → {msg.decode()}")

    client.set_callback(on_msg)
    client.connect()
    print(f"연결됨: {BROKER}:{PORT} (TLS)")

    # --- Retained 메시지: 브로커에 마지막 값 저장 -------------------
    # 구독자 없이 발행해도 브로커가 보관, 새 구독자가 즉시 받음
    client.publish(TOPIC_STATUS, b"online", qos=1, retain=True)
    print("status=online 발행 (retain)")

    # --- QoS 0/1/2 차이 -------------------------------------------------
    # QoS 0: 전달 보장 없음(빠름) / QoS 1: 최소 1회 보장(ACK) / QoS 2: 정확히 1회
    client.publish(TOPIC_DATA, b"hello-qos0", qos=0)
    client.publish(TOPIC_DATA, b"hello-qos1", qos=1)   # PUBACK 확인
    print("QoS 0/1 발행 완료")

    # --- 구독 + 콜백 ----------------------------------------------------
    client.subscribe("iot/adv/command", qos=1)
    print("iot/adv/command 구독 중")

    last_pub = time.ticks_ms()
    while True:
        # 5초마다 QoS 1 + retain으로 센서 값 발행
        if time.ticks_diff(time.ticks_ms(), last_pub) > 5000:
            temp = 21.5 + (time.ticks_ms() % 100) / 100.0
            client.publish(TOPIC_DATA, f"{temp:.2f}".encode(), qos=1, retain=True)
            last_pub = time.ticks_ms()
        client.check_msg()          # 도착한 메시지 처리
        time.sleep_ms(50)


if __name__ == "__main__":
    main()
