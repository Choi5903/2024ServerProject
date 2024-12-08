const express = require('express');
const cors = require('cors');  // CORS 설정을 위해 추가
const playerRoutes = require('./Routes/PlayerRoutes');  //플레이어 라우트 
const app = express();
const port = 4000;

app.use(express.json());
app.use(cors());  // CORS 설정 추가, 외부 요청 허용
app.use('/api', playerRoutes);  //API 라우트 설정 

// 서버 시작 시 불필요한 자원 로딩 및 저장 기능 제거

app.listen(port, () => {
    console.log(`서버가 http://localhost:${port}에서 실행중 입니다.`);
});