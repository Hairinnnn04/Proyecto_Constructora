<?php
session_start();
require_once '../config/db.php';
if (!isset($_SESSION['usuario']) || $_SESSION['usuario']['rol_id'] != 1) {
    header('Location: dashboard.php');
    exit;
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Usuarios</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
</head>
<body>
<div class="container mt-5">
    <h2>Gestión de Usuarios</h2>
    <?php
    // Procesar formulario de agregar usuario
    if (isset($_POST['agregar'])) {
        $nombre = $_POST['nombre'];
        $email = $_POST['email'];
        $password = password_hash($_POST['password'], PASSWORD_DEFAULT);
        $rol_id = $_POST['rol_id'];
        $sql = "INSERT INTO usuarios (nombre, email, password, rol_id) VALUES ('$nombre', '$email', '$password', $rol_id)";
        $conn->query($sql);
    }

    // Procesar formulario de editar usuario
    if (isset($_POST['editar'])) {
        $id = $_POST['id'];
        $nombre = $_POST['nombre'];
        $email = $_POST['email'];
        $rol_id = $_POST['rol_id'];
        $sql = "UPDATE usuarios SET nombre='$nombre', email='$email', rol_id=$rol_id WHERE id=$id";
        $conn->query($sql);
    }

    // Obtener usuarios
    $usuarios = $conn->query("SELECT u.*, r.nombre AS rol FROM usuarios u JOIN roles r ON u.rol_id = r.id");

    // Obtener roles
    $roles = $conn->query("SELECT * FROM roles");
    ?>
    <div class="row">
        <div class="col-md-7">
            <h4>Lista de Usuarios</h4>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Email</th>
                        <th>Rol</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <?php while ($u = $usuarios->fetch_assoc()): ?>
                    <tr>
                        <td><?= $u['id'] ?></td>
                        <td><?= htmlspecialchars($u['nombre']) ?></td>
                        <td><?= htmlspecialchars($u['email']) ?></td>
                        <td><?= htmlspecialchars($u['rol']) ?></td>
                        <td>
                            <button class="btn btn-sm btn-warning" onclick="editarUsuario(<?= $u['id'] ?>, '<?= htmlspecialchars($u['nombre']) ?>', '<?= htmlspecialchars($u['email']) ?>', <?= $u['rol_id'] ?>)">Editar</button>
                        </td>
                    </tr>
                    <?php endwhile; ?>
                </tbody>
            </table>
        </div>
        <div class="col-md-5">
            <h4 id="form-title">Agregar Usuario</h4>
            <form method="POST" id="usuario-form">
                <input type="hidden" name="id" id="usuario-id">
                <div class="mb-2">
                    <label>Nombre</label>
                    <input type="text" name="nombre" id="usuario-nombre" class="form-control" required>
                </div>
                <div class="mb-2">
                    <label>Email</label>
                    <input type="email" name="email" id="usuario-email" class="form-control" required>
                </div>
                <div class="mb-2">
                    <label>Rol</label>
                    <select name="rol_id" id="usuario-rol" class="form-control" required>
                        <?php while ($r = $roles->fetch_assoc()): ?>
                        <option value="<?= $r['id'] ?>"><?= htmlspecialchars($r['nombre']) ?></option>
                        <?php endwhile; ?>
                    </select>
                </div>
                <div class="mb-2" id="password-group">
                    <label>Contraseña</label>
                    <input type="password" name="password" id="usuario-password" class="form-control" required>
                </div>
                <button type="submit" name="agregar" id="agregar-btn" class="btn btn-primary">Agregar</button>
                <button type="submit" name="editar" id="editar-btn" class="btn btn-success d-none">Guardar Cambios</button>
                <button type="button" onclick="resetForm()" class="btn btn-secondary">Cancelar</button>
            </form>
        </div>
    </div>
    <script>
    function editarUsuario(id, nombre, email, rol_id) {
        document.getElementById('form-title').innerText = 'Editar Usuario';
        document.getElementById('usuario-id').value = id;
        document.getElementById('usuario-nombre').value = nombre;
        document.getElementById('usuario-email').value = email;
        document.getElementById('usuario-rol').value = rol_id;
        document.getElementById('password-group').style.display = 'none';
        document.getElementById('agregar-btn').classList.add('d-none');
        document.getElementById('editar-btn').classList.remove('d-none');
    }
    function resetForm() {
        document.getElementById('form-title').innerText = 'Agregar Usuario';
        document.getElementById('usuario-id').value = '';
        document.getElementById('usuario-nombre').value = '';
        document.getElementById('usuario-email').value = '';
        document.getElementById('usuario-rol').selectedIndex = 0;
        document.getElementById('password-group').style.display = 'block';
        document.getElementById('agregar-btn').classList.remove('d-none');
        document.getElementById('editar-btn').classList.add('d-none');
    }
    </script>
    <a href="dashboard.php" class="btn btn-secondary mt-3">Volver</a>
</div>
</body>
</html>
