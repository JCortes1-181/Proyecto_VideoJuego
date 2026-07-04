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
    
    private BoxCollider2D colisionador;
    private Vector3 escalaOriginal; 
    private Vector2 tamañoOriginalCol;
    private Vector2 offsetOriginalCol;
    private bool estaAgachado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colisionador = GetComponent<BoxCollider2D>();

        escalaOriginal = transform.localScale;
        if (colisionador != null)
        {
            tamañoOriginalCol = colisionador.size;
            offsetOriginalCol = colisionador.offset;
        }
    }

  void Update()
    {

        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, distanciaRaycast, LayerMask.GetMask("Suelo"));

        if (anim != null)
        {
            anim.SetBool("estaEnSuelo", enSuelo);
        }


        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Agacharse();
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            Levantarse();
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo && !estaAgachado)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            
            if (audioSource != null && sonidoSalto != null)
            {
                audioSource.PlayOneShot(sonidoSalto);
            }
        }


        if (transform.position.y < -6f)
        {
            PerderVida();
        }
    }

    void Agacharse()
    {
        estaAgachado = true;
        

        transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y * factorAgachado, escalaOriginal.z);
        
        if (colisionador != null)
        {
            colisionador.size = new Vector2(tamañoOriginalCol.x, tamañoOriginalCol.y * factorAgachado);
            colisionador.offset = new Vector2(offsetOriginalCol.x, offsetOriginalCol.y - (tamañoOriginalCol.y * 0.25f)); 
        }
    }

    void Levantarse()
    {
        estaAgachado = false;

        transform.localScale = escalaOriginal;
        if (colisionador != null)
        {
            colisionador.size = tamañoOriginalCol;
            colisionador.offset = offsetOriginalCol;
        }
    }

    public void PerderVida()
    {
        SceneManager.LoadScene("Nivel3");
    }
}