# 38: Build Tools — 빌드 도구 (Maven / Gradle)

## 빌드 도구란?

컴파일, 테스트, 패키징, 배포를 자동화하는 도구입니다.

- **Maven**: XML 기반 (`pom.xml`), 표준 디렉터리 구조, 방대한 플러그인 생태계
- **Gradle**: Groovy/Kotlin DSL 기반, 태스크 그래프, 빠른 증분 빌드, Android 표준

## Maven 표준 디렉터리 구조

```
my-project/
├── pom.xml
└── src/
    ├── main/
    │   ├── java/        # 소스 코드
    │   └── resources/   # 설정 파일
    └── test/
        ├── java/        # 테스트 코드
        └── resources/
```

## Maven pom.xml 예

```xml
<dependencies>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-web</artifactId>
        <version>3.2.0</version>
    </dependency>
</dependencies>
```

## Maven 빌드 라이프사이클

`compile → test → package → install → deploy`

## Gradle build.gradle 예

```gradle
dependencies {
    implementation 'org.springframework.boot:spring-boot-starter-web:3.2.0'
    testImplementation 'org.junit.jupiter:junit-jupiter:5.10.0'
}

tasks.register('hello') {
    doLast { println 'Hello Gradle' }
}
```

## 의존성 관리

- 중앙 저장소(Maven Central)에서 라이브러리 내려받기
- `groupId:artifactId:version` (GAV) 좌표로 식별
- 의존성 충돌은 빌드 도구가 해석

## 실행

```bash
cd JAVA/38-build-tools
javac Main.java && java Main
```

> Maven/Gradle 없이 태스크 그래프와 의존성 순서를 직접 구현해 봅니다.
