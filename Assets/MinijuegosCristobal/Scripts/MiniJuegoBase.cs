using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public abstract class MinijuegoBase : MonoBehaviour
{
    [Header("Configuración General")]
    public string nombreMinijuego;
    public float tiempoLimite = 5f;
    
    protected float cronometro;
    protected bool juegoTerminado = false;

    protected virtual void Start()
    {
        cronometro = tiempoLimite;
    }

    protected virtual void Update()
    {
        if (juegoTerminado) return;

        cronometro -= Time.deltaTime;
        if (cronometro <= 0)
        {
            TerminarJuego(false);
        }
    }

    public abstract void TerminarJuego(bool victoria);

    // --- CORRUTINA CORREGIDA (SIN EL INCREMENTO DOBLE) --
    protected IEnumerator EsperarYRegresar(bool victoria)
    {
        juegoTerminado = true;
        yield return new WaitForSeconds(2f); 

        if (!victoria)
        {
            // DERROTA: Restamos una de tus vidas globales
            ControladorVidas.vidasGlobales--;
            Debug.Log("Derrota: Se resta una vida global. Vidas restantes: " + ControladorVidas.vidasGlobales);
        }
        else
        {
            // VICTORIA: No sumamos nada aquí, ya que JuegoGeneral lo maneja al lanzar el juego.
            Debug.Log("¡Victoria! Regresando de forma segura.");
        }

        // --- LO NUEVO AGREGADO AQUÍ ---
        // Buscamos qué nivel guardó el juego en la memoria. Si no hay ninguno, usa "Nivel2" por seguridad.
        string escenaDestino = PlayerPrefs.GetString("EscenaRetorno", "Nivel2");
        Debug.Log("Cargando escena de regreso de forma dinámica: " + escenaDestino);
        SceneManager.LoadScene(escenaDestino);
    }
}