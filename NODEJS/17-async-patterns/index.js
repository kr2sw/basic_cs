function delay(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

// --- Callback pattern ---
function fetchDataCallback(callback) {
  setTimeout(() => {
    callback(null, { id: 1, name: 'Callback data' });
  }, 500);
}

console.log('--- Callback ---');
fetchDataCallback((err, data) => {
  if (err) return console.error(err);
  console.log('Callback result:', data);
});

// --- Promise pattern ---
function fetchDataPromise(succeed = true) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (succeed) {
        resolve({ id: 2, name: 'Promise data' });
      } else {
        reject(new Error('Promise rejected'));
      }
    }, 500);
  });
}

console.log('\n--- Promise ---');
fetchDataPromise()
  .then(data => console.log('Promise result:', data))
  .catch(err => console.error('Promise error:', err.message));

// --- async/await pattern ---
async function fetchDataAsync() {
  await delay(500);
  return { id: 3, name: 'Async/await data' };
}

async function runAsync() {
  console.log('\n--- async/await ---');
  try {
    const data = await fetchDataAsync();
    console.log('Async result:', data);
  } catch (err) {
    console.error('Async error:', err.message);
  }
}

// --- Promise.all ---
async function runParallel() {
  console.log('\n--- Promise.all ---');
  const start = Date.now();

  const results = await Promise.all([
    delay(1000).then(() => ({ id: 1, value: 'Task 1' })),
    delay(800).then(() => ({ id: 2, value: 'Task 2' })),
    delay(1200).then(() => ({ id: 3, value: 'Task 3' })),
  ]);

  console.log('All results:', results);
  console.log(`Total time: ${Date.now() - start}ms (parallel!)`);
}

// --- Error handling in async ---
async function handleErrors() {
  console.log('\n--- Error handling ---');
  try {
    await fetchDataPromise(false);
  } catch (err) {
    console.log('Caught error:', err.message);
  }

  try {
    await Promise.all([
      fetchDataPromise(true),
      fetchDataPromise(false),
    ]);
  } catch (err) {
    console.log('Promise.all error caught:', err.message);
  }
}

(async () => {
  await runAsync();
  await runParallel();
  await handleErrors();
})();
