using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemProperties;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public ItemName itemName;
    public ItemType itemType;

    public Color couleur;
    public GameObject model;

}
