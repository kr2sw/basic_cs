# 08: 병합 충돌 해결

## 충돌 발생

같은 파일의 같은 부분을 서로 다른 브랜치에서 수정하면 충돌이 발생합니다.

```bash
git merge feature
# Auto-merging file.txt
# CONFLICT (content): Merge conflict in file.txt
```

## 충돌 표시

```text
<<<<<<< HEAD
현재 브랜치의 내용
=======
병합할 브랜치의 내용
>>>>>>> feature
```

## 해결 방법

1. 파일을 열어 충돌 부분 수정
2. `<<<<<<<`, `=======`, `>>>>>>>` 마커 제거
3. 원하는 내용만 남김
4. 저장 후 add + commit

```bash
# 충돌 파일 확인
git status

# 수동 수정 후
git add 파일.txt
git commit -m "Merge feature: resolve conflict"
```

## 도구 사용

```bash
# merge tool 실행
git mergetool    # vimdiff, VS Code 등 설정 필요

# VS Code를 mergetool로 설정
git config --global merge.tool vscode
git config --global mergetool.vscode.cmd 'code --wait $MERGED'
```
