using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorChat : MonoBehaviour
{
    [Header("Componentes de la UI")]
    public TextMeshProUGUI cuadroTextoSpam;

    [Header("Configuración del Minijuego")]
    public int letrasNecesarias = 40;
    // --- NUEVA LÍNEA: Tiempo límite ---
    public float tiempoLimite = 10f; 
    private float cronometro = 0f;
    // ----------------------------------

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
        // Inicializamos el cronómetro
        cronometro = tiempoLimite;
    }

    void Update()
    {
        if (juegoTerminado) return;

        // --- LÓGICA DEL TEMPORIZADOR ---
        cronometro -= Time.deltaTime;
        if (cronometro <= 0)
        {
            TerminarJuego(false); // Pierde por tiempo
            return;
        }
        // -------------------------------

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
            SceneManager.LoadScene("FreddyFazbear"); // Solo vuelve
        }
        else
        {
            Debug.Log("¡Derrota! Avisando al script de efectos para la música triste.");
            if (scriptEfectos != null)
            {
                scriptEfectos.ActivarDerrota();
            }
            
            // Si pierde: Resta vida y vuelve a la oficina
            ControladorVidas.vidasGlobales--; 
            SceneManager.LoadScene("FreddyFazbear");
        }
    }
}