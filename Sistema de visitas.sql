CREATE DATABASE Sistema_visitas
GO

USE Sistema_visitas
GO

CREATE TABLE Usuarios (
ID_Usuario INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
Nombre VARCHAR(50) NOT NULL,
Apellido VARCHAR(50) NOT NULL,
FechaNacimiento DATE NOT NULL,
Tipo_Usuario VARCHAR(70) CHECK (Tipo_Usuario IN ('Administrador', 'General')) NOT NULL,
Usuario AS (LEFT(Nombre,1) + LEFT(Apellido,1) + RIGHT('000' + CONVERT(VARCHAR, ID_Usuario), 3)) PERSISTED,
Contraseña NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE Edificios(
ID_Edificio INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
Nombre_Edificio VARCHAR(70) NOT NULL,
ID_Encargado INT FOREIGN KEY REFERENCES Usuarios (ID_Usuario),
)
GO

CREATE TABLE Aulas(
ID_Aula INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
EdificioID INT,
Nombre_Aula VARCHAR(70) NOT NULL,
FOREIGN KEY (EdificioID) REFERENCES Edificios(ID_Edificio)
)
GO

CREATE TABLE Visitantes (
ID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
Nombre VARCHAR(50) NOT NULL,
Apellido VARCHAR(50) NOT NULL,
Carrera VARCHAR(70) NULL,
Correo NVARCHAR(70) NULL,
EdificioID INT,
AulaID INT NULL,
Hora_Entrada DATETIME NOT NULL,
Hora_Salida DATETIME NOT NULL,
Motivo VARCHAR(150) NOT NULL,
Foto VARBINARY(MAX) NULL,
FOREIGN KEY (EdificioID) REFERENCES Edificios(ID_Edificio),
FOREIGN KEY (AulaID) REFERENCES Aulas(ID_Aula)
)
GO

CREATE PROCEDURE SP_AgregarUsuario
@Nombre VARCHAR(50),
@Apellido VARCHAR(50),
@FechaNacimiento DATE,
@Tipo_Usuario VARCHAR(70),
@Contraseña NVARCHAR(100)
AS
BEGIN
INSERT INTO Usuarios (Nombre, Apellido, FechaNacimiento, Tipo_Usuario, Contraseña)
VALUES (@Nombre, @Apellido, @FechaNacimiento, @Tipo_Usuario, @Contraseña);
END
GO

EXEC SP_AgregarUsuario
@Nombre = 'Jeison',
@Apellido = 'Rosario',
@FechaNacimiento = '2004-10-07',
@Tipo_Usuario = 'Administrador',
@Contraseña = 'Jeison01';
GO

EXEC SP_AgregarUsuario
@Nombre = 'Samuel',
@Apellido = 'Peralta',
@FechaNacimiento = '2004-07-10',
@Tipo_Usuario = 'General',
@Contraseña = 'SamuelP10';
GO

EXEC SP_AgregarUsuario
@Nombre = 'Juan',
@Apellido = 'Perez',
@FechaNacimiento = '2000-07-10',
@Tipo_Usuario = 'General',
@Contraseña = 'Perez10';
GO

EXEC SP_AgregarUsuario
@Nombre = 'Rosa',
@Apellido = 'Encarnación',
@FechaNacimiento = '2002-08-20',
@Tipo_Usuario = 'General',
@Contraseña = 'RosaE20';
GO

EXEC SP_AgregarUsuario
@Nombre = 'Ramón',
@Apellido = 'Orlando',
@FechaNacimiento = '1999-10-07',
@Tipo_Usuario = 'General',
@Contraseña = 'RT007';
GO

CREATE PROCEDURE SP_ModificarUsuario
@ID_Usuario INT,
@Nombre VARCHAR(50),
@Apellido VARCHAR(50),
@FechaNacimiento DATE,
@Tipo_Usuario VARCHAR(70),
@Contraseña NVARCHAR(100) 
AS
BEGIN
UPDATE Usuarios
SET Nombre = @Nombre,
Apellido = @Apellido,
FechaNacimiento = @FechaNacimiento,
Tipo_Usuario = @Tipo_Usuario,
Contraseña = @Contraseña
WHERE ID_Usuario = @ID_Usuario;
END
GO

CREATE PROCEDURE SP_EliminarUsuario
@ID_Usuario INT
AS
BEGIN
DELETE FROM Usuarios
WHERE ID_Usuario = @ID_Usuario;
END
GO

CREATE PROCEDURE SP_AutenticarUsuario
@Usuario NVARCHAR(50),
@Contraseña NVARCHAR(100)
AS
BEGIN
DECLARE @Tipo_Usuario VARCHAR(70);
IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña)
BEGIN
SELECT @Tipo_Usuario = Tipo_Usuario 
FROM Usuarios 
WHERE Usuario = @Usuario AND Contraseña = @Contraseña;
SELECT 1 AS Autenticado, @Tipo_Usuario AS Tipo_Usuario;
END
ELSE
BEGIN
SELECT 0 AS Autenticado, NULL AS Tipo_Usuario;
END
END
GO

