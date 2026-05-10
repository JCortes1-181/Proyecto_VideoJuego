using UnityEngine;
using UnityEngine.SceneManagement; // Importante para cambiar de escena

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        // "MapaCompleto" debe ser el nombre exacto de tu escena del juego
        SceneManager.LoadScene("SampleScene");
    }
}
