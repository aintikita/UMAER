package org.appmedica.utils

import org.apache.pdfbox.pdmodel.PDDocument
import org.apache.pdfbox.text.PDFTextStripper
import java.io.File

object PDFReader {
    fun obtenerTodosLosFragmentos(rutaBase: String): List<String> {
        val base = File("archivos/$rutaBase")
        val fragmentos = mutableListOf<String>()

        if (!base.exists()) return fragmentos

        base.walk().forEach { archivo ->
            if (archivo.extension.lowercase() == "pdf") {
                val texto = extraerTextoDePDF(archivo)
                fragmentos += dividirEnFragmentos(texto)
            }
        }

        return fragmentos.filter { it.length > 40 }
    }

    private fun extraerTextoDePDF(archivo: File): String {
        return try {
            val document = PDDocument.load(archivo)
            val texto = PDFTextStripper().getText(document)
            document.close()
            texto
        } catch (e: Exception) {
            println("❌ Error leyendo PDF: ${archivo.name}: ${e.message}")
            ""
        }
    }

    private fun dividirEnFragmentos(texto: String): List<String> {
        return texto
            .split(Regex("\\n\\s*\\n|\\n•|\\n-"))
            .map { it.trim() }
            .filter { it.isNotEmpty() }
    }
}

