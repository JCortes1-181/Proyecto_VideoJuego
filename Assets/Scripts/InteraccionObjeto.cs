using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InteraccionObjeto : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject indicadorX;
    public GameObject cuadroDialogo;
    public TextMeshProUGUI textoUI;
    public string mensaje;

    [Header("Configuración de Escena")]
    public bool cambiaDeEscena = false; 
    public string nombreEscenaPizzeria = "FreddyFazbear"; 

    private bool jugadorCerca = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.X))
        {
            GameObject jugador = GameObject.FindWithTag("Player");

            if (!cuadroDialogo.activeSelf)
            {
                
                if (jugador != null) 
                {
                    var scriptMov = jugador.GetComponent<MovimientoJugador>();
                    if(scriptMov != null) scriptMov.enabled = false;
                }

                textoUI.text = mensaje;
                cuadroDialogo.SetActive(true);
                indicadorX.SetActive(false);
            }
            else
            {
                
                cuadroDialogo.SetActive(false);

                if (cambiaDeEscena) 
                {
                    
                    SceneManager.LoadScene(nombreEscenaPizzeria); 
                }
                else 
                {
                   
                    if (jugador != null) 
                    {
                        var scriptMov = jugador.GetComponent<MovimientoJugador>();
                        if(scriptMov != null) scriptMov.enabled = true;
                    }
                    indicadorX.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            indicadorX.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            indicadorX.SetActive(false);
            cuadroDialogo.SetActive(false);
        }
    }
}