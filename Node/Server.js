const express = require('express');
const mysql = require('mysql');
const bodyParser = require('body-parser');
const cors = require('cors');
require('dotenv').config(); // .env 파일 로드

const app = express();
app.use(bodyParser.json());
app.use(cors());

// MySQL 연결 설정
const db = mysql.createConnection({
    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_DATABASE
});

db.connect(err => {
    if (err) {
        console.error('MySQL 연결 오류:', err);
        process.exit(1); // 연결 실패 시 서버 종료
    }
    console.log('MySQL Connected');
});

// 기본 라우트
app.get('/', (req, res) => {
    res.send('서버가 정상적으로 실행되고 있습니다!');
});

// 회원가입
app.post('/register', (req, res) => {
    const { username, password } = req.body;
    const randomId = Math.floor(1000 + Math.random() * 9000); // 4자리 랜덤 숫자
    const crewName = `CREW#${randomId}`;

    const query = 'INSERT INTO users (username, password, level) VALUES (?, ?, 1)';
    db.query(query, [crewName, password], (err, result) => {
        if (err) return res.status(400).json({ success: false, message: '회원가입 실패!' }); // JSON 응답
        res.json({ success: true, username: crewName, message: '회원가입 성공!' }); // JSON 응답
    });
});

// 로그인
app.post('/login', (req, res) => {
    const { username, password } = req.body;
    const query = 'SELECT * FROM users WHERE username = ? AND password = ?';
    db.query(query, [username, password], (err, results) => {
        if (err) throw err;
        if (results.length > 0) {
            res.json({ success: true, user: results[0] }); // JSON 응답
        } else {
            res.json({ success: false, message: '로그인 실패!' }); // JSON 응답
        }
    });
});

// 탐사 라우트
app.post('/explore', (req, res) => {
    const { userId } = req.body;
    const success = Math.random() < 0.8; // 80% 확률로 성공 여부 결정

    console.log("탐사 요청받은 userId:", userId); // 요청받은 userId 확인

    if (success) {
        // 탐사 성공 - 레벨 1 증가
        const query = 'UPDATE users SET level = level + 1 WHERE id = ? AND level < 20';
        db.query(query, [userId], (err, result) => {
            if (err) throw err;
            console.log("탐사 성공 후 업데이트 결과:", result); // 업데이트 후 결과 확인
            res.send({ success: true, message: '탐사 성공!' });
        });
    } else {
        // 탐사 실패 - 레벨 초기화
        const query = 'UPDATE users SET level = 1 WHERE id = ?';
        db.query(query, [userId], (err, result) => {
            if (err) throw err;
            console.log("탐사 실패 후 레벨 초기화:", result); // 레벨 초기화 후 결과 확인
            res.send({ success: false, message: '탐사 실패! 레벨 초기화.' });
        });
    }
});

// 유저 레벨 조회
app.get('/getUserLevel', (req, res) => {
    const userId = req.query.userId;
    const query = 'SELECT level FROM users WHERE id = ?';

    db.query(query, [userId], (err, results) => {
        if (err) throw err;
        if (results.length > 0) {
            res.send(results[0].level.toString()); // 레벨 반환
        } else {
            res.status(404).send('유저를 찾을 수 없습니다');
        }
    });
});

// 서버 실행
const PORT = process.env.PORT || 3000; // .env 파일에서 PORT 가져오기 (기본값 3000)
app.listen(PORT, () => console.log(`Server running on http://localhost:${PORT}`));
