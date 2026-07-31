// 종합 프로젝트: CLI 작업 관리 도구
// 사용법: node index.js <명령어> [인자]

const fs = require('fs');
const path = require('path');

const DATA_FILE = path.join(__dirname, 'tasks.json');
const STATUS = { PENDING: 'pending', DONE: 'done' };

// ---------- 데이터 영속화 (fs) ----------
function loadTasks() {
  try {
    return JSON.parse(fs.readFileSync(DATA_FILE, 'utf8'));
  } catch {
    return []; // 파일이 없으면 빈 목록
  }
}

function saveTasks(tasks) {
  fs.writeFileSync(DATA_FILE, JSON.stringify(tasks, null, 2), 'utf8');
}

// ---------- 태스크 CRUD ----------
function addTask(title, tags = []) {
  const tasks = loadTasks();
  const task = {
    id: tasks.length ? Math.max(...tasks.map((t) => t.id)) + 1 : 1,
    title,
    tags,
    status: STATUS.PENDING,
    createdAt: new Date().toISOString(),
    completedAt: null,
  };
  tasks.push(task);
  saveTasks(tasks);
  console.log(`[추가 완료] #${task.id} "${task.title}" (태그: ${tags.join(', ') || '없음'})`);
}

function listTasks({ status, tag } = {}) {
  const tasks = loadTasks();
  if (tasks.length === 0) {
    console.log('등록된 할 일이 없습니다.');
    return;
  }

  const filtered = tasks.filter((t) => {
    if (status && t.status !== status) return false;
    if (tag && !t.tags.includes(tag)) return false;
    return true;
  });

  console.log(`총 ${filtered.length}건의 할 일\n`);
  for (const task of filtered) {
    const mark = task.status === STATUS.DONE ? '[v]' : '[ ]';
    const tagText = task.tags.length ? ` (${task.tags.join(', ')})` : '';
    console.log(`${mark} #${task.id} ${task.title}${tagText}`);
  }
}

function markDone(id) {
  const tasks = loadTasks();
  const task = tasks.find((t) => t.id === id);
  if (!task) {
    console.error(`[오류] #${id} 작업을 찾을 수 없습니다.`);
    process.exit(1);
  }
  task.status = STATUS.DONE;
  task.completedAt = new Date().toISOString();
  saveTasks(tasks);
  console.log(`[완료 처리] #${id} "${task.title}"`);
}

function removeTask(id) {
  const tasks = loadTasks();
  const before = tasks.length;
  const remaining = tasks.filter((t) => t.id !== id);
  if (remaining.length === before) {
    console.error(`[오류] #${id} 작업을 찾을 수 없습니다.`);
    process.exit(1);
  }
  saveTasks(remaining);
  console.log(`[삭제 완료] #${id} 작업을 삭제했습니다.`);
}

function showStats() {
  const tasks = loadTasks();
  const done = tasks.filter((t) => t.status === STATUS.DONE).length;
  const rate = tasks.length ? Math.round((done / tasks.length) * 100) : 0;
  console.log(`전체: ${tasks.length}건`);
  console.log(`완료: ${done}건 (${rate}%)`);
  console.log(`미완료: ${tasks.length - done}건`);
}

function showHelp() {
  console.log(`
CLI 작업 관리 도구

사용법: node index.js <명령어> [옵션]

  add "<제목>" [--tag 태그]   할 일 추가
  list                        목록 조회
  list --status done          완료된 것만 조회
  list --tag study            특정 태그만 조회
  done <id>                   완료 처리
  remove <id>                 삭제
  stats                       통계 조회
  help                        도움말
`);
}

// ---------- 인자 파싱 ----------
function parseArgs(argv) {
  const args = { options: {}, positionals: [] };
  for (let i = 0; i < argv.length; i++) {
    const arg = argv[i];
    if (arg.startsWith('--')) {
      const key = arg.slice(2);
      args.options[key] = argv[i + 1];
      i += 1;
    } else {
      args.positionals.push(arg);
    }
  }
  return args;
}

// ---------- 메인 디스패치 ----------
function main() {
  const [command, ...rest] = process.argv.slice(2);
  const { options, positionals } = parseArgs(rest);

  switch (command) {
    case 'add': {
      const title = positionals[0];
      if (!title) {
        console.error('[오류] 제목을 입력하세요. 예: node index.js add "과제 작성"');
        process.exit(1);
      }
      const tags = options.tag ? options.tag.split(',').map((t) => t.trim()) : [];
      addTask(title, tags);
      break;
    }
    case 'list':
      listTasks({ status: options.status, tag: options.tag });
      break;
    case 'done': {
      const id = Number(positionals[0]);
      if (!id) {
        console.error('[오류] id를 입력하세요. 예: node index.js done 1');
        process.exit(1);
      }
      markDone(id);
      break;
    }
    case 'remove': {
      const id = Number(positionals[0]);
      if (!id) {
        console.error('[오류] id를 입력하세요. 예: node index.js remove 1');
        process.exit(1);
      }
      removeTask(id);
      break;
    }
    case 'stats':
      showStats();
      break;
    case 'help':
    default:
      showHelp();
  }
}

main();
