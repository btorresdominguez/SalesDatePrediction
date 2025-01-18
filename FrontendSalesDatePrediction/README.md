# **FrontendSalesDatePrediction**

Este proyecto fue generado con [Angular CLI](https://github.com/angular/angular-cli) versión 17.3.11.

## **Servidor de desarrollo**

Ejecuta `ng serve` para iniciar el servidor de desarrollo. Navega a `http://localhost:4200/` y la aplicación se recargará automáticamente si realizas cambios en los archivos fuente.

## **Explicación de los Endpoints**

### 1. **Seleccionar Empleado (GET `/api/employees`)**

**Descripción**: Este endpoint devuelve una lista de empleados.

- **Método HTTP**: `GET`
- **Respuesta**: Un array de objetos con `empid` (ID del empleado) y `fullName` (nombre completo del empleado).

**Ejemplo de respuesta**:
```json
[
  { "empid": 1, "fullName": "Juan Pérez" },
  { "empid": 2, "fullName": "Ana Gómez" }
]
2. Crear Orden (POST /api/orders)
Descripción: Este endpoint crea una nueva orden con los datos proporcionados.

Método HTTP: POST
Cuerpo de la solicitud: Incluye detalles como el empid (ID del empleado), información del remitente, dirección de envío, fecha de la orden, productos, cantidades, y más.
Ejemplo de solicitud:
{
  "empid": 1,
  "shipperId": 2,
  "shipname": "Envios Rápidos",
  "shipaddress": "Av. Principal 123",
  "shipcity": "Ciudad X",
  "shipcountry": "País Y",
  "orderdate": "2025-01-18T00:00:00Z",
  "requireddate": "2025-01-20T00:00:00Z",
  "orderDetails": [
    { "productid": 10, "unitprice": 20.00, "qty": 2, "discount": 0 }
  ]
}
Respuesta: Devuelve un mensaje de éxito con los detalles de la orden creada.

Ejemplo de respuesta:
{
  "status": "success",
  "message": "Orden creada exitosamente",
  "order": { ... }
}
