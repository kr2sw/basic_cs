from typing import Set

from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from fastapi.responses import HTMLResponse

app = FastAPI()

connected_clients: Set[WebSocket] = set()

html = """
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8">
    <title>WebSocket Chat</title>
</head>
<body>
    <h1>WebSocket Chat</h1>
    <input type="text" id="msg" placeholder="Type a message...">
    <button onclick="send()">Send</button>
    <ul id="messages"></ul>
    <script>
        const ws = new WebSocket("ws://localhost:8000/ws");
        ws.onmessage = (event) => {
            const msg = document.createElement("li");
            msg.textContent = event.data;
            document.getElementById("messages").appendChild(msg);
        };
        function send() {
            const input = document.getElementById("msg");
            ws.send(input.value);
            input.value = "";
        }
    </script>
</body>
</html>
"""


@app.get("/")
def get():
    return HTMLResponse(html)


@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    connected_clients.add(websocket)
    try:
        while True:
            data = await websocket.receive_text()
            for client in connected_clients:
                await client.send_text(f"User: {data}")
    except WebSocketDisconnect:
        connected_clients.discard(websocket)
