using UnityEngine;

public class EfectosFinalChat : MonoBehaviour
{
    [Header("CONFIGURACIÓN DE UI")]
    public GameObject objetoImagenBaneo;      
    public GameObject objetoImagenPollo;      
    public GameObject textoSpamJugador;

    [Header("AUDIOS DEL MINIJUEGO")]
    public AudioClip musicaFondo;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoDerrota;

    private AudioSource fuenteMusicaInterna;
    private AudioSource fuenteEfectosInterna;
    private bool juegoTerminado = false;

    void Start()
    {
        AudioSource[] fuentesExistentes = GetComponents<AudioSource>();
        
        fuenteMusicaInterna = fuentesExistentes.Length > 0 ? fuentesExistentes[0] : gameObject.AddComponent<AudioSource>();
        fuenteEfectosInterna = fuentesExistentes.Length > 1 ? fuentesExistentes[1] : gameObject.AddComponent<AudioSource>();

        fuenteMusicaInterna.clip = musicaFondo;
        fuenteMusicaInterna.loop = true;
        fuenteMusicaInterna.playOnAwake = false;
        fuenteMusicaInterna.volume = 0.7f;
        
        fuenteEfectosInterna.loop = false;
        fuenteEfectosInterna.playOnAwake = false;
        fuenteEfectosInterna.volume = 1.0f;

        if (musicaFondo != null)
        {
            fuenteMusicaInterna.Play();
        }
    }

    public void ActivarVictoria()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (fuenteMusicaInterna != null) fuenteMusicaInterna.Stop();

        if (textoSpamJugador != null) textoSpamJugador.SetActive(false);
        if (objetoImagenBaneo != null) objetoImagenBaneo.SetActive(true); 

        if (fuenteEfectosInterna != null && sonidoVictoria != null)
        {
            fuenteEfectosInterna.clip = sonidoVictoria;
            fuenteEfectosInterna.Play();
        }
    }

    public void ActivarDerrota()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (fuenteMusicaInterna != null) fuenteMusicaInterna.Stop();

        if (textoSpamJugador != null) textoSpamJugador.SetActive(false);
        if (objetoImagenPollo != null) objetoImagenPollo.SetActive(true); 

        if (fuenteEfectosInterna != null && sonidoDerrota != null)
        {
            fuenteEfectosInterna.clip = sonidoDerrota;
            fuenteEfectosInterna.Play();
        }
    }
}