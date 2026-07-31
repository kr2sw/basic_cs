# 24: MongoDB와 Mongoose — Document Model and CRUD Concepts

MongoDB의 문서 모델과 Mongoose ODM의 개념을 학습합니다.

## MongoDB vs 관계형 DB

MongoDB는 JSON 형태의 **문서(Document)**를 컬렉션(Collection)에 저장합니다. 스키마가 유연하고 수직 확장이 쉽습니다.

```json
{
  "_id": "650f1a2b3c4d5e6f7a8b9c0d",
  "name": "홍길동",
  "email": "hong@example.com",
  "tags": ["백엔드", "Node.js"]
}
```

## 연결

```js
const mongoose = require('mongoose');
await mongoose.connect('mongodb://127.0.0.1:27017/mydb');
```

## 스키마와 모델

```js
const userSchema = new mongoose.Schema({
  name: { type: String, required: true },
  email: { type: String, required: true, unique: true },
  age: Number,
});

const User = mongoose.model('User', userSchema);
```

## CRUD

```js
const user = await User.create({ name: '홍길동', email: 'hong@example.com' });
const users = await User.find({ age: { $gt: 20 } });
const found = await User.findById(user._id);
const updated = await User.findByIdAndUpdate(user._id, { age: 30 }, { new: true });
await User.deleteOne({ _id: user._id });
```

쿼리 연산자: `$gt`, `$lt`, `$in`, `$regex`, `$exists` 등.

## 예제 실행

예제는 Mongoose를 설치하지 않고 in-memory 시뮬레이션으로 CRUD 개념을 보여줍니다.

```bash
node index.js
```