CREATE PROCEDURE SP_ListarUsuarios
AS
BEGIN
SELECT * FROM Usuarios;
END
GO

CREATE PROCEDURE SP_ObtenerUsuario
@UsuarioID int
AS
BEGIN
SELECT
ID_Usuario,
Nombre,
Apellido,
FechaNacimiento,
Tipo_usuario,
Contraseña
FROM Usuarios WHERE ID_Usuario = @UsuarioID
END
GO

CREATE PROCEDURE SP_AgregarEdificio
@Nombre_Edificio VARCHAR(70),
@Nombre_Encargado VARCHAR(70),
@Apellido_Encargado VARCHAR(70)
AS
BEGIN
DECLARE @ID_Encargado INT;
SELECT @ID_Encargado = ID_Usuario
FROM Usuarios
WHERE Nombre = @Nombre_Encargado AND Apellido = @Apellido_Encargado; 
IF @ID_Encargado IS NULL
BEGIN
RAISERROR('Encargado no encontrado', 16, 1);
RETURN;
END
INSERT INTO Edificios (Nombre_Edificio, ID_Encargado)
VALUES (@Nombre_Edificio, @ID_Encargado);
END
GO

EXEC SP_AgregarEdificio
@Nombre_edificio = 'Edificio 1',
@Nombre_Encargado = 'Samuel',
@Apellido_Encargado = 'Peralta'
GO
EXEC SP_AgregarEdificio
@Nombre_edificio = 'Edificio 2',
@Nombre_Encargado = 'Juan',
@Apellido_Encargado = 'Perez'
GO
EXEC SP_AgregarEdificio
@Nombre_edificio = 'Edificio 3',
@Nombre_Encargado = 'Rosa',
@Apellido_Encargado = 'Encarnación'
GO
EXEC SP_AgregarEdificio
@Nombre_edificio = 'Edificio 4',
@Nombre_Encargado = 'Ramón',
@Apellido_Encargado = 'Orlando'
GO

CREATE PROCEDURE SP_ModificarEdificio
@ID_Edificio INT,
@Nombre_Edificio VARCHAR(70),
@Nombre_Encargado VARCHAR(70),
@Apellido_Encargado VARCHAR(70)
AS
BEGIN
DECLARE @ID_Encargado INT;
SELECT @ID_Encargado = ID_Usuario
FROM Usuarios
WHERE Nombre = @Nombre_Encargado AND Apellido = @Apellido_Encargado; 
IF @ID_Encargado IS NULL
BEGIN
RAISERROR('Encargado no encontrado', 16, 1);
RETURN;
END
UPDATE Edificios
SET Nombre_Edificio = @Nombre_Edificio,
ID_Encargado = @ID_Encargado
WHERE ID_Edificio = @ID_Edificio;
END
GO

CREATE PROCEDURE SP_ObtenerEdificio
@EdificioID int
AS
BEGIN
SELECT
e.ID_Edificio,
e.Nombre_Edificio,
u.Nombre AS Nombre_Encargado,
u.Apellido AS Apellido_Encargado
FROM Edificios e 
INNER JOIN 
Usuarios u ON u.ID_Usuario = e.ID_Encargado
WHERE ID_Edificio = @EdificioID
END
GO

CREATE PROCEDURE SP_EliminarEdificio
@ID_Edificio INT
AS
BEGIN
BEGIN TRY      
BEGIN TRANSACTION;
DELETE FROM Visitantes
WHERE EdificioID = @ID_Edificio;
DELETE FROM Aulas
WHERE EdificioID = @ID_Edificio;
DELETE FROM Edificios
WHERE ID_Edificio = @ID_Edificio;
COMMIT TRANSACTION;
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION;
THROW;
END CATCH
END
GO

