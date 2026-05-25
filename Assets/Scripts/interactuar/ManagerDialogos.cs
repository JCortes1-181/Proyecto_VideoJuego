using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerDialogos : MonoBehaviour 
{
    [Header("Referencias de UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre; 
    public TextMeshProUGUI textoDialogo;
    public Image imagenRetrato;

    [Header("Ajustes de Texto")]
    public float velocidadEscritura = 0.02f;

    private Queue<Frase> colaFrases;
    private DialogoData datosActuales;
    private MonoBehaviour scriptMovimiento;
    private bool estaEscribiendo = false;

    void Start() {
        colaFrases = new Queue<Frase>();
        
        // Apaga el panel al inicio para que no estorbe
        if (panelDialogo != null) panelDialogo.SetActive(false);
        
        // Busca automáticamente el script de movimiento de Marsh
        scriptMovimiento = FindAnyObjectByType<Moverse_Mapa2d>();
        if (scriptMovimiento == null) scriptMovimiento = FindAnyObjectByType<PlayerMovement>();
    }

    public void IniciarDialogo(DialogoData data) {
        if (data == null) return;
        
        datosActuales = data;
        if (panelDialogo != null) panelDialogo.SetActive(true);
        if (scriptMovimiento != null) scriptMovimiento.enabled = false;

        colaFrases.Clear();
        foreach (Frase frase in data.frases) {
            colaFrases.Enqueue(frase);
        }
        SiguienteFrase();
    }

    public void SiguienteFrase() {
        if (estaEscribiendo) return; // No pasa de frase si aún está escribiendo

        if (colaFrases.Count == 0) {
            FinalizarInteraccion();
            return;
        }

        Frase fraseActual = colaFrases.Dequeue();
        textoNombre.text = fraseActual.nombre;
        
        if (imagenRetrato != null) {
            if (fraseActual.retrato != null) {
                imagenRetrato.sprite = fraseActual.retrato;
                imagenRetrato.gameObject.SetActive(true);
            } else {
                imagenRetrato.gameObject.SetActive(false);
            }
        }

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
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (scriptMovimiento != null) scriptMovimiento.enabled = true;

        if (datosActuales != null && datosActuales.cambiarEscenaAlFinal) {
            if (!string.IsNullOrEmpty(datosActuales.nombreEscenaDestino)) {
                SceneManager.LoadScene(datosActuales.nombreEscenaDestino);
            }
        }
    }

    void Update() {
        // Pasar frase con Espacio o Click, solo si el panel está prendido
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && panelDialogo.activeSelf) {
            SiguienteFrase();
        }
    }
}