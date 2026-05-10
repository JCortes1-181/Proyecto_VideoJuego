using UnityEngine;
using UnityEngine.SceneManagement;

public class Condiciones : MonoBehaviour
{
    public float tiempoRestante = 15f;
    public GameObject panelDerrota; // La imagen de pelea/explosión
    public AudioSource sonidoMuerte;
    private bool juegoTerminado = false;

    void Update() {
        if (juegoTerminado) return;

        // Cronómetro
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0) {
            Ganar();
        }
    }

    public void Perder() {
        juegoTerminado = true;
        panelDerrota.SetActive(true);
        if (sonidoMuerte) sonidoMuerte.Play();
        Time.timeScale = 0; // Detiene el juego
    }

    void Ganar() {
        juegoTerminado = true;
        Debug.Log("¡Ganaste! Volviendo a la casa...");
        Time.timeScale = 1; // Asegura que el tiempo corra
        SceneManager.LoadScene("EscenaCasa"); // Cambia al nombre de tu escena
    }
}
