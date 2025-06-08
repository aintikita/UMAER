package org.appmedica.db

import org.jetbrains.exposed.sql.Table

object Usuarios : Table("usuarios") {
    val id = integer("id").autoIncrement()
    val nombreUsuario = varchar("nombre_usuario", 50).uniqueIndex()
    val contrasena = varchar("contrasena", 255)
    val esAdmin = bool("es_admin")
    override val primaryKey = PrimaryKey(id)
}
