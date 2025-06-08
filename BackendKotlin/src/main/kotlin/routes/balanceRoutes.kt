package org.appmedica.routes

import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import org.appmedica.db.*
import org.jetbrains.exposed.dao.id.EntityID
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq
import org.jetbrains.exposed.sql.insertAndGetId
import org.jetbrains.exposed.sql.select
import org.jetbrains.exposed.sql.transactions.transaction
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter
import org.jetbrains.exposed.sql.update
import org.jetbrains.exposed.sql.deleteWhere

fun Application.balanceRoutes() {
    routing {
        route("/balance") {
            get("/{pacienteId}") {
                val pacienteId = call.parameters["pacienteId"]?.toIntOrNull()
                if (pacienteId == null) {
                    call.respond(HttpStatusCode.BadRequest, "Paciente inválido")
                    return@get
                }
                val list = transaction {
                    BalanceHidrico.select { BalanceHidrico.pacienteId eq pacienteId }.map {
                        BalanceHidricoDTO(
                            id = it[BalanceHidrico.id].value,
                            pacienteId = it[BalanceHidrico.pacienteId].value,
                            fechaHora = it[BalanceHidrico.fechaHora].toString(),
                            ingreso = it[BalanceHidrico.ingreso],
                            egreso = it[BalanceHidrico.egreso],
                            observaciones = it[BalanceHidrico.observaciones]
                        )
                    }
                }
                call.respond(list)
            }
            post {
                val dto = call.receive<BalanceHidricoDTO>()

                val existePaciente = transaction {
                    Pacientes.select { Pacientes.id eq dto.pacienteId }.count() > 0
                }

                if (!existePaciente) {
                    call.respond(HttpStatusCode.NotFound, "Paciente con ID ${dto.pacienteId} no existe")
                    return@post
                }

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@post
                }

                val id = transaction {
                    BalanceHidrico.insertAndGetId {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[ingreso] = dto.ingreso
                        it[egreso] = dto.egreso
                        it[observaciones] = dto.observaciones
                    }.value
                }
                call.respond(dto.copy(id = id))
            }
            put("/put/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@put
                }

                val dto = call.receive<BalanceHidricoDTO>()

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@put
                }

                val updated = transaction {
                    BalanceHidrico.update({ BalanceHidrico.id eq idParam }) {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[ingreso] = dto.ingreso
                        it[egreso] = dto.egreso
                        it[observaciones] = dto.observaciones
                    }
                }

                if (updated == 0) {
                    call.respond(HttpStatusCode.NotFound, "Registro de balance no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Registro de balance actualizado")
                }
            }

            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }

                val deleted = transaction {
                    BalanceHidrico.deleteWhere { BalanceHidrico.id eq idParam }
                }

                if (deleted == 0) {
                    call.respond(HttpStatusCode.NotFound, "Registro de balance no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Registro de balance eliminado")
                }
            }
        }
    }
}