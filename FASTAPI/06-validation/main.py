import re
from typing import Optional

from fastapi import FastAPI
from pydantic import BaseModel, EmailStr, Field, field_validator, model_validator

app = FastAPI()


class UserCreate(BaseModel):
    username: str = Field(..., min_length=3, max_length=20, pattern=r"^[a-zA-Z0-9_]+$")
    email: EmailStr
    age: int = Field(..., ge=0, le=150)
    password: str = Field(..., min_length=8)
    password_confirm: str

    @field_validator("username")
    @classmethod
    def username_no_admin(cls, v: str) -> str:
        if v.lower() == "admin":
            raise ValueError("Username cannot be 'admin'")
        return v

    @field_validator("password")
    @classmethod
    def password_strength(cls, v: str) -> str:
        if not re.search(r"[A-Z]", v):
            raise ValueError("Password must contain uppercase letter")
        if not re.search(r"[0-9]", v):
            raise ValueError("Password must contain number")
        return v

    @model_validator(mode="after")
    def passwords_match(self):
        if self.password != self.password_confirm:
            raise ValueError("Passwords do not match")
        return self


class ProductCreate(BaseModel):
    name: str = Field(..., max_length=100)
    price: float = Field(..., gt=0, description="Price must be positive")
    discount: Optional[float] = Field(None, ge=0, le=100)
    tags: list[str] = Field(default=[], max_length=5)

    @field_validator("tags")
    @classmethod
    def unique_tags(cls, v: list[str]) -> list[str]:
        if len(v) != len(set(v)):
            raise ValueError("Tags must be unique")
        return v


@app.post("/users")
def create_user(user: UserCreate):
    return {"username": user.username, "email": user.email, "age": user.age}


@app.post("/products")
def create_product(product: ProductCreate):
    final_price = product.price
    if product.discount:
        final_price = product.price * (1 - product.discount / 100)
    return {
        "name": product.name,
        "original_price": product.price,
        "final_price": round(final_price, 2),
        "tags": product.tags,
    }
