CREATE TABLE Person (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Age INT,
    BirthDay DATE,
    Job NVARCHAR(100),
    Education NVARCHAR(100)
);
GO

INSERT INTO Person (Name, Age, BirthDay, Job, Education) VALUES
('Alice', 25, '1998-05-10', 'Software Engineer', 'Bachelor''s Degree'),
('Bob', 30, '1993-03-20', 'Product Manager', 'Master''s Degree'),
('Analistiana', 27, '1997-02-12', 'Backend Developer', 'Bachelor''s Degree');
GO

CREATE PROCEDURE GetAllPersons
AS
BEGIN
    WITH PersonCTE AS (
        SELECT
            Name,
            Age,
            BirthDay,
            Job,
            Education
        FROM Person
    )
    SELECT * FROM PersonCTE;
END;
GO