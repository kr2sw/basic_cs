"""
38: GUI 프로그래밍 — tkinter 기본 위젯과 이벤트
실행하면 창이 뜨고 3초 후 자동으로 닫힙니다 (명령줄에서 바로 실행 가능).
"""
# tkinter가 없는 환경을 대비한 폴백
try:
    import tkinter as tk
except ImportError:
    print("tkinter를 사용할 수 없는 환경입니다. GUI 없이 예제 설명만 출력합니다.")
    tk = None


def main():
    if tk is None:
        print("1. Label: 텍스트 표시")
        print("2. Button: command= 콜백 실행")
        print("3. Entry: 사용자 입력 받기")
        print("4. Listbox: 목록 선택")
        print("5. Checkbutton / Radiobutton: 선택 위젯")
        print("6. bind(): 키보드/마우스 이벤트")
        return

    root = tk.Tk()
    root.title("tkinter 예제")
    root.geometry("360x420")

    # 상태 표시용 라벨
    status = tk.Label(root, text="준비됨", font=("Arial", 12), fg="blue")
    status.pack(pady=4)

    # 1) Label + Entry + Button
    frame = tk.Frame(root)
    frame.pack(pady=6)

    name_var = tk.StringVar()
    entry = tk.Entry(frame, textvariable=name_var, width=20)
    entry.pack(side="left", padx=4)

    def on_click():
        status.config(text=f"안녕, {name_var.get() or '익명'}!", fg="green")

    tk.Button(frame, text="인사하기", command=on_click).pack(side="left", padx=4)

    # 2) Listbox
    listbox = tk.Listbox(root, height=4)
    for item in ["사과", "바나나", "체리", "포도"]:
        listbox.insert("end", item)
    listbox.pack(pady=6)

    def on_select(event):
        selection = listbox.get(listbox.curselection())
        status.config(text=f"선택됨: {selection}", fg="purple")

    listbox.bind("<<ListboxSelect>>", on_select)

    # 3) Checkbutton / Radiobutton
    check_var = tk.BooleanVar()
    tk.Checkbutton(root, text="알림 받기", variable=check_var).pack()

    radio_var = tk.StringVar(value="1")
    tk.Radiobutton(root, text="빨강", variable=radio_var, value="red").pack(anchor="w")
    tk.Radiobutton(root, text="파랑", variable=radio_var, value="blue").pack(anchor="w")

    # 4) 키보드 이벤트 bind
    def on_key(event):
        status.config(text=f"키 입력: {event.keysym}", fg="orange")

    root.bind("<Key>", on_key)

    def show_choices():
        color = radio_var.get()
        status.config(
            text=f"알림: {check_var.get()}, 색: {color}",
            fg="darkgreen",
        )

    tk.Button(root, text="선택 확인", command=show_choices).pack(pady=8)

    # 3초 후 자동 종료 (터미널 실행을 위한 편의)
    root.after(3000, root.destroy)

    root.mainloop()
    print("창이 닫혔습니다.")


if __name__ == "__main__":
    main()
