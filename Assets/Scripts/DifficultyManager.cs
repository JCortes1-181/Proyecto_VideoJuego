using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;


    public enum ModoJuego { Normal, Desafio }
    
    [Header("Selector de Modo")]
    public ModoJuego modoActual = ModoJuego.Normal;

    [Header("Configuración Modo Desafío (Progresivo)")]
    public float velocidadinicial = 1f;
    public float radiodeincremento = 0.05f; 
    public float velocidadmaxima = 5f; 


    public float CurrentMultiplier { get; private set; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
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
        if (modoActual == ModoJuego.Desafio)
        {
            if (CurrentMultiplier < velocidadmaxima)
            {
                CurrentMultiplier += radiodeincremento * Time.deltaTime;
            }
        }
        else
        {
            CurrentMultiplier = 1f;
        }
    }

    public void CambiarModo(int modo)
    {
        modoActual = (ModoJuego)modo;
    }
}