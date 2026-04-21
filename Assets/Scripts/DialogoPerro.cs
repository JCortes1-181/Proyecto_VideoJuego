using UnityEngine;
using TMPro; 

public class DialogoPerro : MonoBehaviour
{
    public GameObject cuadroDeTexto;
    public TextMeshProUGUI textoUI; 
    public string mensajePerro;     

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            
            textoUI.text = mensajePerro; 
            cuadroDeTexto.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            cuadroDeTexto.SetActive(false);
        }
    }
}