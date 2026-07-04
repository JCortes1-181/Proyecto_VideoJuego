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
    public float tiempoLimite = 5f; 
    private float cronometro = 0f;

    [Header("Conexión con Efectos Finales")]
    public EfectosFinalChat scriptEfectos;

    [Header("Conexión con la Cámara")]
    public EfectoVibracion scriptVibracion; 

    private string abecedarioCaotico = "ASDFGHJKLZXCVBNMQWERTYUIOPXNFFJDLSK";
    private int letrasActuales = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        if (cuadroTextoSpam != null)
        {
            cuadroTextoSpam.text = "";
        }
        cronometro = tiempoLimite; 
    }

    void Update()
    {
        if (juegoTerminado) return;

        cronometro -= Time.deltaTime;
        if (cronometro <= 0)
        {
            TerminarJuego(false); 
            return;
        }

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

        if (scriptVibracion != null)
        {
            StartCoroutine(scriptVibracion.Shake(0.05f, 15.0f)); 
        }

        if (letrasActuales >= letrasNecesarias)
        {
            TerminarJuego(true);
        }
    }

    public void TerminarJuego(bool victoria)
    {
        juegoTerminado = true;
        StartCoroutine(EsperarYCambiarEscena(victoria));
    }

    IEnumerator EsperarYCambiarEscena(bool victoria)
    {
        if (victoria)
        {
            Debug.Log("¡Victoria! Activando cartel y sonido de baneo...");
            if (scriptEfectos != null)
            {
                scriptEfectos.ActivarVictoria();
            }

            yield return new WaitForSeconds(2.5f); 

            string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "FreddyFazbear");
            SceneManager.LoadScene(escenaDestino);
        }
        else
        {
            Debug.Log("¡Derrota! El pollo se burla de ti.");
            if (scriptEfectos != null)
            {
                scriptEfectos.ActivarDerrota();
            }

            ControladorVidas.vidasGlobales--; 

            yield return new WaitForSeconds(2.5f); 
            
            string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "FreddyFazbear");
            SceneManager.LoadScene(escenaDestino);
        }
    }
}