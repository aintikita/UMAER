package org.appmedica.routes

import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import io.ktor.server.routing.application
import org.appmedica.utils.DocumentoReader

fun Route.poRouting() {
    get("/po/contenido") {
        val ruta = call.request.queryParameters["ruta"] ?: ""
        try {
            val lista = DocumentoReader.listarContenido("po", ruta)
            call.respond(lista)
        } catch (e: Exception) {
            application.log.error("Error al listar contenido en ruta: $ruta", e)
            call.respond(HttpStatusCode.InternalServerError, "Error interno")
        }
    }
}
