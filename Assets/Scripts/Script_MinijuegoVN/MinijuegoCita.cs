using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinijuegoCitas : MonoBehaviour
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

    [Header("Indicador Visual")]
    public RectTransform trianguloIndicador; // Arrastra aquí tu triangulito
    public float ajusteY = 50f; // Para mover el triángulo un poco arriba del botón

    [Header("Ajustes de Tiempo")]
    public float tiempoLimite = 4f; 

    [Header("Sonido de Error")]
    public AudioSource fuenteAudio; 
    public AudioClip sonidoEquivocado; 

    [Header("Mensajes")]
    [TextArea] public string preguntaInicial = "";
    [TextArea] public string mensajeVictoria = "";
    [TextArea] public string mensajeDerrota = "";

    private int seleccionActual = 0; 
    private bool yaRespondio = false;
    private float tiempoRestante;

    void Start() {
        tiempoRestante = tiempoLimite;
        if (panelOpciones) panelOpciones.SetActive(true);
        if (textoDialogo) textoDialogo.text = preguntaInicial;
        if (imagenPersonaje && Normal) imagenPersonaje.sprite = Normal;

        // El triángulo aparece al inicio
        if (trianguloIndicador) trianguloIndicador.gameObject.SetActive(true);

        ActualizarResaltado();
    }

    void Update() {
        if (yaRespondio) return;

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

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            seleccionActual = 0;
            ActualizarResaltado();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            seleccionActual = 1;
            ActualizarResaltado();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            ConfirmarRespuesta();
        }
    }

    void ActualizarResaltado() {
        Button botonActual = (seleccionActual == 0) ? botonIzquierdo : botonDerecho;
        botonActual.Select();

        // MOVER EL TRIÁNGULO
        if (trianguloIndicador != null && botonActual != null) {
            // Movemos el triángulo a la posición del botón
            trianguloIndicador.position = botonActual.transform.position;
            
            // Le sumamos un poco de altura para que no tape el texto
            trianguloIndicador.anchoredPosition += new Vector2(0, ajusteY);
        }
    }

    void ConfirmarRespuesta() {
        bool esCorrecto = (seleccionActual == 0); 
        if (!esCorrecto) ReproducirError();
        Finalizar(esCorrecto);
    }

    void ReproducirError() {
        if (fuenteAudio && sonidoEquivocado) fuenteAudio.PlayOneShot(sonidoEquivocado);
    }

    void Finalizar(bool ganado) {
        yaRespondio = true;
        
        // Al desactivar el panel, el triángulo (si es hijo) desaparece solo
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

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("FreddyFazbear");
    }
}