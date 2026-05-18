using UnityEngine;

public class Moverse_Mapa2d : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;
    private Rigidbody2D rb; // Añadimos la referencia al Rigidbody

    void Start()
    {
        // Buscamos el componente Rigidbody2D al iniciar
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;
        
        // Usamos "animator" que es como lo llamaste arriba
        animator.SetFloat("Movimiento", Mathf.Abs(velocidadX * velocidad));

        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        Vector3 posicion = transform.position;
        transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z);
    }

    // Este método se ejecuta automáticamente cuando desactivamos el script
    private void OnDisable()
    {
        // Si tienes un Rigidbody, lo frenamos
        if (rb != null) 
        {
            rb.linearVelocity = Vector2.zero;
        }

       
        if (animator != null)
        {
            animator.SetFloat("Movimiento", 0);
        }
    }
}
