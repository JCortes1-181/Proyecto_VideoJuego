using UnityEngine;

public class Movimiento_Mario_nuevo : MonoBehaviour
{
    public float velocidad = 12f;
    public float fuerzaSalto = 15f; 
    public AudioSource sonidoRecoger; // Arrastra aquí el AudioSource del pan
    
    private Rigidbody2D rb;
    private float movH;
    private bool puedeSaltar;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update() {
        movH = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W) && puedeSaltar) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            puedeSaltar = false;
        }
    }

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(movH * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        puedeSaltar = true; 
    }

    private void OnTriggerEnter2D(Collider2D col) {
        string nombreObjeto = col.name.ToLower();
        if (nombreObjeto.Contains("moneda") || nombreObjeto.Contains("pan") || nombreObjeto.Contains("food")) 
        {
            if(sonidoRecoger != null) sonidoRecoger.Play(); // REPRODUCE EL SONIDO
            col.gameObject.SetActive(false); 
        }
    }
}
