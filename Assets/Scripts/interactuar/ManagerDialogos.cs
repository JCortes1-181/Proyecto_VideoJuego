using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ManagerDialogos : MonoBehaviour 
{
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre; 
    public TextMeshProUGUI textoDialogo;

    private Queue<Frase> colaFrases;
    private DialogoData datosActuales;
    
    // Usamos MonoBehaviour para que acepte cualquier script de movimiento
    private MonoBehaviour scriptMovimiento;

    void Start() {
        colaFrases = new Queue<Frase>();
        panelDialogo.SetActive(false);
        
        // Buscamos cualquiera de los dos scripts que podrías estar usando
        scriptMovimiento = FindAnyObjectByType<Moverse_Mapa2d>();
        
        if (scriptMovimiento == null) {
            scriptMovimiento = FindAnyObjectByType<PlayerMovement>();
        }
    }

    public void IniciarDialogo(DialogoData data) {
        datosActuales = data;
        panelDialogo.SetActive(true);
        
        // Congelamos a Marsh
        if (scriptMovimiento != null) scriptMovimiento.enabled = false;

        colaFrases.Clear();
        foreach (Frase frase in data.frases) {
            colaFrases.Enqueue(frase);
        }
        SiguienteFrase();
    }

    public void SiguienteFrase() {
        if (colaFrases.Count == 0) {
            FinalizarInteraccion();
            return;
        }

        Frase fraseActual = colaFrases.Dequeue();
        textoNombre.text = fraseActual.nombre;
        textoDialogo.text = fraseActual.texto;
    }

    void FinalizarInteraccion() {
        panelDialogo.SetActive(false);
        
        // Devolvemos el control a Marsh
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