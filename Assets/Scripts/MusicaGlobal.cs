using UnityEngine;

public class MusicaControl : MonoBehaviour
{
    public static MusicaControl instancia;

    void Awake()
    {
        // Si ya existe una música, destruye esta nueva de inmediato
        if (instancia != null && instancia != this)
        {
            Debug.Log("Música duplicada detectada, destruyendo...");
            Destroy(this.gameObject);
            return;
        }

        // Si es la primera vez que aparece, se vuelve persistente
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
