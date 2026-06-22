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

    // --- FUNCIONES PARA GUARDAR EL PROGRESO ---

    public void SuperarNivel1()
    {
        nivel1Completado = true;
        PlayerPrefs.SetInt("Nivel1", 1); // 1 significa verdadero, 0 falso
        PlayerPrefs.Save();
        Debug.Log("¡Nivel 1 Superado! Guardado en memoria.");
    }

    public void SuperarNivel2()
    {
        nivel2Completado = true;
        PlayerPrefs.SetInt("Nivel2", 1);
        PlayerPrefs.Save();
        Debug.Log("¡Nivel 2 Superado! Guardado en memoria.");
    }

    public void SuperarHistoria()
    {
        historiaCompletada = true;
        PlayerPrefs.SetInt("Historia", 1);
        PlayerPrefs.Save();
        Debug.Log("¡Historia Terminada! Biblioteca Desbloqueada.");
    }

    // --- FUNCIONES PARA LEER Y REINICIAR ---

    private void CargarProgreso()
    {
        // Leemos la memoria. Si no existe el dato, devuelve 0 por defecto (falso)
        nivel1Completado = PlayerPrefs.GetInt("Nivel1", 0) == 1;
        nivel2Completado = PlayerPrefs.GetInt("Nivel2", 0) == 1;
        historiaCompletada = PlayerPrefs.GetInt("Historia", 0) == 1;
    }

    // Función útil por si quieres poner un botón de "Borrar Partida" en el menú
    public void BorrarPartida()
    {
        PlayerPrefs.DeleteAll();
        nivel1Completado = false;
        nivel2Completado = false;
        historiaCompletada = false;
        Debug.Log("Partida borrada desde cero.");
    }
}
