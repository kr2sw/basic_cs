"""
37: 데이터베이스 — sqlite3 인메모리 DB로 CRUD와 조인 연습

SQLAlchemy 사용 예 (서드파티, pip install sqlalchemy):
    from sqlalchemy import create_engine, Column, Integer, String, select
    from sqlalchemy.orm import declarative_base, Session

    Base = declarative_base()
    class User(Base):
        __tablename__ = "users"
        id = Column(Integer, primary_key=True)
        name = Column(String)

    engine = create_engine("sqlite:///:memory:")
    Base.metadata.create_all(engine)
    with Session(engine) as session:
        session.add(User(name="홍길동"))
        session.commit()
"""
import sqlite3


def make_conn():
    # :memory: -> 디스크 없이 인메모리로 동작
    conn = sqlite3.connect(":memory:")
    conn.row_factory = sqlite3.Row  # 컬럼명으로 접근 가능
    return conn


def setup(conn):
    cur = conn.cursor()
    cur.executescript("""
        CREATE TABLE users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            age INTEGER
        );
        CREATE TABLE orders (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER,
            product TEXT,
            price INTEGER,
            FOREIGN KEY (user_id) REFERENCES users(id)
        );
    """)
    cur.execute("INSERT INTO users (name, age) VALUES ('홍길동', 30)")
    cur.execute("INSERT INTO users (name, age) VALUES ('김철수', 28)")
    cur.execute("INSERT INTO users (name, age) VALUES ('이영희', 35)")
    cur.execute("INSERT INTO orders (user_id, product, price) VALUES (1, '노트북', 1500000)")
    cur.execute("INSERT INTO orders (user_id, product, price) VALUES (1, '마우스', 30000)")
    cur.execute("INSERT INTO orders (user_id, product, price) VALUES (2, '키보드', 80000)")
    conn.commit()


def crud_demo(conn):
    print("=== 1) 조회 / 조건 / 정렬 ===")
    cur = conn.cursor()
    cur.execute("SELECT name, age FROM users ORDER BY age DESC")
    for row in cur.fetchall():
        print(f"  {row['name']}: {row['age']}세")

    cur.execute("SELECT name FROM users WHERE age >= 30")
    print("  30세 이상:", [r["name"] for r in cur.fetchall()])
    print()

    print("=== 2) INSERT / UPDATE / DELETE ===")
    cur.execute("INSERT INTO users (name, age) VALUES ('박지민', 26)")
    conn.commit()
    print("  추가 후 사용자 수:", cur.execute("SELECT COUNT(*) c FROM users").fetchone()["c"])

    cur.execute("UPDATE users SET age = 31 WHERE name = '홍길동'")
    conn.commit()
    print("  업데이트 후:", dict(cur.execute("SELECT * FROM users WHERE name='홍길동'").fetchone()))

    cur.execute("DELETE FROM users WHERE name = '박지민'")
    conn.commit()
    print("  삭제 후 사용자 수:", cur.execute("SELECT COUNT(*) c FROM users").fetchone()["c"])
    print()


def join_demo(conn):
    print("=== 3) JOIN + 집계 ===")
    cur = conn.cursor()
    cur.execute("""
        SELECT u.name, o.product, o.price
        FROM users u
        JOIN orders o ON o.user_id = u.id
        ORDER BY o.price DESC
    """)
    for row in cur.fetchall():
        print(f"  {row['name']} - {row['product']}: {row['price']:,}원")

    cur.execute("""
        SELECT u.name, COUNT(o.id) AS order_count, SUM(o.price) AS total
        FROM users u
        LEFT JOIN orders o ON o.user_id = u.id
        GROUP BY u.name
    """)
    print("  사용자별 주문 집계:")
    for row in cur.fetchall():
        print(f"    {row['name']}: {row['order_count']}건, 합계 {row['total'] or 0:,}원")
    print()


def transaction_demo(conn):
    print("=== 4) 트랜잭션 (rollback) ===")
    cur = conn.cursor()
    try:
        cur.execute("INSERT INTO users (name, age) VALUES ('테스트', 99)")
        raise RuntimeError("중간에 실패!")
    except RuntimeError:
        conn.rollback()  # INSERT 롤백
    count = cur.execute("SELECT COUNT(*) c FROM users").fetchone()["c"]
    print(f"  실패 후 사용자 수 (롤백됨): {count}")
    print()


if __name__ == "__main__":
    conn = make_conn()
    setup(conn)
    crud_demo(conn)
    join_demo(conn)
    transaction_demo(conn)
    conn.close()
