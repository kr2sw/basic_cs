const { WebSocketServer, WebSocket } = require('ws');

const wss = new WebSocketServer({ port: 8080 });
console.log('WebSocket server running on ws://localhost:8080');

wss.on('connection', (ws, req) => {
  const clientIp = req.socket.remoteAddress;
  console.log(`Client connected from ${clientIp}`);

  ws.on('message', (raw) => {
    const message = raw.toString();
    console.log(`Received: ${message}`);

    const payload = JSON.stringify({
      sender: clientIp,
      message,
      timestamp: new Date().toISOString()
    });

    wss.clients.forEach(client => {
      if (client.readyState === WebSocket.OPEN) {
        client.send(payload);
      }
    });
  });

  ws.send(JSON.stringify({ system: 'Welcome to the chat!' }));

  ws.on('close', () => {
    console.log(`Client ${clientIp} disconnected`);
  });

  ws.on('error', (err) => {
    console.error('WebSocket error:', err.message);
  });
});

console.log('Waiting for connections...');
