<?php
// 한 줄 주석
# 한 줄 주석 (비권장)
/*
  여러 줄 주석
*/

// 기본 출력
echo "Hello, World!\n";
print "print 함수로 출력\n";

// echo에 여러 인자
echo "Hello", " ", "PHP", "\n";

// 변수
$name = "홍길동";
$age = 25;
echo "이름: $name, 나이: $age\n";

// 변수 파싱 (큰따옴표)
echo "제 이름은 $name입니다.\n";

// 작은따옴표 (변수 파싱 안 함)
echo '제 이름은 $name입니다.\n';

// 타입 확인
var_dump($name);
var_dump($age);
var_dump(3.14);
var_dump(true);

// 명령줄 인수
echo "argc = $argc\n";
print_r($argv);
