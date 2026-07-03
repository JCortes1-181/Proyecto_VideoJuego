using UnityEngine;

public class BotonTuercaMovil : MonoBehaviour
{
    public void TocarTuerca()
    {
        if (ControladorPausa.Instancia != null)
        {
            ControladorPausa.Instancia.AlternarPausa();
        }
        else
        {
            Debug.LogWarning("No se encontró el Controlador de Pausa");
        }
    }
}