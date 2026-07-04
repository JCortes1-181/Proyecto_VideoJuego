using UnityEngine;
using System.Collections; 

public class Movimiento_Mario_nuevo : MonoBehaviour
{
    public float velocidad = 12f;
    public float fuerzaSalto = 15f; 
    public AudioSource sonidoRecoger; 
    
    [Header("Secuencia de Caída")]
    public AudioSource sonidoCaida; 
    public AudioSource sonidoGolpe; 
    public float retrasoGolpe = 1f; 
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

    private IEnumerator SecuenciaCaida()
    {
        yaPerdio = true; 
        

        if(sonidoCaida != null) sonidoCaida.Play(); 
        

        yield return new WaitForSeconds(retrasoGolpe);
        

        if(sonidoGolpe != null) sonidoGolpe.Play();
        

        if (controlador != null)
        {
            controlador.Finalizar(false);
        }
    }
}