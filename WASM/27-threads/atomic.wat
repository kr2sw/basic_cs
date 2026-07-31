(module
  ;; 공유 메모리를 JS에서 import (초기 1페이지, 최대 2페이지, shared)
  (memory (export "memory") (import "env" "memory") 1 2 shared)

  ;; 카운터를 원자적으로 1 증가, 증가 전 값 반환
  (func (export "increment") (param $addr i32) (result i32)
    (i32.atomic.rmw.add (local.get $addr) (i32.const 1)))

  ;; 원자적 읽기
  (func (export "load") (param $addr i32) (result i32)
    (i32.atomic.load (local.get $addr)))

  ;; 원자적 쓰기
  (func (export "store") (param $addr i32) (param $value i32)
    (i32.atomic.store (local.get $addr) (local.get $value)))

  ;; $addr의 값이 $expected와 같을 때까지 대기
  ;; 반환: 0=대기 성공, 1=값이 이미 다름, 2=타임아웃
  (func (export "waitOn") (param $addr i32) (param $expected i32) (result i32)
    (i32.atomic.wait (local.get $addr) (local.get $expected) (i64.const -1)))

  ;; 대기 중인 스레드 하나 깨우기 (깨어난 개수 반환)
  (func (export "notifyOne") (param $addr i32) (result i32)
    (atomic.notify (local.get $addr) (i32.const 1)))

  ;; 대기 중인 스레드 모두 깨우기
  (func (export "notifyAll") (param $addr i32) (result i32)
    (atomic.notify (local.get $addr) (i32.const -1)))

  ;; 원자적이지 않은 증가 (데이터 레이스 데모용 — 실제 사용 금지)
  (func (export "incrementNaive") (param $addr i32)
    (local $v i32)
    (local.set $v (i32.load (local.get $addr)))
    (local.set $v (i32.add (local.get $v) (i32.const 1)))
    (i32.store (local.get $addr) (local.get $v)))
)
