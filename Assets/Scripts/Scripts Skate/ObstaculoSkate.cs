using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaculoSkate : MonoBehaviour
{
    public float velocidadMover = 7f; // Ajusta qué tan rápido viene la roca
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // La roca empieza a moverse a la izquierda apenas aparece
        rb.linearVelocity = Vector2.left * velocidadMover;
    }

    // Se activa cuando el Aguacate entra en el área de la roca
    private void OnTriggerEnter2D(Collider2D collision)
{
    // AHORA SOLO BUSCAMOS EL TAG "Player"
    if (collision.CompareTag("Player"))
    {
        Chocar();
    }
}

    void Chocar()
    {
        // Restamos vida usando tu sistema global
        ControladorVidas.vidasGlobales--; 
        
        // Volvemos a la oficina de Freddy
        SceneManager.LoadScene("FreddyFazbear");
    }
}
