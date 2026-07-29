from datetime import datetime, date, time, timedelta, timezone


if __name__ == "__main__":
    now = datetime.now()
    today = date.today()

    print(f"Now: {now}")
    print(f"Today: {today}")
    print(f"Year: {now.year}, Month: {now.month}, Day: {now.day}")
    print(f"Hour: {now.hour}, Minute: {now.minute}, Second: {now.second}")

    # strftime
    print(f"Formatted: {now.strftime('%Y-%m-%d %H:%M:%S')}")
    print(f"Date only: {now.strftime('%A, %B %d, %Y')}")
    print(f"24h time: {now.strftime('%H:%M:%S')}")
    print(f"12h time: {now.strftime('%I:%M:%S %p')}")

    # strptime
    date_str = "2026-12-25 10:30:00"
    parsed = datetime.strptime(date_str, "%Y-%m-%d %H:%M:%S")
    print(f"Parsed: {parsed}")

    # timedelta
    tomorrow = today + timedelta(days=1)
    last_week = today - timedelta(weeks=1)
    diff = tomorrow - today
    print(f"Tomorrow: {tomorrow}")
    print(f"Last week: {last_week}")
    print(f"Diff (days): {diff.days}")

    # Date arithmetic
    delta = timedelta(days=10, hours=3, minutes=30)
    future = now + delta
    print(f"Now: {now}")
    print(f"Future (+10d 3h 30m): {future}")

    # timezone (UTC)
    utc_now = datetime.now(timezone.utc)
    kst = timezone(timedelta(hours=9))
    kst_now = utc_now.astimezone(kst)
    print(f"UTC: {utc_now}")
    print(f"KST: {kst_now}")

    # date comparison
    d1 = date(2026, 1, 1)
    d2 = date(2026, 12, 31)
    print(f"d1 < d2: {d1 < d2}")
    print(f"Days between: {(d2 - d1).days}")