CREATE PROCEDURE SP_ListarEdificios
AS 
BEGIN
SELECT 
e.ID_Edificio,
e.Nombre_Edificio,
u.Nombre AS Nombre_Encargado,
u.Apellido AS Apellido_Encargado
FROM Edificios e  
INNER JOIN 
Usuarios u ON u.ID_Usuario = e.ID_Encargado
END
GO

CREATE PROCEDURE SP_AgregarAula
@EdificioID INT,
@Nombre_Aula VARCHAR(70)
AS
BEGIN
INSERT INTO Aulas (EdificioID, Nombre_Aula)
VALUES (@EdificioID, @Nombre_Aula);
END
GO

EXEC SP_AgregarAula
@EdificioID = 1,
@Nombre_Aula = '1-1A'
GO
EXEC SP_AgregarAula
@EdificioID = 1,
@Nombre_Aula = '1-1B'
GO
EXEC SP_AgregarAula
@EdificioID = 1,
@Nombre_Aula = '1-1C'
GO
EXEC SP_AgregarAula
@EdificioID = 1,
@Nombre_Aula = '1-1D'
GO

EXEC SP_AgregarAula
@EdificioID = 2,
@Nombre_Aula = '2-1A'
GO
EXEC SP_AgregarAula
@EdificioID = 2,
@Nombre_Aula = '2-1B'
GO
EXEC SP_AgregarAula
@EdificioID = 2,
@Nombre_Aula = '2-1C'
EXEC SP_AgregarAula
@EdificioID = 2,
@Nombre_Aula = '2-1D'
GO

EXEC SP_AgregarAula
@EdificioID = 3,
@Nombre_Aula = '3-1A'
GO
EXEC SP_AgregarAula
@EdificioID = 3,
@Nombre_Aula = '3-1B'
GO
EXEC SP_AgregarAula
@EdificioID = 3,
@Nombre_Aula = '3-1C'
GO
EXEC SP_AgregarAula
@EdificioID = 3,
@Nombre_Aula = '3-1D'
GO

EXEC SP_AgregarAula
@EdificioID = 4,
@Nombre_Aula = '4-1A'
GO
EXEC SP_AgregarAula
@EdificioID = 4,
@Nombre_Aula = '4-1B'
GO
EXEC SP_AgregarAula
@EdificioID = 4,
@Nombre_Aula = '4-1C'
GO
EXEC SP_AgregarAula
@EdificioID = 4,
@Nombre_Aula = '4-1D'
GO

CREATE PROCEDURE SP_ModificarAula
@ID_Aula INT,
@Nombre_Aula VARCHAR(70)
AS
BEGIN
UPDATE Aulas
SET Nombre_Aula = @Nombre_Aula
WHERE ID_Aula = @ID_Aula;
END
GO

CREATE PROCEDURE SP_ObtenerAula
@AulaID int
AS
BEGIN
SELECT
ID_Aula,
EdificioID,
Nombre_Aula
FROM Aulas WHERE ID_Aula = @AulaID
END
GO

CREATE PROCEDURE SP_EliminarAula
@ID_Aula INT
AS
BEGIN
DELETE FROM Aulas
WHERE ID_Aula = @ID_Aula;
END
GO

CREATE PROCEDURE SP_ListarAulas
AS 
BEGIN
SELECT
a.ID_Aula,
e.Nombre_Edificio,
a.Nombre_Aula
FROM Aulas a
INNER JOIN 
Edificios e ON e.ID_Edificio = a.EdificioID 
END
GO

CREATE PROCEDURE SP_ObtenerEdificioYAulaID
@EdificioID int
AS
BEGIN
SELECT
ID_Aula,
Nombre_Aula
FROM Aulas a
WHERE
EdificioID = @EdificioID
END
GO

CREATE PROCEDURE SP_AgregarVisita
@Nombre VARCHAR(50),
@Apellido VARCHAR(50),
@Carrera VARCHAR(70) = NULL,
@Correo NVARCHAR(70) = NULL,
@EdificioID INT,
@AulaID INT = NULL,
@Hora_Entrada DATETIME,
@Hora_Salida DATETIME,
@Motivo VARCHAR(150),
@Foto VARBINARY(MAX) = NULL
AS
BEGIN
INSERT INTO Visitantes (Nombre, Apellido, Carrera, Correo, EdificioID, AulaID, Hora_Entrada, Hora_Salida, Motivo, Foto)
VALUES (@Nombre, @Apellido, @Carrera, @Correo, @EdificioID, @AulaID, @Hora_Entrada, @Hora_Salida, @Motivo, @Foto);
END
GO

