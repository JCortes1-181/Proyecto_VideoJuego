using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorCueva : MonoBehaviour
{
    public GameObject gifGanaste;
    public GameObject gifPerdiste;
    private bool juegoTerminado = false;

    public enemyController scriptEnemigo; 

    void Update()
    {
        
        if (!juegoTerminado && scriptEnemigo != null && scriptEnemigo.estaMuerto) 
        {
             Finalizar(true);
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