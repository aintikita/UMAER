package org.appmedica.routes

import io.ktor.server.routing.*
import io.ktor.server.application.*
import io.ktor.http.content.*
import io.ktor.server.response.*
import io.ktor.http.*
import io.ktor.server.request.*
import org.appmedica.db.Usuarios.esAdmin
import java.io.File

fun Route.uploadPdfRouting() {
    post("/manuales/upload") {

        val multipart = call.receiveMultipart()
        var carpetaDestino = ""
        var nombreArchivo = ""
        var guardadoCorrecto = false

        multipart.forEachPart { part ->
            when (part) {
                is PartData.FormItem -> {
                    if (part.name == "carpeta") {
                        carpetaDestino = part.value
                    }
                    if (part.name == "nombreArchivo") {
                        nombreArchivo = part.value
                    }
                }
                is PartData.FileItem -> {
                    if (carpetaDestino.isNotBlank() && nombreArchivo.isNotBlank()) {
                        val carpeta = File("archivos/$carpetaDestino")
                        if (!carpeta.exists()) carpeta.mkdirs()

                        val archivo = File(carpeta, nombreArchivo)
                        part.streamProvider().use { its -> archivo.outputStream().buffered().use { its.copyTo(it) } }
                        guardadoCorrecto = true
                    }
                }
                else -> {}
            }
            part.dispose()
        }

        if (guardadoCorrecto) {
            call.respondText("Archivo subido correctamente.", status = HttpStatusCode.OK)
        } else {
            call.respondText("Faltan datos o error al guardar.", status = HttpStatusCode.BadRequest)
        }
    }

}
