(module
  (func $add (export "add") (param $a i32) (param $b i32) (result i32)
    local.get $a
    local.get $b
    i32.add
  )

  (func $mul (export "mul") (param $a i32) (param $b i32) (result i32)
    local.get $a
    local.get $b
    i32.mul
  )

  (memory (export "memory") 1)

  (data (i32.const 0) "WABT Tooling Demo!")

  (global $count (export "count") (mut i32) (i32.const 0))

  (func $inc (export "inc") (result i32)
    global.get $count
    i32.const 1
    i32.add
    global.set $count
    global.get $count
  )

  (export "add" (func $add))
  (export "mul" (func $mul))
)
