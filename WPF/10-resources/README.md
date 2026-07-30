# 10 - 리소스 (Resources)

## 학습 목표
- XAML 리소스 시스템 이해
- StaticResource와 DynamicResource 차이
- ResourceDictionary를 사용한 리소스 구성
- MergedDictionaries로 여러 리소스 파일 결합
- 애플리케이션 수준 리소스 정의

## 리소스 조회 순서
1. 요소의 `Resources` 컬렉션
2. 부모 요소의 `Resources` 컬렉션
3. `Application.Resources`
4. 시스템 리소스
