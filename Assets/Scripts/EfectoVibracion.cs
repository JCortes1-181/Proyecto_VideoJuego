using System.Collections;
using UnityEngine;

public class EfectoVibracion : MonoBehaviour
{
    [Header("Arrastra el 'Contenedor_Juego' aquí")]
    public RectTransform contenedorEstruendo;
    
    private Vector2 posicionOriginalUI;

    void Start()
    {

        if (contenedorEstruendo != null)
        {
            posicionOriginalUI = contenedorEstruendo.anchoredPosition;
        }
    }

    public IEnumerator Shake(float duracion, float intensidad)
    {
        if (contenedorEstruendo == null) yield break;

        float tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {

            float x = Random.Range(-1f, 1f) * intensidad;
            float y = Random.Range(-1f, 1f) * intensidad;


            contenedorEstruendo.anchoredPosition = new Vector2(posicionOriginalUI.x + x, posicionOriginalUI.y + y);

            tiempoPasado += Time.deltaTime;
            yield return null;
        }


        contenedorEstruendo.anchoredPosition = posicionOriginalUI;
    }
}