using UnityEngine;
using TMPro; // 👈 Asegúrate de que tenga esto arriba

public class EfectoTituloFeria : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI textoMesh;

    [Header("Configuración del Balanceo (Lados)")]
    public float velocidadMovimiento = 3f;  
    public float amplitudMovimiento = 40f;  

    [Header("Configuración del Giro (Rotación)")]
    public float velocidadGiro = 4.5f;     
    public float anguloMaximoGiro = 8f;     

    [Header("Efecto Pulso (Escala)")]
    public bool usarEfectoPulso = true;
    public float velocidadPulso = 5f;
    public float escalaMinima = 0.95f;
    public float escalaMaxima = 1.05f;

    private Vector2 posicionInicial;
    private bool juegoTerminado = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        textoMesh = GetComponent<TextMeshProUGUI>(); 
        posicionInicial = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float desplazamientoX = Mathf.Sin(Time.time * velocidadMovimiento) * amplitudMovimiento;
        rectTransform.anchoredPosition = posicionInicial + new Vector2(desplazamientoX, 0f);

        float rotacionZ = Mathf.Sin(Time.time * velocidadGiro) * anguloMaximoGiro;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotacionZ);

        if (usarEfectoPulso)
        {
            float velocidadActual = juegoTerminado ? velocidadPulso * 1.5f : velocidadPulso;
            float factorPulso = Mathf.PingPong(Time.time * velocidadActual, 1f);
            float escalaActual = Mathf.Lerp(escalaMinima, escalaMaxima, factorPulso);
            rectTransform.localScale = new Vector3(escalaActual, escalaActual, 1f);
        }
    }

    public void CambiarTextoVictoria()
    {
        juegoTerminado = true;
        if (textoMesh != null)
        {
            textoMesh.text = "¡¡GANASTE!!";
            textoMesh.color = Color.green; 
        }
    }
}