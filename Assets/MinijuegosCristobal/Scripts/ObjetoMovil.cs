using UnityEngine;

public class ObjetoMovil : MonoBehaviour
{
    private Vector2 direccion;
    private float velocidad = 150f;
    private RectTransform rectTransform;
    private RectTransform rectCanvas;

    private float minX, maxX, minY, maxY;

    public void Inicializar(RectTransform canvas, float anchoPanelDerecho)
    {
        rectTransform = GetComponent<RectTransform>();
        rectCanvas = canvas;

        direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        float anchoMedio = rectCanvas.rect.width / 2f;
        float altoMedio = rectCanvas.rect.height / 2f;

        minX = -anchoMedio + (rectTransform.rect.width / 2f);
        maxX = anchoMedio - anchoPanelDerecho - (rectTransform.rect.width / 2f);
        minY = -altoMedio + (rectTransform.rect.height / 2f);
        maxY = altoMedio - (rectTransform.rect.height / 2f);
    }

    void Update()
    {
        if (rectTransform == null || Time.deltaTime == 0) return;

        rectTransform.anchoredPosition += direccion * velocidad * Time.deltaTime;

        Vector2 pos = rectTransform.anchoredPosition;

        if (pos.x <= minX)
        {
            pos.x = minX;
            direccion.x = Mathf.Abs(direccion.x); 
        }
        else if (pos.x >= maxX)
        {
            pos.x = maxX;
            direccion.x = -Mathf.Abs(direccion.x); 
        }

        if (pos.y <= minY)
        {
            pos.y = minY;
            direccion.y = Mathf.Abs(direccion.y); 
        }
        else if (pos.y >= maxY)
        {
            pos.y = maxY;
            direccion.y = -Mathf.Abs(direccion.y); 
        }

        rectTransform.anchoredPosition = pos;
    }
}