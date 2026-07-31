# 24: MQTT 고급 — QoS, Retained, TLS, Last Will

## 개요

기초 과정에서 MQTT 발행/구독을 배웠다면, 이번 레슨에서는 실무 IoT에서 필수인 **QoS, Retained 메시지, TLS 암호화, Last Will(유언)** 를 다룹니다.

## QoS (Quality of Service)

메시지 전달 보증 수준을 지정합니다. `publish()`의 `qos` 인자로 설정합니다.

```python
client.publish("iot/data", b"value", qos=1)
```

- **QoS 0**: 전달 보장 없음, 브로커 부하 최소 (센서 스트림에 적합)
- **QoS 1**: 최소 1회 전달, 중복 가능 (ACK로 확인, 대부분의 데이터에 적합)
- **QoS 2**: 정확히 1회 전달, 왕복 핸드셰이크 (결제/명령에 적합)

## Retained 메시지

브로커가 마지막 메시지를 **저장**했다가, 새 구독자에게 즉시 전달합니다.

```python
client.publish("iot/status", b"online", retain=True)
# 나중에 구독을 시작한 장치는 "online"을 즉시 받음
```

온도 최신값, 기기 상태 등 "마지막 값이 중요한" 토픽에 적합합니다.

## TLS 암호화

포트 8883(TLS)으로 연결하고 CA 인증서를 검증합니다. 신뢰할 수 없는 네트워크에서도 데이터가 암호화됩니다.

```python
import ssl, os
ctx = ssl.SSLContext(ssl.PROTOCOL_TLS_CLIENT)
ctx.load_verify_locations("/cert/ca.crt")
client = MQTTClient("id", "broker", port=8883, ssl=ctx)
```

## Last Will (유언)

연결 시 `set_last_will()`로 등록한 메시지는, **정상적인 DISCONNECT 없이** 연결이 끊어질 때 브로커가 대신 발행합니다. 전원 끊김, 네트워크 장애 등 오프라인 감지에 사용합니다.

```python
client.set_last_will("iot/status", b"offline", qos=1, retain=True)
```

다른 장치는 이 토픽을 구독해 상대 기기의 갑작스러운 이탈을 감지할 수 있습니다.

## 실행/업로드 방법

1. **Thonny IDE**: `MP/24-mqtt-advanced/main.py` 열고 실행(F5). Wi-Fi 정보와 브로커 주소를 실제 값으로 수정하세요.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/24-mqtt-advanced/main.py
   ampy --port COM3 run MP/24-mqtt-advanced/main.py
   ```
3. PC에서 `mosquitto_sub -h test.mosquitto.org -t "iot/adv/#"`로 전송 확인.
4. 연결 상태에서 보드 전원을 끄면 will 메시지 `offline`이 자동 발행됩니다.

## 핵심 개념 요약

- QoS 0/1/2는 신뢰성과 속도의 트레이드오프
- `retain=True`는 "마지막 값" 저장, 새 구독자에게 즉시 전달
- TLS로 Wi-Fi 구간 암호화, will 메시지로 오프라인 감지
