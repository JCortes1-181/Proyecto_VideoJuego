using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; // Necesario para usar la Lista

[System.Serializable]
public struct DatosPregunta
{
    [TextArea] public string pregunta;
    public string opcionIzquierda;
    public string opcionDerecha;
    [Tooltip("0 = La correcta es la Izquierda (A) | 1 = La correcta es la Derecha (D)")]
    public int indexCorrecto; 
}

public class MinijuegoCita : MonoBehaviour // Nombre corregido en singular para evitar errores
{
    [Header("PJ")]
    public Image imagenPersonaje; 
    public Sprite Normal, Feliz, enojado;

    [Header("UI y Opciones")]
    public TextMeshProUGUI textoDialogo; 
    public TextMeshProUGUI textoTiempo; 
    public GameObject panelOpciones; 
    public Button botonIzquierdo;    
    public Button botonDerecho;   

    [Header("Textos de las Opciones (Hijos de los Botones)")]
    public TextMeshProUGUI textoOpcionIzquierda;
    public TextMeshProUGUI textoOpcionDerecha;

    [Header("Indicador Visual")]
    public RectTransform trianguloIndicador; 
    public float ajusteY = 50f; 

    [Header("Ajustes de Tiempo")]
    public float tiempoLimite = 4f; 

    [Header("Sonido de Error")]
    public AudioSource fuenteAudio; 
    public AudioClip sonidoEquivocado; 

    [Header("Mensajes Globales de Fin")]
    [TextArea] public string mensajeVictoria = "";
    [TextArea] public string mensajeDerrota = "";

    [Header("Banco de Preguntas")]
    public List<DatosPregunta> listaDePreguntas = new List<DatosPregunta>();

    private int seleccionActual = 0; 
    private bool yaRespondio = false;
    private float tiempoRestante;
    private DatosPregunta preguntaActual; // La pregunta elegida para esta ronda

    void Start() {
        tiempoRestante = tiempoLimite;
        if (panelOpciones) panelOpciones.SetActive(true);

        // --- SELECCIÓN ALEATORIA DE LA PREGUNTA ---
        if (listaDePreguntas != null && listaDePreguntas.Count > 0)
        {
            int indiceAleatorio = Random.Range(0, listaDePreguntas.Count);
            preguntaActual = listaDePreguntas[indiceAleatorio];

            // Asignamos los textos de la pregunta elegida a la UI
            if (textoDialogo) textoDialogo.text = preguntaActual.pregunta;
            if (textoOpcionIzquierda) textoOpcionIzquierda.text = preguntaActual.opcionIzquierda;
            if (textoOpcionDerecha) textoOpcionDerecha.text = preguntaActual.opcionDerecha;
        }
        else
        {
            Debug.LogError("¡El banco de preguntas está vacío! Añade elementos en el Inspector.");
        }

        if (trianguloIndicador) trianguloIndicador.gameObject.SetActive(true);

        // Forzar el primer resaltado al iniciar
        ActualizarResaltado();
    }

    void Update() {
        if (yaRespondio) return;

        // --- SISTEMA DE TIEMPO ORIGINAL ---
        tiempoRestante -= Time.deltaTime;
        if (textoTiempo) {
            int seg = Mathf.CeilToInt(tiempoRestante);
            textoTiempo.text = (seg < 0 ? 0 : seg).ToString();
        }

        if (tiempoRestante <= 0) {
            ReproducirError();
            Finalizar(false);
            return;
        }

        // --- RECUPERADO: SISTEMA DE MOVIMIENTO CON A / D ---
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            seleccionActual = 0;
            ActualizarResaltado();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            seleccionActual = 1;
            ActualizarResaltado();
        }

        // --- RECUPERADO: CONFIRMAR CON ESPACIO O ENTER ---
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            ConfirmarRespuesta();
        }
    }

    void ActualizarResaltado() {
        Button botonActual = (seleccionActual == 0) ? botonIzquierdo : botonDerecho;
        if (botonActual != null) botonActual.Select();

        // MOVER EL TRIÁNGULO ORIGINAL
        if (trianguloIndicador != null && botonActual != null) {
            trianguloIndicador.position = botonActual.transform.position;
            trianguloIndicador.anchoredPosition += new Vector2(0, ajusteY);
        }
    }

    void ConfirmarRespuesta() {
        // Ahora comprueba dinámicamente según el index correcto de la pregunta elegida
        bool esCorrecto = (seleccionActual == preguntaActual.indexCorrecto); 
        if (!esCorrecto) ReproducirError();
        Finalizar(esCorrecto);
    }

    void ReproducirError() {
        if (fuenteAudio && sonidoEquivocado) fuenteAudio.PlayOneShot(sonidoEquivocado);
    }

    void Finalizar(bool ganado) {
        yaRespondio = true;
        if (panelOpciones) panelOpciones.SetActive(false);
        StartCoroutine(SecuenciaFinal(ganado));
    }

    IEnumerator SecuenciaFinal(bool ganado) {
        if (ganado) {
            if (imagenPersonaje && Feliz) imagenPersonaje.sprite = Feliz;
            if (textoDialogo) textoDialogo.text = mensajeVictoria;
        } else {
            if (imagenPersonaje && enojado) imagenPersonaje.sprite = enojado;
            if (textoDialogo) textoDialogo.text = (tiempoRestante <= 0) ? "¡Te tardaste demasiado!" : mensajeDerrota;
            ControladorVidas.vidasGlobales--; 
        }

        yield return new WaitForSeconds(3f); // Volvemos a tus 3 segundos originales
        SceneManager.LoadScene("FreddyFazbear"); // Tu escena de carga original restaurada
    }
}