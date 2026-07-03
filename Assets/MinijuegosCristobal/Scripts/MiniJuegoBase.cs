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

    // --- CORRUTINA CORREGIDA (SIN EL INCREMENTO DOBLE) ---
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

        // Cargamos la escena central de la oficin
        Debug.Log("Cargando escena de la oficina de Freddy...");
        SceneManager.LoadScene("Nivel2");
    }
}