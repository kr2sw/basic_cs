"""
32: 웹 스크래핑 — 로컬 HTTP 서버를 띄워 실제 HTML 파싱 연습

외부 사이트 스크래핑은 (설치 필요: pip install requests beautifulsoup4):
    import requests
    from bs4 import BeautifulSoup
    r = requests.get("https://example.com", timeout=10)
    soup = BeautifulSoup(r.text, "html.parser")
    for link in soup.select("a"):
        print(link.get("href"))

robots.txt: 요청 전에 "https://site.com/robots.txt"를 확인해 허용 경로만 수집합니다.
"""
import html.parser
import http.server
import threading
import urllib.request

# 1) 로컬 서버가 서빙할 HTML
HTML_PAGE = """<!DOCTYPE html>
<html lang="ko">
<head><title>샘플 뉴스 사이트</title></head>
<body>
  <h1 class="site-title">오늘의 뉴스</h1>
  <div id="news-list">
    <article class="news-item"><h2>파이썬 3.13 릴리즈</h2>
      <a href="/news/1">자세히 보기</a></article>
    <article class="news-item"><h2>AI 시대의 개발자</h2>
      <a href="/news/2">자세히 보기</a></article>
    <article class="news-item"><h2>오픈소스 생태계 보고서</h2>
      <a href="/news/3">자세히 보기</a></article>
  </div>
</body>
</html>"""


class Handler(http.server.BaseHTTPRequestHandler):
    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-Type", "text/html; charset=utf-8")
        self.end_headers()
        self.wfile.write(HTML_PAGE.encode("utf-8"))

    def log_message(self, *args):
        pass  # 서버 로그 억제


# 2) 표준 라이브러리 html.parser로 파싱 (BeautifulSoup 대체)
class NewsParser(html.parser.HTMLParser):
    def __init__(self):
        super().__init__()
        self.in_title = False
        self.titles = []

    def handle_starttag(self, tag, attrs):
        if tag == "h2":
            self.in_title = True

    def handle_endtag(self, tag):
        if tag == "h2":
            self.in_title = False

    def handle_data(self, data):
        if self.in_title and data.strip():
            self.titles.append(data.strip())


def fetch_page(url):
    with urllib.request.urlopen(url, timeout=5) as resp:
        return resp.read().decode("utf-8")


def main():
    # 로컬 서버 시작
    server = http.server.HTTPServer(("127.0.0.1", 8765), Handler)
    thread = threading.Thread(target=server.serve_forever, daemon=True)
    thread.start()
    print("[스크래퍼] 로컬 서버 시작: http://127.0.0.1:8765")

    url = "http://127.0.0.1:8765/news"
    print(f"[스크래퍼] GET {url}")
    html = fetch_page(url)
    print(f"[스크래퍼] 받은 HTML 크기: {len(html)}자")

    print()
    print("=== html.parser로 제목 추출 ===")
    parser = NewsParser()
    parser.feed(html)
    for i, title in enumerate(parser.titles, 1):
        print(f"  {i}. {title}")

    print()
    print("=== CSS 선택자 흉내 (간단한 링크 추출) ===")
    # 정규식으로 href 추출 (실무에서는 BeautifulSoup 사용)
    import re
    hrefs = re.findall(r'href="([^"]+)"', html)
    for href in hrefs:
        print(f"  링크: http://127.0.0.1:8765{href}")

    print()
    print("=== robots.txt 확인 (표준) ===")
    robots = """User-agent: *
Allow: /news/
Disallow: /private/
"""
    allow = [line for line in robots.splitlines() if line.startswith("Allow")]
    print("  /news/ 수집 허용 여부: /news/ 접근 가능")
    print(f"  robots.txt Allow 규칙: {allow}")

    server.shutdown()
    print("\n[스크래퍼] 로컬 서버 종료")


if __name__ == "__main__":
    main()
