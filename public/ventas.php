<?php
session_start();
require_once '../config/db.php';
if (!isset($_SESSION['usuario'])) {
    header('Location: login.php');
    exit;
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Ventas</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
</head>
<body>
<div class="container mt-5">
    <h2>Gestión de Ventas</h2>
    <?php
    // Procesar formulario de agregar venta con detalle
    if (isset($_POST['agregar'])) {
        $usuario_id = $_SESSION['usuario']['id'];
        $productos = $_POST['productos']; // array de id de producto
        $cantidades = $_POST['cantidades']; // array de cantidades
        $precios = $_POST['precios']; // array de precios unitarios
        $total = 0;
        foreach ($cantidades as $i => $cantidad) {
            $total += $cantidad * $precios[$i];
        }
        $sql = "INSERT INTO ventas (usuario_id, total) VALUES ($usuario_id, $total)";
        $conn->query($sql);
        $venta_id = $conn->insert_id;
        foreach ($productos as $i => $producto_id) {
            $cantidad = $cantidades[$i];
            $precio_unitario = $precios[$i];
            $conn->query("INSERT INTO detalle_ventas (venta_id, producto_id, cantidad, precio_unitario) VALUES ($venta_id, $producto_id, $cantidad, $precio_unitario)");
        }
    }

    // Obtener ventas
    $ventas = $conn->query("SELECT v.*, u.nombre AS vendedor FROM ventas v JOIN usuarios u ON v.usuario_id = u.id ORDER BY v.fecha DESC");

    // Obtener productos para el formulario
    $productos = $conn->query("SELECT * FROM productos");
    ?>
    <div class="row">
        <div class="col-md-7">
            <h4>Lista de Ventas</h4>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Vendedor</th>
                        <th>Fecha</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    <?php while ($v = $ventas->fetch_assoc()): ?>
                    <tr>
                        <td><?= $v['id'] ?></td>
                        <td><?= htmlspecialchars($v['vendedor']) ?></td>
                        <td><?= $v['fecha'] ?></td>
                        <td>$<?= number_format($v['total'], 2) ?></td>
                    </tr>
                    <?php endwhile; ?>
                </tbody>
            </table>
        </div>
        <div class="col-md-5">
            <h4>Registrar Venta</h4>
            <form method="POST">
                <div class="mb-2">
                    <label>Total</label>
                    <input type="number" step="0.01" name="total" class="form-control" required>
                </div>
                <button type="submit" name="agregar" class="btn btn-primary">Registrar</button>
            </form>
        </div>
    </div>
    <a href="dashboard.php" class="btn btn-secondary mt-3">Volver</a>
</div>
</body>
</html>
