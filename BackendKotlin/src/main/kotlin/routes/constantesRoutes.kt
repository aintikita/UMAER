package org.appmedica.routes

import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import org.appmedica.db.ConstanteDTO
import org.appmedica.db.Constantes
import org.appmedica.db.Pacientes
import org.jetbrains.exposed.dao.id.EntityID
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq
import org.jetbrains.exposed.sql.insertAndGetId
import org.jetbrains.exposed.sql.select
import org.jetbrains.exposed.sql.transactions.transaction
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter
import java.time.LocalDateTime
import org.jetbrains.exposed.sql.update
import org.jetbrains.exposed.sql.deleteWhere

fun Application.constantesRoutes() {
    routing {
        route("/constantes") {
            get("/{pacienteId}") {
                val pacienteId = call.parameters["pacienteId"]?.toIntOrNull()
                if (pacienteId == null) {
                    call.respond(HttpStatusCode.BadRequest, "Paciente inválido")
                    return@get
                }
                val list = transaction {
                    Constantes.select { Constantes.pacienteId eq pacienteId }.map {
                        ConstanteDTO(
                            id = it[Constantes.id].value,
                            pacienteId = it[Constantes.pacienteId].value,
                            fechaHora = it[Constantes.fechaHora].toString(),
                            temperatura = it[Constantes.temperatura],
                            frecuenciaCardiaca = it[Constantes.frecuenciaCardiaca],
                            frecuenciaRespiratoria = it[Constantes.frecuenciaRespiratoria],
                            saturacionOxigeno = it[Constantes.saturacionOxigeno],
                            presionArterial = it[Constantes.presionArterial]
                        )
                    }
                }
                call.respond(list)
            }
            post {
                val dto = call.receive<ConstanteDTO>()

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
                    Constantes.insertAndGetId {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[temperatura] = dto.temperatura
                        it[frecuenciaCardiaca] = dto.frecuenciaCardiaca
                        it[frecuenciaRespiratoria] = dto.frecuenciaRespiratoria
                        it[saturacionOxigeno] = dto.saturacionOxigeno
                        it[presionArterial] = dto.presionArterial
                    }.value
                }

                call.respond(dto.copy(id = id))
            }

            // PUT para actualizar una constante existente
            put("/put/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@put
                }

                val dto = call.receive<ConstanteDTO>()
                println("PUT /constantes/$idParam -> $dto")

                val fecha = try {
                    val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME
                    ZonedDateTime.parse(dto.fechaHora, formatter).toLocalDateTime()
                } catch (e: Exception) {
                    call.respond(HttpStatusCode.BadRequest, "Formato de fecha inválido: ${dto.fechaHora}")
                    return@put
                }

                transaction {
                    Constantes.update({ Constantes.id eq idParam }) {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[fechaHora] = fecha
                        it[temperatura] = dto.temperatura
                        it[frecuenciaCardiaca] = dto.frecuenciaCardiaca
                        it[frecuenciaRespiratoria] = dto.frecuenciaRespiratoria
                        it[saturacionOxigeno] = dto.saturacionOxigeno
                        it[presionArterial] = dto.presionArterial
                    }
                }

                call.respond(HttpStatusCode.OK, dto)
            }

            // DELETE para eliminar una constante
            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }
                val rowsDeleted = transaction {
                    Constantes.deleteWhere { Constantes.id eq idParam }
                }
                if (rowsDeleted > 0) {
                    call.respond(HttpStatusCode.OK, "Constante eliminada")
                } else {
                    call.respond(HttpStatusCode.NotFound, "Constante no encontrada")
                }
            }
        }
    }
}