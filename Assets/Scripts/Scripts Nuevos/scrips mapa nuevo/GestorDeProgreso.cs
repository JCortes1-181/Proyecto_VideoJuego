using UnityEngine;

public class GestorDeProgreso : MonoBehaviour
{
    // Hacemos que este script sea accesible globalmente desde cualquier parte del juego
    public static GestorDeProgreso Instancia;

    [Header("Estado Actual del Jugador")]
    public bool nivel1Completado = false;
    public bool nivel2Completado = false;
    public bool historiaCompletada = false; // Esto desbloqueará la Biblioteca

    private void Awake()
    {
        // Configuramos el Singleton para que no se duplique al cambiar de escenas
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cargar minijuegos
            CargarProgreso(); // Leemos la memoria del disco duro al iniciar
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- FUNCIÓN INTERNA DE SEGURIDAD ---
    private string ObtenerClavePorSlot(string nombreClave)
    {
        int slotActivo = PlayerPrefs.GetInt("SlotActual", 1);
        return "Slot_" + slotActivo + "_" + nombreClave;
    }

    // --- FUNCIONES PARA GUARDAR EL PROGRESO ---

    public void SuperarNivel1()
    {
        nivel1Completado = true;
        PlayerPrefs.SetInt(ObtenerClavePorSlot("Nivel1"), 1);
        PlayerPrefs.Save();
        Debug.Log("¡Nivel 1 Superado! Guardado en la memoria del Slot " + PlayerPrefs.GetInt("SlotActual", 1));
    }

    public void SuperarNivel2()
    {
        nivel2Completado = true;
        PlayerPrefs.SetInt(ObtenerClavePorSlot("Nivel2"), 1);
        PlayerPrefs.Save();
        Debug.Log("¡Nivel 2 Superado! Guardado en la memoria del Slot " + PlayerPrefs.GetInt("SlotActual", 1));
    }

    public void SuperarHistoria()
    {
        historiaCompletada = true;
        PlayerPrefs.SetInt(ObtenerClavePorSlot("Historia"), 1);
        PlayerPrefs.Save();
        Debug.Log("¡Historia Terminada! Biblioteca Desbloqueada.");
    }

    // --- FUNCIONES PARA LEER Y REINICIAR ---

    public void CargarProgreso()
    {
        nivel1Completado = PlayerPrefs.GetInt(ObtenerClavePorSlot("Nivel1"), 0) == 1;
        nivel2Completado = PlayerPrefs.GetInt(ObtenerClavePorSlot("Nivel2"), 0) == 1;
        historiaCompletada = PlayerPrefs.GetInt(ObtenerClavePorSlot("Historia"), 0) == 1;
        
        Debug.Log("Progreso cargado con éxito para el Slot: " + PlayerPrefs.GetInt("SlotActual", 1));
    }

    public void BorrarPartida()
    {
        PlayerPrefs.DeleteKey(ObtenerClavePorSlot("Nivel1"));
        PlayerPrefs.DeleteKey(ObtenerClavePorSlot("Nivel2"));
        PlayerPrefs.DeleteKey(ObtenerClavePorSlot("Historia"));
        PlayerPrefs.Save();

        nivel1Completado = false;
        nivel2Completado = false;
        historiaCompletada = false;
        Debug.Log("Partida borrada desde cero para el Slot actual.");
    }

    public void BorrarAbsolutamenteTodo()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        nivel1Completado = false;
        nivel2Completado = false;
        historiaCompletada = false;
        Debug.Log("¡Toda la memoria de la aplicación ha sido eliminada!");
    }

    // ==========================================
    //          ¡AÑADIDO NUEVO Y SEGURO!
    // ==========================================

    // Borra físicamente un slot específico enviado desde el menú
    public void BorrarProgresoDeSlotEspecifico(int numeroSlot)
    {
        PlayerPrefs.DeleteKey("Slot_" + numeroSlot + "_Nivel1");
        PlayerPrefs.DeleteKey("Slot_" + numeroSlot + "_Nivel2");
        PlayerPrefs.DeleteKey("Slot_" + numeroSlot + "_Historia");
        PlayerPrefs.Save();

        // Si borramos el slot actual, limpiamos la RAM inmediatamente
        if (PlayerPrefs.GetInt("SlotActual", 1) == numeroSlot)
        {
            ReiniciarVariablesRAM();
        }
    }

    public void ReiniciarVariablesRAM()
    {
        nivel1Completado = false;
        nivel2Completado = false;
        historiaCompletada = false;
        Debug.Log("RAM reseteada: Nivel 1 bloqueado de nuevo.");
    }
}