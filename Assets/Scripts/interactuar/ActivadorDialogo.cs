using UnityEngine;

public class ActivadorDialogo : MonoBehaviour
{
    public DialogoData miDialogo; // Arrastra aquí el archivo que creaste en el paso 2
    private bool jugadorCerca = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            // Busca al manager y le entrega el diálogo
            Object.FindFirstObjectByType<ManagerDialogos>().IniciarDialogo(miDialogo);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }
}
