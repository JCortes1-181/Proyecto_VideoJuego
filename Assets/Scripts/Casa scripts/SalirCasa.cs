using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCasa : MonoBehaviour
{
    
    public string Mapa = "SampleScene"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(Mapa);
        }
    }
}