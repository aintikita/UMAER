package org.appmedica.db

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

object Diuresis : IntIdTable("diuresis") {
    val pacienteId = reference("paciente_id", Pacientes)
    val fechaHora = datetime("fecha_hora")
    val cantidad = float("cantidad")
    val observaciones = text("observaciones").nullable()
}

data class DiuresisDTO(
    val id: Int? = null,
    val pacienteId: Int,
    val fechaHora: String,
    val cantidad: Float,
    val observaciones: String?
)