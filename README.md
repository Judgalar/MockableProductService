# MockableProductService
Proyecto de ejemplo que demuestra cómo crear y probar servicios con dependencias simuladas (mocked dependencies) en .NET, utilizando `Moq` y `NUnit` para realizar pruebas unitarias. El proyecto implementa un servicio de productos a través de una API REST y SQLite en modo memoria. Incluye un diseño desacoplado con capas de acceso a datos, lógica de negocio y objetos de transferencia de datos (DTOs).

## Descripción
MockableProductService proporciona una arquitectura que desacopla el servicio principal de los detalles específicos de almacenamiento de datos, utilizando una interfaz `IProductRepository`. Esto permite realizar pruebas unitarias efectivas, simulando la capa de datos y verificando el comportamiento de `ProductService` de manera independiente.

El objetivo de este proyecto es servir como ejemplo para entender cómo diseñar servicios testables y cómo implementar pruebas unitarias en .NET.

## Tecnologías Utilizadas
- **.NET** - Framework principal del proyecto.
- **SQLite (en memoria)** - Base de datos ligera para almacenar datos durante la ejecución del programa.
- **Moq** - Biblioteca para simular (mock) dependencias en las pruebas.
- **NUnit** - Framework de pruebas para ejecutar y validar pruebas unitarias.
