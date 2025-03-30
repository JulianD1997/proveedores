# Informe técnico – Solución a la prueba de desarrollador .NET
Este proyecto da solución a la prueba técnica solicitada para el perfil de desarrollador .NET, aplicando buenas prácticas de desarrollo, arquitectura limpia y tecnologías actuales para garantizar escalabilidad, mantenibilidad y seguridad.
--

## Arquitectura y estructura del proyecto
Se utilizó el patrón de diseño MVC (Modelo - Vista - Controlador). El proyecto consta de tres modelos principales:
- Supplier: Modelo que representa al proveedor, incluyendo todos los atributos requeridos por la prueba técnica. No se realiza normalización ya que se utiliza una base de datos no relacional (MongoDB). El único criterio de unicidad implementado es el NIT del proveedor.
- Detail: Modelo de respuesta común para todos los endpoints. Asegura que la API tenga una estructura de respuesta uniforme, facilitando el manejo en el cliente o consumidor de la API.
- JWTModel: Modelo para mapear las variables utilizadas en la creación y configuración del Json Web Token (JWT).

Además, se crearon DTOs (Data Transfer Objects) específicos:
- Uno para registrar proveedores.
- Otro para actualizar proveedores.

## Servicios
Los servicios se implementan a través de interfaces, promoviendo la separación de responsabilidades y facilitando la inyección de dependencias. En los servicios se encuentra encapsulada la lógica principal del sistema: crear, leer, actualizar y eliminar proveedores (CRUD).

## Controladores
Por medio de los servicios creados, se hace uso de cada uno en los endpoints creados
- metodo get /get-all/ trae todos los proveedores creados hasta el momento
- metodo get /{nit}/  trae un proveedor por su NIT
- metodo post /save/ guardar un proveedor
- metodo put /{nit}/ actualizar un proveedor por su nit. aca se puede especificar que campos actualizar, solo es necesario ingresar los campos que se desean modificar no es necesario enviar todos los datos del proveedor
- metodo delete /{nit}/ para eliminar un proveedor por medio de su nit

- metodo post /get_token/ obtener el token de autenticación

## Desafíos técnicos

Uno de los principales bloqueos iniciales fue el requerimiento de desarrollar la solución en .NET 6.0 con Entity Framework y MongoDB.
Tras investigar, se concluyó que:
- Entity Framework es una librería enfocada principalmente en bases de datos relacionales.
- Aunque existe soporte experimental para MongoDB, este soporte no está disponible en .NET 6, sino a partir de .NET 7.

Por lo tanto, para garantizar una solución funcional, estable y con soporte oficial de las librerías utilizadas, se optó por desarrollar el proyecto con .NET 8.0, lo cual permite una mejor integración con MongoDB usando Entity Framework Core.

Se configuró el contexto de base de datos y se registró el modelo Supplier dentro de él.
## Contenerización (Docker)
Para cumplir con el requisito de escalabilidad y facilidad de despliegue, se crearon los archivos Dockerfile y docker-compose.yml, permitiendo:

- Ejecutar la aplicación en una máquina virtual o entorno controlado.
- Levantar un contenedor para la API y otro para MongoDB.
- Persistir los datos de MongoDB utilizando un volumen montado en una carpeta /data, para evitar pérdida entre ejecuciones.

## Autenticación con JWT
Se implementó autenticación utilizando JSON Web Tokens (JWT), permitiendo proteger los endpoints sensibles.

Actualmente, se simula el proceso de login con datos estáticos (usuario y contraseña en código), pero la estructura está preparada para integrar un sistema completo de usuarios en el futuro. Bastaría con conectar la lógica a una base de datos de usuarios para validar credenciales de manera real.

## Documentación con Swagger
Se configuró Swagger para:

- Generar documentación automática de los endpoints.
- Permitir pruebas interactivas de los mismos.
- Agregar un cuadro de autenticación Bearer para probar endpoints protegidos con JWT.

## Pruebas unitarias
Se creó un proyecto separado para realizar pruebas unitarias. Estas pruebas validan el correcto funcionamiento de los servicios: creación, lectura, actualización y eliminación de proveedores.

Aunque no se realizaron pruebas de integración automatizadas por falta de experiencia en esa área, se utilizó Postman y Swagger para realizar pruebas manuales exhaustivas. Se tiene la disposición de seguir aprendiendo y mejorar en esta área en futuras implementaciones.
