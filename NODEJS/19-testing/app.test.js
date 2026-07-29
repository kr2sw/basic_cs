const request = require('supertest');
const { app, add, getUser } = require('./app');

describe('Unit tests', () => {
  describe('add()', () => {
    it('adds two positive numbers', () => {
      expect(add(1, 2)).toBe(3);
    });

    it('handles negative numbers', () => {
      expect(add(-1, -2)).toBe(-3);
    });

    it('handles zero', () => {
      expect(add(0, 5)).toBe(5);
    });
  });

  describe('getUser()', () => {
    it('returns user for valid id', () => {
      const user = getUser(1);
      expect(user).toEqual({ id: 1, name: '홍길동' });
    });

    it('returns null for invalid id', () => {
      expect(getUser(999)).toBeNull();
    });
  });
});

describe('API integration tests', () => {
  it('GET /api/users returns all users', async () => {
    const res = await request(app).get('/api/users');
    expect(res.status).toBe(200);
    expect(Array.isArray(res.body)).toBe(true);
    expect(res.body.length).toBe(2);
  });

  it('GET /api/users/:id returns a user', async () => {
    const res = await request(app).get('/api/users/1');
    expect(res.status).toBe(200);
    expect(res.body).toHaveProperty('id', 1);
    expect(res.body).toHaveProperty('name');
  });

  it('GET /api/users/:id returns 404 for unknown user', async () => {
    const res = await request(app).get('/api/users/999');
    expect(res.status).toBe(404);
    expect(res.body).toHaveProperty('error');
  });

  it('POST /api/users creates a new user', async () => {
    const res = await request(app)
      .post('/api/users')
      .send({ name: '이영희' })
      .set('Content-Type', 'application/json');
    expect(res.status).toBe(201);
    expect(res.body).toHaveProperty('id');
    expect(res.body.name).toBe('이영희');
  });

  it('POST /api/users returns 400 without name', async () => {
    const res = await request(app)
      .post('/api/users')
      .send({})
      .set('Content-Type', 'application/json');
    expect(res.status).toBe(400);
  });
});
