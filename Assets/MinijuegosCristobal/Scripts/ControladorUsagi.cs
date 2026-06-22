using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ControladorUsagi : MinijuegoBase
{
    [Header("UI Componentes")]
    public TextMeshProUGUI textoPapelNumero;      
    public TextMeshProUGUI textoPantallaCelular;  

    [Header("Visuales de Usagi en la Pantalla")]
    [Tooltip("Arrastra aquí la Image de Usagi que está dentro de la pantalla del Nokia")]
    public Image componenteImagenUsagi;           
    public Sprite fotoUsagiNormal;     // La foto de inicio
    public Sprite fotoUsagiVictoria;   // Usagi feliz contestando
    public Sprite fotoUsagiDerrota;    // Usagi enojado/asustado

    [Header("Jumpscare Game Over (🔥 NUEVO)")]
    [Tooltip("Arrastra aquí el objeto 'Jumpscare_Usagi' que ocupa toda la pantalla y empieza apagado")]
    public GameObject objetoJumpscare; 
    public AudioSource fuenteEfectos;   // Parlante para el grito o ruido de error
    public AudioClip sonidoSusto;      // Audio fuerte para el jumpscare

    [Header("Configuración del Número")]
    public int cantidadDigitos = 4;

    private string numeroCorrecto = "";
    private string numeroDigitado = "";

    protected override void Start()
    {
        // 7 segundos para reaccionar
        tiempoLimite = 7f;
        base.Start();

        textoPantallaCelular.text = "";

        // Dejamos a Usagi en su estado normal al arrancar
        if (componenteImagenUsagi != null && fotoUsagiNormal != null)
        {
            componenteImagenUsagi.sprite = fotoUsagiNormal;
        }

        // Nos aseguramos de que el susto empiece apagado al iniciar la partida
        if (objetoJumpscare != null)
        {
            objetoJumpscare.SetActive(false);
        }

        GenerarNumeroAleatorio();
    }

    private void GenerarNumeroAleatorio()
    {
        numeroCorrecto = "";
        for (int i = 0; i < cantidadDigitos; i++)
        {
            int digitoAyuda = Random.Range(0, 10);
            numeroCorrecto += digitoAyuda.ToString();
        }
        textoPapelNumero.text = numeroCorrecto;
    }

    public void PresionarBotonNumero(int numeroPresionado)
    {
        if (juegoTerminado) return;

        numeroDigitado += numeroPresionado.ToString();
        textoPantallaCelular.text = numeroDigitado;

        int indiceActual = numeroDigitado.Length - 1;

        // Si se equivoca ➡️ Derrota
        if (numeroDigitado[indiceActual] != numeroCorrecto[indiceActual])
        {
            TerminarJuego(false);
            return;
        }

        // Si completa el número ➡️ Victoria
        if (numeroDigitado == numeroCorrecto)
        {
            TerminarJuego(true);
        }
    }

    // El Update heredado de MinijuegoBase se encarga de restar el tiempo.
    // Cuando el tiempo llega a cero, automáticamente llamará a TerminarJuego(false).

    public override void TerminarJuego(bool victoria)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (victoria)
        {
            Debug.Log("¡Usagi contestó feliz!");
            if (componenteImagenUsagi != null && fotoUsagiVictoria != null)
            {
                componenteImagenUsagi.sprite = fotoUsagiVictoria; // Cambia a foto feliz en el Nokia
            }
        }
        else
        {
            Debug.Log("¡Pum! Perdiste. Activando Jumpscare...");
            
            // Cambiamos la miniatura del celular por si acaso
            if (componenteImagenUsagi != null && fotoUsagiDerrota != null)
            {
                componenteImagenUsagi.sprite = fotoUsagiDerrota; 
            }

            // 🔥 Lanzamos el jumpscare gigante
            ActivarDerrotaJumpscare();
        }

        // Espera los 2 segundos heredados de MinijuegoBase para volver a la oficina
        StartCoroutine(EsperarYRegresar(victoria));
    }

    private void ActivarDerrotaJumpscare()
    {
        if (objetoJumpscare != null)
        {
            // Ponemos la foto terrorífica/enojada en el componente grande antes de mostrarlo
            Image imagenSusto = objetoJumpscare.GetComponent<Image>();
            if (imagenSusto != null && fotoUsagiDerrota != null)
            {
                imagenSusto.sprite = fotoUsagiDerrota;
            }

            objetoJumpscare.SetActive(true);
            
            // Reproducir el sonido estridente
            if (fuenteEfectos != null && sonidoSusto != null)
            {
                fuenteEfectos.PlayOneShot(sonidoSusto);
            }

            // Iniciar la animación de escalado violento
            StartCoroutine(CoAnimarSusto());
        }
    }

    private IEnumerator CoAnimarSusto()
    {
        RectTransform rect = objetoJumpscare.GetComponent<RectTransform>();
        if (rect == null) yield break;

        float tiempo = 0f;
        float duracion = 0.15f; // Súper rápido (150 milisegundos) para que asuste

        // Nace muy pequeño desde el centro de la pantalla
        rect.localScale = new Vector3(0.1f, 0.1f, 1f);

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;

            // Crece superando el tamaño normal de la pantalla (impacto visual)
            float escala = Mathf.Lerp(0.1f, 1.4f, progreso);
            rect.localScale = new Vector3(escala, escala, 1f);

            yield return null;
        }

        // Se estabiliza tapando perfectamente todo el monitor
        rect.localScale = Vector3.one;
    }
}