<?php
class Animal {
    protected string $name;

    public function __construct(string $name) {
        $this->name = $name;
    }

    public function speak(): void {
        echo "{$this->name}이(가) 소리를 냅니다.\n";
    }

    public function __toString(): string {
        return "Animal(name: {$this->name})";
    }
}

class Dog extends Animal {
    private string $breed;

    public function __construct(string $name, string $breed) {
        parent::__construct($name); // 부모 생성자 호출
        $this->breed = $breed;
    }

    // 오버라이딩
    public function speak(): void {
        echo "{$this->name}이(가) 멍멍 짖습니다!\n";
    }

    public function fetch(): void {
        echo "{$this->name}이(가) 공을 물어옵니다.\n";
    }

    public function getBreed(): string {
        return $this->breed;
    }
}

class Cat extends Animal {
    public function speak(): void {
        echo "{$this->name}이(가) 야옹 웁니다.\n";
    }
}

// final 클래스
final class GoldenRetriever extends Dog {
    public function __construct(string $name) {
        parent::__construct($name, "Golden Retriever");
    }
}

// 사용
$dog = new Dog("초코", "골든 리트리버");
$dog->speak();
$dog->fetch();

$cat = new Cat("나비");
$cat->speak();

// 다형성
$animals = [new Dog("멍멍이", "진돗개"), new Cat("야옹이")];
foreach ($animals as $animal) {
    $animal->speak(); // 동적 바인딩
}

// instanceof
echo "dog instanceof Animal: " . ($dog instanceof Animal ? "true" : "false") . "\n";
echo "dog instanceof Dog: " . ($dog instanceof Dog ? "true" : "false") . "\n";

// final 클래스
$golden = new GoldenRetriever("골드");
$golden->speak();
