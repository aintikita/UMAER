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

fun Application.cuidadosRoutes() {
    routing {
        route("/cuidados") {
            get("/{pacienteId}") {
                val pacienteId = call.parameters["pacienteId"]?.toIntOrNull()
                if (pacienteId == null) {
                    call.respond(HttpStatusCode.BadRequest, "Paciente inválido")
                    return@get
                }
                val list = transaction {
                    CuidadosDeEnfermeria.select { CuidadosDeEnfermeria.pacienteId eq pacienteId }.map {
                        CuidadoEnfermeriaDTO(
                            id = it[CuidadosDeEnfermeria.id].value,
                            pacienteId = it[CuidadosDeEnfermeria.pacienteId].value,
                            fechaHora = it[CuidadosDeEnfermeria.fechaHora].toString(),
                            descripcion = it[CuidadosDeEnfermeria.descripcion]
                        )
                    }
                }
                call.respond(list)
            }
            post {
                val dto = call.receive<CuidadoEnfermeriaDTO>()

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
                    CuidadosDeEnfermeria.insertAndGetId {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[descripcion] = dto.descripcion
                    }.value
                }
                call.respond(dto.copy(id = id))
            }
            // Actualizar cuidado existente
            put("/put/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@put
                }

                val dto = call.receive<CuidadoEnfermeriaDTO>()

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@put
                }

                val updated = transaction {
                    CuidadosDeEnfermeria.update({ CuidadosDeEnfermeria.id eq idParam }) {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[descripcion] = dto.descripcion
                    }
                }

                if (updated == 0) {
                    call.respond(HttpStatusCode.NotFound, "Cuidado no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Cuidado actualizado")
                }
            }

            // Eliminar cuidado
            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }

                val deleted = transaction {
                    CuidadosDeEnfermeria.deleteWhere { CuidadosDeEnfermeria.id eq idParam }
                }

                if (deleted == 0) {
                    call.respond(HttpStatusCode.NotFound, "Cuidado no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Cuidado eliminado")
                }
            }
        }
    }
}