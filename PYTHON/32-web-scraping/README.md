# 32: 웹 스크래핑 (Web Scraping) — requests, BeautifulSoup, robots.txt

## 절차
1. `requests.get(url)`로 HTML 받기
2. `BeautifulSoup(html, "html.parser")`로 파싱
3. CSS 선택자 `.select()` / `.select_one()`으로 원하는 요소 추출

```python
import requests
from bs4 import BeautifulSoup

r = requests.get("https://example.com", timeout=10)
soup = BeautifulSoup(r.text, "html.parser")
for link in soup.select("a"):
    print(link.get("href"))
```

## robots.txt
스크래핑 전에 사이트의 `robots.txt`를 확인해 허용 범위를 지켜야 합니다. `https://example.com/robots.txt`

## 주의사항
- `time.sleep()`으로 요청 간격을 둡니다
- User-Agent를 정직하게 밝힙니다
- 사이트의 이용 약관과 저작권을 준수합니다

> 본 파일은 외부 서버 없이 로컬 HTTP 서버로 HTML을 서빙해 스크래핑을 연습합니다. 외부 요청 코드는 주석으로 제공됩니다.

## 실행

```bash
python main.py
```
