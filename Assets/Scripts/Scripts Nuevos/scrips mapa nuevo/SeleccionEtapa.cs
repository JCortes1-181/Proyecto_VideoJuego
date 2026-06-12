using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para las animaciones suaves (Corrutinas)

public class SeleccionEtapa : MonoBehaviour
{
    [Header("Configuración de la Etapa")]
    public string nombreEscenaMinijuego; 
    public Sprite fotoDelMinijuego;      

    [Header("Referencias de la UI")]
    public GameObject panelDetalle;      
    public Image visualizadorImagen;     
    public Button botonCerrar;        
    public Button botonJugar;           

    [Header("Configuración de la Animación")]
    [Tooltip("Tiempo en segundos que tarda en deslizarse el panel")]
    public float duracionAnimacion = 0.3f;

    // Posiciones matemáticas en la pantalla
    private Vector2 posicionOculto;
    private Vector2 posicionVisible;
    private RectTransform panelRectTransform;
    private bool estaAnimando = false;

    private void Awake()
    {
        // Guardamos el componente de posición de la UI
        if (panelDetalle != null)
        {
            panelRectTransform = panelDetalle.GetComponent<RectTransform>();
            
            // La posición donde lo dejaste en el Canvas es la posición VISIBLE
            posicionVisible = panelRectTransform.anchoredPosition;
            
            // Calculamos la posición OCULTA (sacándolo completamente por la derecha de la pantalla)
            posicionOculto = new Vector2(posicionVisible.x + panelRectTransform.rect.width + 200f, posicionVisible.y);
            
            // Aseguramos que empiece completamente oculto y apagado en el primer frame
            panelRectTransform.anchoredPosition = posicionOculto;
            panelDetalle.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // Si ya se está moviendo, no dejamos que el jugador sature el botón a clics
        if (!estaAnimando)
        {
            AbrirInterfazEtapa();
        }
    }

    private void AbrirInterfazEtapa()
    {
        panelDetalle.SetActive(true);

        if (fotoDelMinijuego != null)
        {
            visualizadorImagen.sprite = fotoDelMinijuego;
        }

        // Limpiamos eventos previos
        botonJugar.onClick.RemoveAllListeners();
        botonCerrar.onClick.RemoveAllListeners();

        // Configurar botones
        botonJugar.onClick.AddListener(() => {
            SceneManager.LoadScene(nombreEscenaMinijuego);
        });

        botonCerrar.onClick.AddListener(() => {
            if (!estaAnimando)
            {
                StartCoroutine(AnimarPanel(posicionOculto, false));
            }
        });

        // Lanzamos la animación para que entre desde la derecha
        StartCoroutine(AnimarPanel(posicionVisible, true));
    }

    // Lógica para deslizar el panel frame por frame de forma suave
    private IEnumerator AnimarPanel(Vector2 destino, bool activarAlFinal)
    {
        estaAnimando = true;
        Vector2 posicionInicial = panelRectTransform.anchoredPosition;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionAnimacion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / duracionAnimacion;
            
            // Usamos SmoothStep para que el movimiento empiece rápido y desacelere suavemente al llegar
            panelRectTransform.anchoredPosition = Vector2.Lerp(posicionInicial, destino, Mathf.SmoothStep(0f, 1f, porcentaje));
            yield return null;
        }

        panelRectTransform.anchoredPosition = destino;
        
        // Si la orden era ocultar, apagamos el objeto al terminar de deslizarse
        if (!activarAlFinal)
        {
            panelDetalle.SetActive(false);
        }

        estaAnimando = false;
    }
}
