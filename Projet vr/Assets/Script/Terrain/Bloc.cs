using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemProperties;

[System.Serializable]
public class Bloc
{  
    public ItemName blocType;
    //s'il faut y afficher, = ce n'est pas de l'air/du vide
    public bool terre = false;

}
