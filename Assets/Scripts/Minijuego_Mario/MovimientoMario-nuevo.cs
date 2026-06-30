using UnityEngine;
using System.Collections; // NUEVO: Necesario para usar las pausas de tiempo (Corrutinas)

public class Movimiento_Mario_nuevo : MonoBehaviour
{
    public float velocidad = 12f;
    public float fuerzaSalto = 15f; 
    public AudioSource sonidoRecoger; 
    
    [Header("Secuencia de Caída")]
    public AudioSource sonidoCaida; // El grito
    public AudioSource sonidoGolpe; // NUEVO: El golpe contra el piso
    public float retrasoGolpe = 1f; // NUEVO: Segundos que tarda en sonar el golpe después del grito
    public float limiteCaida = -10f; 
    
    private Rigidbody2D rb;
    private float movH;
    private bool puedeSaltar;
    private bool yaPerdio = false; 
    private ControladorEscenaMario controlador;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        
        controlador = Object.FindFirstObjectByType<ControladorEscenaMario>();
    }

    void Update() {
        if (yaPerdio) return; 

        movH = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W) && puedeSaltar) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            puedeSaltar = false;
        }

        if (transform.position.y < limiteCaida)
        {
            // En vez de ejecutar la caída de golpe, iniciamos la secuencia de tiempo
            StartCoroutine(SecuenciaCaida());
        }
    }

    void FixedUpdate() {
        if (yaPerdio) return;
        rb.linearVelocity = new Vector2(movH * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        puedeSaltar = true; 
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (yaPerdio) return; 
        
        string nombreObjeto = col.name.ToLower();
        if (nombreObjeto.Contains("moneda") || nombreObjeto.Contains("pan") || nombreObjeto.Contains("food")) 
        {
            if(sonidoRecoger != null) sonidoRecoger.Play(); 
            col.gameObject.SetActive(false); 
        }
    }

    // --- NUEVO: Corrutina para la secuencia de grito y golpe ---
    private IEnumerator SecuenciaCaida()
    {
        yaPerdio = true; // Bloquea el movimiento del jugador
        
        // 1. Reproduce el grito
        if(sonidoCaida != null) sonidoCaida.Play(); 
        
        // 2. Espera el tiempo que configuraste en el inspector
        yield return new WaitForSeconds(retrasoGolpe);
        
        // 3. Reproduce el golpe final
        if(sonidoGolpe != null) sonidoGolpe.Play();
        
        // 4. Le avisa a la escena que muestre el GIF de derrota
        if (controlador != null)
        {
            controlador.Finalizar(false);
        }
    }
}