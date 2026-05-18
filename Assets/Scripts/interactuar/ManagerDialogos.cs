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
    private DialogoData datosActuales; // Guardamos los datos de la charla actual

    void Start() {
        colaFrases = new Queue<Frase>();
        panelDialogo.SetActive(false);
    }

    public void IniciarDialogo(DialogoData data) {
        datosActuales = data; // Guardamos la referencia
        panelDialogo.SetActive(true);
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

        // Verificamos si los datos piden un cambio de escena
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
