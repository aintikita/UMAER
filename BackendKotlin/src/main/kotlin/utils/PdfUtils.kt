package org.appmedica.utils

import org.apache.pdfbox.pdmodel.PDDocument
import org.apache.pdfbox.text.PDFTextStripper
import java.io.File

object PdfUtils {
    fun extraerTexto(archivo: File): String {
        PDDocument.load(archivo).use { doc ->
            return PDFTextStripper().getText(doc)
        }
    }

    fun dividirTextoEnFragmentos(texto: String, maxPalabras: Int = 300): List<String> {
        val palabras = texto.split("\\s+".toRegex())
        val fragmentos = mutableListOf<String>()

        var i = 0
        while (i < palabras.size) {
            val sub = palabras.subList(i, minOf(i + maxPalabras, palabras.size))
            fragmentos.add(sub.joinToString(" "))
            i += maxPalabras
        }

        return fragmentos
    }

}