using UnityEngine;

public class ActivadorDialogo : MonoBehaviour
{
    [Header("Configuración")]
    public DialogoData miDialogo; 
    public GameObject iconoInteractuar; 
    
    private bool jugadorCerca = false;

    void Start() {
        if (iconoInteractuar != null) iconoInteractuar.SetActive(false);
    }

    void Update() {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E)) {
            ManagerDialogos manager = Object.FindFirstObjectByType<ManagerDialogos>();
            if (manager != null && miDialogo != null) {
                manager.IniciarDialogo(miDialogo);
                if (iconoInteractuar != null) iconoInteractuar.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            jugadorCerca = true;
            if (iconoInteractuar != null) iconoInteractuar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            jugadorCerca = false;
            if (iconoInteractuar != null) iconoInteractuar.SetActive(false);
        }
    }
}