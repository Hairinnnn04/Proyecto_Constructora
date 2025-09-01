<?php
// Configuración de la conexión a la base de datos
$host = 'localhost';
$user = 'root';
$pass = '';
$db = 'tienda';

$conn = new mysqli($host, $user, $pass, $db);

if ($conn->connect_error) {
    die('Error de conexión: ' . $conn->connect_error);
}
?>
