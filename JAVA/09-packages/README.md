# 09: Packages — 패키지와 import

## 패키지 (Package)

관련 클래스를 그룹화하는 네임스페이스입니다. 디렉토리 구조와 일치합니다.

```java
package com.example.myapp;  // 파일 최상단에 선언
```

## import

다른 패키지의 클래스를 사용할 때 선언합니다.

```java
import java.util.List;       // 특정 클래스
import java.util.*;          // 패키지 전체
import static java.lang.Math.*;  // static 멤버 임포트
```

## 클래스패스 (Classpath)

JVM이 클래스를 찾는 경로입니다. `-cp` 또는 `-classpath` 옵션으로 지정합니다.

```bash
javac -cp .;lib/* Main.java
java -cp .;lib/* Main
```
