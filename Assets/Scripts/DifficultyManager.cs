using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Configuración de Dificultad")]
    public float velocidadinicial = 1f;
    public float radiodeincremento = 0.05f; 
    public float velocidadmaxima = 5f; 

    public float CurrentMultiplier { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        CurrentMultiplier = velocidadinicial;
    }

    private void Update()
    {
        if (CurrentMultiplier < velocidadmaxima)
        {
            CurrentMultiplier += radiodeincremento * Time.deltaTime;
        }
    }
}