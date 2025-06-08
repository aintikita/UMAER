package org.appmedica.db

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

object CuidadosDeEnfermeria : IntIdTable("cuidados_enfermeria") {
    val pacienteId = reference("paciente_id", Pacientes)
    val fechaHora = datetime("fecha_hora")
    val descripcion = text("descripcion")
}

data class CuidadoEnfermeriaDTO(
    val id: Int? = null,
    val pacienteId: Int,
    val fechaHora: String,
    val descripcion: String
)