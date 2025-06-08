package org.appmedica.routes

import io.ktor.server.application.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import io.ktor.http.*
import org.appmedica.db.*
import org.jetbrains.exposed.sql.*
import org.jetbrains.exposed.sql.transactions.transaction
import org.jetbrains.exposed.dao.id.EntityID
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq

fun Application.pacienteRoutes() {
    routing {
        route("/pacientes") {
            // Obtener todos los pacientes
            get {
                val pacientes = transaction {
                    Pacientes.selectAll().map {
                        PacienteDTO(
                            Id = it[Pacientes.id].value,
                            Nombre = it[Pacientes.Nombre],
                            Habitacion = it[Pacientes.Habitacion],
                            Unidad = it[Pacientes.Unidad],
                            Edad = it[Pacientes.Edad],
                            Peso = it[Pacientes.Peso],
                            Altura = it[Pacientes.Altura]
                        )
                    }
                }
                call.respond(pacientes)
            }

            // Crear un paciente nuevo
            post {
                val dto = call.receive<PacienteDTO>()

                if(dto.Nombre.isBlank()) {
                    call.respond(HttpStatusCode.BadRequest, "El nombre es obligatorio")
                    return@post
                }

                val nuevoPaciente = transaction {
                    val id = Pacientes.insertAndGetId {
                        it[Nombre] = dto.Nombre
                        it[Habitacion] = dto.Habitacion ?: ""
                        it[Unidad] = dto.Unidad ?: ""
                        it[Edad] = dto.Edad
                        it[Peso] = dto.Peso
                        it[Altura] = dto.Altura
                    }.value

                    Pacientes.select { Pacientes.id eq id }.single().let {
                        PacienteDTO(
                            Id = it[Pacientes.id].value,
                            Nombre = it[Pacientes.Nombre],
                            Habitacion = it[Pacientes.Habitacion],
                            Unidad = it[Pacientes.Unidad],
                            Edad = it[Pacientes.Edad],
                            Peso = it[Pacientes.Peso],
                            Altura = it[Pacientes.Altura]
                        )
                    }
                }

                call.respond(nuevoPaciente)
            }

            // Opcional: obtener paciente por id
            get("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@get
                }
                val paciente = transaction {
                    Pacientes.select { Pacientes.id eq idParam }.singleOrNull()?.let {
                        PacienteDTO(
                            Id = it[Pacientes.id].value,
                            Nombre = it[Pacientes.Nombre],
                            Habitacion = it[Pacientes.Habitacion],
                            Unidad = it[Pacientes.Unidad],
                            Edad = it[Pacientes.Edad],
                            Peso = it[Pacientes.Peso],
                            Altura = it[Pacientes.Altura]
                        )
                    }
                }
                if (paciente == null) {
                    call.respond(HttpStatusCode.NotFound, "Paciente no encontrado")
                } else {
                    call.respond(paciente)
                }
            }
            put("/put/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@put
                }

                val dto = call.receive<PacienteDTO>()

                transaction {
                    Pacientes.update({ Pacientes.id eq idParam }) {
                        it[Nombre] = dto.Nombre
                        it[Habitacion] = dto.Habitacion ?: ""
                        it[Unidad] = dto.Unidad ?: ""
                        it[Edad] = dto.Edad
                        it[Peso] = dto.Peso
                        it[Altura] = dto.Altura
                    }
                }

                call.respond(HttpStatusCode.OK, dto)
            }

            delete("/{id}") {
                val idParam = call.parameters["id"]?.toIntOrNull()
                if (idParam == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@delete
                }

                transaction {
                    Constantes.deleteWhere { Constantes.pacienteId eq idParam }
                    Medicacion.deleteWhere { Medicacion.pacienteId eq idParam }
                    Alergias.deleteWhere { Alergias.pacienteId eq idParam }
                    Diuresis.deleteWhere { Diuresis.pacienteId eq idParam }
                    BalanceHidrico.deleteWhere { BalanceHidrico.pacienteId eq idParam }
                    CuidadosDeEnfermeria.deleteWhere { CuidadosDeEnfermeria.pacienteId eq idParam }

                    Pacientes.deleteWhere { Pacientes.id eq idParam }
                }

                call.respond(HttpStatusCode.OK, "Paciente eliminado")
            }
        }
    }
}