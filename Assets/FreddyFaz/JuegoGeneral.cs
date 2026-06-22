using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JuegoGeneral : MonoBehaviour
{
    [Header("Efectos de Transición")]
    public GameObject Estatica_Efecto;
    public GameObject ContenedorCorazones; 

    [Header("UI de Progreso")]
    public TextMeshProUGUI textoContadorMinijuegos; 
    
    [Header("Ajustes de Juego")]
    public static int minijuegosCompletados = 0; 
    public int totalMinijuegosParaGanar = 6;

    [Header("Paneles de Fin de Juego")]
    public GameObject panelVictoria;
    public GameObject panelGameOver;

    // --- ESTO ES LO NUEVO: Variables para saber a dónde volver ---
    [Header("Conexión con el Mapa Principal")]
    [Tooltip("¿Qué nivel es este macro-juego? (1, 2 o 3)")]
    public int numeroDeNivelActual = 1;
    public string nombreEscenaMapa = "NuevoMenu";
    // -------------------------------------------------------------

    private static List<string> bolsaMinijuegos = new List<string>();
    private string[] listaMaestra = { "Minijuego_Chat", "Minijuego_recolectar", "MinijuegoSC", "minijuego_espacio", "MinijuegoMatar", "MinijuegoSkate" }; 

    void Start() {
        StopAllCoroutines();

        if (bolsaMinijuegos == null || bolsaMinijuegos.Count == 0) {
            bolsaMinijuegos = new List<string>();
            bolsaMinijuegos.AddRange(listaMaestra);
        }

        // Encendemos la UI al empezar
        if(ContenedorCorazones != null) ContenedorCorazones.SetActive(true);
        if(textoContadorMinijuegos != null) {
            textoContadorMinijuegos.gameObject.SetActive(true);
            ActualizarTextoProgreso();
        }

        ControladorVidas gestor = Object.FindFirstObjectByType<ControladorVidas>();
        if (gestor != null) gestor.ActualizarVisualVidas();

        if (minijuegosCompletados >= totalMinijuegosParaGanar) {
            Ganaste();
        } else {
            Invoke("DecidirSiguienteReto", 2.5f);
        }
    }

    // --- NUEVA FUNCIÓN PARA LIMPIAR LA PANTALLA ---
    void OcultarUIInGame() {
        if (textoContadorMinijuegos != null) textoContadorMinijuegos.gameObject.SetActive(false);
        if (ContenedorCorazones != null) ContenedorCorazones.SetActive(false);
    }

    void DecidirSiguienteReto() {
        if (this == null || ControladorVidas.vidasGlobales <= 0) {
            // Si el jugador perdió, ocultamos el contador antes de mostrar el GameOver
            OcultarUIInGame();
            return;
        }

        if (bolsaMinijuegos.Count == 0) bolsaMinijuegos.AddRange(listaMaestra);

        int indiceAleatorio = Random.Range(0, bolsaMinijuegos.Count);
        string escenaElegida = bolsaMinijuegos[indiceAleatorio];
        bolsaMinijuegos.RemoveAt(indiceAleatorio);

        minijuegosCompletados++;
        StartCoroutine(TransicionAMinijuego(escenaElegida));
    }

    IEnumerator TransicionAMinijuego(string nombreEscena) {
        if(Estatica_Efecto != null) Estatica_Efecto.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(nombreEscena);
    }

    void ActualizarTextoProgreso() {
        if (textoContadorMinijuegos != null) {
            textoContadorMinijuegos.text = "RONDA: " + (minijuegosCompletados + 1) + " / " + totalMinijuegosParaGanar;
        }
    }

    void Ganaste() {
        OcultarUIInGame(); // Apagamos contador y corazones al ganar
        if(panelVictoria != null) panelVictoria.SetActive(true);
        if(bolsaMinijuegos != null) bolsaMinijuegos.Clear(); 

        // --- ESTO ES LO NUEVO: Avisar al Cerebro que ganamos ---
        if (GestorDeProgreso.Instancia != null)
        {
            if (numeroDeNivelActual == 1) GestorDeProgreso.Instancia.SuperarNivel1();
            else if (numeroDeNivelActual == 2) GestorDeProgreso.Instancia.SuperarNivel2();
            else if (numeroDeNivelActual == 3) GestorDeProgreso.Instancia.SuperarHistoria();
        }
        // --------------------------------------------------------
    }

    // --- ESTO ES LO NUEVO: Función para el botón del panel de victoria ---
    public void VolverAlMapa()
    {
        // Reseteamos las variables para cuando el jugador vuelva a entrar a otro nivel
        minijuegosCompletados = 0;
        ControladorVidas.vidasGlobales = 4; // Resetea las vidas a su valor inicial
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(nombreEscenaMapa);
    }
    // ---------------------------------------------------------------------
}