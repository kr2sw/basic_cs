//! 03-control-flow: 제어 흐름 (조건문, 반복문, 패턴 매칭)

fn main() {
    // 1. if / else if / else
    let number = 7;
    if number % 2 == 0 {
        println!("{}는 짝수입니다.", number);
    } else {
        println!("{}는 홀수입니다.", number);
    }

    // if-else if 체인
    let score = 85;
    let grade = if score >= 90 {
        'A'
    } else if score >= 80 {
        'B'
    } else if score >= 70 {
        'C'
    } else {
        'F'
    };
    println!("점수 {}의 학점: {}", score, grade);

    // 조건식에서 표현식 사용
    let condition = true;
    let value = if condition { 100 } else { 200 };  // if는 표현식
    println!("if 표현식 결과: {}", value);

    // 2. loop (무한 반복)
    let mut counter = 0;
    let result = loop {
        counter += 1;
        if counter == 10 {
            break counter * 2;  // break로 값 반환
        }
    };
    println!("loop 반환값: {}", result);  // 20

    // 3. while (조건 반복)
    let mut countdown = 3;
    while countdown > 0 {
        println!("카운트다운: {}", countdown);
        countdown -= 1;
    }
    println!("발사!");

    // 4. for (범위 반복)
    println!("1부터 5까지:");
    for i in 1..=5 {  // ..=는 포함 범위
        print!("{} ", i);
    }
    println!();

    println!("0부터 4까지:");
    for i in 0..5 {  // ..는 미포함 범위
        print!("{} ", i);
    }
    println!();

    // 배열 반복
    let arr = [10, 20, 30, 40, 50];
    for element in arr {
        print!("{} ", element);
    }
    println!();

    // 인덱스와 값 함께
    for (index, value) in arr.iter().enumerate() {
        println!("arr[{}] = {}", index, value);
    }

    // 5. match 표현식
    let day = 3;
    let day_name = match day {
        1 => "월요일",
        2 => "화요일",
        3 => "수요일",
        4 => "목요일",
        5 => "금요일",
        6 | 7 => "주말",  // |로 여러 패턴
        _ => "알 수 없음",  // 나머지 모두
    };
    println!("{}: {}", day, day_name);

    // match with 범위
    let num = 42;
    match num {
        0 => println!("0입니다"),
        1..=10 => println!("1~10 사이"),
        11..=50 => println!("11~50 사이"),
        _ => println!("51 이상"),
    }

    // match with 값 바인딩
    let optional = Some(7);
    match optional {
        Some(x) if x > 5 => println!("5보다 큰 값: {}", x),  // 가드
        Some(x) => println!("값: {}", x),
        None => println!("값 없음"),
    }

    // 6. if let (한 패턴만 매칭)
    let favorite_color: Option<&str> = Some("파랑");
    if let Some(color) = favorite_color {
        println!("좋아하는 색상: {}", color);
    } else {
        println!("좋아하는 색상 없음");
    }

    // if let with else
    let mut optional_val = Some(99);
    if let Some(value) = optional_val {
        println!("if let으로 추출한 값: {}", value);
    }

    // 7. loop label (레이블)
    let mut i = 0;
    'outer: loop {
        i += 1;
        let mut j = 0;
        loop {
            j += 1;
            if i == 2 && j == 3 {
                break 'outer;  // 외부 루프 탈출
            }
            if j >= 5 {
                break;  // 내부 루프만 탈출
            }
            println!("i={}, j={}", i, j);
        }
    }
    println!("레이블로 외부 루프 탈출: i={}", i);

    // 레이블 예제 2: for
    'outer_for: for x in 0..5 {
        for y in 0..5 {
            if x + y > 6 {
                break 'outer_for;
            }
            print!("({},{}) ", x, y);
        }
    }
    println!();

    // 8. while let
    let mut stack = vec![1, 2, 3, 4, 5];
    while let Some(top) = stack.pop() {
        print!("{} ", top);
    }
    println!();

    // 9. continue
    for n in 0..10 {
        if n % 2 == 0 {
            continue;  // 짝수는 건너뜀
        }
        print!("{} ", n);
    }
    println!(" (홀수만)");
}
