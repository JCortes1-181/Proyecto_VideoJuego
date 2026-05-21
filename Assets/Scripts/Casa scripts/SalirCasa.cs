using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de mapa
using TMPro; // Si usas TextMeshPro

public class SalirCasa : MonoBehaviour
{
[SerializeField] private GameObject indicadorE; // Arrastra aquí el objeto "Indicador"
    [SerializeField] private string nombreEscena; // Nombre del mapa al que vas
    private bool estaCerca = false;

    void Update()
    {
        // Solo si está cerca y presiona E, cambia de mapa
        if (estaCerca && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }

    // Se activa al entrar al collider de la puerta
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = true;
            indicadorE.SetActive(true); // Muestra el mensaje
        }
    }

    // Se activa al salir del área
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = false;
            indicadorE.SetActive(false); // Oculta el mensaje
        }
    }
}