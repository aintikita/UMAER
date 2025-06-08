package org.appmedica.db

import org.jetbrains.exposed.dao.id.IntIdTable

object Pacientes : IntIdTable("pacientes") {
    val Nombre = varchar("Nombre", 100)
    val Habitacion = varchar("Habitacion", 50).nullable()
    val Unidad = varchar("Unidad", 50).nullable()
    val Edad = integer("Edad").default(0)
    val Peso = float("Peso").default(0f)
    val Altura = float("Altura").default(0f)
}


data class PacienteDTO(
    val Id: Int? = null,
    val Nombre: String,
    val Habitacion: String? = null,
    val Unidad: String? = null,
    val Edad: Int = 0,
    val Peso: Float = 0f,
    val Altura: Float = 0f
)

