# 13. 파일 업로드 (File Uploads)

Multer 미들웨어를 사용하여 파일 업로드를 처리하는 방법을 학습합니다.

## Multer

Multer는 `multipart/form-data` 형식의 파일 업로드를 처리하는 Express 미들웨어입니다.

### 설치

```bash
npm install multer
```

## 기본 설정

```js
const multer = require('multer');
const upload = multer({ dest: 'uploads/' });
```

## 파일 필터링

특정 확장자만 허용하도록 필터링할 수 있습니다.

```js
function fileFilter(req, file, cb) {
  if (file.mimetype.startsWith('image/')) {
    cb(null, true);
  } else {
    cb(new Error('Only images allowed'), false);
  }
}
```

## 단일 파일 업로드

```js
app.post('/upload', upload.single('file'), (req, res) => {
  res.json({ file: req.file });
});
```

## 여러 파일 업로드

```js
app.post('/uploads', upload.array('files', 5), (req, res) => {
  res.json({ files: req.files });
});
```

## 정적 파일 제공

업로드된 파일을 `express.static`으로 제공합니다.

```js
app.use('/uploads', express.static('uploads'));
```

## 예제 실행

```bash
node index.js
```
