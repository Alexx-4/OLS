# OpenLatinoGIS
Pasos para configurar OLS

1- Configurar las Bases de Datos de Administracio y GeoCuba en SQL Server
2- Instalar y dejar compilando el codigo del servidor de mapas OLS

Para ello:
-Descargar codigo de GitHub
-Hacer Restore
-Instalar, si es requerido, los paquetes (Microsoft.AspNetCore.Identity.EntityFrameworkCore y Microsoft.AspNetCore.Identity) ambos en la version 2.2.0
-Si usas la Visual Studio > 16.4 probablemente tendras que agregar en el proyecto: OpenLatino.Admin.Server un archivo global.json con el siguiente contenido:

```json
{
  "sdk": {
    "version": "3.1.200"
  }
}
```

y luego instalar .NET SDK 3.1.200

-Una vez que compile el proyecto ir a la configuracion (appsettings.json y appsettings.Develop.json) y arreglar el connection sting poniendo el
connection string correspondiente a la Bd de administracion de OpenLatino.

3- Una vez corriendo el server y viendose la interfaz visual de Administracion, autenticarse usando las siguientes credenciales:

Nombre de Usuario: adminopenlatino@gmail.com
Contraseña: Admin_1234

4- Elegir la opcion Registered Apps para poder acceder a la lista de aplicaciones clientes registradas
5- Elegir una de las aplicaciones y ver los detalles
6- En la vista de Detalles debe de salir AccessKey y UpdateKey
7- Usar el AccessKey y el UpdateKey para generar unas credenciales actualizadas haciendo el siguiente Request:

```json

"request": {
		"method": "POST",
		"header": [
			{
				"key": "expired_token",
				"value": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwOGE1YWNiNjYzZTQ0MzQ5YjdkNTNiNzk0ZTQzMTI0NCIsInVuaXF1ZV9uYW1lIjoidGVzdDIiLCJBbGxvd2VkT3JpZ2VuIjoiMEhMVTRBMTlRUFQ0NiIsImV4cGlyZXNGdCI6IjEzMjM1NTMzOTU2MzQxNDk2MyIsIm5iZiI6MTU5MDk3Mzk1NiwiZXhwIjoxNTkxMDYwMzU2LCJpYXQiOjE1OTA5NzM5NTZ9.Kjqxqm9pL4GocfZGh_ZpgcxHXuIsOmHpeIYKn8nSkoK4L33hZwAG01r5TTeY5LLdfRvVy3ptDbqVJ4fr4kj2rw",
				"type": "text"
			},
			{
				"key": "update_key",
				"value": "2719c3301c194de1b8de7b9d9f0a1fcf",
				"type": "text"
			}
		],
		"url": {
			"raw": "https://localhost:5001/api/clients/Refresh",
			"protocol": "https",
			"host": [
				"localhost"
			],
			"port": "5001",
			"path": [
				"api",
				"clients",
				"Refresh"
			]
		}
	},
```
	
NOTA: Usar Postman para reproducir este Request

8- Usar el token de acceso obtenido para hacer el siguiente Request para comprobar que todo funciona bien:

```json
"request": {
	"method": "GET",
	"header": [
		{
			"key": "Authorization",
			"value": "bearer <Valor del token de acceso obtenido>",
			"type": "text"
		},
		{
			"key": "Workspace",
			"value": "2003",
			"type": "text"
		}
	],
	"url": {
		"raw": "https://localhost:5001/api/?SERVICE=WMS&VERSION=1.3.0&REQUEST=GetMap&FORMAT=image/png&TRANSPARENT=true&LAYERS=1,2,3&CRS=EPSG:4326&STYLES=&WIDTH=2009&HEIGHT=1013&BBOX=23.079164028167765,-82.47275590896606,23.166067600250244,-82.30036497116089",
		
```
					
Si el Request anterior da como resultado la foto del mapa de La Habana -> OLS esta correctamente configurado y listo para ser usado.

2- Instalar y dejar compilando el codigo del visor y el server del visor (OLMC Server)

2.1- Entrar al proyecto OLMC y configurar en el appsettings el connectionString de la Bd.
2.2- Logearse usando las credenciales:

User: admin@gmail.com
Password: AdminOLMC!1

Si se logran loguear ya deberia de estar bien configurada la aplicacion cliente

3- COmprobar que se pinta el mapa en la aplicacion cliente
