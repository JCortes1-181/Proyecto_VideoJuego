using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento_Mario : MonoBehaviour
{
    public float velocidad = 12f;
    public float fuerzaSalto = 15f; 
    public int panesNecesarios = 5;
    private int panesActuales = 0;
    
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

        // Si te caes del mapa (opcional)
        if (transform.position.y < -10f) {
            PerderMinijuego();
        }
    }

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(movH * velocidad, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        puedeSaltar = true; 
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.name.ToLower().Contains("pan")) {
            col.gameObject.SetActive(false);
            panesActuales++;
            
            if (panesActuales >= panesNecesarios) {
                GanarMinijuego();
            }
        }
    }

    void GanarMinijuego() {
        SceneManager.LoadScene("FreddyFazbear");
    }

    void PerderMinijuego() {
        ControladorVidas.vidasGlobales--;
        SceneManager.LoadScene("FreddyFazbear");
    }
}