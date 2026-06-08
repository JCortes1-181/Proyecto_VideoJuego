using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para detectar el componente Button

public class ControladorHistoria : MonoBehaviour
{
    private VideoPlayer miVideoPlayer;

    [Header("Configuración de Salida")]
    [Tooltip("El nombre exacto de la escena del minijuego que viene después del video")]
    public string escenaSiguiente = "FreddyFazbear";

    [Header("Referencias de la UI")]
    [Tooltip("Arrastra aquí el botón de la interfaz que servirá para saltar")]
    public Button botonSaltar;

    void Start()
    {
        miVideoPlayer = GetComponent<VideoPlayer>();

        // 1. Configurar la detección automática del fin del video
        if (miVideoPlayer != null)
        {
            miVideoPlayer.loopPointReached += VideoTerminado;
        }
        else
        {
            Debug.LogError("¡Falta el componente Video Player en este GameObject!");
        }

        // 2. Configurar la acción del botón al hacer clic
        if (botonSaltar != null)
        {
            botonSaltar.onClick.RemoveAllListeners();
            botonSaltar.onClick.AddListener(SaltarEscena);
        }
    }

    // Se ejecuta automáticamente al llegar al último frame del video
    void VideoTerminado(VideoPlayer vp)
    {
        SaltarEscena();
    }

    // Función centralizada para cambiar de escena
    public void SaltarEscena()
    {
        // Cargamos la etapa del gameplay
        SceneManager.LoadScene(escenaSiguiente);
    }

    // Por si además quieres dejar que salten presionando la tecla Espacio en el teclado
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaltarEscena();
        }
    }
}
