CREATE DATABASE ADO_WEBAPI;

USE ADO_WEBAPI;

CREATE TABLE Students (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    ImagePath NVARCHAR(255)
)

CREATE PROCEDURE sp_AddStudent
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @ImagePath NVARCHAR(255)
AS
BEGIN
    INSERT INTO Students(Name, Email, ImagePath)
    VALUES(@Name, @Email, @ImagePath)

    SELECT SCOPE_IDENTITY() AS Id
END


CREATE PROCEDURE sp_GetStudents
AS
BEGIN
    SELECT * FROM Students
END

CREATE PROCEDURE sp_GetStudentById
    @Id INT
AS
BEGIN
    SELECT * FROM Students WHERE Id = @Id
END

CREATE PROCEDURE sp_UpdateStudent
    @Id INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @ImagePath NVARCHAR(255)
AS
BEGIN
    UPDATE Students
    SET Name = @Name,
        Email = @Email,
        ImagePath = @ImagePath
    WHERE Id = @Id
END


CREATE PROCEDURE sp_DeleteStudent
    @Id INT
AS
BEGIN
    DELETE FROM Students WHERE Id = @Id
END