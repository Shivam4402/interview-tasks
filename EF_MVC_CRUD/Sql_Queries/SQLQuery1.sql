CREATE DATABASE EF_MVC_CRUD

USE EF_MVC_CRUD


CREATE TABLE States
(
    StateId INT IDENTITY(1,1) PRIMARY KEY,
    StateName NVARCHAR(100) NOT NULL
);

INSERT INTO States (StateName)
VALUES 
('Maharashtra'),
('Gujarat'),
('Karnataka'),
('Delhi');

CREATE TABLE Technologies
(
    TechnologyId INT IDENTITY(1,1) PRIMARY KEY,
    TechnologyName NVARCHAR(100) NOT NULL
);

INSERT INTO Technologies (TechnologyName)
VALUES
('C#'),
('SQL'),
('React'),
('Angular'),
('Java');

CREATE TABLE Students
(
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    StateId INT NOT NULL,
    DOB DATE NOT NULL,
    ImagePath NVARCHAR(255) NULL,
    Technologies NVARCHAR(MAX) NULL, 

    CONSTRAINT FK_Students_States 
    FOREIGN KEY (StateId) REFERENCES States(StateId)
);

SELECT * FROM Students;