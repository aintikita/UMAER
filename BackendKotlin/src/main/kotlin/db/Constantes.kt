package org.appmedica.db

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

object Constantes : IntIdTable("constantes") {
    val pacienteId = reference("paciente_id", Pacientes)
    val fechaHora = datetime("fecha_hora")
    val temperatura = float("temperatura").nullable()
    val frecuenciaCardiaca = integer("frecuencia_cardiaca").nullable()
    val frecuenciaRespiratoria = integer("frecuencia_respiratoria").nullable()
    val saturacionOxigeno = integer("saturacion_oxigeno").nullable()
    val presionArterial = varchar("presion_arterial", 20).nullable()
}

data class ConstanteDTO(
    val id: Int? = null,
    val pacienteId: Int,
    val fechaHora: String,
    val temperatura: Float?,
    val frecuenciaCardiaca: Int?,
    val frecuenciaRespiratoria: Int?,
    val saturacionOxigeno: Int?,
    val presionArterial: String?
)