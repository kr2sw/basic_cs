# 18: File Upload — 파일 업로드

## 파일 업로드 설정 (php.ini)

```
file_uploads = On
upload_max_filesize = 10M
post_max_size = 10M
max_file_uploads = 20
```

## HTML 폼

```html
<form method="POST" enctype="multipart/form-data">
    <input type="file" name="file">
    <input type="submit">
</form>
```

## $_FILES 구조

| 키 | 설명 |
|----|------|
| `name` | 원본 파일명 |
| `type` | MIME 타입 |
| `size` | 파일 크기 (bytes) |
| `tmp_name` | 임시 저장 경로 |
| `error` | 에러 코드 (UPLOAD_ERR_OK = 0) |

## 에러 코드

| 상수 | 값 | 설명 |
|------|----|------|
| `UPLOAD_ERR_OK` | 0 | 성공 |
| `UPLOAD_ERR_INI_SIZE` | 1 | php.ini 최대 크기 초과 |
| `UPLOAD_ERR_FORM_SIZE` | 2 | 폼 최대 크기 초과 |
| `UPLOAD_ERR_PARTIAL` | 3 | 일부만 업로드 |
| `UPLOAD_ERR_NO_FILE` | 4 | 파일 없음 |
