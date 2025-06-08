package org.appmedica.db

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

object Medicacion : IntIdTable("medicacion") {
    val pacienteId = reference("paciente_id", Pacientes)
    val fechaHora = datetime("fecha_hora")
    val medicamento = varchar("medicamento", 100)
    val dosis = varchar("dosis", 50)
    val frecuencia = varchar("frecuencia", 50)
    val observaciones = text("observaciones").nullable()
}

data class MedicacionDTO(
    val id: Int? = null,
    val pacienteId: Int,
    val fechaHora: String,
    val medicamento: String,
    val dosis: String,
    val frecuencia: String,
    val observaciones: String?
)