package org.appmedica

import io.ktor.serialization.gson.*
import io.ktor.serialization.kotlinx.json.*
import io.ktor.server.engine.*
import io.ktor.server.netty.*
import io.ktor.server.application.*
import io.ktor.server.plugins.contentnegotiation.*
import io.ktor.server.plugins.cors.routing.*
import io.ktor.server.routing.*
import org.appmedica.controller.pdfRouting
import org.appmedica.db.insertarDatosPorDefecto
import org.appmedica.routes.*
import org.appmedica.routes.uploadPdfRouting
import utils.DocumentoLoader

val documentos by lazy {
    listOf("po", "mochilas", "manuales_medicinas", "manuales_equipos")
        .flatMap { carpeta ->
            DocumentoLoader.cargarTodos(carpeta).entries
        }
        .associate { it.toPair() }
}

fun main() {
    val host = "0.0.0.0"
    val port = 8081


    println("✅ Iniciando servidor en $host:$port")

    embeddedServer(Netty, port = 8081, host = "0.0.0.0") {
        module()
        install(CORS) {
            anyHost()
        }


        routing {
            pdfRouting() // Enlace al controlador
            uploadPdfRouting()
            poRouting()

        }

    }.start(wait = true)
}

fun Application.module() {
    install(ContentNegotiation) {
        gson()
    }

    org.appmedica.db.DatabaseConfig.init()
    insertarDatosPorDefecto()
    authRoutes()
    calculadoraRoutes()
    documentRoutes()
    pacienteRoutes()
    constantesRoutes()
    medicacionRoutes()
    diuresisRoutes()
    balanceRoutes()
    alergiasRoutes()
    cuidadosRoutes()


}
