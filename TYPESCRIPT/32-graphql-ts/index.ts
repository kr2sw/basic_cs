// 32: GraphQL + TS — 타입 안전 스키마

// === 1. 스키마 정의 (GraphQL 스키마 언어와 1:1) ===
type User = {
  id: number;
  name: string;
  email: string;
  posts: Post[];
};

type Post = {
  id: number;
  title: string;
  authorId: number;
};

// === 2. 리졸버 타입 ===
type Resolver<TArgs, TResult> = (args: TArgs) => TResult;

const users: User[] = [
  { id: 1, name: "Alice", email: "a@e.com", posts: [] },
  { id: 2, name: "Bob", email: "b@e.com", posts: [] },
];

const posts: Post[] = [
  { id: 1, title: "GraphQL 입문", authorId: 1 },
  { id: 2, title: "TypeScript 심화", authorId: 1 },
  { id: 3, title: "Rust 기초", authorId: 2 },
];

// === 3. 쿼리 파서 ===
type QueryField = { name: string; args: Record<string, string | number> };
type ParsedQuery = { fields: QueryField[]; selections: string[] };

function parseQuery(query: string): ParsedQuery {
  // query { user(id: 1) { name email } } 형태를 파싱
  const match = query.match(/(\w+)\s*\(([^)]*)\)?\s*\{\s*([^}]+)\s*\}/);
  if (!match) throw new Error("쿼리 파싱 실패");
  const args: Record<string, string | number> = {};
  for (const pair of match[2] ? match[2].split(",") : []) {
    const [k, v] = pair.split(":").map((s) => s.trim());
    args[k] = Number.isNaN(Number(v)) ? v : Number(v);
  }
  return {
    fields: [{ name: match[1], args }],
    selections: match[3].split(/\s+/).filter(Boolean),
  };
}

// === 4. 리졸버 등록 ===
interface Resolvers {
  user: (args: { id: number }, selections: string[]) => Partial<User> | null;
  post: (args: { id: number }, selections: string[]) => Partial<Post> | null;
  userPosts: (authorId: number) => Post[];
}

const resolvers: Resolvers = {
  user: (args, selections) => {
    const user = users.find((u) => u.id === args.id);
    if (!user) return null;
    return selections.reduce((acc, key) => {
      if (key === "posts") (acc as Record<string, unknown>).posts = resolvers.userPosts(user.id);
      else (acc as Record<string, unknown>)[key] = (user as Record<string, unknown>)[key];
      return acc;
    }, {} as Partial<User>);
  },
  post: (args, selections) => {
    const post = posts.find((p) => p.id === args.id);
    if (!post) return null;
    return selections.reduce((acc, key) => {
      (acc as Record<string, unknown>)[key] = (post as Record<string, unknown>)[key];
      return acc;
    }, {} as Partial<Post>);
  },
  userPosts: (authorId) => posts.filter((p) => p.authorId === authorId),
};

// === 5. 실행 엔진 ===
function executeGraphQL(query: string): unknown {
  const parsed = parseQuery(query);
  const field = parsed.fields[0];
  if (field.name === "user") return resolvers.user(field.args as { id: number }, parsed.selections);
  if (field.name === "post") return resolvers.post(field.args as { id: number }, parsed.selections);
  throw new Error(`알 수 없는 필드: ${field.name}`);
}

// === 6. 쿼리 실행 ===
console.log("user(id:1) { name email }:");
console.log(executeGraphQL("user(id: 1) { name email }"));

console.log("\nuser(id:1) { name posts { title } } (타입 안전 선택):");
console.log(JSON.stringify(executeGraphQL("user(id: 1) { name posts }"), null, 2));

console.log("\npost(id:2) { title }:");
console.log(executeGraphQL("post(id: 2) { title }"));

// === 7. 뮤테이션 개념 ===
type Mutation = (args: { title: string; authorId: number }) => Post;

const createPost: Mutation = (args) => {
  const post: Post = { id: posts.length + 1, title: args.title, authorId: args.authorId };
  posts.push(post);
  return post;
};

const created = createPost({ title: "GraphQL 뮤테이션", authorId: 1 });
console.log("\n뮤테이션으로 생성:", created);

console.log("\nGraphQL + TS 데모 완료!");
