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
    public string opcionSuperior; 
    public string opcionCentral;
    public string opcionInferior;  
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

    private List<DatosPreguntaDificil> preguntasDeEstaPartida = new List<DatosPreguntaDificil>();
    private DatosPreguntaDificil preguntaActual;
    private int rondaActual = 0; 
    private int seleccionActual = 1; 
    private float tiempoRestante;
    private bool yaRespondio = false;
    private bool juegoTerminado = false;

    void Start()
    {
        yaRespondio = false;
        juegoTerminado = false;
        rondaActual = 0;

        if (botonSuperior) botonSuperior.onClick.AddListener(() => { seleccionActual = 0; ActualizarPosicionFlecha(); });
        if (botonCentral) botonCentral.onClick.AddListener(() => { seleccionActual = 1; ActualizarPosicionFlecha(); });
        if (botonInferior) botonInferior.onClick.AddListener(() => { seleccionActual = 2; ActualizarPosicionFlecha(); });

        PrepararPreguntasDeLaPartida();

        CargarPregunta(rondaActual);
    }

    void Update()
    {
        if (juegoTerminado || yaRespondio) return;

        tiempoRestante -= Time.deltaTime;
        if (textoTiempo) textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString();

        if (tiempoRestante <= 0)
        {
            FinalizarModoDificil(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            seleccionActual = Mathf.Max(0, seleccionActual - 1);
            ActualizarPosicionFlecha();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            seleccionActual = Mathf.Min(2, seleccionActual + 1);
            ActualizarPosicionFlecha();
        }

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
        seleccionActual = 1;
        ActualizarPosicionFlecha();
    }

    void ActualizarPosicionFlecha()
    {
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
                FinalizarModoDificil(true);
            }
            else
            {
                StartCoroutine(SiguientePreguntaSecuencia());
            }
        }
        else
        {
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