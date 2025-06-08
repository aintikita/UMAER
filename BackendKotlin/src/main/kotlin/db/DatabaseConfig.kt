package org.appmedica.db

import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.transactions.transaction

object DatabaseConfig {
    fun init() {
        Database.connect(
            url = "jdbc:sqlite:appmedica.db",
            driver = "org.sqlite.JDBC"
        )

        transaction {
            // Crea las tablas si no existen
            SchemaUtils.create(
                Usuarios,
                Pacientes,
                Constantes,
                Medicacion,
                Diuresis,
                BalanceHidrico,
                CuidadosDeEnfermeria,
                Alergias)
        }
    }
}
