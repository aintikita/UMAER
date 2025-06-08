package org.appmedica.routes

import io.ktor.server.application.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import io.ktor.http.*
import java.io.File
import java.net.URLDecoder
import org.appmedica.utils.DocumentoReader
import io.ktor.server.request.*


fun Application.documentRoutes() {
    routing {
        val categorias = listOf("po", "mochilas", "manuales_medicinas", "manuales_equipos")

        categorias.forEach { categoria ->
            route("/$categoria") {
                get("/contenido") {
                    val ruta = call.request.queryParameters["ruta"] ?: ""
                    val decodedRuta = URLDecoder.decode(ruta, "UTF-8")
                    val lista = DocumentoReader.listarContenido(categoria, decodedRuta)
                    call.respond(lista)
                }

                get("/abrir") {
                    val archivoParam = call.request.queryParameters["archivo"]
                    if (archivoParam == null) {
                        call.respond(HttpStatusCode.BadRequest, "Falta el parámetro 'archivo'")
                        return@get
                    }

                    val decodedPath = URLDecoder.decode(archivoParam, "UTF-8")
                    val archivo = File("archivos/$decodedPath")

                    if (!archivo.exists()) {
                        call.respond(HttpStatusCode.NotFound, "Archivo no encontrado")
                        return@get
                    }

                    call.respondFile(archivo)
                }
            }
        }

        // 👇 NUEVO ENDPOINT PARA CREAR CARPETAS
        post("/carpeta/crear") {
            val request = call.receive<Map<String, String>>()
            val carpetaPadre = request["carpetaPadre"] ?: return@post call.respond(HttpStatusCode.BadRequest, "Falta carpetaPadre")
            val nombreNuevaCarpeta = request["nombreNuevaCarpeta"] ?: return@post call.respond(HttpStatusCode.BadRequest, "Falta nombreNuevaCarpeta")

            val rutaCompleta = File("archivos/$carpetaPadre/$nombreNuevaCarpeta")

            try {
                if (rutaCompleta.exists()) {
                    call.respond(HttpStatusCode.Conflict, "La carpeta ya existe.")
                    return@post
                }

                rutaCompleta.mkdirs()

                if (rutaCompleta.exists())
                    call.respond(HttpStatusCode.OK, "Carpeta creada con éxito.")
                else
                    call.respond(HttpStatusCode.InternalServerError, "Error al crear la carpeta.")

            } catch (e: Exception) {
                call.respond(HttpStatusCode.InternalServerError, "Error del servidor: ${e.message}")
            }
        }
    }
}
