using UnityEngine;

public class Moverse_Cueva : MonoBehaviour
{
    public ControladorCueva controladorJuego;
    public float velocidad = 5f;
    public Animator animator;
    private Rigidbody2D rb;
    private float movimientoX;

    private bool estoyMuerto = false;


    public float fuerzaSalto = 10f;
    public Transform controladorSuelo;   
    public Vector2 dimensionesCajaSuelo; 
    public LayerMask queEsSuelo;        
    private bool enSuelo;

    public Transform controladorAtaque;   
    public float radioAtaque = 0.5f;      
    public LayerMask queEsEnemigo;       
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (estoyMuerto) return;

        movimientoX = Input.GetAxisRaw("Horizontal"); 

        if (animator != null)
        {
            animator.SetFloat("Movement", Mathf.Abs(movimientoX));
        }

        if (movimientoX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movimientoX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSuelo, 0f, queEsSuelo);

        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            Atacar();
        }
    }

    void FixedUpdate()
    {
        if (estoyMuerto) return;
        rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);
    }

    private void Atacar()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack"); 
        }

        StartCoroutine(EsperarParaHacerDanio());
    }

    System.Collections.IEnumerator EsperarParaHacerDanio()
    {
        yield return new WaitForSeconds(0.15f);

        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, queEsEnemigo);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            if (enemigo.TryGetComponent<enemyController>(out enemyController scriptEnemigo))
            {
                scriptEnemigo.RecibirDanio();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSuelo);
        }
        if (controladorAtaque != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
        }
    }

    public void Morir()
    {
        estoyMuerto = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f; 
            rb.simulated = false; 
        }

        if (TryGetComponent<Collider2D>(out Collider2D col))
        {
            col.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        StartCoroutine(AcomodarCuerpoEnElSuelo());
    
        if (controladorJuego != null)
        {
            controladorJuego.Finalizar(false);
        }
    }

    System.Collections.IEnumerator AcomodarCuerpoEnElSuelo()
    {
        yield return new WaitForSeconds(0.4f); 


        transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
    }
}