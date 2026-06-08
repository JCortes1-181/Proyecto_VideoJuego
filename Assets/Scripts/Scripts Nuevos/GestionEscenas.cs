using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionEscenas : MonoBehaviour
{
    // Función única de Reintentar para todos los casos (Victoria y Derrota)
    public void ReintentarTodo() {
        // 1. Aseguramos que el tiempo corra (por si acaso)
        Time.timeScale = 1f;
        
        // 2. Reseteamos variables globales
        ControladorVidas.vidasGlobales = 4;
        JuegoGeneral.minijuegosCompletados = 0;

        // 3. Cargamos la escena (Asegúrate que se llame exactamente así)
        SceneManager.LoadScene("FreddyFazbear");
    }

    public void RendirseEnDerrota() {
        Time.timeScale = 1f;
        EstadoMundo.estadoActual = EstadoMundo.EstadoNpc.VolvioDerrotado;
        ControladorVidas.vidasGlobales = 4;
        JuegoGeneral.minijuegosCompletados = 0;
        SceneManager.LoadScene("NuevoMenu"); 
    }

    public void ContinuarVictoria() {
        Time.timeScale = 1f;
        EstadoMundo.estadoActual = EstadoMundo.EstadoNpc.VolvioVictorioso;
        ControladorVidas.vidasGlobales = 4;
        JuegoGeneral.minijuegosCompletados = 0;
        SceneManager.LoadScene("NuevoMenu"); 
    }
}
