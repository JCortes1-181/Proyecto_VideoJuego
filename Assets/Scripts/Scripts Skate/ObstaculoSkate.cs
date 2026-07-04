using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaculoSkate : MonoBehaviour
{
    public float velocidadMover = 7f; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * velocidadMover;
    }

    private void OnTriggerEnter2D(Collider2D collision)
{

    if (collision.CompareTag("Player"))
    {
        Chocar();
    }
}

    void Chocar()
    {

        ControladorVidas.vidasGlobales--; 
        

        SceneManager.LoadScene("FreddyFazbear");
    }
}
