<?php
date_default_timezone_set('Asia/Seoul');

echo "=== date() 함수 ===\n";
echo "Y-m-d H:i:s: " . date('Y-m-d H:i:s') . "\n";
echo "Y년 m월 d일: " . date('Y년 m월 d일') . "\n";
echo "요일: " . date('l') . "\n";
echo "타임스탬프: " . time() . "\n";

// strtotime
echo "\n=== strtotime ===\n";
echo "내일: " . date('Y-m-d', strtotime('+1 day')) . "\n";
echo "일주일 후: " . date('Y-m-d', strtotime('+1 week')) . "\n";
echo "한 달 전: " . date('Y-m-d', strtotime('-1 month')) . "\n";
echo "다음 월요일: " . date('Y-m-d', strtotime('next Monday')) . "\n";
echo "크리스마스: " . date('Y-m-d', strtotime('2026-12-25')) . "\n";

// DateTime 클래스
echo "\n=== DateTime 클래스 ===\n";
$now = new DateTime();
echo "현재: " . $now->format('Y-m-d H:i:s') . "\n";

$christmas = new DateTime('2026-12-25');
echo "크리스마스: " . $christmas->format('Y-m-d') . "\n";

// 날짜 차이 (diff)
$diff = $now->diff($christmas);
echo "크리스마스까지: {$diff->days}일\n";
echo "  {$diff->y}년 {$diff->m}월 {$diff->d}일 남음\n";

// modify
$date = new DateTime('2026-07-30');
$date->modify('+1 week');
echo "일주일 후: " . $date->format('Y-m-d') . "\n";

$date->modify('first day of next month');
echo "다음 달 첫날: " . $date->format('Y-m-d') . "\n";

// add / sub (DateInterval)
$date = new DateTime('2026-07-30');
$date->add(new DateInterval('P1Y2M')); // 1년 2월 추가
echo "1년 2월 후: " . $date->format('Y-m-d') . "\n";

$date->sub(new DateInterval('P3M')); // 3월 감소
echo "3개월 전: " . $date->format('Y-m-d') . "\n";

// 시간대 변환
echo "\n=== 시간대 ===\n";
$seoul = new DateTime('now', new DateTimeZone('Asia/Seoul'));
echo "서울: " . $seoul->format('Y-m-d H:i:s') . "\n";

$ny = clone $seoul;
$ny->setTimezone(new DateTimeZone('America/New_York'));
echo "뉴욕: " . $ny->format('Y-m-d H:i:s') . "\n";

$london = clone $seoul;
$london->setTimezone(new DateTimeZone('Europe/London'));
echo "런던: " . $london->format('Y-m-d H:i:s') . "\n";

// 날짜 비교
echo "\n=== 날짜 비교 ===\n";
$d1 = new DateTime('2026-07-30');
$d2 = new DateTime('2026-12-25');

echo "d1 < d2: " . ($d1 < $d2 ? "true" : "false") . "\n";
echo "d1 == d2: " . ($d1 == $d2 ? "true" : "false") . "\n";
echo "diff days: " . $d1->diff($d2)->days . "\n";
