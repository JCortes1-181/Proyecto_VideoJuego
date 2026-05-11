using UnityEngine;

public class GestionJuego : MonoBehaviour
{
    public float tiempoParaGanar = 10f; 
    private float tiempoActual = 0f;
    private bool ganado = false;

    void Update()
    {
        if (ganado) return;

        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoParaGanar)
        {
            ganado = true;
            Debug.Log("¡Sobreviviste! Victoria.");
            
            
            FindObjectOfType<ControladorJugador>().TerminarMinijuegoSinMorir();
        }
    }
}
