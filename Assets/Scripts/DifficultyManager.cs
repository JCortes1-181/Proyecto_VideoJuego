using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    // 1. Añadimos el selector de modos
    public enum ModoJuego { Normal, Desafio }
    
    [Header("Selector de Modo")]
    public ModoJuego modoActual = ModoJuego.Normal;

    [Header("Configuración Modo Desafío (Progresivo)")]
    public float velocidadinicial = 1f;
    public float radiodeincremento = 0.05f; 
    public float velocidadmaxima = 5f; 

    // Esta es la variable que lee tu GridEnemigos.cs
    public float CurrentMultiplier { get; private set; }

    private void Awake()
    {
        // Sistema de seguridad Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para que no se borre al cambiar de minijuegos
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Al iniciar, si es modo normal el multiplicador es 1. Si no, arranca con tu valor inicial.
        if (modoActual == ModoJuego.Normal)
        {
            CurrentMultiplier = 1f;
        }
        else
        {
            CurrentMultiplier = velocidadinicial;
        }
    }

    private void Update()
    {
        // === AQUÍ SE DECIDE TODO ===
        if (modoActual == ModoJuego.Desafio)
        {
            // Si es Modo Desafío, ejecuta TU lógica original (aumenta con el tiempo)
            if (CurrentMultiplier < velocidadmaxima)
            {
                CurrentMultiplier += radiodeincremento * Time.deltaTime;
            }
        }
        else
        {
            // MODO NORMAL: Se clava en 1f para siempre. Matemática neutra, nada cambia.
            CurrentMultiplier = 1f;
        }
    }

    // Función por si necesitas cambiar el modo desde un botón del menú principal
    public void CambiarModo(int modo)
    {
        modoActual = (ModoJuego)modo;
    }
}