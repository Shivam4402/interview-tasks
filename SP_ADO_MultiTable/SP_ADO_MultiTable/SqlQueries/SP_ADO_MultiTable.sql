CREATE DATABASE SP_ADO_MultiTable;

USE SP_ADO_MultiTable;

CREATE TABLE Student
(
 StudentId INT IDENTITY(1,1) PRIMARY KEY,
 StudentName VARCHAR(100),
 Email VARCHAR(100),
 MobileNo VARCHAR(15),
 DOB DATE,
 State VARCHAR(100)
)

CREATE TABLE Qualification
(
 QualificationId INT IDENTITY(1,1) PRIMARY KEY,
 StudentId INT,
 QualificationName VARCHAR(100),
 PassingYear INT,
 Percentage DECIMAL(5,2),
 University VARCHAR(100),

 FOREIGN KEY(StudentId) REFERENCES Student(StudentId)
)

SELECT * FROM Student
SELECT * FROM Qualification

----------------------------------------------------------------------------------
CREATE PROCEDURE sp_InsertStudent
(
 @StudentName VARCHAR(100),
 @Email VARCHAR(100),
 @MobileNo VARCHAR(15),
 @DOB DATE,
 @State VARCHAR(100)
)
AS
BEGIN

INSERT INTO Student(StudentName,Email,MobileNo,DOB,State)
VALUES(@StudentName,@Email,@MobileNo,@DOB,@State)

SELECT SCOPE_IDENTITY()

END


----------------------------------------------------------------------------------

CREATE PROCEDURE sp_InsertQualification
(
 @StudentId INT,
 @QualificationName VARCHAR(100),
 @PassingYear INT,
 @Percentage DECIMAL(5,2),
 @University VARCHAR(100)
)
AS
BEGIN

INSERT INTO Qualification(StudentId,QualificationName,PassingYear,Percentage,University)
VALUES(@StudentId,@QualificationName,@PassingYear,@Percentage,@University)

END
----------------------------------------------------------------------------------

CREATE PROCEDURE sp_DeleteStudent
(
 @StudentId INT
)
AS
BEGIN

DELETE FROM Qualification WHERE StudentId=@StudentId
DELETE FROM Student WHERE StudentId=@StudentId	

END

----------------------------------------------------------------------------------
CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN

SELECT 
s.StudentId,
s.StudentName,
s.Email,
s.MobileNo,
s.DOB,
s.State,

q.QualificationId,
q.QualificationName,
q.PassingYear,
q.Percentage,
q.University

FROM Student s
LEFT JOIN Qualification q
ON s.StudentId = q.StudentId

END