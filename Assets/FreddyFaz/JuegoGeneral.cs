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
    public TextMeshProUGUI textoContadorMinijuegos; // El nuevo texto para "1/10"
    
    [Header("Ajustes de Juego")]
    public static int minijuegosCompletados = 0; // Variable que persiste entre escenas
    public int totalMinijuegosParaGanar = 10;

    void Start() {
        if(ContenedorCorazones) ContenedorCorazones.SetActive(true);
        
        // Actualizamos el texto de progreso (ej: "Minijuegos: 1 / 10")
        ActualizarTextoProgreso();

        // Refrescamos las vidas visuales
        ControladorVidas gestor = Object.FindFirstObjectByType<ControladorVidas>();
        if (gestor != null) gestor.ActualizarVisualVidas();

        // Verificamos si ya ganamos el juego completo
        if (minijuegosCompletados >= totalMinijuegosParaGanar) {
            GanarJuegoCompleto();
            return;
        }

        Invoke("DecidirSiguienteReto", 3f);
    }

    void ActualizarTextoProgreso() {
        if (textoContadorMinijuegos != null) {
            // Sumamos 1 para que el jugador vea "1/10" en lugar de "0/10" al empezar
            textoContadorMinijuegos.text = "Ronda: " + (minijuegosCompletados + 1) + " / " + totalMinijuegosParaGanar;
        }
    }

    void DecidirSiguienteReto() {
        if (ControladorVidas.vidasGlobales <= 0) return;

        // Aumentamos el contador: cada vez que entramos a la oficina tras un reto, cuenta como uno más
        // (Nota: minijuegosCompletados se aumenta antes de lanzar el siguiente)

        string[] minijuegos = { "Minijuego_Chat", "Minijuego_recolectar", "MinijuegoSC" };
        int indiceAleatorio = Random.Range(0, minijuegos.Length);
        string escenaElegida = minijuegos[indiceAleatorio];

        // Aumentamos el contador global antes de ir al minijuego
        minijuegosCompletados++;

        StartCoroutine(TransicionAMinijuego(escenaElegida));
    }

    IEnumerator TransicionAMinijuego(string nombreEscena) {
        if(Estatica_Efecto) Estatica_Efecto.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(nombreEscena);
    }

    void GanarJuegoCompleto() {
        // Reiniciamos el contador para la próxima vez que el usuario juegue
        minijuegosCompletados = 0;
        // Te devuelve a la SampleScene (Menú o inicio)
        SceneManager.LoadScene("SampleScene"); 
    }
}