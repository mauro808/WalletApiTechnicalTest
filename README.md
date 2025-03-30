# WalletApiTechnicalTest

# Documentación del Proyecto: Wallet API

## Descripción General
Wallet API es un servicio backend desarrollado en .NET Core con Entity Framework Core y SQLite. Permite la gestión de billeteras digitales, incluyendo la creación de usuarios, autenticación mediante JWT, registro de transacciones y transferencias entre billeteras.

## Tecnologías Utilizadas
- **Lenguaje**: C#
- **Framework**: .NET Core
- **ORM**: Entity Framework Core
- **Base de Datos**: SQLite
- **Autenticación**: JWT (JSON Web Token)
- **Pruebas**: xUnit
- **Herramientas**: Swagger para documentación de API

## Instalación y Configuración
### Requisitos Previos
- .NET Core SDK
- SQLite

### Ejecucion del proyecto.
El proyecto cuenta con SQLite, lo que permite
una facil ejecucion del codigo sin necesidad
de configuraciones de BD externas. Lo unico que
se requiere es iniciar el proyecto que
esta tiene WalletApi como proyecto de arranque.

## Endpoints Principales

### **Autenticación**
- **POST /api/auth/login**: Inicia sesión y devuelve un token JWT. Por defecto se agrego una validacion de usuario 
para efectos de test y cuestiones de tiempo. El usuario para realizar la prueba es: userTest y el password es: 1234Test


### **Billeteras**
- **POST /api/wallets**: Crea una nueva billetera.
- **GET /api/wallets/{id}**: Obtener las billeteras existentes.
- **GET /api/wallets/{id}**: Obtiene una billetera por ID.
- **PUT /api/wallets/{id}**: Actualiza los datos de una billetera (DocumentId, balance y Name).
- **DELETE /api/wallets/{id}**: Elimina una billetera.

### **Transacciones**
- **POST /api/transactions**: Crea una transacción entre billeteras.
- **GET /api/transactions/{walletId}**: Obtiene el historial de transacciones de una billetera.

## Validaciones y Reglas de Negocio
- No se puede crear una billetera con un `DocumentId` que ya exista.
- No se pueden realizar transacciones con saldo insuficiente.
- No se pueden transferir fondos a una billetera inexistente.
- Autenticación requerida para realizar transacciones y gestionar billeteras.

## Pruebas
Por cuestiones de tiempo solo se creo un test y se instalaron todas las dependencias requeridas
para realizar testeo con Moq para simular Base de datos y Xunit para el desarrollo de dichos test.


