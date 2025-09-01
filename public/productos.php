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
    <title>Productos</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
</head>
<body>
<div class="container mt-5">
    <h2>Gestión de Productos</h2>
    <?php
    // Procesar formulario de agregar producto
    if (isset($_POST['agregar'])) {
        $nombre = $_POST['nombre'];
        $descripcion = $_POST['descripcion'];
        $precio = $_POST['precio'];
        $stock = $_POST['stock'];
        $sql = "INSERT INTO productos (nombre, descripcion, precio, stock) VALUES ('$nombre', '$descripcion', $precio, $stock)";
        $conn->query($sql);
    }

    // Procesar formulario de editar producto
    if (isset($_POST['editar'])) {
        $id = $_POST['id'];
        $nombre = $_POST['nombre'];
        $descripcion = $_POST['descripcion'];
        $precio = $_POST['precio'];
        $stock = $_POST['stock'];
        $sql = "UPDATE productos SET nombre='$nombre', descripcion='$descripcion', precio=$precio, stock=$stock WHERE id=$id";
        $conn->query($sql);
    }

    // Obtener productos
    $productos = $conn->query("SELECT * FROM productos");
    ?>
    <div class="row">
        <div class="col-md-7">
            <h4>Lista de Productos</h4>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Descripción</th>
                        <th>Precio</th>
                        <th>Stock</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <?php while ($p = $productos->fetch_assoc()): ?>
                    <tr>
                        <td><?= $p['id'] ?></td>
                        <td><?= htmlspecialchars($p['nombre']) ?></td>
                        <td><?= htmlspecialchars($p['descripcion']) ?></td>
                        <td>$<?= number_format($p['precio'], 2) ?></td>
                        <td><?= $p['stock'] ?></td>
                        <td>
                            <button class="btn btn-sm btn-warning" onclick="editarProducto(<?= $p['id'] ?>, '<?= htmlspecialchars($p['nombre']) ?>', '<?= htmlspecialchars($p['descripcion']) ?>', <?= $p['precio'] ?>, <?= $p['stock'] ?>)">Editar</button>
                        </td>
                    </tr>
                    <?php endwhile; ?>
                </tbody>
            </table>
        </div>
        <div class="col-md-5">
            <h4 id="form-title">Agregar Producto</h4>
            <form method="POST" id="producto-form">
                <input type="hidden" name="id" id="producto-id">
                <div class="mb-2">
                    <label>Nombre</label>
                    <input type="text" name="nombre" id="producto-nombre" class="form-control" required>
                </div>
                <div class="mb-2">
                    <label>Descripción</label>
                    <input type="text" name="descripcion" id="producto-descripcion" class="form-control">
                </div>
                <div class="mb-2">
                    <label>Precio</label>
                    <input type="number" step="0.01" name="precio" id="producto-precio" class="form-control" required>
                </div>
                <div class="mb-2">
                    <label>Stock</label>
                    <input type="number" name="stock" id="producto-stock" class="form-control" required>
                </div>
                <button type="submit" name="agregar" id="agregar-btn" class="btn btn-primary">Agregar</button>
                <button type="submit" name="editar" id="editar-btn" class="btn btn-success d-none">Guardar Cambios</button>
                <button type="button" onclick="resetForm()" class="btn btn-secondary">Cancelar</button>
            </form>
        </div>
    </div>
    <script>
    function editarProducto(id, nombre, descripcion, precio, stock) {
        document.getElementById('form-title').innerText = 'Editar Producto';
        document.getElementById('producto-id').value = id;
        document.getElementById('producto-nombre').value = nombre;
        document.getElementById('producto-descripcion').value = descripcion;
        document.getElementById('producto-precio').value = precio;
        document.getElementById('producto-stock').value = stock;
        document.getElementById('agregar-btn').classList.add('d-none');
        document.getElementById('editar-btn').classList.remove('d-none');
    }
    function resetForm() {
        document.getElementById('form-title').innerText = 'Agregar Producto';
        document.getElementById('producto-id').value = '';
        document.getElementById('producto-nombre').value = '';
        document.getElementById('producto-descripcion').value = '';
        document.getElementById('producto-precio').value = '';
        document.getElementById('producto-stock').value = '';
        document.getElementById('agregar-btn').classList.remove('d-none');
        document.getElementById('editar-btn').classList.add('d-none');
    }
    </script>
    <a href="dashboard.php" class="btn btn-secondary mt-3">Volver</a>
</div>
</body>
</html>
