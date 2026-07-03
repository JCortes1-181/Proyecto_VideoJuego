using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorSpaceDificil : MonoBehaviour
{
    public GameObject gifGanaste;
    public GameObject gifPerdiste;
    private bool juegoTerminado = false;

    [Header("Configuración Modo Difícil")]
    public int columnasPorOleada = 1;
    public int filasPorColumna = 8;
    public int oleadasParaGanar = 3;

    private int oleadaActual = 0;
    private int enemigosRestantes = 0;

    [Header("Referencias")]
    public GeneradorEnemigosDificil generador;

    void Start()
    {
        oleadaActual = 0; // Aseguramos que empiece en 0
        SiguienteOleada();
    }

    public void SiguienteOleada()
    {
        oleadaActual++;
        
        // Si ya pasamos el número de oleadas, ganamos
        if (oleadaActual > oleadasParaGanar)
        {
            Finalizar(true);
        }
        else
        {
            Debug.Log("Iniciando oleada: " + oleadaActual);
            if (generador != null)
            {
                generador.GenerarOleada(filasPorColumna, columnasPorOleada, this);
            }
        }
    }

    public void RegistrarEnemigos(int cantidad)
    {
        // Fuerza el valor al iniciar la oleada
        enemigosRestantes = cantidad;
        Debug.Log("Enemigos en esta oleada: " + enemigosRestantes);
    }

    public void EnemigoDestruido()
    {
        if (juegoTerminado) return;

        enemigosRestantes--;
        
        // Solo pasamos de oleada si ya no quedan enemigos
        if (enemigosRestantes <= 0)
        {
            SiguienteOleada();
        }
    }

    // ... (Mantén tus funciones JugadorTocado, EnemigosLlegaronAbajo y Finalizar igual)
    
    public void JugadorTocado() { if (!juegoTerminado) Finalizar(false); }
    public void EnemigosLlegaronAbajo() { if (!juegoTerminado) Finalizar(false); }

    public void Finalizar(bool ganado)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;
        StartCoroutine(VolverAOficina(ganado));
    }

    IEnumerator VolverAOficina(bool ganado)
    {
        if (ganado) { if (gifGanaste) gifGanaste.SetActive(true); }
        else { if (gifPerdiste) gifPerdiste.SetActive(true); ControladorVidas.vidasGlobales--; }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("FreddyFazbear");
    }
}