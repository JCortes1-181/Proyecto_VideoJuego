using UnityEngine;
using UnityEngine.UI;

public class LogicaPelea : MonoBehaviour
{
    [Header("Vidas del Jugador")]
    public GameObject[] corazones; 
    private int vidasRestantes = 3;

    [Header("Referencias de Escena")]
    public GameObject fotoMalote;      
    public GameObject fotoMarshmallow; 
    public GameObject cajaMinijuego;   

    public void IniciarPelea()
    {
        // 1. Despertamos el panel
        gameObject.SetActive(true); 

        // 2. Congelar movimiento (doble seguridad)
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null) 
        {
            var scriptMov = jugador.GetComponent<MovimientoJugador>();
            if(scriptMov != null) scriptMov.enabled = false;
            
            Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
            if(rb != null) rb.linearVelocity = Vector2.zero; 
        }

        vidasRestantes = 3;
        ActualizarVidas();

        // 3. Resetear elementos visuales para la animación
        fotoMalote.SetActive(false);
        fotoMarshmallow.SetActive(false);
        cajaMinijuego.SetActive(false);

        // 4. Secuencia cinemática
        Invoke("ApareceVillano", 0.5f);
        Invoke("ApareceJugador", 1.5f);
        Invoke("LanzarPrimerMinijuego", 3.5f);
    }

    void ApareceVillano() { fotoMalote.SetActive(true); }
    void ApareceJugador() { fotoMarshmallow.SetActive(true); }

    void LanzarPrimerMinijuego()
    {
        cajaMinijuego.SetActive(true); 
        Transform juegoTenedor = cajaMinijuego.transform.Find("Juego_Tenedor");
        if(juegoTenedor != null) juegoTenedor.gameObject.SetActive(true);
    }

    public void RecibirDanio() { FalloEnMinijuego(); }

    public void FalloEnMinijuego()
    {
        vidasRestantes--;
        ActualizarVidas();
        cajaMinijuego.SetActive(false);

        if (vidasRestantes <= 0)
        {
            Debug.Log("GAME OVER");
            FinalizarPelea(); // Te devuelve el movimiento al morir
        }
        else
        {
            Invoke("LanzarPrimerMinijuego", 2.0f);
        }
    }

    public void FinalizarPelea()
    {
        gameObject.SetActive(false);
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            var scriptMov = jugador.GetComponent<MovimientoJugador>();
            if (scriptMov != null) scriptMov.enabled = true;
        }
    }

    void ActualizarVidas()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if(corazones[i] != null) corazones[i].SetActive(i < vidasRestantes);
        }
    }
}