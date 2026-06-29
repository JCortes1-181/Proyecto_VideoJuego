using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuSeleccionSlots : MonoBehaviour
{
    [Header("Textos de los Botones")]
    public TextMeshProUGUI textoSlot1;
    public TextMeshProUGUI textoSlot2;
    public TextMeshProUGUI textoSlot3;

    [Header("Texto de Instrucciones (Nuevo/Opcional)")]
    public TextMeshProUGUI textoIndicador; 

    // Ya no usamos una variable privada aquí. Usamos la global de MenuPrincipal.

    void Start()
    {
        ActualizarInterfazSlots();
        
        if (textoIndicador != null) 
            textoIndicador.text = "Selecciona un Slot para jugar";
    }

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

    public void PresionarSlot(int numeroSlot)
    {
        // --- INTERCEPCIÓN DE BORRADO SINCRONIZADA ---
        if (MenuPrincipal.modoBorradoGlobal)
        {
            EjecutarBorradoDeSlot(numeroSlot);
            return; 
        }

        GestorGuardado.slotActual = numeroSlot;

        PlayerPrefs.SetInt("SlotActual", numeroSlot);
        PlayerPrefs.Save();

        if (GestorGuardado.ExistePartidaEnSlot(numeroSlot))
        {
            GestorGuardado.CargarPartida();
        }
        else
        {
            ControladorVidas.vidasGlobales = 4;
            JuegoGeneral.minijuegosCompletados = 0;
            
            GestorGuardado.GuardarPartida();
        }

        if (GestorDeProgreso.Instancia != null)
        {
            GestorDeProgreso.Instancia.CargarProgreso();
        }

        SceneManager.LoadScene("MenuPrincipal"); 
    }

    public void AlternarModoBorrado()
    {
        MenuPrincipal.modoBorradoGlobal = !MenuPrincipal.modoBorradoGlobal;

        if (MenuPrincipal.modoBorradoGlobal)
        {
            Debug.Log("Modo Borrado Activado.");
            if (textoIndicador != null) 
                textoIndicador.text = "<color=red>¡SELECCIONA EL ARCHIVO QUE DESEAS BORRAR!</color>";
        }
        else
        {
            Debug.Log("Modo Borrado Cancelado.");
            if (textoIndicador != null) 
                textoIndicador.text = "Selecciona un Slot para jugar";
        }
    }

    private void EjecutarBorradoDeSlot(int numeroSlot)
    {
        PlayerPrefs.DeleteKey("Progreso_Slot_" + numeroSlot);
        
        if (GestorDeProgreso.Instancia != null)
        {
            GestorDeProgreso.Instancia.BorrarProgresoDeSlotEspecifico(numeroSlot);
        }
        else
        {
            PlayerPrefs.DeleteKey("Slot_" + numeroSlot + "_Nivel1");
            PlayerPrefs.DeleteKey("Slot_" + numeroSlot + "_Nivel2");
            PlayerPrefs.DeleteKey("Slot_" + numeroSlot + "_Historia");
        }

        PlayerPrefs.Save(); 

        Debug.Log("¡Datos del Slot " + numeroSlot + " eliminados correctamente!");

        MenuPrincipal.modoBorradoGlobal = false;

        if (textoIndicador != null) 
            textoIndicador.text = "Slot " + numeroSlot + " borrado con éxito. Elige un Slot para jugar.";

        ActualizarInterfazSlots();
    }
}