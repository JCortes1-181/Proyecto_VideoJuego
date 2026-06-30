using UnityEngine;
using System.Collections;

public class EfectoPouMalo : MonoBehaviour
{
    private RectTransform rectTransform;

    [Header("Configuración del Estiramiento Chistoso")]
    public float tiempoEstiramiento = 0.4f; 
    
    public Vector3 escalaInicial = new Vector3(0.2f, 2.5f, 1f); 
    
    public Vector3 escalaFinal = new Vector3(2.2f, 0.6f, 1f);   

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (rectTransform != null)
        {
            rectTransform.localScale = escalaInicial;
            StartCoroutine(CoEstirarPantalla());
        }
    }

    private IEnumerator CoEstirarPantalla()
    {
        float tiempoPasado = 0f;

        while (tiempoPasado < tiempoEstiramiento)
        {
            tiempoPasado += Time.deltaTime;
            float progreso = tiempoPasado / tiempoEstiramiento;

            rectTransform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, progreso);
            yield return null;
        }

        rectTransform.localScale = escalaFinal;
    }
}