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
            // Victoria: vuelve a la oficina sin restar vidas
            SceneManager.LoadScene("FreddyFazbear");
        }
    }
}
