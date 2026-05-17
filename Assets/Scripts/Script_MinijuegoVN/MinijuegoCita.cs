using UnityEngine;
using UnityEngine.UI;
using TMPro; // ESTO ES CLAVE para que acepte el Text (TMP)

public class MinijuegoCitas : MonoBehaviour
{
    [Header("Referencias de CJ")]
    public Image imagenPersonaje; 
    public Sprite cjNormal, cjFeliz, cjPistola;

    [Header("Configuracion de Textos")]
    public TextMeshProUGUI textoDialogo; // Cambiado a TextMeshPro
    [TextArea] public string mensajeInicial = "CJ te mira fijamente... ¿Qué le dices?";
    [TextArea] public string mensajeVictoria = "¡Esa era! CJ te invita una Sprunk.";
    [TextArea] public string mensajeDerrota = "¡Mal ahí! CJ no perdona.";

    void Start() {
        if(imagenPersonaje != null) imagenPersonaje.sprite = cjNormal;
        if(textoDialogo != null) textoDialogo.text = mensajeInicial;
    }

    public void SeleccionarOpcion(bool esCorrecta) {
        if (esCorrecta) {
            imagenPersonaje.sprite = cjFeliz;
            textoDialogo.text = mensajeVictoria;
        } else {
            imagenPersonaje.sprite = cjPistola;
            textoDialogo.text = mensajeDerrota;
        }
    }
}
