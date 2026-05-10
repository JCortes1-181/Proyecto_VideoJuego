using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float fuerzaSalto = 12f;
    private Rigidbody2D rb;
    private bool estaEnSuelo = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Detecta salto con Espacio o Flecha Arriba
        if (estaEnSuelo && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            estaEnSuelo = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Suelo")) {
            estaEnSuelo = true;
        }
    }
}
