package org.appmedica.services

import io.ktor.client.*
import io.ktor.client.engine.cio.*
import io.ktor.client.request.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.serialization.json.*

object OpenRouterService {
    private val apiKey = "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" // Reemplaza con tu clave personal
    private val client = HttpClient(CIO)
    private val json = Json { ignoreUnknownKeys = true }

    suspend fun obtenerRespuesta(documento: String, pregunta: String): String {
        val prompt = """
            Usa solo la siguiente información para responder.

            Documento:
            $documento

            Pregunta:
            $pregunta
        """.trimIndent()

        val response = client.post("https://openrouter.ai/api/v1/chat/completions") {
            headers {
                append(HttpHeaders.Authorization, "Bearer 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")
                append(HttpHeaders.ContentType, "application/json")
                append("HTTP-Referer", "http://localhost") // Requerido por OpenRouter
                append("X-Title", "Chat UMAER") // Opcional: título de tu proyecto
            }

            setBody(buildJsonObject {
                put("model", "meta-llama/llama-3.3-8b-instruct:free") // Puedes cambiar por otro modelo listado en OpenRouter
                putJsonArray("messages") {
                    addJsonObject {
                        put("role", "system")
                        put("content", "Eres un asistente que responde exclusivamente usando el contenido proporcionado.")
                    }
                    addJsonObject {
                        put("role", "user")
                        put("content", prompt)
                    }
                }
            }.toString())
        }


        val cuerpo = response.bodyAsText()
        println("🧠 OpenRouter Response:\n$cuerpo")

        val parsed = json.parseToJsonElement(cuerpo).jsonObject
        return parsed["choices"]
            ?.jsonArray?.getOrNull(0)
            ?.jsonObject?.get("message")
            ?.jsonObject?.get("content")
            ?.jsonPrimitive?.content
            ?: "⚠️ No se obtuvo una respuesta válida de OpenRouter."
    }

}
