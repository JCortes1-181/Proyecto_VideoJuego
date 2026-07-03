using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordarNivel : MonoBehaviour
{
    void Start()
    {

        string nivelActual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("EscenaRetorno", nivelActual);
        PlayerPrefs.Save();
        
        Debug.Log("[Sistema Retorno] Registro completado. Al salir del minijuego volverás a: " + nivelActual);
    }
}
