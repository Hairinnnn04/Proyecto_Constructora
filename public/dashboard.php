<?php
session_start();
require_once '../config/db.php';
if (!isset($_SESSION['usuario'])) {
    header('Location: login.php');
    exit;
}
$usuario = $_SESSION['usuario'];
$rol = $usuario['rol_id'];
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Panel de Control</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">
    <style>
        body { background: #f8f9fa; }
        .sidebar { min-height: 100vh; background: #343a40; color: #fff; }
        .sidebar a { color: #fff; text-decoration: none; display: block; padding: 10px 20px; }
        .sidebar a.active, .sidebar a:hover { background: #495057; }
        .content { padding: 30px; }
    </style>
</head>
<body>
<div class="container-fluid">
    <div class="row">
        <nav class="col-md-2 sidebar">
            <h4 class="mt-4 ms-3">Tienda</h4>
            <a href="dashboard.php" class="active"><i class="bi bi-house"></i> Inicio</a>
            <?php if ($rol == 1): ?>
                <a href="usuarios.php"><i class="bi bi-people"></i> Usuarios</a>
            <?php endif; ?>
            <a href="productos.php"><i class="bi bi-box"></i> Productos</a>
            <a href="ventas.php"><i class="bi bi-cart"></i> Ventas</a>
            <a href="logout.php"><i class="bi bi-box-arrow-right"></i> Salir</a>
        </nav>
        <main class="col-md-10 content">
            <h2>Bienvenido, <?= htmlspecialchars($usuario['nombre']) ?>!</h2>
            <p>Rol: <?= $rol == 1 ? 'Administrador' : 'Vendedor' ?></p>
            <div class="row mt-4">
                <div class="col-md-4">
                    <div class="card text-bg-primary mb-3">
                        <div class="card-body">
                            <h5 class="card-title"><i class="bi bi-people"></i> Usuarios</h5>
                            <p class="card-text">Gestiona los usuarios de la tienda.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card text-bg-success mb-3">
                        <div class="card-body">
                            <h5 class="card-title"><i class="bi bi-box"></i> Productos</h5>
                            <p class="card-text">Administra los productos disponibles.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card text-bg-warning mb-3">
                        <div class="card-body">
                            <h5 class="card-title"><i class="bi bi-cart"></i> Ventas</h5>
                            <p class="card-text">Consulta y registra ventas.</p>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </div>
</div>
</body>
</html>
