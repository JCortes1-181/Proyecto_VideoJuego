using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjetoPou : MonoBehaviour
{
    private Button boton;
    private Image imagenComponente;
    private RectTransform rectTransform;
    
    [Header("Sprites de Pou")]
    public Sprite pouNormal;    
    public Sprite pouGolpeado;  

    [Header("Audio Individual")]
    public AudioSource fuenteAudio;
    public AudioClip sonidoAlSalir;   
    public AudioClip sonidoAlGolpear;

    [Header("Configuración de Animación")]
    public float alturaSubida = 130f; 
    public float velocidadAnimacion = 0.15f; 

    private Vector2 posicionEscondido;
    private Vector2 posicionAsomado;
    private bool yaLePegaron = false;
    private Coroutine corrutinaMovimiento;

    void Awake()
    {
        boton = GetComponent<Button>();
        imagenComponente = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        if (boton != null)
        {
            boton.onClick.AddListener(RecibirGolpe);
        }

        posicionEscondido = rectTransform.anchoredPosition;
        posicionAsomado = posicionEscondido + new Vector2(0f, alturaSubida);
    }

    public void ActivarPou()
    {
        yaLePegaron = false;
        if (imagenComponente != null && pouNormal != null)
        {
            imagenComponente.sprite = pouNormal; 
        }
        
        gameObject.SetActive(true);

        if (fuenteAudio != null && sonidoAlSalir != null)
        {
            fuenteAudio.PlayOneShot(sonidoAlSalir);
        }

        if (corrutinaMovimiento != null) StopCoroutine(corrutinaMovimiento);
        corrutinaMovimiento = StartCoroutine(CoMoverPou(posicionEscondido, posicionAsomado));
    }

    public void RecibirGolpe()
    {
        if (!gameObject.activeInHierarchy) return;
        if (yaLePegaron) return; 
        yaLePegaron = true;

        if (imagenComponente != null && pouGolpeado != null)
        {
            imagenComponente.sprite = pouGolpeado;
        }


        if (fuenteAudio != null && sonidoAlGolpear != null)
        {
            fuenteAudio.PlayOneShot(sonidoAlGolpear);
        }

        ControladorPou controlador = FindObjectOfType<ControladorPou>();
        if (controlador != null)
        {
            controlador.SumarPunto();
        }

        if (gameObject.activeInHierarchy)
        {
            if (corrutinaMovimiento != null) StopCoroutine(corrutinaMovimiento);
            corrutinaMovimiento = StartCoroutine(CoEsconderRapido());
        }
    }

    private IEnumerator CoMoverPou(Vector2 inicio, Vector2 fin)
    {
        float tiempo = 0f;
        rectTransform.anchoredPosition = inicio;

        while (tiempo < velocidadAnimacion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / velocidadAnimacion;
            rectTransform.anchoredPosition = Vector2.Lerp(inicio, fin, Mathf.Sin(progreso * Mathf.PI * 0.5f));
            yield return null;
        }
        rectTransform.anchoredPosition = fin;
    }

    void OnDisable()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = posicionEscondido;
        }
    }

    private IEnumerator CoEsconderRapido()
    {
        yield return new WaitForSeconds(0.25f);

        float tiempo = 0f;
        Vector2 posicionActual = rectTransform.anchoredPosition;

        while (tiempo < velocidadAnimacion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / velocidadAnimacion;
            rectTransform.anchoredPosition = Vector2.Lerp(posicionActual, posicionEscondido, progreso);
            yield return null;
        }

        rectTransform.anchoredPosition = posicionEscondido;
        gameObject.SetActive(false);
    }
}