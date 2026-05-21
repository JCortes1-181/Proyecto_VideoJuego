using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugadorSkate : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float fuerzaSalto = 12f;
    public float distanciaRaycast = 1.2f; // Puedes ajustar esto en el Inspector

    private Rigidbody2D rb;
    private bool enSuelo;
    private Animator anim; // NUEVO: Para controlar las animaciones

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Inicializamos el Animator
    }

    void Update()
    {
        // 1. DETECCIÓN DE SUELO PARA ANIMACIÓN (Raycast)
        // Lanza una línea hacia abajo para saber si estamos en el aire
        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, distanciaRaycast, LayerMask.GetMask("Suelo"));

        // 2. ACTUALIZAR ANIMATOR
        // Enviamos el valor al parámetro "estaEnSuelo" que creamos en Unity
        if (anim != null)
        {
            anim.SetBool("estaEnSuelo", enSuelo);
        }

        // 3. LÓGICA DE SALTO
        // Saltamos si presionamos Espacio/W y el Raycast detecta suelo
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // 4. CAÍDA AL VACÍO
        if (transform.position.y < -6f)
        {
            PerderVida();
        }
    }

    // Mantenemos esto como respaldo para colisiones exactas con el Tilemap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    public void PerderVida()
    {
        ControladorVidas.vidasGlobales--; // Resta vida global
        SceneManager.LoadScene("FreddyFazbear"); // Vuelve a la escena de la oficina
    }

    // Dibuja la línea roja en el editor para que puedas calibrar la altura
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanciaRaycast);
    }
}