-- Active: 1666715463005@@SG-butter-rabbit-3552-6842-mysql-master.servers.mongodirector.com@3306@garbagecollector

CREATE TABLE
    IF NOT EXISTS accounts(
        id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        name varchar(255) COMMENT 'User Name',
        email varchar(255) COMMENT 'User Email',
        picture varchar(255) COMMENT 'User Picture'
    ) default charset utf8 COMMENT '';

ALTER TABLE accounts ADD COLUMN bio VARCHAR(255) DEFAULT "";

ALTER TABLE accounts ADD COLUMN phone_number VARCHAR(255) DEFAULT "";

CREATE TABLE
    IF NOT EXISTS cults(
        id int NOT NULL primary key AUTO_INCREMENT COMMENT 'primary key',
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        name VARCHAR(255) NOT NULL,
        fee DECIMAL(10, 2) NOT NULL CHECK(fee >= 0),
        description MEDIUMTEXT NOT NULL,
        coverImg VARCHAR(255),
        leaderId VARCHAR(255) NOT NULL,
        Foreign Key (leaderId) REFERENCES accounts(id) ON DELETE CASCADE
    ) default charset utf8 COMMENT '';

CREATE TABLE
    IF NOT EXISTS cult_members(
        id int NOT NULL primary key AUTO_INCREMENT COMMENT 'primary key',
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        accountId VARCHAR(255) NOT NULL,
        cultId int NOT NULL,
        member_role VARCHAR(255),
        Foreign Key (cultId) REFERENCES cults(id) ON DELETE CASCADE,
        Foreign Key (accountId) REFERENCES accounts(id) ON DELETE CASCADE
    ) default charset utf8 COMMENT '';

INSERT INTO
    cults(
        name,
        fee,
        description,
        coverImg,
        leaderId
    )
VALUES (
        "Executive Success Program",
        1000.00,
        "Bring to the table win-win survival strategies to ensure proactive domination. At the end of the day, going forward, a new normal that has evolved from generation X is on the runway heading towards a streamlined cloud solution. User generated content in real-time will have multiple touchpoints for offshoring.",
        "https://cloudfront.slrlounge.com/wp-content/uploads/2020/02/Tips-Tricks-to-Get-Through-A-Full-Day-of-Corporate-Portraits-SLR-Lounge-2000x1333.jpg",
        "632a05b532095e369ff96b93"
    );

INSERT INTO
    cult_members(accountId, cultId, member_role)
VALUES (
        "632a05b532095e369ff96b93",
        1,
        "Executive Leader"
    );

SELECT
    c.*,
    COUNT(cm.id) AS MemberCount,
    a.*
FROM cults c
    JOIN accounts a ON a.id = c.leaderId
    LEFT JOIN cult_members cm ON cm.cultId = c.id
GROUP BY c.id