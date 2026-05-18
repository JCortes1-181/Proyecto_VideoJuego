using UnityEngine;

public class Musicacontrol : MonoBehaviour
{
    // La variable 'instancia' ahora coincide con el nombre de la clase
     public static Musicacontrol instancia;
    private AudioSource miAudio;

    void Awake()
    {
        // Sistema Singleton: evita que la música se reinicie al cambiar de escena
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            miAudio = GetComponent<AudioSource>();
        }
        else
        {
            // Si ya existe un gestor de música, destruye el nuevo para que no se solapen
            Destroy(gameObject);
        }
    }

    // Esta es la función que llamamos desde JuegoGeneral
    public void ReproducirMusica()
    {
        if (miAudio != null && !miAudio.isPlaying)
        {
            miAudio.Play();
        }
    }

    // Esta es la función que llamamos desde ControladorVidas al perder
    public void DetenerMusica()
    {
        if (miAudio != null)
        {
            miAudio.Stop();
        }
    }
}
