-- 데이터베이스 생성 
CREATE DATABASE Game_DB_03;
-- 생성한 데이터베이스 사용
USE Game_DB_03;

-- 'users' 테이블 생성
CREATE TABLE users (
	id INT AUTO_INCREMENT PRIMARY KEY, -- 기본 키로 id 컬럼, 자동 증가
	username VARCHAR(20) UNIQUE NOT NULL, -- 고유한 사용자 이름 (NULL 허용하지 않음)
	password VARCHAR(50) NOT NULL, -- 비밀번호 (NULL 허용하지 않음)
	LEVEL INT DEFAULT 1 -- 기본값이 1인 사용자 레벨
);


	

