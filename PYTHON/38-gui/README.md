# 38: GUI 프로그래밍 (Tkinter) — 기본 위젯, 이벤트

## tkinter
Python에 내장된 GUI 툴킷입니다. 별도 설치 없이 윈도우/리눅스/맥에서 동작합니다.

```python
import tkinter as tk
root = tk.Tk()
label = tk.Label(root, text="안녕")
label.pack()
root.mainloop()
```

## 기본 위젯
`Label`, `Button`, `Entry`, `Text`, `Listbox`, `Checkbutton`, `Radiobutton`, `Frame`, `Canvas`

## 이벤트 처리
`command=` 콜백, `bind()`로 키보드/마우스 이벤트 바인딩

## 레이아웃
`pack()`, `grid()`, `place()` 세 가지 배치 방식을 제공합니다.

> 본 예제는 실행 후 3초 뒤 자동으로 창을 닫아 명령줄에서 바로 실행할 수 있습니다.

## 실행

```bash
python main.py
```
