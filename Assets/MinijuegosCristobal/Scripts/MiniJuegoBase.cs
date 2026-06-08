using System.Collections;
using UnityEngine;

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


    protected IEnumerator EsperarYRegresar(bool victoria)
    {
        juegoTerminado = true;
        yield return new WaitForSeconds(2f); 

        if (!victoria)
        {

            Debug.Log("Derrota: Se resta una vida global.");
        }


        Debug.Log("Cargando escena de la oficina de Freddy...");
    }
}