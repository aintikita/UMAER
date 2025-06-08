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
import org.jetbrains.exposed.sql.update
import org.jetbrains.exposed.sql.deleteWhere

fun Application.alergiasRoutes() {
    routing {
        route("/alergias") {
            get("/{pacienteId}") {
                val pacienteId = call.parameters["pacienteId"]?.toIntOrNull()
                if (pacienteId == null) {
                    call.respond(HttpStatusCode.BadRequest, "Paciente inválido")
                    return@get
                }
                val list = transaction {
                    Alergias.select { Alergias.pacienteId eq pacienteId }.map {
                        AlergiaDTO(
                            id = it[Alergias.id].value,
                            pacienteId = it[Alergias.pacienteId].value,
                            alergia = it[Alergias.alergia],
                            descripcion = it[Alergias.descripcion]
                        )
                    }
                }
                call.respond(list)
            }
            post {
                val dto = call.receive<AlergiaDTO>()

                val existePaciente = transaction {
                    Pacientes.select { Pacientes.id eq dto.pacienteId }.count() > 0
                }

                if (!existePaciente) {
                    call.respond(HttpStatusCode.NotFound, "Paciente con ID ${dto.pacienteId} no existe")
                    return@post
                }

                val id = transaction {
                    Alergias.insertAndGetId {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[alergia] = dto.alergia
                        it[descripcion] = dto.descripcion
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

                val dto = call.receive<AlergiaDTO>()

                val updated = transaction {
                    Alergias.update({ Alergias.id eq idParam }) {
                        it[pacienteId] = EntityID(dto.pacienteId, Pacientes)
                        it[alergia] = dto.alergia
                        it[descripcion] = dto.descripcion
                    }
                }

                if (updated == 0) {
                    call.respond(HttpStatusCode.NotFound, "Alergia no encontrada")
                } else {
                    call.respond(HttpStatusCode.OK, "Alergia actualizada")
                }
            }

            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }

                val deleted = transaction {
                    Alergias.deleteWhere { Alergias.id eq idParam }
                }

                if (deleted == 0) {
                    call.respond(HttpStatusCode.NotFound, "Alergia no encontrada")
                } else {
                    call.respond(HttpStatusCode.OK, "Alergia eliminada")
                }
            }
        }
    }
}