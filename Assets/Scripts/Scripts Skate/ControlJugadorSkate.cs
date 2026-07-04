using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugadorSkate : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float fuerzaSalto = 12f;
    public float distanciaRaycast = 1.2f;

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip sonidoSalto;   

    private Rigidbody2D rb;
    private bool enSuelo;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, distanciaRaycast, LayerMask.GetMask("Suelo"));

        if (anim != null)
        {
            anim.SetBool("estaEnSuelo", enSuelo);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo)
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

    public void PerderVida()
    {
        ControladorVidas.vidasGlobales--;
        SceneManager.LoadScene("FreddyFazbear");
    }
}