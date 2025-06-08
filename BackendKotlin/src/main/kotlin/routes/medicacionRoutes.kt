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

fun Application.medicacionRoutes() {
    routing {
        route("/medicacion") {
            get("/{pacienteId}") {
                val pacienteId = call.parameters["pacienteId"]?.toIntOrNull()
                if (pacienteId == null) {
                    call.respond(HttpStatusCode.BadRequest, "Paciente inválido")
                    return@get
                }
                val list = transaction {
                    Medicacion.select { Medicacion.pacienteId eq pacienteId }.map {
                        MedicacionDTO(
                            id = it[Medicacion.id].value,
                            pacienteId = it[Medicacion.pacienteId].value,
                            fechaHora = it[Medicacion.fechaHora].toString(),
                            medicamento = it[Medicacion.medicamento],
                            dosis = it[Medicacion.dosis],
                            frecuencia = it[Medicacion.frecuencia],
                            observaciones = it[Medicacion.observaciones]
                        )
                    }
                }
                call.respond(list)
            }
            post {
                val dto = call.receive<MedicacionDTO>()

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
                    Medicacion.insertAndGetId {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[medicamento] = dto.medicamento
                        it[dosis] = dto.dosis
                        it[frecuencia] = dto.frecuencia
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

                val dto = call.receive<MedicacionDTO>()

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@put
                }

                val updated = transaction {
                    Medicacion.update({ Medicacion.id eq idParam }) {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[medicamento] = dto.medicamento
                        it[dosis] = dto.dosis
                        it[frecuencia] = dto.frecuencia
                        it[observaciones] = dto.observaciones
                    }
                }

                if (updated == 0) {
                    call.respond(HttpStatusCode.NotFound, "Registro de medicación no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Registro de medicación actualizado")
                }
            }

            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }

                val deleted = transaction {
                    Medicacion.deleteWhere { Medicacion.id eq idParam }
                }

                if (deleted == 0) {
                    call.respond(HttpStatusCode.NotFound, "Registro de medicación no encontrado")
                } else {
                    call.respond(HttpStatusCode.OK, "Registro de medicación eliminado")
                }
            }
        }
    }
}