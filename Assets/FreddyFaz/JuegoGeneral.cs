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
    public int totalMinijuegosParaGanar = 6; // Ajustado a 6 según tu inspector

    [Header("Paneles de Fin de Juego")]
    public GameObject panelVictoria;
    public GameObject panelGameOver;

    void Start() {
        // Aseguramos que la UI principal esté visible al empezar
        if(ContenedorCorazones) ContenedorCorazones.SetActive(true);
        if(textoContadorMinijuegos) textoContadorMinijuegos.gameObject.SetActive(true);
        
        ActualizarTextoProgreso();

        // Refrescamos las vidas visuales al entrar a la oficina
        ControladorVidas gestor = Object.FindFirstObjectByType<ControladorVidas>();
        if (gestor != null) gestor.ActualizarVisualVidas();

        // Verificamos si ya se alcanzó la meta
        if (minijuegosCompletados >= totalMinijuegosParaGanar) {
            GanarJuegoCompleto();
            return;
        }

        // Si no hemos ganado ni perdido, esperamos para el siguiente minijuego
        if (ControladorVidas.vidasGlobales > 0) {
            Invoke("DecidirSiguienteReto", 3f);
        }
    }

    void Update() {
        // Verificación constante por si las vidas llegan a 0
        if (ControladorVidas.vidasGlobales <= 0) {
            OcultarUIProgreso();
        }
    }

    void ActualizarTextoProgreso() {
        if (textoContadorMinijuegos != null) {
            textoContadorMinijuegos.text = "RONDA: " + (minijuegosCompletados + 1) + " / " + totalMinijuegosParaGanar;
        }
    }

    // Función nueva para limpiar la pantalla cuando termina el juego
    void OcultarUIProgreso() {
        if (textoContadorMinijuegos != null) {
            textoContadorMinijuegos.gameObject.SetActive(false);
        }
    }

    void DecidirSiguienteReto() {
        if (ControladorVidas.vidasGlobales <= 0) return;

        string[] minijuegos = { "Minijuego_Chat", "Minijuego_recolectar", "MinijuegoSC" };
        int indiceAleatorio = Random.Range(0, minijuegos.Length);
        string escenaElegida = minijuegos[indiceAleatorio];

        minijuegosCompletados++;

        StartCoroutine(TransicionAMinijuego(escenaElegida));
    }

    IEnumerator TransicionAMinijuego(string nombreEscena) {
        if(Estatica_Efecto) Estatica_Efecto.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(nombreEscena);
    }

    void GanarJuegoCompleto() {
        OcultarUIProgreso();
        if (panelVictoria != null) {
            panelVictoria.SetActive(true);
        } else {
            // Si no hay panel, vuelve al inicio
            minijuegosCompletados = 0;
            SceneManager.LoadScene("SampleScene"); 
        }
    }
}