package org.appmedica.services

import org.appmedica.utils.PdfUtils
import java.io.File

class PdfService {
    private val basePath = File("archivos")



    fun obtenerTextoPdf(subcarpeta: String, nombre: String): String {
        val archivo = File(basePath, "$subcarpeta/$nombre")
        if (!archivo.exists() || !archivo.name.endsWith(".pdf"))
            return "PDF no encontrado o inválido"

        return PdfUtils.extraerTexto(archivo)
    }

    fun listarPdfs(subcarpeta: String): List<String> {
        val carpeta = File(basePath, subcarpeta)
        if (!carpeta.exists() || !carpeta.isDirectory) return emptyList()

        return carpeta.listFiles { f -> f.extension == "pdf" }?.map { it.name } ?: emptyList()
    }
}