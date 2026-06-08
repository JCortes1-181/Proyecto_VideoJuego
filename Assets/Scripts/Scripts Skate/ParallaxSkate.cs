using UnityEngine;

public class ParallaxSkate : MonoBehaviour
{
    public MeshRenderer[] capas; // Aquí arrastras tus 6 planos en el inspector
    public float velocidadGlobal = 0.15f;

    void Update()
    {
        for (int i = 0; i < capas.Length; i++)
        {
            // Cuanto más lejos esté el plano (Z más alto), más lento se mueve
            float factorProfundidad = 10f / (capas[i].transform.position.z + 1f);
            float offset = Time.time * velocidadGlobal * factorProfundidad;

            // Desplaza la textura para crear el efecto infinito
            capas[i].material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
