using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorSpace : MonoBehaviour
{
    public GameObject gifGanaste;
    public GameObject gifPerdiste;
    private bool juegoTerminado = false;

    [Header("Configuración")]
    public int filas =3;
    public int columnas = 10;

    public int enemigosTotales; 
    private int enemigosDestruidos = 0;

    void Start()
    {
        enemigosTotales = filas * columnas;
    }

    public void EnemigoDestruido()
    {
        enemigosDestruidos++;
        
        if (enemigosDestruidos >= enemigosTotales && !juegoTerminado)
        {
            Finalizar(true);
        }
    }

    public void JugadorTocado()
    {
        if(!juegoTerminado)
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

        yield return new WaitForSeconds(0.5f);
        
        // --- NUEVO: Regreso Inteligente ---
        string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "FreddyFazbear");
        SceneManager.LoadScene(escenaDestino);
    }
}