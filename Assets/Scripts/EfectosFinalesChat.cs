using UnityEngine;

public class EfectosFinalChat : MonoBehaviour
{
    [Header("Efecto de Victoria (Explosión)")]
    public GameObject prefabExplosion;   
    public Transform posicionExplosion;  

    [Header("Efecto de Derrota (Audio)")]
    public AudioSource altavozSonido;  
    public AudioClip sonidoDerrota;      


public void ActivarVictoria()
    {
        if (prefabExplosion != null && posicionExplosion != null)
        {
            // 1. Creamos la explosión
            GameObject explosionClonada = Instantiate(prefabExplosion);
            
            Canvas canvasActual = FindObjectOfType<Canvas>();
            if (canvasActual != null)
            {
                // 2. La hacemos hija del Canvas
                explosionClonada.transform.SetParent(canvasActual.transform, false);
                explosionClonada.transform.SetAsLastSibling(); // Al frente de todo

                // 3. LA CORRECCIÓN: Copiamos la posición exacta de la UI de forma segura
                RectTransform rectExplosion = explosionClonada.GetComponent<RectTransform>();
                RectTransform rectDestino = posicionExplosion.GetComponent<RectTransform>();

                if (rectExplosion != null && rectDestino != null)
                {
                    // Esto iguala los pivotes y la posición exacta en la pantalla
                    rectExplosion.anchoredPosition = rectDestino.anchoredPosition;
                }
            }

            Debug.Log("¡BOOM! Explosión reubicada perfectamente en la UI.");
        }
    }
    public void ActivarDerrota()
    {
        if (altavozSonido != null && sonidoDerrota != null)
        {
            altavozSonido.PlayOneShot(sonidoDerrota);
            Debug.Log("Sonido triste reproducido desde el script de efectos.");
        }
    }
}