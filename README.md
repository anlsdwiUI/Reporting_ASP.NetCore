# Reporting Application (.NET Core)

Aplikasi sederhana untuk generate laporan PDF dan Excel menggunakan Rotativa dan NPOI, dengan data diambil dari MS SQL Server melalui stored procedure (menggunakan CTE). Data diakses menggunakan ADO.NET dengan arsitektur Repository Pattern.

---

## Fitur

- Download laporan PDF dengan format custom menggunakan Rotativa
- Download laporan Excel menggunakan NPOI
- Data diambil dari database MS SQL Server melalui stored procedure
- Implementasi data access menggunakan ADO.NET tanpa ORM
- Menggunakan Repository Pattern untuk pemisahan data access

---

## Setup Database

Sebelum menjalankan aplikasi, jalankan script SQL berikut pada database MS SQL Server Anda:

File SQL ada di `DatabaseScripts/InitializePersonTable.sql`

```sql
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
