CREATE DATABASE JS_CRUD_SinglePage;

USE JS_CRUD_SinglePage;

CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY,
    Name VARCHAR(100),
    Email VARCHAR(100),
    Gender VARCHAR(10),
    DOB DATE,
    StateId INT,
    CityId INT,
    Technologies VARCHAR(MAX),
    ProfileImage VARCHAR(200)
)

-- State table
CREATE TABLE States (
    StateID INT PRIMARY KEY IDENTITY(1,1),
    StateName VARCHAR(100) NOT NULL
);

-- City table
CREATE TABLE Cities (
    CityID INT PRIMARY KEY IDENTITY(1,1),
    CityName VARCHAR(100) NOT NULL,
    StateID INT,
    FOREIGN KEY (StateID) REFERENCES States(StateID)
);

-- Insert States
INSERT INTO States (StateName)
VALUES 
('Maharashtra'),
('Karnataka'),
('Gujarat');

-- Insert Cities
INSERT INTO Cities (CityName, StateID)
VALUES
('Mumbai', 1),
('Pune', 1),
('Nagpur', 1),
('Bangalore', 2),
('Mysore', 2),
('Ahmedabad', 3),
('Surat', 3);



SELECT * FROM Students
SELECT * FROM States
SELECT * FROM Cities

-- Scaffold-Dbcontext "Server=BOTMASTER\SQLEXPRESS; Database=JS_CRUD_SinglePage; TrustServerCertificate=True; Trusted_Connection=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables Students