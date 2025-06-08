package org.appmedica.db

import org.jetbrains.exposed.sql.*
import org.jetbrains.exposed.sql.transactions.transaction
import java.time.LocalDateTime

fun insertarDatosPorDefecto() {
    transaction {

        // Usuarios
        if (Usuarios.selectAll().empty()) {
            Usuarios.insert {
                it[nombreUsuario] = "admin"
                it[contrasena] = "admin123"
                it[esAdmin] = true
            }
            Usuarios.insert {
                it[nombreUsuario] = "juan"
                it[contrasena] = "juan1234"
                it[esAdmin] = false
            }
            Usuarios.insert {
                it[nombreUsuario] = "maria"
                it[contrasena] = "maria1234"
                it[esAdmin] = false
            }
            Usuarios.insert {
                it[nombreUsuario] = "pedro"
                it[contrasena] = "pedro1234"
                it[esAdmin] = false
            }
        }

        if (Pacientes.selectAll().empty()) {
            val ahora = LocalDateTime.now()

            val paciente1 = Pacientes.insertAndGetId {
                it[Nombre] = "Ana Ruiz"
                it[Habitacion] = "A101"
                it[Unidad] = "UCI"
                it[Edad] = 70
                it[Peso] = 60.0f
                it[Altura] = 1.60f
            }.value

            Constantes.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-17T10:00:00")
                it[temperatura] = 37.8f
                it[frecuenciaCardiaca] = 85
                it[frecuenciaRespiratoria] = 22
                it[saturacionOxigeno] = 96
                it[presionArterial] = "130/85"
            }
            Constantes.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-16T10:00:00")
                it[temperatura] = 38.0f
                it[frecuenciaCardiaca] = 90
                it[frecuenciaRespiratoria] = 25
                it[saturacionOxigeno] = 95
                it[presionArterial] = "135/88"
            }
            Constantes.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-15T10:00:00")
                it[temperatura] = 36.9f
                it[frecuenciaCardiaca] = 78
                it[frecuenciaRespiratoria] = 20
                it[saturacionOxigeno] = 97
                it[presionArterial] = "120/80"
            }

            Medicacion.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-27T08:00:00")
                it[medicamento] = "Paracetamol"
                it[dosis] = "500mg"
                it[frecuencia] = "8h"
                it[observaciones] = "Dolor leve"
            }
            Medicacion.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-26T20:00:00")
                it[medicamento] = "Enalapril"
                it[dosis] = "10mg"
                it[frecuencia] = "24h"
                it[observaciones] = "Control de presión arterial"
            }
            Medicacion.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-25T08:00:00")
                it[medicamento] = "Metformina"
                it[dosis] = "850mg"
                it[frecuencia] = "12h"
                it[observaciones] = "Diabetes tipo 2"
            }


            Alergias.insert {
                it[pacienteId] = paciente1
                it[alergia] = "Penicilina"
                it[descripcion] = "Reacción cutánea con erupciones al tomar antibióticos del grupo penicilínico"
            }


            Diuresis.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.now().minusHours(5)
                it[cantidad] = 700f
                it[observaciones] = "Color normal"
            }
            Diuresis.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.now().minusHours(10)
                it[cantidad] = 600f
                it[observaciones] = "Color claro"
            }


            BalanceHidrico.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-27T09:00:00")
                it[ingreso] = 1200f
                it[egreso] = 1000f
                it[observaciones] = "Balance moderadamente positivo"
            }
            BalanceHidrico.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.parse("2025-05-26T09:00:00")
                it[ingreso] = 950f
                it[egreso] = 1050f
                it[observaciones] = "Leve deshidratación"
            }


            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.now().minusDays(1)
                it[descripcion] = "Cambio de apósito en pierna derecha"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.now().minusDays(2)
                it[descripcion] = "Control de signos vitales"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.now().minusDays(3)
                it[descripcion] = "Hidratación oral supervisada"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente1
                it[fechaHora] = LocalDateTime.now().minusDays(4)
                it[descripcion] = "Movilización pasiva en cama"
            }

            val paciente2 = Pacientes.insertAndGetId {
                it[Nombre] = "Raúl Alvarez"
                it[Habitacion] = "A102"
                it[Unidad] = "UCI"
                it[Edad] = 70
                it[Peso] = 60.0f
                it[Altura] = 1.60f
            }.value

            Constantes.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-28T10:00:00")
                it[temperatura] = 36.6f
                it[frecuenciaCardiaca] = 88
                it[frecuenciaRespiratoria] = 19
                it[saturacionOxigeno] = 98
                it[presionArterial] = "122/82"
            }
            Constantes.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-27T10:00:00")
                it[temperatura] = 37.2f
                it[frecuenciaCardiaca] = 91
                it[frecuenciaRespiratoria] = 21
                it[saturacionOxigeno] = 95
                it[presionArterial] = "128/86"
            }
            Constantes.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-26T10:00:00")
                it[temperatura] = 36.8f
                it[frecuenciaCardiaca] = 83
                it[frecuenciaRespiratoria] = 17
                it[saturacionOxigeno] = 97
                it[presionArterial] = "118/79"
            }

            BalanceHidrico.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-27T12:30:00")
                it[ingreso] = 1100f
                it[egreso] = 900f
                it[observaciones] = "Control estable"
            }
            BalanceHidrico.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-26T11:00:00")
                it[ingreso] = 1300f
                it[egreso] = 1300f
                it[observaciones] = "Balance hídrico neutro"
            }



            Medicacion.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-27T09:30:00")
                it[medicamento] = "Ibuprofeno"
                it[dosis] = "400mg"
                it[frecuencia] = "6h"
                it[observaciones] = "Inflamación postoperatoria"
            }
            Medicacion.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-26T13:00:00")
                it[medicamento] = "Omeprazol"
                it[dosis] = "20mg"
                it[frecuencia] = "24h"
                it[observaciones] = "Protección gástrica"
            }
            Medicacion.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.parse("2025-05-25T21:00:00")
                it[medicamento] = "Amoxicilina"
                it[dosis] = "500mg"
                it[frecuencia] = "8h"
                it[observaciones] = "Infección dental"
            }


            Alergias.insert {
                it[pacienteId] = paciente2
                it[alergia] = "Lácteos"
                it[descripcion] = "Intolerancia con síntomas gastrointestinales leves al consumir leche o derivados"
            }


            Diuresis.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.now().minusHours(6)
                it[cantidad] = 800f
                it[observaciones] = "Color ligeramente oscuro"
            }
            Diuresis.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.now().minusHours(12)
                it[cantidad] = 500f
                it[observaciones] = "Olor fuerte"
            }


            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.now().minusDays(1)
                it[descripcion] = "Control de glucemia antes del desayuno"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.now().minusDays(2)
                it[descripcion] = "Aplicación de insulina según pauta"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.now().minusDays(3)
                it[descripcion] = "Curación de úlcera por presión"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente2
                it[fechaHora] = LocalDateTime.now().minusDays(4)
                it[descripcion] = "Asistencia en higiene matutina"
            }

            val paciente3 = Pacientes.insertAndGetId {
                it[Nombre] = "Raúl Alvarez"
                it[Habitacion] = "A102"
                it[Unidad] = "UCI"
                it[Edad] = 70
                it[Peso] = 60.0f
                it[Altura] = 1.60f
            }.value

            Constantes.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-08T10:00:00")
                it[temperatura] = 38.1f
                it[frecuenciaCardiaca] = 105
                it[frecuenciaRespiratoria] = 24
                it[saturacionOxigeno] = 93
                it[presionArterial] = "132/88"
            }
            Constantes.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-07T10:00:00")
                it[temperatura] = 37.6f
                it[frecuenciaCardiaca] = 97
                it[frecuenciaRespiratoria] = 22
                it[saturacionOxigeno] = 94
                it[presionArterial] = "130/85"
            }
            Constantes.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-06T10:00:00")
                it[temperatura] = 36.4f
                it[frecuenciaCardiaca] = 75
                it[frecuenciaRespiratoria] = 18
                it[saturacionOxigeno] = 96
                it[presionArterial] = "119/76"
            }

            BalanceHidrico.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-27T07:45:00")
                it[ingreso] = 1500f
                it[egreso] = 1400f
                it[observaciones] = "Leve sobrecarga de líquidos"
            }
            BalanceHidrico.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-26T08:15:00")
                it[ingreso] = 1000f
                it[egreso] = 1100f
                it[observaciones] = "Pérdida hídrica controlada"
            }




            Medicacion.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-27T07:00:00")
                it[medicamento] = "Furosemida"
                it[dosis] = "40mg"
                it[frecuencia] = "12h"
                it[observaciones] = "Control de edema"
            }
            Medicacion.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-26T18:30:00")
                it[medicamento] = "Losartán"
                it[dosis] = "50mg"
                it[frecuencia] = "24h"
                it[observaciones] = "Hipertensión"
            }
            Medicacion.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.parse("2025-05-25T10:30:00")
                it[medicamento] = "Insulina"
                it[dosis] = "10U"
                it[frecuencia] = "Antes del desayuno"
                it[observaciones] = "Glucosa elevada en ayunas"
            }


            Alergias.insert {
                it[pacienteId] = paciente3
                it[alergia] = "Polen"
                it[descripcion] = "Estornudos frecuentes y picazón en los ojos durante primavera"
            }


            Diuresis.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.now().minusHours(4)
                it[cantidad] = 900f
                it[observaciones] = "Muy clara, posible sobrehidratación"
            }
            Diuresis.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.now().minusHours(8)
                it[cantidad] = 450f
                it[observaciones] = "Con espuma"
            }



            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.now().minusDays(1)
                it[descripcion] = "Ejercicios respiratorios asistidos"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.now().minusDays(2)
                it[descripcion] = "Cambio de vía periférica"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.now().minusDays(3)
                it[descripcion] = "Administración de oxígeno por mascarilla"
            }
            CuidadosDeEnfermeria.insert {
                it[pacienteId] = paciente3
                it[fechaHora] = LocalDateTime.now().minusDays(4)
                it[descripcion] = "Control de eliminación intestinal"
            }


        }
    }
}


