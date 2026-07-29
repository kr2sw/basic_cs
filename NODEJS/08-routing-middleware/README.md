# 08. 라우팅과 미들웨어

## Express Router

`express.Router()`로 라우트를 모듈화합니다.

```javascript
// routes/users.js
const router = express.Router();

router.get('/', (req, res) => res.json([]));
router.post('/', (req, res) => res.status(201).json({}));

module.exports = router;

// app.js
app.use('/users', require('./routes/users'));
```

## 미들웨어

미들웨어는 요청-응답 사이클 중간에 실행되는 함수입니다.

```javascript
app.use((req, res, next) => {
  console.log(`${req.method} ${req.url}`);
  next(); // 다음 미들웨어로 전달
});
```

### 자주 쓰는 서드파티 미들웨어

#### morgan - HTTP 요청 로깅
```bash
npm install morgan
```

```javascript
const morgan = require('morgan');
app.use(morgan('dev')); // 'combined', 'common', 'short'
```

#### cors - Cross-Origin 요청 허용
```bash
npm install cors
```

```javascript
const cors = require('cors');
app.use(cors());
```

#### body-parser - 요청 body 파싱
```bash
npm install body-parser
```
Express 4.16+에서는 `express.json()`, `express.urlencoded()` 내장.

```javascript
app.use(express.json());           // JSON body
app.use(express.urlencoded({ extended: true })); // form data
```

### 정적 파일 제공

```javascript
app.use(express.static('public'));
// http://localhost:3000/style.css → public/style.css
```
