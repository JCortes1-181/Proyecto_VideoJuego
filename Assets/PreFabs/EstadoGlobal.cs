using UnityEngine;

public class EstadoGlobal : MonoBehaviour
{
    public static EstadoGlobal instancia;
    public int vidas = 3; // Empiezas con 3 vidas

    void Awake()
    {
        // Esto hace que este objeto no se destruya al cambiar de escena
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestarVida()
    {
        vidas--;
        Debug.Log("Vidas restantes: " + vidas);
        
        if (vidas <= 0)
        {
            Debug.Log("Game Over Total");
            // Aquí podrías mandar a una escena de Perdiste
        }
    }
}