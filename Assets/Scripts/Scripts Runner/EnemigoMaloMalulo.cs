using UnityEngine;

public class Muerte : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            
            FindObjectOfType<Condiciones>().Perder();
        }
    }
}
