using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionEscenas : MonoBehaviour
{
    public void ReintentarTodo() {
        Time.timeScale = 1f;
        ControladorVidas.vidasGlobales = 4;
        JuegoGeneral.minijuegosCompletados = 0;

        // --- CAMBIO: Automáticamente detecta en qué nivel estás y lo reinicia ---
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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