CREATE PROCEDURE SP_ListarVisitas
AS
BEGIN
SELECT
v.ID,
v.Foto,
v.Nombre as Nombre_Visitante,
v.Apellido as Apellido_Visitante,
v.Carrera,
v.Correo,
e.Nombre_Edificio,
a.Nombre_Aula,
v.Hora_Entrada,
v.Hora_Salida,
v.Motivo
FROM 
dbo.Visitantes v
INNER JOIN
dbo.Edificios e ON v.EdificioID = e.ID_Edificio
LEFT JOIN 
dbo.Aulas a ON v.AulaID = a.ID_Aula AND v.EdificioID = a.EdificioID;
END
GO

CREATE PROCEDURE SP_ObtenerVisita
@VisitaID int
AS
BEGIN
SELECT
v.ID,
v.Foto,
v.Nombre,
v.Apellido,
v.Carrera,
v.Correo,
e.Nombre_Edificio,
a.Nombre_Aula,
v.Hora_Entrada,
v.Hora_Salida,
v.Motivo
FROM 
dbo.Visitantes v
INNER JOIN
dbo.Edificios e ON v.EdificioID = e.ID_Edificio
LEFT JOIN 
dbo.Aulas a ON v.AulaID = a.ID_Aula AND v.EdificioID = a.EdificioID
WHERE
v.ID = @VisitaID;
END
GO

CREATE PROCEDURE SP_ObtenerVisitaEncargado
@usuario VARCHAR(70),
@contraseña VARCHAR(70)
AS
BEGIN
DECLARE @ID_Encargado INT;
SELECT @ID_Encargado = ID_Usuario
FROM Usuarios
WHERE Usuario = @usuario AND Contraseña = @contraseña; 
IF @ID_Encargado IS NULL
BEGIN
RAISERROR('Encargado no encontrado', 16, 1);
RETURN;
END
SELECT
v.ID,
v.Foto,
v.Nombre,
v.Apellido,
v.Carrera,
v.Correo,
e.Nombre_Edificio,
a.Nombre_Aula,
v.Hora_Entrada,
v.Hora_Salida,
v.Motivo
FROM 
dbo.Visitantes v
INNER JOIN
dbo.Edificios e ON v.EdificioID = e.ID_Edificio
LEFT JOIN 
dbo.Aulas a ON v.AulaID = a.ID_Aula AND v.EdificioID = a.EdificioID
WHERE
e.ID_Encargado = @ID_Encargado;
END
GO

CREATE PROCEDURE SP_ModificarVisita
@ID INT,
@Nombre VARCHAR(50),
@Apellido VARCHAR(50),
@Carrera VARCHAR(70) = NULL,
@Correo NVARCHAR(70) = NULL,
@EdificioID INT,
@AulaID INT = NULL,
@Hora_Entrada DATETIME,
@Hora_Salida DATETIME,
@Motivo VARCHAR(150),
@Foto VARBINARY(MAX) = NULL
AS
BEGIN
UPDATE Visitantes
SET Nombre = @Nombre,
Apellido = @Apellido,
Carrera = @Carrera,
Correo = @Correo,
EdificioID = @EdificioID,
AulaID = @AulaID,
Hora_Entrada = @Hora_Entrada,
Hora_Salida = @Hora_Salida,
Motivo = @Motivo,
Foto = @Foto
WHERE ID = @ID;
END
GO

CREATE PROCEDURE SP_EliminarVisita
@ID INT
AS
BEGIN
DELETE FROM Visitantes
WHERE ID = @ID;
END
GO

EXECUTE SP_ListarEdificios
GO
EXECUTE SP_ObtenerEdificio
@EdificioID = 2
GO
EXECUTE SP_ListarAulas
GO
EXEC SP_ObtenerVisitaEncargado
@usuario = 'SP002',
@contraseña = 'SamuelP10'
GO	
EXEC SP_ObtenerVisitaEncargado
@usuario = 'JP003',
@contraseña = 'Perez10'

