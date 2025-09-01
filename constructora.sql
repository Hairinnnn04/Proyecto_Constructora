
CREATE DATABASE constructora;
USE constructora;

-- Tabla de roles
CREATE TABLE roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL
);

-- Tabla de usuarios
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    rol_id INT,
    FOREIGN KEY (rol_id) REFERENCES roles(id)
);


-- Tabla de materiales
CREATE TABLE materiales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL
);


-- Tabla de proyectos
CREATE TABLE proyectos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT,
    fecha_inicio DATETIME DEFAULT CURRENT_TIMESTAMP,
    fecha_fin DATETIME,
    presupuesto DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);


-- Tabla de detalle de proyectos
CREATE TABLE detalle_proyectos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    proyecto_id INT,
    material_id INT,
    cantidad INT NOT NULL,
    costo_unitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (proyecto_id) REFERENCES proyectos(id),
    FOREIGN KEY (material_id) REFERENCES materiales(id)
);


-- Insertar roles por defecto
INSERT INTO roles (nombre) VALUES ('Administrador'), ('Vendedor');

INSERT INTO usuarios (nombre, email, password, rol_id) VALUES ('Administrador', '123@gmail.com', 'admin123', 1);

-- Insertar materiales de ejemplo
INSERT INTO materiales (nombre, descripcion, precio, stock) VALUES
('Cemento Portland', 'Saco de cemento de 50kg', 120.00, 100),
('Arena fina', 'Metro cúbico de arena', 80.00, 50),
('Varilla de acero', 'Varilla de 3/8" para construcción', 35.00, 200),
('Bloque de concreto', 'Bloque estándar para muros', 12.00, 500),
('Pintura acrílica', 'Galón de pintura blanca', 95.00, 30);

-- Insertar proyectos de ejemplo
INSERT INTO proyectos (usuario_id, fecha_inicio, fecha_fin, presupuesto) VALUES
(1, '2025-09-01', '2026-03-01', 50000.00),
(1, '2025-10-15', '2026-04-15', 35000.00);

-- Insertar detalles de proyectos
INSERT INTO detalle_proyectos (proyecto_id, material_id, cantidad, costo_unitario) VALUES
(1, 1, 200, 120.00), -- Cemento para proyecto 1
(1, 2, 30, 80.00),   -- Arena para proyecto 1
(1, 3, 500, 35.00),  -- Varilla para proyecto 1
(2, 4, 1000, 12.00), -- Bloques para proyecto 2
(2, 5, 20, 95.00);   -- Pintura para proyecto 2