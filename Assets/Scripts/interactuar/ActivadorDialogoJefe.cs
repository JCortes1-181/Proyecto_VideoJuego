using UnityEngine;

public class ActivadorDialogoJefe : MonoBehaviour
{
    [Header("Los Diálogos")]
    public DialogoData dialogo1_Intro;      // Primera vez
    public DialogoData dialogo2_Derrota;    // Volvió tras rendirse en GameOver
    public DialogoData dialogo3_Victoria;   // Volvió tras ganar y continuar
    public DialogoData dialogo4_Fijo;       // Texto para siempre después de terminar

    [Header("Indicador Visual")]
    public GameObject iconoInteractuar; 
    private bool jugadorCerca = false;

    void Start() {
        if (iconoInteractuar != null) iconoInteractuar.SetActive(false);
    }

    void Update() {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E)) {
            ManagerDialogos manager = Object.FindFirstObjectByType<ManagerDialogos>();
            
            if (manager != null) {
                DialogoData dialogoAElegir = SeleccionarDialogoSegunEstado();
                
                if (dialogoAElegir != null) {
                    manager.IniciarDialogo(dialogoAElegir);
                    if (iconoInteractuar != null) iconoInteractuar.SetActive(false);
                }
            }
        }
    }

    private DialogoData SeleccionarDialogoSegunEstado() {
        switch (EstadoMundo.estadoActual) {
            case EstadoMundo.EstadoNpc.PrimeraVez:
                return dialogo1_Intro;

            case EstadoMundo.EstadoNpc.VolvioDerrotado:
                EstadoMundo.estadoActual = EstadoMundo.EstadoNpc.YaTerminoTodo;
                return dialogo2_Derrota;

            case EstadoMundo.EstadoNpc.VolvioVictorioso:
                EstadoMundo.estadoActual = EstadoMundo.EstadoNpc.YaTerminoTodo;
                return dialogo3_Victoria;

            case EstadoMundo.EstadoNpc.YaTerminoTodo:
                return dialogo4_Fijo;

            default:
                return dialogo1_Intro;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            jugadorCerca = true;
            if (iconoInteractuar != null) iconoInteractuar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            jugadorCerca = false;
            if (iconoInteractuar != null) iconoInteractuar.SetActive(false);
        }
    }
}
