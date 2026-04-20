CREATE DATABASE ADOCRUD;
GO
USE ADOCRUD;

CREATE TABLE Qualification
(
    QualificationId INT IDENTITY PRIMARY KEY,
    QualificationName VARCHAR(100)
)

INSERT INTO Qualification VALUES
('BCA'),('BSc'),('B.Tech'),('MCA')

CREATE TABLE Student
(
    StudentId INT IDENTITY PRIMARY KEY,
    Name VARCHAR(100),
    Gender VARCHAR(10),
    DOB DATE,
    QualificationId INT,
    Technologies VARCHAR(200),
    ImagePath VARCHAR(200),
    FOREIGN KEY (QualificationId) REFERENCES Qualification(QualificationId))


SELECT  * FROM Student;