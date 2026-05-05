using UnityEngine;
using TMPro;

public class Hablar_npc : MonoBehaviour
{
    [Header("Configuración del Mensaje")]
    [TextArea(3, 10)]
    public string mensajeChistoso; 

    [Header("Referencias de la UI")]
    public GameObject globoTexto;       
    public TextMeshProUGUI componenteTexto; 
    public GameObject indicadorE;      

    private bool jugadorCerca = false;

    void Start()
    {
        // Forzamos que todo empiece apagado al dar Play
        if(globoTexto != null) globoTexto.SetActive(false);
        if(indicadorE != null) indicadorE.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            // Si el globo está apagado, lo prendemos y ponemos el texto
            if (!globoTexto.activeSelf)
            {
                globoTexto.SetActive(true);
                componenteTexto.text = mensajeChistoso;
                if(indicadorE != null) indicadorE.SetActive(false); // Escondemos la E mientras habla
            }
            else
            {
                // Si ya estaba prendido, lo apagamos (para cerrar el diálogo)
                globoTexto.SetActive(false);
                if(indicadorE != null) indicadorE.SetActive(true); // Regresa la E
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IMPORTANTE: Tu jugador debe tener el Tag "Player"
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if(indicadorE != null) indicadorE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if(indicadorE != null) indicadorE.SetActive(false);
            if(globoTexto != null) globoTexto.SetActive(false);
        }
    }
}