using UnityEngine;

public class MenuNavegacion : MonoBehaviour
{
    public GameObject menuPrincipal; 
    public GameObject menuOpciones;   

    public void IrAOpciones()
    {
        menuPrincipal.SetActive(false);
        menuOpciones.SetActive(true);
    }

    public void VolverAlMenu()
    {
        menuOpciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void SetFullScren(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}