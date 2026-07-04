using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorVictoria : MonoBehaviour
{
    public float tiempoParaGanar = 10f;

    void Update()
    {
        tiempoParaGanar -= Time.deltaTime;

        if (tiempoParaGanar <= 0)
        {
            SceneManager.LoadScene("FreddyFazbear");
        }
    }
}
