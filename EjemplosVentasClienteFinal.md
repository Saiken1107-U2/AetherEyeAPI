# Módulo de Ventas a Cliente Final - Ejemplos de Uso

## 1. Registrar una nueva venta
POST http://localhost:5118/api/Ventas
Content-Type: application/json
Authorization: Bearer YOUR_JWT_TOKEN

{
    "nombreCliente": "Juan Pérez",
    "correoCliente": "juan.perez@email.com",
    "telefonoCliente": "555-1234",
    "direccionCliente": "Calle Principal 123, Ciudad",
    "metodoPago": "Tarjeta",
    "observaciones": "Entrega urgente",
    "descuento": 10.00,
    "productos": [
        {
            "productoId": 1,
            "cantidad": 2,
            "precioUnitario": 25.50
        },
        {
            "productoId": 2,
            "cantidad": 1
        }
    ]
}

## 2. Obtener todas las ventas
GET http://localhost:5118/api/Ventas
Authorization: Bearer YOUR_JWT_TOKEN

## 3. Obtener una venta específica
GET http://localhost:5118/api/Ventas/1
Authorization: Bearer YOUR_JWT_TOKEN

## 4. Actualizar estado de una venta
PUT http://localhost:5118/api/Ventas/1/estado
Content-Type: application/json
Authorization: Bearer YOUR_JWT_TOKEN

{
    "estado": "Enviado",
    "observaciones": "Envío realizado mediante courier"
}

## 5. Obtener estadísticas de ventas
GET http://localhost:5118/api/Ventas/estadisticas
Authorization: Bearer YOUR_JWT_TOKEN

## 6. Buscar ventas por término
GET http://localhost:5118/api/Ventas/buscar?termino=Juan
Authorization: Bearer YOUR_JWT_TOKEN

## 7. Obtener ventas por estado
GET http://localhost:5118/api/Ventas/por-estado/Pendiente
Authorization: Bearer YOUR_JWT_TOKEN

## Ejemplo de respuesta para una venta:
{
    "id": 1,
    "nombreCliente": "Juan Pérez",
    "correoCliente": "juan.perez@email.com",
    "telefonoCliente": "555-1234",
    "direccionCliente": "Calle Principal 123, Ciudad",
    "fecha": "2025-01-24T12:00:00",
    "estado": "Pendiente",
    "metodoPago": "Tarjeta",
    "observaciones": "Entrega urgente",
    "numeroFactura": "FACT-20250124-0001",
    "subtotal": 51.00,
    "impuestos": 9.18,
    "descuento": 10.00,
    "total": 50.18,
    "vendedorNombre": "Administrador Sistema",
    "productos": [
        {
            "id": 1,
            "productoId": 1,
            "productoNombre": "Producto A",
            "cantidad": 2,
            "precioUnitario": 25.50,
            "subtotal": 51.00
        }
    ]
}

## Estados válidos para ventas:
- Pendiente
- Procesando
- Enviado
- Entregado
- Cancelado

## Métodos de pago válidos:
- Efectivo
- Tarjeta
- Transferencia
