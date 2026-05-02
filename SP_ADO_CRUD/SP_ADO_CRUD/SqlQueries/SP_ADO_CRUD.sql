

CREATE DATABASE SP_ADO_CRUD;
GO
USE  SP_ADO_CRUD;



----------------------------------------------------------------------------------------------------------------------


CREATE TABLE Qualifications (
    QualificationId INT PRIMARY KEY IDENTITY,
    QualificationName NVARCHAR(100)
)


CREATE PROCEDURE sp_GetQualifications
AS
BEGIN
    SELECT QualificationId, QualificationName FROM Qualifications
END


CREATE TABLE Technologies (
    TechnologyId INT PRIMARY KEY IDENTITY,
    TechnologyName NVARCHAR(100)
)


CREATE PROCEDURE sp_GetTechnologies
AS
BEGIN
    SELECT TechnologyId, TechnologyName FROM Technologies
END


CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Gender NVARCHAR(10),
    QualificationId INT,
    Technologies NVARCHAR(MAX),
    ImagePath NVARCHAR(255),
    DOB DATE,
    FOREIGN KEY (QualificationId) REFERENCES Qualifications(QualificationId)
)


CREATE PROCEDURE sp_InsertStudent
    @Name NVARCHAR(100),
    @Gender NVARCHAR(10),
    @QualificationId INT,
    @Technologies NVARCHAR(MAX),
    @ImagePath NVARCHAR(255),
    @DOB DATE
AS
BEGIN
    INSERT INTO Students VALUES (@Name,@Gender,@QualificationId,@Technologies,@ImagePath,@DOB)
END


CREATE PROCEDURE sp_GetStudents
AS
BEGIN
    SELECT * FROM Students
END

CREATE PROCEDURE sp_GetStudentById @Id INT
AS
BEGIN
    SELECT * FROM Students WHERE Id=@Id
END

CREATE PROCEDURE sp_UpdateStudent
    @Id INT,
    @Name NVARCHAR(100),
    @Gender NVARCHAR(10),
    @QualificationId INT,
    @Technologies NVARCHAR(MAX),
    @ImagePath NVARCHAR(255),
    @DOB DATE
AS
BEGIN
    UPDATE Students SET
        Name=@Name,
        Gender=@Gender,
        QualificationId=@QualificationId,
        Technologies=@Technologies,
        ImagePath=@ImagePath,
        DOB=@DOB
    WHERE Id=@Id
END

CREATE PROCEDURE sp_DeleteStudent @Id INT
AS
BEGIN
    DELETE FROM Students WHERE Id=@Id
END









INSERT INTO Qualifications (QualificationName) VALUES
('10th Pass'),
('12th Pass'),
('Diploma'),
('BCA'),
('BSc IT'),
('B.E / B.Tech'),
('MCA'),
('MSc IT'),
('MBA');



-- Insert Sample Data
INSERT INTO Technologies (TechnologyName) VALUES
('C#'),
('ASP.NET Core'),
('ADO.NET'),
('SQL Server'),
('JavaScript'),
('React JS'),
('Angular'),
('HTML'),
('CSS'),
('Bootstrap'),
('jQuery'),
('Web API');

SELECT * FROM Qualifications;
SELECT * FROM Technologies;