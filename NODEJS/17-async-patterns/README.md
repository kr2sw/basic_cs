# 17. 비동기 패턴 (Async Patterns)

Node.js의 비동기 처리 패턴을 콜백 → 프로미스 → async/await 순으로 학습합니다.

## 콜백 (Callback)

가장 기본적인 비동기 패턴이지만 콜백 지옥을 유발할 수 있습니다.

```js
fs.readFile('a.txt', 'utf8', (err, data) => {
  if (err) return console.error(err);
  fs.readFile('b.txt', 'utf8', (err, data2) => {
    // 콜백 지옥...
  });
});
```

## 프로미스 (Promise)

ES6에서 도입된 비동기 처리 객체입니다. 체이닝이 가능합니다.

```js
const promise = new Promise((resolve, reject) => {
  setTimeout(() => resolve('완료'), 1000);
});
promise.then(result => console.log(result)).catch(err => console.error(err));
```

## async / await

ES2017에서 도입된 문법으로 프로미스를 동기 코드처럼 작성할 수 있습니다.

```js
async function run() {
  try {
    const result = await someAsyncFunction();
    console.log(result);
  } catch (err) {
    console.error(err);
  }
}
```

## Promise.all

여러 비동기 작업을 병렬로 실행하고 모두 완료될 때까지 기다립니다.

```js
const [user, posts] = await Promise.all([
  fetchUser(id),
  fetchPosts(id)
]);
```

## 예제 실행

```bash
node index.js
```
