using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;   
    public GameObject panelGuardados;   

    [Header("Textos de los Slots")]
    public TextMeshProUGUI textoSlot1;
    public TextMeshProUGUI textoSlot2;
    public TextMeshProUGUI textoSlot3;

    [Header("Texto de Instrucciones (Opcional)")]
    public TextMeshProUGUI textoIndicador; 

    private bool mirandoOpciones = false;
    
    public static bool modoBorradoGlobal = false;     

    void Start()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(true);
        if (panelGuardados != null) panelGuardados.SetActive(false);

        if (textoIndicador != null) 
            textoIndicador.text = "Selecciona un Slot para jugar";
    }

    void Update()
    {
        if (mirandoOpciones && Time.timeScale == 1f)
        {
            mirandoOpciones = false;
            if (panelPrincipal != null) panelPrincipal.SetActive(true);
        }
    }

    public void AbrirPanelGuardados()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelGuardados != null) panelGuardados.SetActive(true);
        ActualizarInterfazSlots(); 
    }

    public void VolverAlMenuPrincipal()
    {
        if (panelGuardados != null) panelGuardados.SetActive(false);
        if (panelPrincipal != null) panelPrincipal.SetActive(true);
    }

    public void AbrirOpciones()
    {
        if (ControladorPausa.Instancia != null)
        {
            if (panelPrincipal != null) panelPrincipal.SetActive(false);
            
            ControladorPausa.Instancia.Pausar();
            mirandoOpciones = true;
        }
        else
        {
            Debug.LogError("¡No se encontró el prefab SistemaDePausa en la escena actual!");
        }
    }

    public void ActualizarInterfazSlots()
    {
        if (textoSlot1 != null) textoSlot1.text = GestorGuardado.ExistePartidaEnSlot(1) 
            ? "Partida 1\nRonda: " + PlayerPrefs.GetInt("Progreso_Slot_1") 
            : "Slot Vacío";

        if (textoSlot2 != null) textoSlot2.text = GestorGuardado.ExistePartidaEnSlot(2) 
            ? "Partida 2\nRonda: " + PlayerPrefs.GetInt("Progreso_Slot_2") 
            : "Slot Vacío";

        if (textoSlot3 != null) textoSlot3.text = GestorGuardado.ExistePartidaEnSlot(3) 
            ? "Partida 3\nRonda: " + PlayerPrefs.GetInt("Progreso_Slot_3") 
            : "Slot Vacío";
    }

    public void SeleccionarSlot(int numeroSlot)
    {
        if (modoBorradoGlobal)
        {
            EjecutarBorradoDeSlot(numeroSlot);
            return; 
        }

        Debug.Log("Se seleccionó el Slot número: " + numeroSlot);
        Time.timeScale = 1f;

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

        SceneManager.LoadScene("NuevoMenu");
    }

    public void AlternarModoBorrado()
    {
        modoBorradoGlobal = !modoBorradoGlobal;

        if (modoBorradoGlobal)
        {
            if (textoIndicador != null) 
                textoIndicador.text = "<color=red>¡SELECCIONA EL ARCHIVO QUE DESEAS BORRAR!</color>";
        }
        else
        {
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

        modoBorradoGlobal = false;

        if (textoIndicador != null) 
            textoIndicador.text = "Slot " + numeroSlot + " borrado con éxito. Selecciona un Slot para jugar.";

        ActualizarInterfazSlots();
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}