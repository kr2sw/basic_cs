#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#define SCREEN_W 128
#define SCREEN_H 64
#define OLED_ADDR 0x3C

Adafruit_SSD1306 display(SCREEN_W, SCREEN_H, &Wire, -1);

// 버튼 (INPUT_PULLUP, 누르면 LOW)
const int BTN_UP = 2;
const int BTN_DOWN = 3;
const int BTN_SELECT = 4;

// 메뉴 항목
const char* menuItems[] = {"온도", "습도", "조도", "정보"};
const int MENU_COUNT = 4;

int selected = 0;
int screen = 0;  // 0=메뉴, 1=데이터 표시

// 가상 센서 값
float temp = 24.5, hum = 55.0;
int light = 512;

void drawMenu() {
  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(20, 0);
  display.println("== MENU ==");

  for (int i = 0; i < MENU_COUNT; i++) {
    display.setCursor(10, 12 + i * 12);
    if (i == selected) {
      display.print("> ");  // 현재 선택 표시
      display.print(menuItems[i]);
    } else {
      display.print("  ");
      display.print(menuItems[i]);
    }
  }
  display.display();
}

void drawSensorScreen() {
  display.clearDisplay();
  display.setCursor(0, 0);
  display.setTextSize(2);
  display.println(menuItems[selected]);
  display.setTextSize(1);
  display.setCursor(0, 24);

  switch (selected) {
    case 0:
      display.print("Temp: ");
      display.print(temp);
      display.println(" C");
      break;
    case 1:
      display.print("Hum: ");
      display.print(hum);
      display.println(" %");
      break;
    case 2:
      display.print("Light: ");
      display.print(light);
      break;
    case 3:
      display.println("Arduino OLED");
      display.println("중급 30장");
      display.println("2026");
      break;
  }

  display.setCursor(0, 56);
  display.println("B: 뒤로");
  display.display();
}

void setup() {
  Serial.begin(9600);

  pinMode(BTN_UP, INPUT_PULLUP);
  pinMode(BTN_DOWN, INPUT_PULLUP);
  pinMode(BTN_SELECT, INPUT_PULLUP);

  if (!display.begin(SSD1306_SWITCHCAPVCC, OLED_ADDR)) {
    Serial.println("OLED 초기화 실패!");
    while (1) delay(10);
  }
  display.clearDisplay();

  // 시작 애니메이션: 원 그리기
  for (int r = 1; r <= 30; r += 2) {
    display.clearDisplay();
    display.drawCircle(64, 32, r, SSD1306_WHITE);
    display.display();
    delay(20);
  }
  display.clearDisplay();
  Serial.println("OLED 메뉴 시작");
}

void loop() {
  if (screen == 0) {
    drawMenu();
    // 위/아래 버튼으로 메뉴 이동
    if (digitalRead(BTN_UP) == LOW) {
      selected = (selected - 1 + MENU_COUNT) % MENU_COUNT;
      delay(150);  // 디바운스
    }
    if (digitalRead(BTN_DOWN) == LOW) {
      selected = (selected + 1) % MENU_COUNT;
      delay(150);
    }
    if (digitalRead(BTN_SELECT) == LOW) {
      screen = 1;  // 선택한 메뉴 화면으로
      delay(150);
    }
  } else {
    drawSensorScreen();
    if (digitalRead(BTN_SELECT) == LOW) {
      screen = 0;  // 메뉴로 복귀
      delay(150);
    }
  }

  // 센서 값 갱신 (시뮬레이션)
  temp += random(-5, 6) / 10.0;
  hum += random(-3, 4);
  light += random(-20, 21);
  delay(100);
}
