// Extism Node SDK를 이용한 호스트 예제
// 설치: npm install @extism/extism
const { createPlugin } = require('@extism/extism');

async function main() {
  // 플러그인 로드
  const plugin = await createPlugin({
    wasm: [{ path: 'plugin.wasm' }],
  });

  // 플러그인 관례: 입력/출력은 메모리 0번부터의 바이트입니다.
  // i32 값 4를 리틀엔디언 바이트로 전달
  const input = new Uint8Array([4, 0, 0, 0]);
  const out = await plugin.call('double', input);
  const value = new DataView(out.buffer).getInt32(0, true);
  console.log('double(4) =', value);

  // 두 수 더하기 (a, b 두 개의 i32)
  const in2 = new Uint8Array([3, 0, 0, 0, 5, 0, 0, 0]);
  const out2 = await plugin.call('add', in2);
  const sum = new DataView(out2.buffer).getInt32(0, true);
  console.log('add(3, 5) =', sum);

  await plugin.close();
}

main().catch((e) => {
  console.error(e);
  process.exit(1);
});
