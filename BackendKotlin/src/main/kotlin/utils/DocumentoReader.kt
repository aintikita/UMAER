package org.appmedica.utils

import org.appmedica.models.ArchivoDocumento
import java.io.File

object DocumentoReader {

    fun listarContenido(categoria: String, directorioRelativo: String): List<ArchivoDocumento> {
        val baseDir = File("archivos/$categoria").resolve(directorioRelativo)
        if (!baseDir.exists() || !baseDir.isDirectory) return emptyList()

        return baseDir.listFiles()
            ?.sortedWith(compareBy({ !it.isDirectory }, { it.name }))
            ?.map { file ->
                ArchivoDocumento(
                    Nombre = file.name,
                    EsCarpeta = file.isDirectory,
                    Ruta = File(categoria).resolve(directorioRelativo).resolve(file.name).path.replace("\\", "/")
                )
            } ?: emptyList()
    }

}

