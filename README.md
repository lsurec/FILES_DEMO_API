# 📁 File Upload API (Demo)

Este proyecto es una API mínima desarrollada en ASP.NET Core para la carga de archivos. La API permite subir múltiples archivos a una carpeta específica del servidor, generando nombres únicos para cada archivo.

## 🚀 Características

* Soporta subida de múltiples archivos en una sola petición.
* No tiene límite de tamaño para los archivos subidos.
* Generación de nombre único para evitar sobrescribir archivos.
* Incluye validaciones básicas y manejo de errores.
* Permite especificar una carpeta destino mediante un header (`urlCarpeta`).

## 📦 Requisitos

* .NET 8.0
* Visual Studio o cualquier editor compatible con ASP.NET Core

## 📥 Endpoint disponible

### `POST /api/upload`

Sube uno o más archivos al servidor.

#### Headers

* `urlCarpeta` (string, requerido): Ruta completa del recurso donde se desea guardar el archivo (por ejemplo, `http://localhost:5000/uploads/`). Esta url se toma de la propiedad Empresa_Img de los datos de la empresa.

#### Request (multipart/form-data)

* Uno o más archivos a subir (clave: `files`).

#### Response

* `200 OK` – Archivos procesados exitosamente.
* `404 NotFound` – Si no se proporciona la URL o no hay archivos en el request.
* `400 BadRequest` – Si ocurre un error durante el procesamiento.

### Ejemplo con `curl`

```bash
curl -X POST http://localhost:5000/api/upload \
  -H "urlCarpeta: http://localhost:5000/uploads/" \
  -F "files=@/ruta/a/archivo1.jpg" \
  -F "files=@/ruta/a/archivo2.png"
```

## 🧠 Lógica principal

* **Generación de nombres:** Se crea un nombre único para cada archivo basado en un UUID y una marca de tiempo.
* **Ubicación del archivo:** Se calcula dinámicamente la ruta base del proyecto y se concatena con la URL proporcionada.
* **Manejo de errores:** Se capturan errores comunes y se retornan mensajes claros.

## 🧪 Pendientes / Mejoras

* Integrar un procedimiento almacenado para registrar los nombres originales y nuevos de los archivos.
* Validaciones adicionales en los tipos de archivos.
* Seguridad: Sanitización del input y restricciones en las rutas.

## 📁 Estructura del proyecto

Actualmente, el proyecto cuenta con un solo controlador y no incluye estandar de respuestas, persistencia de datos ni autenticación.

---
