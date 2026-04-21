using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class ControladorMenu : MonoBehaviour
{
    public void Jugar()
    {
        // Esto cargará la escena de la casa. 
        // Asegúrate de que el nombre entre comillas sea EXACTO al de tu escena de la casa.
        SceneManager.LoadScene("SampleScene"); 
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); // Esto cierra el juego (solo funciona cuando lo exportas)
    }
}