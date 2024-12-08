require('dotenv').config();  // 환경 변수 설정
const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcrypt');

const app = express();

// CORS 설정
app.use(cors());
app.use(bodyParser.json());

// 사용자 데이터 및 탐험 데이터 저장소 (실제로는 데이터베이스를 사용해야 함)
const users = []; // { username, password (hashed), progress }
const refreshTokens = {}; // { refreshToken: username }

// 환경 변수 로드
const PORT = process.env.PORT || 3000;
const JWT_SECRET = process.env.JWT_SECRET;
const REFRESH_TOKEN_SECRET = process.env.REFRESH_TOKEN_SECRET;
const MAX_LEVEL = parseInt(process.env.MAX_LEVEL, 10) || 20;
const MIN_SUCCESS_RATE = parseInt(process.env.MIN_SUCCESS_RATE, 10) || 40;
const MAX_SUCCESS_RATE = parseInt(process.env.MAX_SUCCESS_RATE, 10) || 60;

// 기본 라우트
app.get('/', function(req, res) {
    res.send('Hello world');
});

// about 라우트
app.get('/about', function(req, res) {
    res.send('about world');
});

// 회원가입 라우트
app.post('/register', async (req, res) => {
    const { username, password } = req.body;

    if (users.find(user => user.username === username)) {
        return res.status(400).json({ error: '이미 존재하는 사용자입니다.' });
    }

    const hashedPassword = await bcrypt.hash(password, 10);
    users.push({ username, password: hashedPassword, progress: 0 }); // 초기 탐험 단계 0으로 설정
    res.status(201).json({ message: '회원 가입 성공' });
});

// 로그인 라우트
app.post('/login', async (req, res) => {
    const { username, password } = req.body;
    const user = users.find(user => user.username === username);

    if (!user || !(await bcrypt.compare(password, user.password))) {
        return res.status(400).json({ error: '잘못된 사용자명 또는 비밀번호 입니다.' });
    }

    const accessToken = generateAccessToken(username);
    const refreshToken = jwt.sign({ username }, REFRESH_TOKEN_SECRET);
    refreshTokens[refreshToken] = username;

    res.json({ accessToken, refreshToken });
});

// 탐험 진행 라우트
app.post('/explore', authenticateToken, (req, res) => {
    const username = req.user.username;
    const user = users.find(user => user.username === username);

    if (!user) return res.status(404).json({ error: '사용자를 찾을 수 없습니다.' });

    const currentLevel = user.progress;
    if (currentLevel >= MAX_LEVEL) {
        return res.json({ message: '축하합니다! 이미 모든 단계를 완료했습니다.', progress: MAX_LEVEL });
    }

    const successRate = Math.floor(Math.random() * (MAX_SUCCESS_RATE - MIN_SUCCESS_RATE + 1)) + MIN_SUCCESS_RATE;
    const isSuccessful = Math.random() * 100 < successRate;

    if (isSuccessful) {
        user.progress += 1;
        const message = user.progress >= MAX_LEVEL
            ? '탐험 성공! 축하합니다. 모든 단계를 완료했습니다!'
            : '탐험 성공! 다음 단계로 이동합니다.';
        res.json({ message, progress: user.progress });
    } else {
        user.progress = 0; // 탐험 실패 시 초기화
        res.json({ message: '탐험 실패! 시작 단계로 돌아갑니다.', progress: user.progress });
    }
});

// 사용자 진행 상황 라우트
app.get('/progress', authenticateToken, (req, res) => {
    const username = req.user.username;
    const user = users.find(user => user.username === username);

    if (!user) return res.status(404).json({ error: '사용자를 찾을 수 없습니다.' });

    res.json({ message: '현재 진행 상황 조회 성공', progress: user.progress });
});

// 엑세스 토큰 생성 함수
function generateAccessToken(username) {
    return jwt.sign({ username }, JWT_SECRET, { expiresIn: '15m' });
}

// 토큰 인증 미들웨어
function authenticateToken(req, res, next) {
    const authHeader = req.headers['authorization'];
    const token = authHeader && authHeader.split(' ')[1];

    if (!token) return res.sendStatus(401); // Unauthorized

    jwt.verify(token, JWT_SECRET, (err, user) => {
        if (err) return res.sendStatus(403); // Forbidden
        req.user = user;
        next();
    });
}

// 서버 시작
app.listen(PORT, () => console.log(`서버가 포트 ${PORT}에서 실행 중입니다.`));
