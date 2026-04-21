using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    
    public float velocidad = 5f; 
    
    
    private Rigidbody2D rb; 
    
    private Vector2 direccion; 

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
        float moverX = Input.GetAxisRaw("Horizontal"); 
        float moverY = Input.GetAxisRaw("Vertical"); 

        direccion = new Vector2(moverX, moverY).normalized;
    }

    void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + direccion * velocidad * Time.fixedDeltaTime);
    }
}