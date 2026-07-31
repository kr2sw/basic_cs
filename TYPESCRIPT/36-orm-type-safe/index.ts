// 36: 타입 안전 ORM — Prisma/Drizzle 개념, DTO 변환

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 스키마 정의 (Prisma 모델 유사) ===
interface UserRow {
  id: number;
  name: string;
  email: string;
  createdAt: Date;
}

interface PostRow {
  id: number;
  title: string;
  content: string;
  authorId: number;
}

// === 2. DTO 타입 (사용자 노출용) ===
type UserPublic = Omit<UserRow, "createdAt">;  // 내부 필드 숨김
type CreateUserDTO = Pick<UserRow, "name" | "email">;
type UpdateUserDTO = Partial<CreateUserDTO>;

// === 3. 미니 쿼리 빌더 (타입 안전 where) ===
type WhereInput<T> = {
  [K in keyof T]?: T[K] | { contains: string };
};

function matches<T>(row: T, where: WhereInput<T>): boolean {
  return (Object.entries(where) as [keyof T, unknown][]).every(([key, value]) => {
    const cell = row[key];
    if (value && typeof value === "object" && "contains" in value) {
      return String(cell).includes((value as { contains: string }).contains);
    }
    return cell === value;
  });
}

// === 4. 미니 ORM 클라이언트 ===
class PrismaLikeModel<T extends { id: number }> {
  private rows: T[] = [];
  private nextId = 1;

  create(data: Omit<T, "id">): T {
    const row = { ...(data as object), id: this.nextId++ } as T;
    this.rows.push(row);
    return row;
  }

  findFirst(where: WhereInput<T>): T | null {
    return this.rows.find((r) => matches(r, where)) ?? null;
  }

  findMany(where?: WhereInput<T>): T[] {
    return where ? this.rows.filter((r) => matches(r, where)) : [...this.rows];
  }

  update(where: WhereInput<T>, data: Partial<T>): T | null {
    const idx = this.rows.findIndex((r) => matches(r, where));
    if (idx === -1) return null;
    this.rows[idx] = { ...this.rows[idx], ...data, id: this.rows[idx].id };
    return this.rows[idx];
  }

  delete(where: WhereInput<T>): T | null {
    const idx = this.rows.findIndex((r) => matches(r, where));
    if (idx === -1) return null;
    return this.rows.splice(idx, 1)[0];
  }
}

// === 5. DB 연결 (스키마에 따라 모델 생성) ===
interface Db {
  user: PrismaLikeModel<UserRow>;
  post: PrismaLikeModel<PostRow>;
}

const db: Db = {
  user: new PrismaLikeModel<UserRow>(),
  post: new PrismaLikeModel<PostRow>(),
};

// === 6. DTO 변환 계층 ===
function toPublic(user: UserRow): UserPublic {
  const { createdAt: _createdAt, ...rest } = user;
  return rest;
}

// === 7. 서비스 계층 (타입 안전 CRUD) ===
function createUser(dto: CreateUserDTO): UserPublic {
  const row = db.user.create({ ...dto, createdAt: new Date() });
  return toPublic(row);
}

function findUser(where: WhereInput<UserRow>): UserPublic | null {
  const row = db.user.findFirst(where);
  return row ? toPublic(row) : null;
}

function updateUser(id: number, dto: UpdateUserDTO): UserPublic | null {
  const row = db.user.update({ id }, dto);
  return row ? toPublic(row) : null;
}

// === 8. 실제 사용 ===
const alice = createUser({ name: "Alice", email: "a@e.com" });
createUser({ name: "Bob", email: "b@e.com" });
createUser({ name: "Alice", email: "a2@e.com" });

console.log("생성:", JSON.stringify(alice));
console.log("name에 'Ali' 포함:", db.user.findMany({ name: { contains: "Ali" } }).map(toPublic));
console.log("id=1 조회:", JSON.stringify(findUser({ id: 1 })));
console.log("업데이트:", JSON.stringify(updateUser(1, { name: "Alice Kim" })));
console.log("삭제 후 전체:", db.user.findMany().map(toPublic));

// === 9. 관계 쿼리 (include 개념) ===
const post = db.post.create({ title: "ORM", content: "본문", authorId: 1 });
const author = db.user.findFirst({ id: post.authorId });
console.log("게시글 작성자:", author?.name);

// === 타입 검증 ===
type T1 = Expect<Equal<UserPublic, { id: number; name: string; email: string }>>;
type T2 = Expect<Equal<CreateUserDTO, { name: string; email: string }>>;

console.log("\n타입 안전 ORM 데모 완료!");
