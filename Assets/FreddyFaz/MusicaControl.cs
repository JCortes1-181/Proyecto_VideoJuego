using UnityEngine;

public class musicacontrol : MonoBehaviour
{
    public static musicacontrol instancia; // Corregido el tipo a minúsculas
    private AudioSource miAudio;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            miAudio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirMusica()
    {
        if (miAudio != null && !miAudio.isPlaying)
        {
            miAudio.Play();
        }
    }
}