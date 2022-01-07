using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemProperties;

public class Bloc : MonoBehaviour
{  
    public ItemName blocType;
    //s'il faut y afficher, = ce n'est pas de l'air/du vide
    public bool terre = false;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Use the Specular shader on the material
        //rend.material.shader = Shader.Find("Specular");
    }

    void Update()
    {
        // Animate the Shininess value
        float shininess = Mathf.PingPong(Time.time, 1.0f);
        rend.material.SetFloat("_Shininess", shininess);
        switch (blocType)
        {
            case ItemName.STONE_BLOC:
                rend.material.SetColor("_Color",Color.gray);
                break;
            case ItemName.DIRT_BLOC:
                rend.material.SetColor("_Color", Color.blue);
                break;
            case ItemName.SNOW_BLOC:
                rend.material.SetColor("_Color", Color.white);
                break;
        }
        
    }
}
