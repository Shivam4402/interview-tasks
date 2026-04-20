

CREATE DATABASE SP_ADO_CRUD;
GO
USE  SP_ADO_CRUD;


CREATE TABLE Qualification
(
    QualificationId INT IDENTITY PRIMARY KEY,
    QualificationName VARCHAR(100)
)

INSERT INTO Qualification VALUES
('BCA'),('BSc'),('B.Tech'),('MCA')



CREATE TABLE Students
(
    Id INT IDENTITY PRIMARY KEY,
    Name VARCHAR(100),
    Gender VARCHAR(10),
    QualificationID INT,
    Technologies VARCHAR(200),
    ImagePath VARCHAR(200),
    FOREIGN KEY (QualificationId) REFERENCES Qualification(QualificationId)

)


SELECT * FROM Students;

SELECT * FROM Qualification;

Truncate Table Students;

--Insert Student

CREATE PROCEDURE sp_InsertStudent
(
@Name VARCHAR(100),
@Gender VARCHAR(10),
@QualificationId INT,
@Technologies VARCHAR(200),
@ImagePath VARCHAR(200) 
)

AS
BEGIN

INSERT INTO Students
(Name,Gender,QualificationId,Technologies,ImagePath)

VALUES
(@Name,@Gender,@QualificationId,@Technologies,@ImagePath)

END


--Get Student By Id
CREATE PROCEDURE sp_GetStudentById
@Id INT
AS
BEGIN

SELECT
s.Id,
s.Name,
s.Gender, 
s.QualificationId,
q.QualificationName,
s.Technologies,
s.ImagePath

FROM Students s
INNER JOIN Qualification q
ON s.QualificationId = q.QualificationId
WHERE Id = @Id

END


--Stored Procedure (Update)

CREATE PROCEDURE sp_UpdateStudent
(
@Id INT,
@Name VARCHAR(100),
@Gender VARCHAR(10),
@QualificationId INT,
@Technologies VARCHAR(200),
@ImagePath VARCHAR(200)
)
AS
BEGIN

UPDATE Students
SET
Name = @Name,
Gender = @Gender,
QualificationId = @QualificationId,
Technologies = @Technologies,
ImagePath = @ImagePath

WHERE Id = @Id

END



--Get All Students (Join Qualification)

CREATE PROCEDURE sp_GetStudents
AS
BEGIN

SELECT
s.Id,
s.Name,
s.Gender,
q.QualificationName,
s.Technologies,
s.ImagePath

FROM Students s
INNER JOIN Qualification q
ON s.QualificationId = q.QualificationId

END


--Get Qualifications for Dropdown

CREATE PROCEDURE sp_GetQualifications
AS
BEGIN

SELECT * FROM Qualification

END

--Delete

CREATE PROCEDURE sp_DeleteStudent
@Id INT
AS
BEGIN

DELETE FROM Students WHERE Id=@Id

END
