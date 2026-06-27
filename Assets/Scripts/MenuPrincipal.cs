using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;   // Contenedor de tus BotonesPrincipales
    public GameObject panelGuardados;   // Tu panel de Guardados (el que sacaste al Canvas)

    [Header("Textos de los Slots")]
    public TextMeshProUGUI textoSlot1;
    public TextMeshProUGUI textoSlot2;
    public TextMeshProUGUI textoSlot3;

    private bool mirandoOpciones = false;

    void Start()
    {
        // Aseguramos que el menú inicie en su estado correcto
        if (panelPrincipal != null) panelPrincipal.SetActive(true);
        if (panelGuardados != null) panelGuardados.SetActive(false);
    }

    void Update()
    {
        // Si abrimos las opciones desde tu prefab y luego el jugador las cierra (Reanudar),
        // el Time.timeScale de tu ControladorPausa vuelve a 1f. Ahí reactivamos los botones.
        if (mirandoOpciones && Time.timeScale == 1f)
        {
            mirandoOpciones = false;
            if (panelPrincipal != null) panelPrincipal.SetActive(true);
        }
    }

    // --- MANEJO DE PANELES ---
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
        // Se comunica con la Instancia de tu script ControladorPausa.cs
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

    // --- INTERFAZ DE LOS SLOTS (PLANTILLA LIMPIA) ---
    public void ActualizarInterfazSlots()
    {
        // Textos base para que compile sin errores. Luego podrás conectar tu guardado real aquí.
        if (textoSlot1 != null) textoSlot1.text = "PARTIDA 01\n- CLÁSICO -";
        if (textoSlot2 != null) textoSlot2.text = "PARTIDA 02\n- VACÍO -";
        if (textoSlot3 != null) textoSlot3.text = "PARTIDA 03\n- VACÍO -";
    }

    public void SeleccionarSlot(int numeroSlot)
    {
        Debug.Log("Se seleccionó el Slot número: " + numeroSlot);

        // Nos aseguramos de que el tiempo corra normalmente (1) antes de ir al nivel
        Time.timeScale = 1f;

        // Carga la escena de tu juego directamente
        SceneManager.LoadScene("NuevoMenu");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}