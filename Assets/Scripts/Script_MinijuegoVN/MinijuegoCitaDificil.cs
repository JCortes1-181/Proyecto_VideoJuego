using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct DatosPreguntaDificil
{
    [TextArea] public string pregunta;
    public string opcionSuperior; // Antes Izquierda
    public string opcionCentral;
    public string opcionInferior;  // Antes Derecha
    [Tooltip("0 = Superior (W) | 1 = Centro | 2 = Inferior (S)")]
    public int indexCorrecto; 
}

public class MinijuegoCitaDificil : MonoBehaviour
{
    [Header("Componentes de Personaje")]
    public Image imagenPersonaje; 
    public Sprite spriteNormal;
    public Sprite spriteFeliz;
    public Sprite spriteEnojado;

    [Header("Componentes de UI (3 Botones Verticales)")]
    public TextMeshProUGUI textoDialogo; 
    public TextMeshProUGUI textoTiempo; 
    public TextMeshProUGUI textoContadorPreguntas; 
    public GameObject panelOpciones; 
    
    [Space]
    public Button botonSuperior;    
    public Button botonCentral; 
    public Button botonInferior;   
    
    [Space]
    public TextMeshProUGUI textoOpcionSuperior;
    public TextMeshProUGUI textoOpcionCentral; 
    public TextMeshProUGUI textoOpcionInferior;

    [Header("Indicador Visual (Flecha)")]
    public RectTransform trianguloIndicador; 
    public float ajusteY = 50f; 

    [Header("Ajustes de Tiempo y Mensajes")]
    public float tiempoPorPregunta = 5f; 
    [TextArea] public string mensajeVictoria = "¡Increíble! Me la pasé de maravilla.";
    [TextArea] public string mensajeDerrota = "Qué decepción... adiós.";

    [Header("Banco de Preguntas")]
    [Tooltip("Agrega tus preguntas con las 3 respuestas posibles.")]
    public List<DatosPreguntaDificil> bancoDePreguntas;

    // Gestión interna
    private List<DatosPreguntaDificil> preguntasDeEstaPartida = new List<DatosPreguntaDificil>();
    private DatosPreguntaDificil preguntaActual;
    private int rondaActual = 0; // Cambiará de 0 a 1 (para completar las 2 preguntas)
    private int seleccionActual = 1; // Empezamos en el centro (0=Superior, 1=Centro, 2=Inferior)
    private float tiempoRestante;
    private bool yaRespondio = false;
    private bool juegoTerminado = false;

    void Start()
    {
        yaRespondio = false;
        juegoTerminado = false;
        rondaActual = 0;

        // Configuración de clicks de mouse por si acaso
        if (botonSuperior) botonSuperior.onClick.AddListener(() => { seleccionActual = 0; ActualizarPosicionFlecha(); });
        if (botonCentral) botonCentral.onClick.AddListener(() => { seleccionActual = 1; ActualizarPosicionFlecha(); });
        if (botonInferior) botonInferior.onClick.AddListener(() => { seleccionActual = 2; ActualizarPosicionFlecha(); });

        // Seleccionar 2 preguntas aleatorias del banco sin repetir
        PrepararPreguntasDeLaPartida();

        // Lanzar la primera pregunta
        CargarPregunta(rondaActual);
    }

    void Update()
    {
        if (juegoTerminado || yaRespondio) return;

        // Contador de tiempo
        tiempoRestante -= Time.deltaTime;
        if (textoTiempo) textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString();

        if (tiempoRestante <= 0)
        {
            FinalizarModoDificil(false);
            return;
        }

        // --- MOVIMIENTO DE LA FLECHA CON W Y S (O FLECHAS) ---
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Subir en la lista implica restar índice (ej: de Centro(1) a Superior(0))
            seleccionActual = Mathf.Max(0, seleccionActual - 1);
            ActualizarPosicionFlecha();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Bajar en la lista implica sumar índice (ej: de Centro(1) a Inferior(2))
            seleccionActual = Mathf.Min(2, seleccionActual + 1);
            ActualizarPosicionFlecha();
        }

