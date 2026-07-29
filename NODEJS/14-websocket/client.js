const WebSocket = require('ws');
const readline = require('readline');

const ws = new WebSocket('ws://localhost:8080');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

ws.on('open', () => {
  console.log('Connected to server. Type a message:');
  promptUser();
});

ws.on('message', (raw) => {
  const data = JSON.parse(raw.toString());
  if (data.system) {
    console.log(`[System] ${data.system}`);
  } else {
    console.log(`[${data.sender}] ${data.message}`);
  }
  promptUser();
});

ws.on('close', () => {
  console.log('Disconnected from server');
  rl.close();
  process.exit(0);
});

ws.on('error', (err) => {
  console.error('WebSocket error:', err.message);
  rl.close();
  process.exit(1);
});

function promptUser() {
  rl.question('> ', (input) => {
    if (input.toLowerCase() === '/quit') {
      ws.close();
      return;
    }
    ws.send(input);
  });
}
