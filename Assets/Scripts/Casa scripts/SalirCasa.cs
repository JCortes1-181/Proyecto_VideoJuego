using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCasa : MonoBehaviour
{
    // Aquí pondremos el nombre de tu escena del mapa
    public string Mapa = "SampleScene"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que toca la puerta tiene el nombre o etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(Mapa);
        }
    }
}