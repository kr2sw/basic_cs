# Git & GitHub 완벽 강좌 (기초 20개 + 중급 20개 챕터)

## Git이란?

Git은 2005년 리누스 토르발스가 Linux 커널 개발을 위해 만든 분산 버전 관리 시스템(DVCS)입니다. 이후 GitHub, GitLab, Bitbucket 등 다양한 플랫폼의 기반이 되었으며, 현대 소프트웨어 개발의 표준 도구로 자리잡았습니다.

### 주요 특징

- **분산 저장소**: 모든 개발자가 전체 이력을 로컬에 보관
- **브랜치**: 가볍고 빠른 브랜치 생성/전환
- **스테이징**: 커밋 전 변경 파일 선택 가능
- **체크섬**: SHA-1 해시로 데이터 무결성 보장
- **비파괴적**: 대부분의 작업이 데이터를 삭제하지 않음
- **오픈소스**: 무료, 활발한 커뮤니티

### 역사

| 버전 | 출시일 | 주요 기능 |
|------|--------|-----------|
| 1.0 | 2005-12 | 최초 릴리즈 |
| 1.5 | 2007-06 | stash, gitignore |
| 1.6 | 2008-08 | git bisect 개선 |
| 1.7 | 2010-02 | git notes, stash --patch |
| 1.8 | 2012-10 | git rebase --autostash |
| 1.9 | 2014-02 | git mergetool 개선 |
| 2.0 | 2014-05 | 기본값 변경 (push.default=simple) |
| 2.5 | 2015-07 | worktree, filter-branch |
| 2.10 | 2016-09 | push --force-with-lease |
| 2.20 | 2018-12 | git switch/restore (실험적) |
| 2.23 | 2019-08 | switch/restore 정식 |
| 2.30 | 2020-12 | stale fetch 개선 |
| 2.35 | 2022-01 | merge-ort 기본 |
| 2.40 | 2023-03 | diff-tree, fsmonitor 개선 |

## 목차

| 장 | 제목 | 설명 |
|----|------|------|
| 00 | 개발 환경 설정 | Git 설치, GitHub 계정, 기본 설정 |
| 01 | Git 소개 | 저장소, 커밋, 기본 개념 |
| 02 | 기본 명령어 | add, commit, status, diff |
| 03 | 파일 관리 | .gitignore, git rm, git mv |
| 04 | 변경 이력 | log, diff, show, blame |
| 05 | 되돌리기 | reset, restore, revert |
| 06 | 브랜치 | branch, checkout, switch |
| 07 | 병합 | merge, fast-forward, 3-way |
| 08 | 병합 충돌 | 충돌 해결, mergetool |
| 09 | 원격 저장소 | remote, push, pull, fetch |
| 10 | GitHub 기초 | 저장소, Issues, Fork |
| 11 | Pull Request | PR 워크플로, 코드 리뷰 |
| 12 | 태그와 릴리즈 | tag, SemVer, Releases |
| 13 | Stashing | stash 임시 저장 |
| 14 | Rebase | rebase, interactive rebase |
| 15 | Cherry-pick | cherry-pick, revert |
| 16 | GitHub Actions | CI/CD 자동화 |
| 17 | 협업 워크플로우 | GitHub Flow, GitFlow |
| 18 | 고급 기능 | submodule, worktree, bisect |
| 19 | .gitignore와 보안 | 보안, 민감 정보 관리 |
| 20 | 실전 프로젝트 | 전체 워크플로 실습 |
| 21 | 브랜치 전략 | trunk-based, GitFlow, feature branch |
| 22 | Actions 워크플로우 | 워크플로우, 이벤트, 잡 구조 |
| 23 | Actions 고급 | 매트릭스 빌드, 재사용 워크플로우 |
| 24 | Actions 배포 | environments, secrets, 승인 |
| 25 | Git 훅 | pre-commit, commit-msg, 커스텀 훅 |
| 26 | Git 내부 | objects, refs, HEAD, packfiles |
| 27 | 고급 리베이스 | rerere, autosquash, fixup |
| 28 | bisect 디버깅 | 이진 탐색, 자동 실행 |
| 29 | 서브모듈 | submodule, subtree |
| 30 | 워크트리 | 병렬 작업, 컨텍스트 전환 |
| 31 | 히스토리 편집 | filter-branch, filter-repo |
| 32 | CI/CD 설계 | 파이프라인, 게이트, 아티팩트 |
| 33 | GitHub API | gh CLI, REST API, 자동화 |
| 34 | 팀 워크플로 | 보호된 브랜치, 코드 리뷰 |
| 35 | 릴리즈 엔지니어링 | SemVer, changelog, 태그 |
| 36 | Git 보안 | 서명 커밋, 시크릿 스캔 |
| 37 | 모노레포 | 전략, 툴, 경로 제한 |
| 38 | Git 성능 | LFS, shallow, partial clone |
| 39 | 멀티레포 | 저장소 간 자동화 |
| 40 | 실전 프로젝트 | CI/CD 전체 파이프라인 |
