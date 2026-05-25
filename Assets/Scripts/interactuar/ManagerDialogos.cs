using UnityEngine;
using TMPro;
using System.Collections; // Necesario para el efecto de escritura
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para la imagen del retrato

public class ManagerDialogos : MonoBehaviour 
{
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre; 
    public TextMeshProUGUI textoDialogo;
    public Image imagenRetrato; // NUEVO: Arrastra aquí el objeto Image del retrato

    [Header("Ajustes de Texto")]
    public float velocidadEscritura = 0.02f; // Tiempo entre letras

    private Queue<Frase> colaFrases;
    private DialogoData datosActuales;
    private MonoBehaviour scriptMovimiento;
    
    private bool estaEscribiendo = false;
    private string textoCompletoActual;

    void Start() {
        colaFrases = new Queue<Frase>();
        panelDialogo.SetActive(false);
        
        scriptMovimiento = FindAnyObjectByType<Moverse_Mapa2d>();
        if (scriptMovimiento == null) {
            scriptMovimiento = FindAnyObjectByType<PlayerMovement>();
        }
    }

    public void IniciarDialogo(DialogoData data) {
        datosActuales = data;
        panelDialogo.SetActive(true);
        
        if (scriptMovimiento != null) scriptMovimiento.enabled = false;

        colaFrases.Clear();
        foreach (Frase frase in data.frases) {
            colaFrases.Enqueue(frase);
        }
        SiguienteFrase();
    }

    public void SiguienteFrase() {
        // Si el jugador pulsa Espacio mientras se escribe, mostramos el texto completo
        if (estaEscribiendo) {
            StopAllCoroutines();
            textoDialogo.text = textoCompletoActual;
            estaEscribiendo = false;
            return;
        }

        if (colaFrases.Count == 0) {
            FinalizarInteraccion();
            return;
        }

        Frase fraseActual = colaFrases.Dequeue();
        textoNombre.text = fraseActual.nombre;
        
        // Lógica del Retrato
        if (imagenRetrato != null) {
            if (fraseActual.retrato != null) {
                imagenRetrato.sprite = fraseActual.retrato;
                imagenRetrato.gameObject.SetActive(true);
            } else {
                imagenRetrato.gameObject.SetActive(false);
            }
        }

        // Iniciar efecto de escritura
        textoCompletoActual = fraseActual.texto;
        StartCoroutine(EscribirLetraPorLetra(fraseActual.texto));
    }

    IEnumerator EscribirLetraPorLetra(string texto) {
        textoDialogo.text = "";
        estaEscribiendo = true;

        foreach (char letra in texto.ToCharArray()) {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;
    }

    void FinalizarInteraccion() {
        panelDialogo.SetActive(false);
        if (scriptMovimiento != null) scriptMovimiento.enabled = true;

        if (datosActuales != null && datosActuales.cambiarEscenaAlFinal) {
            if (!string.IsNullOrEmpty(datosActuales.nombreEscenaDestino)) {
                SceneManager.LoadScene(datosActuales.nombreEscenaDestino);
            }
        }
    }

    void Update() {
        if (panelDialogo.activeSelf && Input.GetKeyDown(KeyCode.Space)) {
            SiguienteFrase();
        }
    }
}