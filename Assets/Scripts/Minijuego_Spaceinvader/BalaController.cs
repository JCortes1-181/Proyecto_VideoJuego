using UnityEngine;

public class BalaController : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 10f;
    public string tagObjetivo; 

    private Rigidbody2D rb;
    
    private ControladorSpace controladorJuego;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * velocidad;

        controladorJuego = FindObjectOfType<ControladorSpace>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("La bala acaba de chocar contra el objeto llamado: " + collision.gameObject.name);
        
        if (collision.CompareTag("Enemigos"))
        {
            if (controladorJuego != null)
            {
                controladorJuego.EnemigoDestruido();
            }

            Destroy(collision.gameObject); 
            Destroy(gameObject);            
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}