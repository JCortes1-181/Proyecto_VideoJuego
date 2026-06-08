using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Sprite fotoUsagiDerrota;    // Usagi enojado/asustado (Jumpscare)

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

    public override void TerminarJuego(bool victoria)
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (victoria)
        {
            Debug.Log("¡Usagi contestó feliz!");
            if (componenteImagenUsagi != null && fotoUsagiVictoria != null)
            {
                componenteImagenUsagi.sprite = fotoUsagiVictoria; // Cambia a foto feliz
            }
        }
        else
        {
            Debug.Log("¡Pum! Perdiste.");
            if (componenteImagenUsagi != null && fotoUsagiDerrota != null)
            {
                componenteImagenUsagi.sprite = fotoUsagiDerrota; // Cambia a foto enojado
            }
        }

        // Espera los 2 segundos heredados de MinijuegoBase para volver a la oficina
        StartCoroutine(EsperarYRegresar(victoria));
    }
}