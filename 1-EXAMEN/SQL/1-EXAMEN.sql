
--SE CREA LA BASE DE DATOS 
CREATE DATABASE RECICLADO
--SE CRE LA TABLA DE USUARIOS A RECICLAR
DROP TABLE USUARIOS
CREATE TABLE USUARIOS
(
USUARIOCODIGO INT PRIMARY KEY AUTO_INCREMENT,
NOMBRE NVARCHAR(100),
EMAIL NVARCHAR(150),
);

--CREAR LA TABLA DE MATERIALES A RECICLAR
DROP TABLE MATERIALES
CREATE TABLE MATERIALES
(
MATERIALCODIGO INT PRIMARY KEY AUTO_INCREMENT,
NOMBRE NVARCHAR(100),
PUNTOSKG INT 
);

--CREAR LA TRABLA DE RECICLAR 
DROP TABLE RECICLAJE
CREATE TABLE RECICLAJE (
    RECICLAJECODIGO INT PRIMARY KEY AUTO_INCREMENT,
    USUARIOCODIGO INT,
    MATERIALCODIGO INT,
    CANTIDADKG DECIMAL(10, 2),
    FECHA DATE,
    FOREIGN KEY (USUARIOCODIGO) REFERENCES USUARIOS(USUARIOCODIGO),
    FOREIGN KEY (MATERIALESCODIGO) REFERENCES MATERIALES(MATERIALESCODIGO)
);

--CREAR LA TABLA DE RECOMPENSAS
DROP TABLE RECOMPENSAS
CREATE TABLE RECOMPENSAS (
    RECOMPENSASCODIGO INT PRIMARY KEY AUTO_INCREMENT,
    DESCRIPCION NVARCHAR(255) NOT NULL,
    PUNTOSNECESARIOS INT NOT NULL
);

--CREAR LA TABLA DE CANJES
DROP TABLE CANJES
CREATE TABLE CANJES (
    CANJESCODIGO INT PRIMARY KEY AUTO_INCREMENT,
    USUARIOCODIGO INT,
    RECOMPENSACODIGO INT,
    FECHA DATE NOT NULL,
    FOREIGN KEY (USUARIOCODIGO) REFERENCES USUARIOS(USUARIOCODIGO),
    FOREIGN KEY (RECOMPENSACODIGO) REFERENCES RECOMPENSAS(RECOMPENSACODIGO)
);

INSERT INTO USUARIOS (NOMBRE, EMAIL) VALUES
('Juan Perez', 'juan@example.com'),
('Maria Lopez', 'maria@example.com');

INSERT INTO MATERIALES (NOMBRE, PUNTOSKG) VALUES
('Pl�stico', 10),
('Vidrio', 5),
('Papel', 3);

INSERT INTO RECICLAJE (USUARIOCODIGO, MATERIALCODIGO, CANTIDADKG, FECHA) VALUES
(1, 1, 2.5, '2024-06-01'),
(1, 2, 3.0, '2024-06-05'),
(2, 1, 1.0, '2024-06-10'),
(2, 3, 4.5, '2024-06-15');

INSERT INTO RECOMPENSAS (DESCRIPCION, PUNTOSNECESARIOS) VALUES
('QUE VIVA EL RECICLAJE', 50),
('ENTRADA AL ESTADIO', 100);

--CREAR LAS ESTADISTICAS Y LOS PUNTOS
SELECT 
    r.USUARIOCODIGO,
    u.NOMBRE,
    SUM(r.CANTIDADKG * m.PUNTOSKG) AS PUNTOSTOTALES
FROM 
    RECICLAJE r
    JOIN USUARIOS u ON r.USUARIOCODIGO = u.USUARIOCODIGO
    JOIN MATERIALES m ON r.MATERIALCODIGO = m.MATERIALCODIGO
GROUP BY 
    r.USUARIOCODIGO , u.NOMBRE;

SELECT 
    m.NOMBRE AS MATERIAL,
    SUM(r.CANTIDADKG) AS TOTALRECICLADOKG
FROM 
    RECICLAJE r
    JOIN MATERIALES m ON r.MATERIALCODIGO = m.MATERIALCODIGO
GROUP BY 
    m.NOMBRE;

SELECT 
    r.RECOMPENSACODIGO,
    r.DESCRIPCION,
    r.PUNTOSNECESARIOS
FROM 
    RECOMPENSAS r
WHERE 
    r.PUNTOSNECESARIOS <= (
        SELECT SUM(rec.CANTIDADKG * m.PUNTOSKG)
        FROM RECICLAJE rec
        JOIN MATERIALES m ON rec.MATERIALESCODIGO = m.MATERIALESCODIGO
        WHERE rec.USUARIOCODIGO = 1
    );

INSERT INTO CANJES (USUARIOCODIGO, RECOMPENSACODIGO, FECHA)
VALUES (1, 1, '2024-06-16');