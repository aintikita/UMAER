plugins {
    kotlin("jvm") version "1.9.10"
    application
    kotlin("plugin.serialization") version "1.9.10"
    kotlin("kapt") version "1.9.0"
}

group = "org.appmedica"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    // Ktor Core
    implementation("io.ktor:ktor-server-core-jvm:2.3.3")
    implementation("io.ktor:ktor-server-netty-jvm:2.3.3")
    implementation("io.ktor:ktor-server-content-negotiation-jvm:2.3.3")
    implementation("io.ktor:ktor-serialization-gson-jvm:2.3.3") // o kotlinx si prefieres

    // Ktor Utilities
    implementation("io.ktor:ktor-server-call-logging-jvm:2.3.3")

    // Exposed ORM + SQLite
    implementation("org.jetbrains.exposed:exposed-core:0.41.1")
    implementation("org.jetbrains.exposed:exposed-dao:0.41.1")
    implementation("org.jetbrains.exposed:exposed-jdbc:0.41.1")
    implementation("org.xerial:sqlite-jdbc:3.41.2.1")

    // OpenAI (con OkHttp)
    implementation("com.squareup.okhttp3:okhttp:4.12.0")
    implementation("com.squareup.okhttp3:logging-interceptor:4.12.0")

    // PDF Generation
    implementation("org.apache.pdfbox:pdfbox:2.0.27")

    // Excel / Word (Apache POI)
    implementation("org.apache.poi:poi:5.2.3")
    implementation("org.apache.poi:poi-ooxml:5.2.3")

    // Logging
    implementation("org.slf4j:slf4j-simple:1.7.36")

    // Word a Pdf
    implementation("org.docx4j:docx4j-JAXB-MOXy:11.4.9")
    implementation("org.docx4j:docx4j-export-fo:11.4.9")

    implementation("org.apache.pdfbox:pdfbox:2.0.27")

    implementation("io.ktor:ktor-server-core:2.3.3")
    implementation("io.ktor:ktor-server-netty:2.3.3")
    implementation("io.ktor:ktor-server-content-negotiation:2.3.3")
    implementation("io.ktor:ktor-serialization-gson:2.3.3")
    implementation("org.apache.pdfbox:pdfbox:2.0.27")
    implementation("ch.qos.logback:logback-classic:1.4.5")

    implementation("org.jetbrains.kotlinx:kotlinx-serialization-json:1.6.0")

    implementation("io.ktor:ktor-server-netty:2.3.0")
    implementation("io.ktor:ktor-server-core:2.3.0")
    implementation("io.ktor:ktor-server-content-negotiation:2.3.0")
    implementation("io.ktor:ktor-serialization-kotlinx-json:2.3.0")
    implementation("org.jetbrains.exposed:exposed-core:0.41.1")
    implementation("org.jetbrains.exposed:exposed-dao:0.41.1")
    implementation("org.jetbrains.exposed:exposed-jdbc:0.41.1")
    implementation("com.zaxxer:HikariCP:5.0.1")
    implementation("org.postgresql:postgresql:42.6.0") // O el driver que uses
    implementation("ch.qos.logback:logback-classic:1.2.11")

    implementation("org.jetbrains.exposed:exposed-java-time:0.41.1")

    // Para gráficos
    implementation("org.knowm.xchart:xchart:3.8.4")

    // Para trabajar con PDFs
    implementation("org.apache.pdfbox:pdfbox:2.0.27")

    implementation("io.ktor:ktor-server-netty:2.3.4")
    implementation("io.ktor:ktor-server-core:2.3.4")
    implementation("io.ktor:ktor-server-cors:2.3.4")
    implementation("io.ktor:ktor-server-content-negotiation:2.3.4")
    implementation("io.ktor:ktor-serialization-kotlinx-json:2.3.4")
    implementation("org.apache.pdfbox:pdfbox:2.0.27")

    implementation("io.ktor:ktor-server-cors:2.3.4")

    // Cliente HTTP de Ktor
    implementation("io.ktor:ktor-client-core:2.3.4")
    implementation("io.ktor:ktor-client-cio:2.3.4")
    implementation("io.ktor:ktor-client-content-negotiation:2.3.4")

// JSON (si usas kotlinx.serialization)
    implementation("org.jetbrains.kotlinx:kotlinx-serialization-json:1.5.1")

// Solo si usas funciones como Json.encodeToString
    implementation("org.jetbrains.kotlinx:kotlinx-serialization-core:1.5.1")


}

tasks.test {
    useJUnitPlatform()
}
kotlin {
    jvmToolchain(17)
}