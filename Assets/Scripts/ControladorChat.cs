using System.Collections;
using UnityEngine;
using TMPro;

public class ControladorChat : MonoBehaviour
{
    [Header("Componentes de la UI")]
    public TextMeshProUGUI cuadroTextoSpam;

    [Header("Configuración del Minijuego")]
    public int letrasNecesarias = 40;

    [Header("Conexión con Efectos Finales")]
    public EfectosFinalChat scriptEfectos;

    // --- NUEVA LÍNEA: Casilla para conectar la cámara ---
    [Header("Conexión con la Cámara")]
    public EfectoVibracion scriptVibracion; 
    // ----------------------------------------------------

    private string abecedarioCaotico = "ASDFGHJKLZXCVBNMQWERTYUIOPXNFFJDLSK";
    private int letrasActuales = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        if (cuadroTextoSpam != null)
        {
            cuadroTextoSpam.text = "";
        }
    }

    void Update()
    {
        if (juegoTerminado) return;

        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            SimularEscritura();
        }
    }

    void SimularEscritura()
    {
        char letraRandom = abecedarioCaotico[Random.Range(0, abecedarioCaotico.Length)];
        cuadroTextoSpam.text += letraRandom;
        letrasActuales++;

        // --- NUEVA LÍNEA: Hace vibrar la cámara con cada letra ---
        if (scriptVibracion != null)
        {
            // Duración: 0.05 segundos, Intensidad: 0.1 (puedes cambiarlo en el inspector)
            StartCoroutine(scriptVibracion.Shake(0.05f, 15.0f)); 
        }
        // --------------------------------------------------------

        if (letrasActuales >= letrasNecesarias)
        {
            TerminarJuego(true);
        }
    }

    public void TerminarJuego(bool victoria)
    {
        juegoTerminado = true;

        if (victoria)
        {
            Debug.Log("¡Victoria! Avisando al script de efectos para el BOOM.");
            if (scriptEfectos != null)
            {
                scriptEfectos.ActivarVictoria();
            }
        }
        else
        {
            Debug.Log("¡Derrota! Avisando al script de efectos para la música triste.");
            if (scriptEfectos != null)
            {
                scriptEfectos.ActivarDerrota();
            }
        }
    }
}