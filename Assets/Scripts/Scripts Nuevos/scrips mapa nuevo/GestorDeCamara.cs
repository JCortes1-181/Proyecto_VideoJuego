using UnityEngine;
using Unity.Cinemachine;
using System.Collections; 

public class GestorDeCamara : MonoBehaviour
{
    [Header("Configuración de Cámaras")]
    public CinemachineCamera[] misCamarasVirtuales; 

    [Header("Referencias de la UI de Navegación")]
    public GameObject botonFlechaArriba;   
    public GameObject botonFlechaAbajo;    

    [Header("Sistema de Bloqueo (La X)")]
    [Tooltip("Arrastra aquí la imagen de la X que creaste en el Canvas")]
    public GameObject imagenEquis; 
    public float tiempoXVisible = 0.8f; 

    private int zonaActual = 0; 
    private bool mostrandoError = false; 

    private void Awake()
    {
        zonaActual = 0;
        ActualizarCamarasYZonas();
        
  
        if(imagenEquis != null) imagenEquis.SetActive(false);
    }

    public void SubirPiso()
    {

        bool puedeSubir = false;

        if (zonaActual == 0) 
        {
            puedeSubir = GestorDeProgreso.Instancia.nivel1Completado;
        }
        else if (zonaActual == 1) 
        {
            puedeSubir = GestorDeProgreso.Instancia.nivel2Completado;
        }


        if (puedeSubir && zonaActual < misCamarasVirtuales.Length - 1)
        {
            zonaActual++;
            ActualizarCamarasYZonas();
        }

        else if (!puedeSubir)
        {
            if (!mostrandoError)
            {
                StartCoroutine(AnimarEquis());
            }
        }
    }

    public void BajarPiso()
    {

        if (zonaActual > 0)
        {
            zonaActual--;
            ActualizarCamarasYZonas();
        }
    }

    private void ActualizarCamarasYZonas()
    {
        for (int i = 0; i < misCamarasVirtuales.Length; i++)
        {
            if (misCamarasVirtuales[i] != null)
            {
                misCamarasVirtuales[i].Priority = (i == zonaActual) ? 20 : 0;
            }
        }

        // Visibilidad de flechas (ahora siempre se ven a menos que estemos en el tope arriba o abajo)
        if (zonaActual == 0) 
        {
            if(botonFlechaArriba != null) botonFlechaArriba.SetActive(true);
            if(botonFlechaAbajo != null) botonFlechaAbajo.SetActive(false);
        }
        else if (zonaActual == misCamarasVirtuales.Length - 1) 
        {
            if(botonFlechaArriba != null) botonFlechaArriba.SetActive(false);
            if(botonFlechaAbajo != null) botonFlechaAbajo.SetActive(true);
        }
        else 
        {
            if(botonFlechaArriba != null) botonFlechaArriba.SetActive(true);
            if(botonFlechaAbajo != null) botonFlechaAbajo.SetActive(true);
        }
    }

    // --- ANIMACIÓN DE LA X ---
    private IEnumerator AnimarEquis()
    {
        if (imagenEquis == null) yield break;

        mostrandoError = true;
        imagenEquis.SetActive(true);

        // Efecto jugoso: La X "salta" creciendo un poco y vuelve a su tamaño normal
        Vector3 escalaOriginal = imagenEquis.transform.localScale;
        imagenEquis.transform.localScale = escalaOriginal * 1.3f; 
        
        float timer = 0;
        while(timer < 0.15f)
        {
            timer += Time.deltaTime;
            imagenEquis.transform.localScale = Vector3.Lerp(escalaOriginal * 1.3f, escalaOriginal, timer / 0.15f);
            yield return null;
        }

        // Esperamos a que pase el tiempo configurado
        yield return new WaitForSeconds(tiempoXVisible - 0.15f);

        imagenEquis.SetActive(false);
        mostrandoError = false;
    }
}
