<?php
// --- 테스팅: PHPUnit 스타일 어설션과 미니 테스트 러너 (간단 구현) ---

echo "=== 1. 테스트 대상 클래스 ===\n";

class Calculator {
    public function add(int $a, int $b): int {
        return $a + $b;
    }

    public function divide(int $a, int $b): float {
        if ($b === 0) {
            throw new DivisionByZeroError('0으로 나눌 수 없습니다.');
        }
        return $a / $b;
    }

    public function isEven(int $n): bool {
        return $n % 2 === 0;
    }
}

class StringHelper {
    public static function reverse(string $s): string {
        return strrev($s);
    }

    public static function slugify(string $s): string {
        return strtolower(trim(preg_replace('/\s+/', '-', $s)));
    }
}

echo "테스트 대상 준비 완료\n\n";

echo "=== 2. 미니 테스트 프레임워크 ===\n\n";

abstract class TestCase {
    private int $assertionCount = 0;
    private array $failures = [];

    public function setUp(): void {}
    public function tearDown(): void {}

    public function getAssertionCount(): int {
        return $this->assertionCount;
    }

    public function getFailures(): array {
        return $this->failures;
    }

    protected function fail(string $message): void {
        $this->failures[] = $message;
    }

    // 값 비교 (==)
    protected function assertEquals(mixed $expected, mixed $actual, string $message = ''): void {
        $this->recordAssertion();
        if ($expected != $actual) {
            $this->fail($message ?: sprintf(
                'assertEquals 실패 — 기대: %s, 실제: %s',
                var_export($expected, true),
                var_export($actual, true)
            ));
        }
    }

    // 타입까지 비교 (===)
    protected function assertSame(mixed $expected, mixed $actual, string $message = ''): void {
        $this->recordAssertion();
        if ($expected !== $actual) {
            $this->fail($message ?: sprintf(
                'assertSame 실패 — 기대: %s, 실제: %s',
                var_export($expected, true),
                var_export($actual, true)
            ));
        }
    }

    protected function assertTrue(bool $condition, string $message = ''): void {
        $this->recordAssertion();
        if (!$condition) {
            $this->fail($message ?: 'assertTrue 실패');
        }
    }

    protected function assertFalse(bool $condition, string $message = ''): void {
        $this->recordAssertion();
        if ($condition) {
            $this->fail($message ?: 'assertFalse 실패');
        }
    }

    protected function assertNull(mixed $actual, string $message = ''): void {
        $this->recordAssertion();
        if ($actual !== null) {
            $this->fail($message ?: 'assertNull 실패');
        }
    }

    protected function assertCount(int $expected, array $actual, string $message = ''): void {
        $this->recordAssertion();
        if (count($actual) !== $expected) {
            $this->fail($message ?: "assertCount 실패 — 기대: $expected, 실제: " . count($actual));
        }
    }

    protected function assertStringContainsString(string $needle, string $haystack, string $message = ''): void {
        $this->recordAssertion();
        if (!str_contains($haystack, $needle)) {
            $this->fail($message ?: "assertStringContainsString 실패 — \"$needle\"을 찾을 수 없음");
        }
    }

    protected function assertInstanceOf(string $class, mixed $actual, string $message = ''): void {
        $this->recordAssertion();
        if (!$actual instanceof $class) {
            $this->fail($message ?: "assertInstanceOf 실패 — 기대: $class");
        }
    }

    // 예외 발생을 확인
    protected function expectException(callable $fn, string $exceptionClass): ?Throwable {
        $this->recordAssertion();
        try {
            $fn();
        } catch (Throwable $e) {
            if ($e instanceof $exceptionClass) {
                return $e;
            }
            $this->fail("기대 예외 {$exceptionClass}가 아닌 {$e::class} 발생: {$e->getMessage()}");
            return $e;
        }
        $this->fail("기대한 예외가 발생하지 않았습니다: $exceptionClass");
        return null;
    }

    private function recordAssertion(): void {
        $this->assertionCount++;
    }
}

class TestResult {
    public int $tests = 0;
    public int $passed = 0;
    public int $assertions = 0;
    public array $failures = [];

    public function isSuccessful(): bool {
        return $this->failures === [];
    }
}

class TestRunner {
    public function runTests(array $testClasses): TestResult {
        $result = new TestResult();

        foreach ($testClasses as $testClass) {
            foreach (get_class_methods($testClass) as $method) {
                if (!str_starts_with($method, 'test')) {
                    continue;
                }

                $result->tests++;
                $instance = new $testClass();   // 테스트마다 격리된 인스턴스
                $instance->setUp();

                try {
                    $instance->$method();
                    $result->assertions += $instance->getAssertionCount();

                    if ($instance->getFailures() === []) {
                        $result->passed++;
                        echo "  \033[32mPASS\033[0m " . basename(str_replace('\\', '/', $testClass)) . "::$method\n";
                    } else {
                        foreach ($instance->getFailures() as $failure) {
                            $result->failures[] = "$testClass::$method → $failure";
                            echo "  \033[31mFAIL\033[0m " . basename(str_replace('\\', '/', $testClass))
                                . "::$method → $failure\n";
                        }
                    }
                } catch (Throwable $e) {
                    $result->failures[] = "$testClass::$method → 예외: {$e->getMessage()}";
                    echo "  \033[31mERROR\033[0m $testClass::$method → {$e->getMessage()}\n";
                } finally {
                    $instance->tearDown();
                }
            }
        }

        return $result;
    }
}

echo "=== 3. 테스트 작성 ===\n\n";

class CalculatorTest extends TestCase {
    private Calculator $calc;

    public function setUp(): void {
        $this->calc = new Calculator();
    }

    public function testAddition(): void {
        $this->assertEquals(5, $this->calc->add(2, 3));
        $this->assertEquals(0, $this->calc->add(-1, 1));
    }

    public function testAdditionSameType(): void {
        $this->assertSame(7, $this->calc->add(3, 4));
    }

    public function testDivision(): void {
        $this->assertEquals(2.5, $this->calc->divide(5, 2));
    }

    public function testDivisionByZeroThrows(): void {
        $e = $this->expectException(
            fn() => $this->calc->divide(1, 0),
            DivisionByZeroError::class
        );
        $this->assertStringContainsString('0으로', $e->getMessage());
        $this->assertInstanceOf(DivisionByZeroError::class, $e);
    }

    public function testIsEven(): void {
        $this->assertTrue($this->calc->isEven(4));
        $this->assertFalse($this->calc->isEven(3));
    }
}

class StringHelperTest extends TestCase {
    public function testReverse(): void {
        $this->assertSame('olleh', StringHelper::reverse('hello'));
    }

    public function testSlugify(): void {
        $this->assertSame('hello-world', StringHelper::slugify('Hello   World'));
    }

    // 의도적인 실패 예시: 실패 출력을 보여주기 위함
    public function testIntentionallyFailing(): void {
        $this->assertEquals('hello', StringHelper::reverse('hello'));
    }
}

$result = (new TestRunner())->runTests([CalculatorTest::class, StringHelperTest::class]);

echo "\n=== 4. 결과 요약 ===\n";
printf("테스트: %d개, 통과: %d, 실패: %d, 어설션: %d회\n",
    $result->tests, $result->passed, count($result->failures), $result->assertions);
echo "전체 성공 여부: " . ($result->isSuccessful() ? 'true' : 'false') . "\n";
