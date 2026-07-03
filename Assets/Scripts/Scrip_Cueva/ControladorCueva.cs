using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorCueva : MonoBehaviour
{
    public GameObject gifGanaste;
    public GameObject gifPerdiste;
    private bool juegoTerminado = false;

    public enemyController scriptEnemigo; 

    public float tiempoRestante = 5f; 

    void Update()
    {
        if (juegoTerminado) return;

        if (scriptEnemigo != null && scriptEnemigo.estaMuerto) 
        {
             Finalizar(true);
        }

        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0)
        {
            Finalizar(false);
        }
    }

    public void Finalizar(bool ganado)
    {
        if (juegoTerminado) return; 
        
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

        string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "FreddyFazbear");
        Debug.Log("[Sistema Retorno] Saliendo del minijuego de la Cueva hacia: " + escenaDestino);
        SceneManager.LoadScene(escenaDestino);
    }
}