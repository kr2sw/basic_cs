// 고정 주기 샘플링 + 시리얼 플로터/CSV 출력 예제
const int SENSOR_PIN = A0;

const unsigned long SAMPLE_MS = 100;  // 100ms (10Hz) 샘플링

unsigned long lastSample = 0;
int sampleCount = 0;
long sum = 0;
int minVal = 1023;
int maxVal = 0;

float smooth = 0;  // 지수 이동 평균
bool smoothInit = false;

void setup() {
  Serial.begin(9600);
  Serial.println("데이터 수집 시작 (100ms 간격 샘플링)");
  Serial.println("시리얼 플로터에서 raw/smooth 그래프 확인");
}

void sample() {
  int value = analogRead(SENSOR_PIN);

  // 지수 이동 평균 계산
  if (!smoothInit) {
    smooth = value;
    smoothInit = true;
  } else {
    smooth = smooth * 0.9 + value * 0.1;
  }

  // 시리얼 플로터용 출력 (2채널)
  Serial.print("raw:");
  Serial.print(value);
  Serial.print(" smooth:");
  Serial.println(smooth, 1);

  // 통계 갱신
  sampleCount++;
  sum += value;
  if (value < minVal) minVal = value;
  if (value > maxVal) maxVal = value;
}

void loop() {
  // 고정 주기 샘플링
  if (millis() - lastSample >= SAMPLE_MS) {
    lastSample = millis();
    sample();
  }

  // 10초마다 CSV 형식 통계 출력
  static unsigned long lastStats = 0;
  if (millis() - lastStats >= 10000) {
    lastStats = millis();

    float avg = (sampleCount > 0) ? (float)sum / sampleCount : 0;

    // CSV 형식: 시간,개수,평균,최소,최대
    Serial.print("STATS, ");
    Serial.print(lastStats);
    Serial.print(", ");
    Serial.print(sampleCount);
    Serial.print(", ");
    Serial.print(avg, 1);
    Serial.print(", ");
    Serial.print(minVal);
    Serial.print(", ");
    Serial.println(maxVal);

    // 통계 초기화
    sampleCount = 0;
    sum = 0;
    minVal = 1023;
    maxVal = 0;
  }
}
