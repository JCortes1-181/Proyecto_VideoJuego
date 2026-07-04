using UnityEngine;

public class ParallaxSkate : MonoBehaviour
{
    public MeshRenderer[] capas; 
    public float velocidadGlobal = 0.15f;

    void Update()
    {
        for (int i = 0; i < capas.Length; i++)
        {
            float factorProfundidad = 10f / (capas[i].transform.position.z + 1f);
            float offset = Time.time * velocidadGlobal * factorProfundidad;

            capas[i].material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