        // Confirmar respuesta con Espacio o Enter
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmarRespuesta();
        }
    }

    void PrepararPreguntasDeLaPartida()
    {
        if (bancoDePreguntas.Count < 2)
        {
            Debug.LogError("¡Necesitas al menos 2 preguntas en el banco para jugar!");
            return;
        }

        List<DatosPreguntaDificil> copiaBanco = new List<DatosPreguntaDificil>(bancoDePreguntas);
        preguntasDeEstaPartida.Clear();

        // Extraemos exactamente 2 preguntas al azar
        for (int i = 0; i < 2; i++)
        {
            int indiceAleatorio = Random.Range(0, copiaBanco.Count);
            preguntasDeEstaPartida.Add(copiaBanco[indiceAleatorio]);
            copiaBanco.RemoveAt(indiceAleatorio);
        }
    }

    void CargarPregunta(int indiceRonda)
    {
        yaRespondio = false;
        preguntaActual = preguntasDeEstaPartida[indiceRonda];

        // Asignar textos a los 3 botones verticales
        if (textoDialogo) textoDialogo.text = preguntaActual.pregunta;
        if (textoOpcionSuperior) textoOpcionSuperior.text = preguntaActual.opcionSuperior;
        if (textoOpcionCentral) textoOpcionCentral.text = preguntaActual.opcionCentral;
        if (textoOpcionInferior) textoOpcionInferior.text = preguntaActual.opcionInferior;
        
        if (textoContadorPreguntas != null)
        {
            textoContadorPreguntas.text = "Progreso: " + (indiceRonda + 1) + " / 2";
        }

        if (imagenPersonaje && spriteNormal) imagenPersonaje.sprite = spriteNormal;

        tiempoRestante = tiempoPorPregunta;
        
        // La flecha aparece por defecto en el botón del Centro (1) al cambiar de pregunta
        seleccionActual = 1;
        ActualizarPosicionFlecha();
    }

    void ActualizarPosicionFlecha()
    {
        // Buscamos cuál es el botón según el índice actual (0, 1 o 2)
        Button botonObjetivo = null;
        if (seleccionActual == 0) botonObjetivo = botonSuperior;
        else if (seleccionActual == 1) botonObjetivo = botonCentral;
        else if (seleccionActual == 2) botonObjetivo = botonInferior;

        if (trianguloIndicador != null && botonObjetivo != null)
        {
            trianguloIndicador.position = botonObjetivo.transform.position;
            trianguloIndicador.anchoredPosition += new Vector2(0, ajusteY);
        }
    }

    void ConfirmarRespuesta()
    {
        yaRespondio = true; 

        bool esCorrecto = (seleccionActual == preguntaActual.indexCorrecto);

        if (esCorrecto)
        {
            rondaActual++;

            if (rondaActual >= 2)
            {
                // Respondió correctamente las 2 preguntas consecutivas -> VICTORIA
                FinalizarModoDificil(true);
            }
            else
            {
                // Pasó la primera pregunta, vamos a la segunda
                StartCoroutine(SiguientePreguntaSecuencia());
            }
        }
        else
        {
            // Un fallo en modo difícil te hace perder de inmediato
            FinalizarModoDificil(false);
        }
    }

    IEnumerator SiguientePreguntaSecuencia()
    {
        if (imagenPersonaje && spriteFeliz) imagenPersonaje.sprite = spriteFeliz;
        if (textoDialogo) textoDialogo.text = "¡Bien hecho...! Falta una más.";
        
        yield return new WaitForSeconds(1.2f); 
        
        CargarPregunta(rondaActual);
    }

    void FinalizarModoDificil(bool ganado)
    {
        juegoTerminado = true;
        yaRespondio = true;

        if (panelOpciones) panelOpciones.SetActive(false);
        if (trianguloIndicador) trianguloIndicador.gameObject.SetActive(false);

        StartCoroutine(SecuenciaFinal(ganado));
    }

    IEnumerator SecuenciaFinal(bool ganado)
    {
        if (ganado)
        {
            if (imagenPersonaje && spriteFeliz) imagenPersonaje.sprite = spriteFeliz;
            if (textoDialogo) textoDialogo.text = mensajeVictoria;
        }
        else
        {
            if (imagenPersonaje && spriteEnojado) imagenPersonaje.sprite = spriteEnojado;
            if (textoDialogo)
            {
                textoDialogo.text = (tiempoRestante <= 0) ? "¡Te quedaste sin tiempo!" : mensajeDerrota;
            }

            ControladorVidas.vidasGlobales--;
        }

        yield return new WaitForSeconds(3f); 
        SceneManager.LoadScene("Nivel3");
    }
}