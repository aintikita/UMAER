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

fun Application.diuresisRoutes() {
    routing {
        route("/diuresis") {
            get("/{pacienteId}") {
                val pacienteId = call.parameters["pacienteId"]?.toIntOrNull()
                if (pacienteId == null) {
                    call.respond(HttpStatusCode.BadRequest, "Paciente inválido")
                    return@get
                }
                val list = transaction {
                    Diuresis.select { Diuresis.pacienteId eq pacienteId }.map {
                        DiuresisDTO(
                            id = it[Diuresis.id].value,
                            pacienteId = it[Diuresis.pacienteId].value,
                            fechaHora = it[Diuresis.fechaHora].toString(),
                            cantidad = it[Diuresis.cantidad],
                            observaciones = it[Diuresis.observaciones]
                        )
                    }
                }
                call.respond(list)
            }
            post {
                val dto = call.receive<DiuresisDTO>()

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@post
                }
                val id = transaction {
                    Diuresis.insertAndGetId {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[cantidad] = dto.cantidad
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

                val dto = call.receive<DiuresisDTO>()

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@put
                }

                val updated = transaction {
                    Diuresis.update({ Diuresis.id eq idParam }) {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[cantidad] = dto.cantidad
                        it[observaciones] = dto.observaciones
                    }
                }

                if (updated == 0) {
                    call.respond(HttpStatusCode.NotFound, "Registro de diuresis no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Registro de diuresis actualizado")
                }
            }

            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }

                val deleted = transaction {
                    Diuresis.deleteWhere { Diuresis.id eq idParam }
                }

                if (deleted == 0) {
                    call.respond(HttpStatusCode.NotFound, "Registro de diuresis no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Registro de diuresis eliminado")
                }
            }
        }
    }
}