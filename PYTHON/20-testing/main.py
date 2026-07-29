"""
# 실행 방법
# unittest:  python -m unittest main.py
# pytest:    python -m pytest main.py -v
"""

import unittest
from unittest.mock import Mock, patch


def add(a, b):
    return a + b


def divide(a, b):
    if b == 0:
        raise ValueError("Cannot divide by zero")
    return a / b


import json
import urllib.request


def get_user_name(user_id):
    """Mock external API call"""
    url = f"https://api.example.com/users/{user_id}"
    with urllib.request.urlopen(url) as resp:
        data = json.loads(resp.read())
    return data["name"]


class Calculator:
    def multiply(self, a, b):
        return a * b


class TestAssertDemo(unittest.TestCase):
    def test_add(self):
        self.assertEqual(add(2, 3), 5)
        self.assertEqual(add(-1, 1), 0)

    def test_divide(self):
        self.assertEqual(divide(10, 2), 5)
        self.assertAlmostEqual(divide(1, 3), 0.33333, places=4)

    def test_divide_by_zero(self):
        with self.assertRaises(ValueError):
            divide(5, 0)

    def test_calculator(self):
        calc = Calculator()
        self.assertEqual(calc.multiply(3, 4), 12)
        self.assertIsInstance(calc, Calculator)


class TestSetupTeardown(unittest.TestCase):
    def setUp(self):
        self.data = [1, 2, 3, 4, 5]

    def tearDown(self):
        self.data.clear()

    def test_sum(self):
        self.assertEqual(sum(self.data), 15)

    def test_len(self):
        self.assertEqual(len(self.data), 5)


class TestMocking(unittest.TestCase):
    @patch("main.urllib.request.urlopen")
    def test_get_user_name(self, mock_urlopen):
        mock_resp = Mock()
        mock_resp.read.return_value = json.dumps({"name": "Alice"}).encode()
        mock_urlopen.return_value.__enter__.return_value = mock_resp
        result = get_user_name(1)
        self.assertEqual(result, "Alice")
        mock_urlopen.assert_called_once()


# pytest-style tests (functions, no class needed)
def test_pytest_add():
    assert add(2, 3) == 5
    assert add(0, 0) == 0
    assert add(-1, 5) == 4


def test_pytest_divide():
    assert divide(10, 2) == 5
    import pytest
    with pytest.raises(ValueError):
        divide(1, 0)


def test_pytest_calculator():
    calc = Calculator()
    assert calc.multiply(3, 4) == 12
    assert isinstance(calc, Calculator)


# Fixture example (pytest-style, run with pytest)
try:
    import pytest
except ImportError:
    pytest = None


if pytest:

    @pytest.fixture
    def sample_data():
        return [10, 20, 30, 40, 50]

    def test_fixture_sum(sample_data):
        assert sum(sample_data) == 150

    def test_fixture_len(sample_data):
        assert len(sample_data) == 5


if __name__ == "__main__":
    unittest.main(verbosity=2)
