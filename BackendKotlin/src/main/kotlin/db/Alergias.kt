package org.appmedica.db

import org.appmedica.db.Constantes.nullable
import org.jetbrains.exposed.dao.id.IntIdTable

object Alergias : IntIdTable("alergias") {
    val pacienteId = reference("paciente_id", Pacientes)
    val alergia = varchar("alergia", 100).nullable()
    val descripcion = text("descripcion").nullable()
}

data class AlergiaDTO(
    val id: Int? = null,
    val pacienteId: Int,
    val alergia: String?,
    val descripcion: String?
)