package org.appmedica.routes

data class RegistroRequest(
    val usuario: String,
    val contrasena: String,
    val adminPassword: String
)
