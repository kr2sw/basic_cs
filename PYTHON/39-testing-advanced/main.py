"""
39: 고급 테스팅 — fixture, parametrize, mock (pytest 없이도 자체 실행기 제공)
pytest로 실행하려면: python -m pytest .  (pip install pytest)
"""
import time
import unittest.mock as mock


# ---------- 테스트 대상 코드 ----------
def add(a, b):
    return a + b


def divide(a, b):
    if b == 0:
        raise ZeroDivisionError("0으로 나눌 수 없음")
    return a / b


class WeatherAPI:
    """외부 API를 호출한다고 가정하는 클래스 (실제로는 외부 호출 없음)"""
    def get_temperature(self, city):
        # 실제로는 requests로 API를 호출한다고 상상
        raise NotImplementedError("외부 네트워크 호출 위치")

    def report(self, city):
        temp = self.get_temperature(city)
        return f"{city}의 현재 온도는 {temp}°C"


def retry(func, times=3, delay=0.01):
    """일시적 실패 시 재시도하는 함수"""
    for attempt in range(times):
        try:
            return func()
        except ConnectionError:
            if attempt == times - 1:
                raise
            time.sleep(delay)


# ---------- 테스트 (pytest 스타일, 자체 실행기도 있음) ----------
def test_add_positive():
    assert add(1, 2) == 3


def test_add_negative():
    assert add(-1, -1) == -2


# parametrize를 흉내: 입력/기대값 목록을 돌면서 각각 검증
def test_add_parametrized():
    cases = [(1, 2, 3), (0, 0, 0), (-1, 1, 0), (100, -100, 0)]
    for a, b, expected in cases:
        yield f"add({a},{b})", a, b, expected


def test_divide():
    assert divide(10, 2) == 5
    assert divide(1, 3) == 1 / 3


def test_divide_by_zero():
    try:
        divide(1, 0)
        assert False, "예외가 발생해야 함"
    except ZeroDivisionError:
        pass


# fixture를 흉내: 테스트마다 객체 생성
def test_weather_report_with_mock():
    api = WeatherAPI()
    with mock.patch.object(api, "get_temperature", return_value=23.5):
        assert api.report("서울") == "서울의 현재 온도는 23.5°C"


def test_weather_report_real_fails():
    api = WeatherAPI()
    try:
        api.report("서울")
        assert False, "NotImplementedError가 발생해야 함"
    except NotImplementedError:
        pass


# mock으로 retry 로직 검증 (실제 sleep 없이)
def test_retry_with_mock():
    call_counts = {"n": 0}

    def flaky():
        call_counts["n"] += 1
        if call_counts["n"] < 3:
            raise ConnectionError("일시적 오류")
        return "성공"

    with mock.patch("time.sleep"):  # sleep을 mock 처리해 테스트를 빠르게
        result = retry(flaky, times=3, delay=0.01)
    assert result == "성공"
    assert call_counts["n"] == 3


# ---------- 자체 테스트 실행기 (pytest 대체) ----------
def run_own_tests():
    print("=== 자체 테스트 실행 (pytest 흉내) ===")
    tests = [test_add_positive, test_add_negative, test_divide,
             test_divide_by_zero, test_weather_report_with_mock,
             test_weather_report_real_fails, test_retry_with_mock]

    passed = failed = 0
    for fn in tests:
        try:
            fn()
            print(f"  [PASS] {fn.__name__}")
            passed += 1
        except AssertionError as e:
            print(f"  [FAIL] {fn.__name__}: {e}")
            failed += 1

    # parametrize 시뮬레이션 테스트
    for name, a, b, expected in test_add_parametrized():
        try:
            assert add(a, b) == expected
            print(f"  [PASS] {name}")
            passed += 1
        except AssertionError:
            print(f"  [FAIL] {name}")
            failed += 1

    print(f"\n결과: {passed} 통과, {failed} 실패")


if __name__ == "__main__":
    run_own_tests()
