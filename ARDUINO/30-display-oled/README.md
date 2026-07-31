# 30: OLED/TFT 디스플레이 — OLED/TFT Display

I2C SSD1306 OLED에 그래픽을 그리고 버튼으로 조작하는 메뉴 UI를 만듭니다.

## 학습 내용
- Adafruit SSD1306 + GFX 라이브러리
- 텍스트, 도형, 선 그리기
- 좌표계와 기본 도형 함수
- 버튼 입력 기반 메뉴 UI

## SSD1306 OLED

128×64 픽셀 OLED는 I2C로 연결되고, `Adafruit_SSD1306`와 `Adafruit_GFX` 라이브러리로 제어합니다. GFX 라이브러리는 도형/텍스트/이미지 그리기 함수를 제공합니다.

```cpp
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
Adafruit_SSD1306 display(128, 64, &Wire, -1);

display.begin(SSD1306_SWITCHCAPVCC, 0x3C);
display.clearDisplay();
display.display();
```

## 기본 그리기 함수

좌표는 왼쪽 위가 (0,0)이고 오른쪽/아래로 커집니다. 그린 후 `display.display()`를 호출해야 화면에 반영됩니다.

```cpp
display.setTextSize(2);
display.setTextColor(SSD1306_WHITE);
display.setCursor(0, 0);
display.println("Hello");
display.drawRect(10, 20, 50, 20, SSD1306_WHITE);  // 사각형
display.drawCircle(90, 30, 15, SSD1306_WHITE);    // 원
display.fillRect(0, 50, 128, 14, SSD1306_WHITE);  // 채운 사각형
display.display();
```

## 메뉴 UI

선택 항목 배열과 `selectedIndex`를 두고 버튼(위/아래)으로 인덱스를 바꾸면 메뉴가 됩니다.

```cpp
const char* menuItems[] = {"온도", "습도", "설정", "정보"};
int selected = 0;
// 위 버튼: selected = (selected - 1 + N) % N;
// 아래 버튼: selected = (selected + 1) % N;
```

## 회로 연결

| 부품 | Arduino Uno |
|------|-------------|
| OLED VCC | 5V |
| OLED GND | GND |
| OLED SDA | A4 |
| OLED SCL | A5 |
| 버튼 A | D2 (다른 쪽 GND) |
| 버튼 B | D3 (다른 쪽 GND) |

> 버튼은 `INPUT_PULLUP`을 사용해 외부 풀업 저항 없이 연결할 수 있습니다. (누르면 LOW)

## 실행 방법

1. **라이브러리 관리자**에서 `Adafruit SSD1306`, `Adafruit GFX Library`를 설치합니다.
2. OLED 모듈의 주소가 `0x3C`가 아니면 코드의 `0x3C`를 확인하세요(일부 모듈은 `0x3D`).
3. 이 챕터의 `.ino`를 업로드합니다.
4. 버튼으로 메뉴를 이동하고 선택한 항목의 화면이 표시됩니다.

## 응용 아이디어

- 센서(21, 27장) 값을 메뉴로 보여주는 대시보드
- 아이콘/애니메이션 게임(퐁, 스네이크)
- 40장(종합 프로젝트)의 기상 관측소 화면
