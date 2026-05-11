using UnityEngine;
using UnityEngine.SceneManagement;

public class Condiciones : MonoBehaviour
{
    public float tiempoRestante = 15f;
    public GameObject panelDerrota; 
    public AudioSource sonidoMuerte;
    private bool juegoTerminado = false;

    void Update() {
        if (juegoTerminado) return;

        
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0) {
            Ganar();
        }
    }

    public void Perder() {
        juegoTerminado = true;
        panelDerrota.SetActive(true);
        if (sonidoMuerte) sonidoMuerte.Play();
        Time.timeScale = 0; 
    }

    void Ganar() {
        juegoTerminado = true;
        Debug.Log("¡Ganaste! Volviendo a la casa...");
        Time.timeScale = 1; 
        SceneManager.LoadScene("EscenaCasa"); 
}
}
