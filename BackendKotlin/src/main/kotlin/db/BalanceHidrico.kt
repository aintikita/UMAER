package org.appmedica.db

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

object BalanceHidrico : IntIdTable("balance_hidrico") {
    val pacienteId = reference("paciente_id", Pacientes)
    val fechaHora = datetime("fecha_hora")
    val ingreso = float("ingreso")
    val egreso = float("egreso")
    val observaciones = text("observaciones").nullable()
}

data class BalanceHidricoDTO(
    val id: Int? = null,
    val pacienteId: Int,
    val fechaHora: String,
    val ingreso: Float,
    val egreso: Float,
    val observaciones: String?
)