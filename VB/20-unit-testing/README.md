# 20: 단위 테스트 — MSTest 기초

## 실행

```bash
dotnet test
```

## 주요 개념

- **MSTest**: Microsoft의 단위 테스트 프레임워크
- **[TestClass] / [TestMethod]**: 테스트 클래스/메서드
- **[TestInitialize] / [TestCleanup]**: 테스트 전후 설정
- **Assert**: 결과 검증 (AreEqual, IsTrue, IsNotNull)
- **DataRow**: 여러 입력 데이터로 테스트
