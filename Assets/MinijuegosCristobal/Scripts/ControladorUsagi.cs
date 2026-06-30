using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ControladorUsagi : MinijuegoBase
{
    [Header("UI Componentes")]
    public TextMeshProUGUI textoPapelNumero;      
    public TextMeshProUGUI textoPantallaCelular;  
    public TextMeshProUGUI textoInstrucciones;  

    [Header("Visuales de Usagi en la Pantalla")]
    [Tooltip("Arrastra aquí la Image de Usagi que está dentro de la pantalla del Nokia")]
    public Image componenteImagenUsagi;           
    public Sprite fotoUsagiNormal;    
    public Sprite fotoUsagiVictoria;  
    public Sprite fotoUsagiDerrota;   

    [Header("Jumpscare Game Over (Integrado)")]
    [Tooltip("Arrastra aquí el objeto de la jerarquía que ocupa toda la pantalla y empieza apagado")]
    public GameObject objetoJumpscare; 
    public AudioSource fuenteEfectos;  
    public AudioSource fuenteMusica;   
    
    [Header("Audios del Minijuego (.mp3)")]
    public AudioClip clipMusicaFondo;   
    public AudioClip sonidoBoton;        
    public AudioClip sonidoVictoria;    
    public AudioClip sonidoSusto;      

    [Header("Configuración del Número")]
    public int cantidadDigitos = 4;

    private string numeroCorrecto = "";
    private string numeroDigitado = "";

    protected override void Start()
    {
        tiempoLimite = 7f;
        base.Start();

        textoPantallaCelular.text = "";

        if (textoInstrucciones != null)
        {
            textoInstrucciones.text = "Marca los números para llamar a Usagi";
            textoInstrucciones.color = Color.white; 
        }

        if (componenteImagenUsagi != null && fotoUsagiNormal != null)
        {
            componenteImagenUsagi.sprite = fotoUsagiNormal;
        }

        if (objetoJumpscare != null)
        {
            objetoJumpscare.SetActive(false);
        }

        if (fuenteMusica != null && clipMusicaFondo != null)
        {
            fuenteMusica.clip = clipMusicaFondo;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
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

        if (fuenteEfectos != null && sonidoBoton != null)
        {
            fuenteEfectos.PlayOneShot(sonidoBoton);
        }

        numeroDigitado += numeroPresionado.ToString();
        textoPantallaCelular.text = numeroDigitado;

        int indiceActual = numeroDigitado.Length - 1;

        if (numeroDigitado[indiceActual] != numeroCorrecto[indiceActual])
        {
            TerminarJuego(false);
            return;
        }

        if (numeroDigitado == numeroCorrecto)
        {
            TerminarJuego(true);
        }
    }

    public override void TerminarJuego(bool victoria)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (fuenteMusica != null) fuenteMusica.Stop();

        if (victoria)
        {
            Debug.Log("¡Usagi contestó feliz!");
            
            if (textoInstrucciones != null)
            {
                textoInstrucciones.text = "¡Llamada completada con éxito!";
                textoInstrucciones.color = Color.green;
            }

            if (componenteImagenUsagi != null && fotoUsagiVictoria != null)
            {
                componenteImagenUsagi.sprite = fotoUsagiVictoria; 
            }

            if (fuenteEfectos != null && sonidoVictoria != null)
            {
                fuenteEfectos.PlayOneShot(sonidoVictoria);
            }
        }
        else
        {
            Debug.Log("¡Pum! Perdiste. Activando Jumpscare...");
            
            if (textoInstrucciones != null)
            {
                textoInstrucciones.text = "¡Número equivocado!";
                textoInstrucciones.color = Color.red;
            }

            if (componenteImagenUsagi != null && fotoUsagiDerrota != null)
            {
                componenteImagenUsagi.sprite = fotoUsagiDerrota; 
            }

            ActivarDerrotaJumpscare();
        }

        StartCoroutine(EsperarYRegresar(victoria));
    }

    private void ActivarDerrotaJumpscare()
    {
        if (objetoJumpscare != null)
        {
            Image imagenSusto = objetoJumpscare.GetComponent<Image>();
            if (imagenSusto != null && fotoUsagiDerrota != null)
            {
                imagenSusto.sprite = fotoUsagiDerrota;
            }

            objetoJumpscare.SetActive(true);
            
            if (fuenteEfectos != null && sonidoSusto != null)
            {
                fuenteEfectos.Stop();
                fuenteEfectos.PlayOneShot(sonidoSusto);
            }

            StartCoroutine(CoAnimarSusto());
        }
    }

    private IEnumerator CoAnimarSusto()
    {
        RectTransform rect = objetoJumpscare.GetComponent<RectTransform>();
        if (rect == null) yield break;

        float tiempo = 0f;
        float duracion = 0.12f; 

        rect.localScale = new Vector3(0.1f, 0.1f, 1f);

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;

            float escala = Mathf.Lerp(0.1f, 1.4f, progreso);
            rect.localScale = new Vector3(escala, escala, 1f);

            yield return null;
        }

        rect.localScale = Vector3.one;
    }
}