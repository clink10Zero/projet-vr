using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemProperties
{
    public static Dictionary<ItemName, ItemType> itemTypes ;
    public static Dictionary<ItemName, int> stackable;
    public static ItemName itemName;

    public enum ItemType
    {
        BLOC,
        TOOL
    }

    public enum ItemName
    {
        DIRT_BLOC,
        STONE_BLOC,
        SNOW_BLOC,
        PICKAXE,
        AXE
    }

    // Start is called before the first frame update
    static void Start()
    {
        createItemProperties(ItemName.DIRT_BLOC, ItemType.BLOC, 64);
        createItemProperties(ItemName.STONE_BLOC, ItemType.BLOC, 64);
        createItemProperties(ItemName.SNOW_BLOC, ItemType.BLOC, 64);
        createItemProperties(ItemName.PICKAXE, ItemType.TOOL, 1);
        createItemProperties(ItemName.AXE, ItemType.TOOL, 1);
    }
    static void createItemProperties(ItemName name, ItemType type, int stack)
    {
        itemTypes.Add(name, type);
        stackable.Add(name, stack);
    }
}
