using UnityEngine;

public class EnemigoDificil : MonoBehaviour
{
    private ControladorSpaceDificil controlador;

    void Start()
    {
        controlador = FindObjectOfType<ControladorSpaceDificil>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bala")) 
        {
            if (controlador != null)
            {
                controlador.EnemigoDestruido(); 
            }
            
            Destroy(collision.gameObject); 
            Destroy(gameObject); 
        }
    }
}