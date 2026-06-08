using UnityEngine;
using Unity.Cinemachine; // Namespace correcto para Unity 6

public class GestorDeCamara : MonoBehaviour
{
    [Header("Configuración de Cámaras")]
    [Tooltip("Arrastra aquí tus cámaras virtuales en orden (Elemento 0 = Nivel 1, Elemento 1 = Nivel 2...)")]
    public CinemachineCamera[] misCamarasVirtuales; 

    [Header("Referencias de la UI de Navegación")]
    public GameObject botonFlechaArriba;   // El objeto de la flecha que apunta arriba
    public GameObject botonFlechaAbajo;    // El objeto de la flecha que apunta abajo

    private int zonaActual = 0; // Empezamos en el Nivel 1 (índice 0)

    private void Start()
    {
        // Al arrancar el juego nos aseguramos de estar en la posición inicial correcta
        ActualizarCamarasYZonas();
    }

    // Función que llamará el botón de la flecha ARRIBA
    public void SubirPiso()
    {
        if (zonaActual < misCamarasVirtuales.Length - 1)
        {
            zonaActual++;
            ActualizarCamarasYZonas();
        }
    }

    // Función que llamará el botón de la flecha ABAJO
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
        // 1. Control de Prioridades de Cinemachine
        for (int i = 0; i < misCamarasVirtuales.Length; i++)
        {
            if (misCamarasVirtuales[i] != null)
            {
                // La cámara del piso actual toma prioridad 10, las demás se apagan con 0
                misCamarasVirtuales[i].Priority = (i == zonaActual) ? 10 : 0;
            }
        }

        // 2. Lógica de visibilidad de los botones según el piso actual
        if (zonaActual == 0) 
        {
            // Nivel 1: Solo subir
            if(botonFlechaArriba != null) botonFlechaArriba.SetActive(true);
            if(botonFlechaAbajo != null) botonFlechaAbajo.SetActive(false);
        }
        else if (zonaActual == misCamarasVirtuales.Length - 1) 
        {
            // Nivel Último (Nivel 3): Solo bajar
            if(botonFlechaArriba != null) botonFlechaArriba.SetActive(false);
            if(botonFlechaAbajo != null) botonFlechaAbajo.SetActive(true);
        }
        else 
        {
            // Pisos intermedios (Nivel 2): Mostrar ambos botones
            if(botonFlechaArriba != null) botonFlechaArriba.SetActive(true);
            if(botonFlechaAbajo != null) botonFlechaAbajo.SetActive(true);
        }
    }
}
