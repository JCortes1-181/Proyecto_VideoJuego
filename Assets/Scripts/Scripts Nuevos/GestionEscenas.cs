using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionEscenas : MonoBehaviour
{
    public void Reintentar()
    {
        // Reiniciamos las variables estáticas antes de cargar
        ControladorVidas.vidasGlobales = 4;
        JuegoGeneral.minijuegosCompletados = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}
