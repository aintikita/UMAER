package org.appmedica.controller

import io.ktor.server.routing.*
import io.ktor.server.application.*
import io.ktor.server.response.*
import io.ktor.server.request.*
import io.ktor.http.*
import org.appmedica.documentos
import org.appmedica.services.OpenRouterService // ⬅️ Importa el nuevo servicio

fun Route.pdfRouting() {
    route("/pdf") {
        post("preguntar") {
            val body = call.receive<Map<String, String>>()
            val pregunta = body["pregunta"] ?: return@post call.respond(HttpStatusCode.BadRequest)

            val textoCompleto = documentos.values.joinToString("\n\n").take(30000)

            try {
                val respuesta = OpenRouterService.obtenerRespuesta(textoCompleto, pregunta)
                call.respond(mapOf("respuesta" to respuesta))
            } catch (e: Exception) {
                e.printStackTrace()
                call.respond(HttpStatusCode.InternalServerError, "Error al contactar con OpenRouter.")
            }
        }
    }
}


