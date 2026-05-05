using UnityEngine;

public class MovimientoMapa : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    
    [Header("Sprites de Animación")]
    public Sprite spriteQuieto;   // Arrastra aquí "quieto.png"
    public Sprite spriteMoverse;  // Arrastra aquí "moverse.png"
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float movimientoX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Esto evita que el personaje se caiga de lado o rote
        rb.freezeRotation = true; 
    }

    void Update()
    {
        // Detecta si presionas flecha izquierda (-1) o derecha (1)
        movimientoX = Input.GetAxisRaw("Horizontal");

        // Lógica de Sprites y Dirección
        if (movimientoX != 0)
        {
            // Ponemos el sprite de movimiento
            spriteRenderer.sprite = spriteMoverse;

            // ESTA ES LA MAGIA: 
            // Si movimientoX es menor a 0 (vas a la izquierda), flipX es TRUE (se voltea).
            // Si movimientoX es mayor a 0 (vas a la derecha), flipX es FALSE (mira normal).
            spriteRenderer.flipX = (movimientoX < 0);
        }
        else
        {
            // Si no te mueves, vuelve al sprite quieto
            spriteRenderer.sprite = spriteQuieto;
        }
    }

    void FixedUpdate()
    {
        // Aplica el movimiento físico al Rigidbody2D
        rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);
    }
}
