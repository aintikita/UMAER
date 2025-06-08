package utils

import org.apache.pdfbox.pdmodel.PDDocument
import org.apache.pdfbox.text.PDFTextStripper
import java.io.File

object DocumentoLoader {
    fun cargarTextoDePdf(ruta: String): String {
        val documento = PDDocument.load(File(ruta))
        val texto = PDFTextStripper().getText(documento)
        documento.close()
        return texto
    }

    fun cargarTodos(baseFolder: String): Map<String, String> {
        val base = "archivos/$baseFolder"
        val archivos = File(base).walk().filter { it.extension.equals("pdf", ignoreCase = true) }.toList()
        return archivos.associate { it.nameWithoutExtension to cargarTextoDePdf(it.absolutePath) }
    }


}

