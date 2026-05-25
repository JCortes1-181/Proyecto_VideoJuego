using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorCueva : MonoBehaviour
{
    public GameObject gifGanaste;
    public GameObject gifPerdiste;
    private bool juegoTerminado = false;

    public enemyController scriptEnemigo; 

    // --- NUEVO: SISTEMA DE TIEMPO ---
    public float tiempoRestante = 5f; 

    void Update()
    {
        if (juegoTerminado) return;

        // 1. Condición de Victoria: Enemigo muere
        if (scriptEnemigo != null && scriptEnemigo.estaMuerto) 
        {
             Finalizar(true);
        }

        // 2. Condición de Derrota: El tiempo se agota
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0)
        {
            Finalizar(false);
        }
    }

    public void Finalizar(bool ganado)
    {
        if(juegoTerminado) return; 
        
        juegoTerminado = true;
        StartCoroutine(VolverAOficina(ganado));
    }

    IEnumerator VolverAOficina(bool ganado)
    {
        if (ganado)
        {
            if (gifGanaste) gifGanaste.SetActive(true);
        }
        else
        {
            if (gifPerdiste) gifPerdiste.SetActive(true);
            ControladorVidas.vidasGlobales--; 
        }

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("FreddyFazbear");
    }
}