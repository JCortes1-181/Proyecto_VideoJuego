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

    // --- NUEVO: SONIDO DE ATAQUE ---
    public AudioSource audioSource; 
    public AudioClip sonidoAtaque;    

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

        if (movimientoX < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (movimientoX > 0) transform.localScale = new Vector3(1, 1, 1);

        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSuelo, 0f, queEsSuelo);

        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // ATAQUE
        if (Input.GetButtonDown("Fire1")) 
        {
            Atacar();
        }
    }

    private void Atacar()
    {
        if (animator != null) animator.SetTrigger("Attack");

        // REPRODUCIR SONIDO SI ESTÁ CONFIGURADO
        if (audioSource != null && sonidoAtaque != null)
        {
            audioSource.PlayOneShot(sonidoAtaque);
        }

        Collider2D[] objetosGolpeados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, queEsEnemigo);

        foreach (Collider2D enemigo in objetosGolpeados)
        {
            if (enemigo.TryGetComponent<enemyController>(out enemyController scriptEnemigo))
            {
                scriptEnemigo.RecibirDanio();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!estoyMuerto)
        {
            rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);
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
        if (TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;
        if (animator != null) animator.SetTrigger("Die");
    }
}