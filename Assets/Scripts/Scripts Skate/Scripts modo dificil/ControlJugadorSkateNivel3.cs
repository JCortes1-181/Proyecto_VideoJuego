using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugadorSkateNivel3 : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float fuerzaSalto = 12f;
    public float distanciaRaycast = 1.2f;

    [Header("Ajustes de Agacharse")]
    public float factorAgachado = 0.5f; 

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip sonidoSalto;   

    private Rigidbody2D rb;
    private bool enSuelo;
    private Animator anim;
    
    // Variables para guardar el tamaño original y poder restaurarlo
    private BoxCollider2D colisionador;
    private Vector3 escalaOriginal; // CORREGIDO: Ahora es Vector3 para incluir Z
    private Vector2 tamañoOriginalCol;
    private Vector2 offsetOriginalCol;
    private bool estaAgachado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colisionador = GetComponent<BoxCollider2D>();

        // Guardamos cómo era el personaje originalmente
        escalaOriginal = transform.localScale;
        if (colisionador != null)
        {
            tamañoOriginalCol = colisionador.size;
            offsetOriginalCol = colisionador.offset;
        }
    }

  void Update()
    {
        // 1. Detección de suelo
        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, distanciaRaycast, LayerMask.GetMask("Suelo"));

        if (anim != null)
        {
            anim.SetBool("estaEnSuelo", enSuelo);
        }

        // 2. Lógica de AGACHARSE (MODIFICADA)
        // Quitamos el "&& enSuelo" para que puedas agacharte incluso si estás saltando
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Agacharse();
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            Levantarse();
        }

        // 3. Lógica de Salto
        // Mantenemos la condición de que solo salte si está en suelo y NO está agachado
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo && !estaAgachado)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            
            if (audioSource != null && sonidoSalto != null)
            {
                audioSource.PlayOneShot(sonidoSalto);
            }
        }

        // 4. Caída al vacío
        if (transform.position.y < -6f)
        {
            PerderVida();
        }
    }

    void Agacharse()
    {
        estaAgachado = true;
        
        // Efecto visual: Aplastamos el sprite
        transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y * factorAgachado, escalaOriginal.z);
        
        // Efecto físico: Achicamos la hitbox
        if (colisionador != null)
        {
            colisionador.size = new Vector2(tamañoOriginalCol.x, tamañoOriginalCol.y * factorAgachado);
            colisionador.offset = new Vector2(offsetOriginalCol.x, offsetOriginalCol.y - (tamañoOriginalCol.y * 0.25f)); 
        }
    }

    void Levantarse()
    {
        estaAgachado = false;
        
        // Restauramos el tamaño visual y físico
        transform.localScale = escalaOriginal;
        if (colisionador != null)
        {
            colisionador.size = tamañoOriginalCol;
            colisionador.offset = offsetOriginalCol;
        }
    }

    public void PerderVida()
    {
        // Recarga la escena de castigo
        SceneManager.LoadScene("FreddyFazbear");
    }
}