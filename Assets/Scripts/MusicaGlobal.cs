using UnityEngine;

public class MusicaControl : MonoBehaviour
{
    public static MusicaControl instancia;

    void Awake()
    {
       
        if (instancia != null && instancia != this)
        {
            Debug.Log("Música duplicada detectada, destruyendo...");
            Destroy(this.gameObject);
            return;
        }

        
        instancia = this;
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Música global iniciada correctamente.");
    }

    public void DetenerMusica()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Stop();
    }
}
