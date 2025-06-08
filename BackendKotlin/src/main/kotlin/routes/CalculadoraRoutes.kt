package org.appmedica.routes

import io.ktor.server.application.*
import io.ktor.server.routing.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.http.*

fun Application.calculadoraRoutes() {
    routing {

        post("/calculadora/botellas") {
            val data = call.receive<Map<String, Double>>() // recibe JSON con los valores

            val horasVuelo = data["horasVuelo"]
            val consumo = data["consumoOxigeno"]
            val presion = data["presionBotella"]
            val litros = data["litrosBotella"]

            if (horasVuelo == null || consumo == null || presion == null || litros == null) {
                call.respond(HttpStatusCode.BadRequest, "Faltan valores")
                return@post
            }

            val resultado = ((horasVuelo * 60 * consumo) / presion) / litros
            call.respond(mapOf("resultado" to String.format("%.2f", resultado)))
        }

        post("/calculadora/horas") {
            val data = call.receive<Map<String, Double>>()

            val litros = data["litrosBotella"]
            val numBotellas = data["numeroBotellas"]
            val presion = data["presionBotella"]
            val consumo = data["consumoOxigeno"]

            if (litros == null || numBotellas == null || presion == null || consumo == null) {
                call.respond(HttpStatusCode.BadRequest, "Faltan valores")
                return@post
            }

            val resultado = (((litros * numBotellas) * presion) / consumo) / 60
            call.respond(mapOf("resultado" to String.format("%.2f", resultado)))
        }

        post("/calculadora/dosis") {
            val data = call.receive<Map<String, Double>>()

            val dosisPorKg = data["dosisPorKg"]
            val peso = data["peso"]

            if (dosisPorKg == null || peso == null) {
                call.respond(HttpStatusCode.BadRequest, "Faltan valores")
                return@post
            }

            val dosis = dosisPorKg * peso
            call.respond(mapOf("resultado" to String.format("%.2f", dosis)))
        }
    }
}
