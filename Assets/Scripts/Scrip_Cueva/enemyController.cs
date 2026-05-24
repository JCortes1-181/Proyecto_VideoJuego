using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyController : MonoBehaviour
{
    public Transform player;
    public float speed = 2.0f;
    public Animator animator; 

    [Header("Rangos")]
    public float detectionRadius = 5.0f;
    public float attackRadius = 1.0f; 

    [Header("Configuración de Ataque")]
    public float cooldownAtaque = 1.5f; 

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool yaAtaco = false; 
    private bool estaMuerto = false; 
    private bool atacandoEnEsteInstante = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaMuerto) return;

        if (player != null && !yaAtaco)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRadius)
            {
                movement = Vector2.zero;
                StartCoroutine(SecuenciaAtaque());
            }
            else if (distanceToPlayer < detectionRadius)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                movement = new Vector2(direction.x, 0);
            }
            else
            {
                movement = Vector2.zero;
            }
        }
        
        if (!atacandoEnEsteInstante && !estaMuerto)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    System.Collections.IEnumerator SecuenciaAtaque()
    {
        yaAtaco = true;
        atacandoEnEsteInstante = true;
        
        if (animator != null)
        {
            animator.SetTrigger("Attack"); 
        }

        yield return new WaitForSeconds(0.35f); 

        if (!estaMuerto && player != null)
        {
            float distanciaFinal = Vector2.Distance(transform.position, player.position);
            
            if (distanciaFinal <= attackRadius)
            {
                Moverse_Cueva scriptJugador = player.GetComponent<Moverse_Cueva>();
                
                if (scriptJugador != null && scriptJugador.enabled == true)
                {
                    Debug.Log("¡El golpe conectó justamente!");
                    
                    scriptJugador.Morir();
                    
                    scriptJugador.enabled = false;
                    
                    StartCoroutine(ReiniciarNivel());
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        atacandoEnEsteInstante = false; 


        yield return new WaitForSeconds(cooldownAtaque);
        yaAtaco = false; 
    }

    public void RecibirDanio()
    {
        if (estaMuerto) return;
        Morir();
    }

    void Morir()
    {
        estaMuerto = true;
        movement = Vector2.zero;
        atacandoEnEsteInstante = false;
        StopAllCoroutines(); 
        
        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D col))
        {
            col.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die"); 
        }

        Debug.Log("¡El enemigo ha muerto!");
        StartCoroutine(EsperarParaDestruir());
    }

    System.Collections.IEnumerator EsperarParaDestruir()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    System.Collections.IEnumerator ReiniciarNivel()
    {
        yield return new WaitForSeconds(1.5f); 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}