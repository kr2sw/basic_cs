import json
import urllib.request
import urllib.parse
import urllib.error


def json_dumps_demo():
    data = {
        "name": "홍길동",
        "age": 30,
        "skills": ["Python", "Java", "SQL"],
        "active": True,
        "address": None,
    }
    text = json.dumps(data, ensure_ascii=False, indent=2)
    print("=== json.dumps ===")
    print(text)

    parsed = json.loads(text)
    print("\n=== json.loads ===")
    print(f"Name: {parsed['name']}, Age: {parsed['age']}")
    print(f"Skills: {', '.join(parsed['skills'])}")
    return data


def json_file_demo():
    data = {"framework": "Django", "version": "5.0", "features": ["ORM", "Admin", "Auth"]}
    with open("sample.json", "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    with open("sample.json", "r", encoding="utf-8") as f:
        loaded = json.load(f)
    print("\n=== json.dump / json.load ===")
    print(f"Loaded: {loaded}")
    return loaded


def fetch_get_example():
    print("\n=== GET request (JSONPlaceholder) ===")
    url = "https://jsonplaceholder.typicode.com/posts/1"
    try:
        with urllib.request.urlopen(url, timeout=5) as resp:
            print(f"Status: {resp.status}")
            data = json.loads(resp.read().decode("utf-8"))
            print(f"Title: {data['title']}")
            print(f"Body: {data['body'][:80]}...")
    except urllib.error.URLError as e:
        print(f"Network error: {e.reason}")


def fetch_post_example():
    print("\n=== POST request (JSONPlaceholder) ===")
    url = "https://jsonplaceholder.typicode.com/posts"
    payload = json.dumps({
        "userId": 1,
        "title": "Test Post",
        "body": "This is a test post created via Python."
    }).encode("utf-8")
    headers = {"Content-Type": "application/json"}
    req = urllib.request.Request(url, data=payload, headers=headers, method="POST")
    try:
        with urllib.request.urlopen(req, timeout=5) as resp:
            print(f"Status: {resp.status}")
            result = json.loads(resp.read().decode("utf-8"))
            print(f"Created ID: {result['id']}")
            print(f"Title: {result['title']}")
    except urllib.error.URLError as e:
        print(f"Network error: {e.reason}")


def status_code_example():
    print("\n=== HTTP Status Codes ===")
    test_urls = [
        ("200 OK", "https://jsonplaceholder.typicode.com/posts/1"),
        ("404 Not Found", "https://jsonplaceholder.typicode.com/posts/99999"),
    ]
    for label, url in test_urls:
        try:
            with urllib.request.urlopen(url, timeout=5) as resp:
                print(f"  [{label}] Status: {resp.status} {resp.reason}")
        except urllib.error.HTTPError as e:
            print(f"  [{label}] Status: {e.code} {e.reason}")
        except urllib.error.URLError as e:
            print(f"  [{label}] Network error: {e.reason}")


if __name__ == "__main__":
    json_dumps_demo()
    json_file_demo()
    fetch_get_example()
    fetch_post_example()
    status_code_example()
