using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuSeleccionSlots : MonoBehaviour
{
    [Header("Textos de los Botones")]
    public TextMeshProUGUI textoSlot1;
    public TextMeshProUGUI textoSlot2;
    public TextMeshProUGUI textoSlot3;

    void Start()
    {
        ActualizarInterfazSlots();
    }

    // Esto cambia los textos de tus botones para avisarle al jugador si hay datos guardados
    public void ActualizarInterfazSlots()
    {
        if (textoSlot1) textoSlot1.text = GestorGuardado.ExistePartidaEnSlot(1) 
            ? "Partida 1\nRonda: " + PlayerPrefs.GetInt("Progreso_Slot_1") 
            : "Slot Vacío";

        if (textoSlot2) textoSlot2.text = GestorGuardado.ExistePartidaEnSlot(2) 
            ? "Partida 2\nRonda: " + PlayerPrefs.GetInt("Progreso_Slot_2") 
            : "Slot Vacío";

        if (textoSlot3) textoSlot3.text = GestorGuardado.ExistePartidaEnSlot(3) 
            ? "Partida 3\nRonda: " + PlayerPrefs.GetInt("Progreso_Slot_3") 
            : "Slot Vacío";
    }

    // Esta función la van a llamar tus botones desde el Inspector
    public void PresionarSlot(int numeroSlot)
    {
        // Indicamos cuál es el slot activo en esta sesión de juego
        GestorGuardado.slotActual = numeroSlot;

        if (GestorGuardado.ExistePartidaEnSlot(numeroSlot))
        {
            // Si ya tiene datos, los cargamos
            GestorGuardado.CargarPartida();
        }
        else
        {
            // Si está vacío, configuramos una partida totalmente limpia
            ControladorVidas.vidasGlobales = 4;
            JuegoGeneral.minijuegosCompletados = 0;
            
            // Guardamos inmediatamente para inicializar el archivo
            GestorGuardado.GuardarPartida();
        }

        // Una vez elegido el slot, mandamos al jugador al menú de inicio o al juego
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}
