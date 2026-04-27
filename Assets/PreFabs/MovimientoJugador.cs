using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJugador : MonoBehaviour
{
    public float fuerzaSalto = 12f;
    private bool estaEnSuelo = false;
    private bool juegoTerminado = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public AudioSource audioGrito; // Arrastra el AudioSource aquí

    public Sprite spriteNormal; // teto1
    public Sprite spriteSalto;  // tetosalto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (juegoTerminado) return;

        // Salto
        if (estaEnSuelo && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            rb.linearVelocity = Vector2.up * fuerzaSalto;
            estaEnSuelo = false;
            spriteRenderer.sprite = spriteSalto; 
        }

        // CONDICIÓN 1: Caída al vacío (Y muy baja)
        if (transform.position.y < -6f)
        {
            ActivarMuerte();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (juegoTerminado) return;

        if (collision.gameObject.CompareTag("Suelo"))
        {
            // CONDICIÓN 2: Choque lateral (Muro natural)
            // Si el centro del Pou está más abajo que la parte superior del suelo, es un choque
            float puntoContactoSuperior = collision.transform.position.y + (collision.transform.localScale.y / 2.5f);

            if (transform.position.y > puntoContactoSuperior)
            {
                estaEnSuelo = true;
                spriteRenderer.sprite = spriteNormal;
            }
            else 
            {
                // El jugador chocó con el costado del bloque
                ActivarMuerte();
            }
        }
    }

    void ActivarMuerte()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (audioGrito != null)
        {
            audioGrito.Play();
        }

        // ACCESO DIRECTO: Restamos a la variable estática de tu script de oficina
        ControladorVidas.vidasGlobales--; 
        Debug.Log("Vidas restantes en el sistema global: " + ControladorVidas.vidasGlobales);

        Invoke("VolverAOficina", 1.3f);
    }

    public void TerminarMinijuegoSinMorir()
    {
        juegoTerminado = true;
        Invoke("VolverAOficina", 1.5f);
    }

    void VolverAOficina()
    {
        SceneManager.LoadScene("FreddyFazbear");
    }
}