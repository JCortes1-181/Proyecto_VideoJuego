using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJugador : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float fuerzaSalto = 12f;
    private bool estaEnSuelo = false;
    private bool juegoTerminado = false;

    [Header("Referencias de Audio")]
    public AudioSource audioGrito; 
    public AudioSource audioSalto;  

    [Header("Sprites")]
    public Sprite spriteNormal; 
    public Sprite spriteSalto; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
       
        if(audioGrito) audioGrito.playOnAwake = false;
        if(audioSalto) audioSalto.playOnAwake = false;
    }

    void Update()
    {
        if (juegoTerminado) return;

        
        if (estaEnSuelo && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            rb.linearVelocity = Vector2.up * fuerzaSalto;
            estaEnSuelo = false;
            spriteRenderer.sprite = spriteSalto; 

            if (audioSalto != null) audioSalto.Play();
        }

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
           
            float limiteSuperiorSuelo = collision.transform.position.y + (collision.transform.localScale.y / 2.2f);

            if (transform.position.y > limiteSuperiorSuelo)
            {
                estaEnSuelo = true;
                spriteRenderer.sprite = spriteNormal;
            }
            else 
            {
                
                ActivarMuerte();
            }
        }
    }

    void ActivarMuerte()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (audioGrito != null) audioGrito.Play();

        
        ControladorVidas.vidasGlobales--; 
        Debug.Log("Vidas restantes: " + ControladorVidas.vidasGlobales);

        Invoke("VolverAOficina", 1.3f);
    }

    
    public void TerminarMinijuegoSinMorir()
    {
        juegoTerminado = true;
        Debug.Log("¡Victoria! Tiempo completado.");
        Invoke("VolverAOficina", 1.5f);
    }

    void VolverAOficina()
    {
        SceneManager.LoadScene("FreddyFazbear");
    }
}