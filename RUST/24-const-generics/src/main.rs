// 24: 제네릭 심화 — const generics

use std::fmt::Debug;

// === 1. 고정 길이 배열 래퍼 ===
struct Array<T, const N: usize> {
    data: [T; N],
}

impl<T: Copy + Default, const N: usize> Array<T, N> {
    fn new() -> Self {
        Array { data: [T::default(); N] }
    }

    fn from_array(data: [T; N]) -> Self {
        Array { data }
    }

    fn len(&self) -> usize {
        N
    }

    fn get(&self, i: usize) -> Option<&T> {
        self.data.get(i)
    }
}

// === 2. 벡터 수학 (차원을 타입으로) ===
struct Vec3<T, const D: usize> {
    comps: [T; D],
}

impl<T, const D: usize> Vec3<T, D>
where
    T: std::ops::Mul<Output = T> + std::ops::Add<Output = T> + Copy,
{
    fn new(comps: [T; D]) -> Self {
        Vec3 { comps }
    }

    fn dot(&self, other: &Self) -> T {
        let mut acc = self.comps[0] * other.comps[0];
        for i in 1..D {
            acc = acc + self.comps[i] * other.comps[i];
        }
        acc
    }
}

// === 3. const 제네릭과 조건부 구현 ===
impl<T: Debug, const N: usize> Array<T, N> {
    fn dump(&self) {
        println!("Array[{N}]: {:?}", self.data);
    }
}

// N=0일 때만 특수 메서드
impl<T: Copy + Default> Array<T, 0> {
    fn is_empty(&self) -> bool {
        true
    }
}

// === 4. 제네릭 함수에서 const 사용 ===
fn sum_elems<T, const N: usize>(arr: [T; N]) -> T
where
    T: std::ops::Add<Output = T> + Copy + Default,
{
    let mut acc = T::default();
    for v in arr {
        acc = acc + v;
    }
    acc
}

// === 5. const 연산식 ===
const fn double_size(n: usize) -> usize {
    n * 2
}

struct Buffer<const SIZE: usize> {
    bytes: [u8; SIZE],
}

fn main() {
    // 고정 길이 배열
    let mut a = Array::<i32, 5>::new();
    println!("len: {}", a.len());
    if let Some(v) = a.get(2) {
        println!("index 2: {}", v);
    }

    let arr = Array::from_array([1, 2, 3, 4, 5]);
    arr.dump();
    println!("sum: {}", sum_elems([1, 2, 3, 4]));

    // 빈 배열 특수 구현
    let empty = Array::<f64, 0>::new();
    println!("is_empty: {}", empty.is_empty());

    // 벡터 내적
    let v1 = Vec3::new([1.0, 2.0, 3.0]);
    let v2 = Vec3::new([4.0, 5.0, 6.0]);
    println!("3D 내적: {}", v1.dot(&v2));

    let v1 = Vec3::new([1, 2]);
    let v2 = Vec3::new([3, 4]);
    println!("2D 내적: {}", v1.dot(&v2));

    // const 연산식
    let buf = Buffer::<{ double_size(16) }> { bytes: [0u8; 32] };
    println!("Buffer size: {} bytes", buf.bytes.len());
}
