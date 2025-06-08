package org.appmedica.routes

import io.ktor.server.application.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.http.*
import io.ktor.server.routing.*
import org.jetbrains.exposed.sql.*
import org.jetbrains.exposed.sql.transactions.transaction
import org.appmedica.db.Usuarios

import org.jetbrains.exposed.sql.deleteWhere
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq
import java.io.File


// routes/AuthRoutes.kt
data class LoginRequest(val usuario: String, val contrasena: String)


fun Application.authRoutes() {
    routing {
        post("/login") {
            try {
                val body = call.receive<String>()
                println("📥 Raw body: $body")

                val datos = call.receive<LoginRequest>()


                // 👇 AÑADE ESTO:
                println("🔐 Intento de login")
                println("➤ Usuario recibido: '${datos.usuario}'")
                println("➤ Contraseña recibida: '${datos.contrasena}'")

                val user = transaction {
                    Usuarios.select {
                        (Usuarios.nombreUsuario eq datos.usuario) and
                                (Usuarios.contrasena eq datos.contrasena)
                    }.singleOrNull()
                }

                if (user != null) {
                    call.respond(HttpStatusCode.OK, mapOf("mensaje" to "Login correcto", "admin" to user[Usuarios.esAdmin]))
                } else {
                    println("❌ Usuario o contraseña incorrectos.")
                    call.respond(HttpStatusCode.Unauthorized, "Credenciales incorrectas")
                }
            } catch (e: Exception) {
                println("💥 Error en /login: ${e.message}")
                call.respond(HttpStatusCode.InternalServerError, "Error interno")
            }
        }



        post("/registro") {
            try {
                val datos = call.receive<RegistroRequest>()

                println("🛠️ Datos de registro:")
                println("  ➤ Usuario: ${datos.usuario}")
                println("  ➤ Contraseña nueva: ${datos.contrasena}")
                println("  ➤ Clave admin recibida: ${datos.adminPassword}")

                val admin = transaction {
                    Usuarios.select {
                        (Usuarios.nombreUsuario eq "admin") and
                                (Usuarios.contrasena eq datos.adminPassword)
                    }.singleOrNull()
                }

                if (admin == null) {
                    println("❌ Contraseña de administrador incorrecta.")
                    call.respond(HttpStatusCode.Forbidden, "Contraseña de administrador incorrecta")
                    return@post
                }

                transaction {
                    Usuarios.insert {
                        it[nombreUsuario] = datos.usuario
                        it[contrasena] = datos.contrasena
                        it[esAdmin] = false
                    }
                }

                println("✅ Usuario ${datos.usuario} registrado exitosamente.")
                call.respond(HttpStatusCode.OK, mapOf("mensaje" to "Usuario registrado correctamente."))
            } catch (e: Exception) {
                println("💥 Error en /registro: ${e.message}")
                call.respond(HttpStatusCode.InternalServerError, "Error interno")
            }
        }

        // NUEVO: Modelo para actualización
        data class UsuarioUpdateRequest(val nombreUsuario: String)

// NUEVO: Obtener todos los usuarios
        get("/usuarios") {
            try {
                val usuarios = transaction {
                    Usuarios.selectAll().map {
                        mapOf(
                            "id" to it[Usuarios.id],
                            "nombreUsuario" to it[Usuarios.nombreUsuario],
                            "esAdmin" to it[Usuarios.esAdmin]
                        )
                    }
                }
                call.respond(HttpStatusCode.OK, usuarios)
            } catch (e: Exception) {
                println("💥 Error en /usuarios: ${e.message}")
                call.respond(HttpStatusCode.InternalServerError, "Error interno")
            }
        }

// NUEVO: Actualizar usuario
        put("/usuario/{id}") {
            try {
                val id = call.parameters["id"]?.toIntOrNull()
                if (id == null) {
                    call.respond(HttpStatusCode.BadRequest, "ID inválido")
                    return@put
                }

                val datos = call.receive<UsuarioUpdateRequest>()

                transaction {
                    Usuarios.update({ Usuarios.id eq id }) {
                        it[nombreUsuario] = datos.nombreUsuario
                    }
                }

                call.respond(HttpStatusCode.OK, mapOf("mensaje" to "Usuario actualizado correctamente"))
            } catch (e: Exception) {
                println("💥 Error en /usuario/{id} (PUT): ${e.message}")
                call.respond(HttpStatusCode.InternalServerError, "Error interno")
            }
        }

        put("/usuarios/reset-password") {
            val body = call.receive<Map<String, String>>()
            val nombreUsuario = body["nombreUsuario"]
            val nuevaContrasena = body["nuevaContrasena"]

            if (nombreUsuario == null || nuevaContrasena == null) {
                call.respond(HttpStatusCode.BadRequest, "Datos incompletos.")
                return@put
            }

            val usuario = transaction {
                Usuarios.select { Usuarios.nombreUsuario eq nombreUsuario }.singleOrNull()
            }

            if (usuario == null) {
                call.respond(HttpStatusCode.NotFound, "Usuario no encontrado.")
                return@put
            }

            if (usuario[Usuarios.contrasena] == nuevaContrasena) {
                call.respond(HttpStatusCode.BadRequest, "La nueva contraseña no puede ser igual a la anterior.")
                return@put
            }

            transaction {
                Usuarios.update({ Usuarios.nombreUsuario eq nombreUsuario }) {
                    it[contrasena] = nuevaContrasena
                }
            }

            call.respond(HttpStatusCode.OK)
        }


        delete("/usuario/{id}") {
        try {
            val id = call.parameters["id"]?.toIntOrNull()
            if (id == null) {
                call.respond(HttpStatusCode.BadRequest, "ID inválido")
                return@delete
            }

            val filasEliminadas = transaction {
                Usuarios.deleteWhere { Usuarios.id eq id }
            }

            if (filasEliminadas > 0) {
                call.respond(HttpStatusCode.OK, mapOf("mensaje" to "Usuario eliminado correctamente"))
            } else {
                call.respond(HttpStatusCode.NotFound, "Usuario no encontrado")
            }

        } catch (e: Exception) {
            println("💥 Error al eliminar usuario: ${e.message}")
            call.respond(HttpStatusCode.InternalServerError, "Error interno")
        }
    }

    }
}

