using UnityEngine;

public class EnemigoDificil : MonoBehaviour
{
    private ControladorSpaceDificil controlador;

    void Start()
    {
        // Busca automáticamente el controlador difícil en la escena
        controlador = FindObjectOfType<ControladorSpaceDificil>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo toca la bala (Asegúrate de que tu bala tenga el Tag "Bala")
        if (collision.CompareTag("Bala")) 
        {
            if (controlador != null)
            {
                // Le avisa al controlador que murió para poder avanzar de oleada
                controlador.EnemigoDestruido(); 
            }
            
            Destroy(collision.gameObject); // Destruye la bala
            Destroy(gameObject); // Destruye a este enemigo
        }
    }
}