using UnityEngine;

public class BalaController : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 10f;
    public string tagObjetivo; 

    private Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        
        
        rb.linearVelocity = Vector2.up * velocidad;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log("La bala acaba de chocar contra el objeto llamado: " + collision.gameObject.name);
        
        if (collision.CompareTag(tagObjetivo))
        {
            Destroy(collision.gameObject); 
            Destroy(gameObject);            
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}