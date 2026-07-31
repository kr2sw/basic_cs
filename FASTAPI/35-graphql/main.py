from typing import Optional

import strawberry
from fastapi import FastAPI
from strawberry.fastapi import GraphQLRouter

# 인메모리 데이터 (데모)
fake_books = [
    {"id": 1, "title": "FastAPI 완전정복", "author": "홍길동", "price": 28000},
    {"id": 2, "title": "Python 비동기 프로그래밍", "author": "김영희", "price": 32000},
]


# ---- 타입 정의: GraphQL 스키마의 기본 단위 ----
@strawberry.type
class Book:
    id: int
    title: str
    author: str
    price: int


@strawberry.input
class BookInput:
    """Mutation의 입력값 타입"""
    title: str
    author: str
    price: int


# ---- Query: 읽기 연산 (리졸버) ----
@strawberry.type
class Query:
    @strawberry.field
    def books(self, author: Optional[str] = None) -> list[Book]:
        """전체 도서 조회 (선택적으로 작가 필터)"""
        books = fake_books
        if author:
            books = [b for b in books if b["author"] == author]
        return [Book(**b) for b in books]

    @strawberry.field
    def book(self, book_id: int) -> Optional[Book]:
        """ID로 도서 한 권 조회"""
        for b in fake_books:
            if b["id"] == book_id:
                return Book(**b)
        return None


# ---- Mutation: 쓰기 연산 ----
@strawberry.type
class Mutation:
    @strawberry.mutation
    def add_book(self, data: BookInput) -> Book:
        book = {"id": len(fake_books) + 1, "title": data.title, "author": data.author, "price": data.price}
        fake_books.append(book)
        return Book(**book)

    @strawberry.mutation
    def delete_book(self, book_id: int) -> bool:
        global fake_books
        before = len(fake_books)
        fake_books = [b for b in fake_books if b["id"] != book_id]
        return len(fake_books) < before


schema = strawberry.Schema(query=Query, mutation=Mutation)

app = FastAPI(title="GraphQL - Strawberry")
app.include_router(GraphQLRouter(schema), prefix="/graphql")
