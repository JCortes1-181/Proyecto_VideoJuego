using UnityEngine;

public class Movimiento_Mario : MonoBehaviour
{
    public float velocidad = 12f;
    public float fuerzaSalto = 15f; 
    private Rigidbody2D rb;
    private float movH;
    private bool puedeSaltar;
    private Vector3 posicionInicial;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 2f; 
        posicionInicial = transform.position;
    }

    public void ResetearPosicion() {
        transform.position = posicionInicial;
        if(rb != null) rb.linearVelocity = Vector2.zero;
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
        if (col.name.ToLower().Contains("moneda") || col.name.ToLower().Contains("pan")) {
            col.gameObject.SetActive(false);
        }
    }
}