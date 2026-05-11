using UnityEngine;

public class EstadoGlobal : MonoBehaviour
{
    public static EstadoGlobal instancia;
    public int vidas = 3; 

    void Awake()
    {
        
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
            
        }
    }
}