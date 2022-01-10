using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public static class ItemProperties
{
    public static Dictionary<ItemName, ItemType> itemTypes;
    public static Dictionary<ItemName, FaceTexture> itemTextures;
    public static Dictionary<ItemName, int> stackable;
    public static ItemName itemName;

    [System.Serializable] public enum ItemType
    {
        BLOC,
        TOOL
    }

    [SerializeField] public enum ItemName
    {
        DIRT_BLOC,
        STONE_BLOC,
        SNOW_BLOC,
        PICKAXE,
        AXE
    }

    public struct FaceTexture
    {
        int backFaceTexture;
        int frontFaceTexture;
        int topFaceTexture;
        int bottomFaceTexture;
        int leftFaceTexture;
        int rightFaceTexture;

        public FaceTexture(int v1, int v2, int v3, int v4, int v5, int v6) : this()
        {
            this.backFaceTexture = v1;
            this.frontFaceTexture = v2;
            this.topFaceTexture = v3;
            this.bottomFaceTexture = v4;
            this.leftFaceTexture = v5;
            this.rightFaceTexture = v6;
        }

        public int GetTextureID(FACE face)
        {
            switch (face)
            {
               case FACE.BACK:
                    return backFaceTexture;
                case FACE.FRONT:
                    return frontFaceTexture;
                case FACE.TOP:
                    return topFaceTexture;
                case FACE.BOTTOM:
                    return bottomFaceTexture;
                case FACE.LEFT:
                    return leftFaceTexture;
                case FACE.RIGHT:
                    return rightFaceTexture;
                default:
                    Debug.Log("Error in GetTextureID; invalid face index");
                    return 6;
            }
        }
    }

    public enum FACE
    {
        BACK,
        FRONT,
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    // Start is called before the first frame update
    public static void GenerationDico()
    {
        itemTextures = new Dictionary<ItemName, FaceTexture>();
        itemTypes = new Dictionary<ItemName, ItemType>();
        stackable = new Dictionary<ItemName, int>();

        FaceTexture dirtTexture = new FaceTexture(2, 2, 7, 1, 2, 2);
        FaceTexture stoneTexture = new FaceTexture(0, 0, 0, 0, 0, 0);
        FaceTexture woodTexture = new FaceTexture(4, 4, 4, 4, 4, 4);

        createItemProperties(ItemName.DIRT_BLOC, ItemType.BLOC, 64, dirtTexture);
        createItemProperties(ItemName.STONE_BLOC, ItemType.BLOC, 64, stoneTexture);
        createItemProperties(ItemName.SNOW_BLOC, ItemType.BLOC, 64, woodTexture);
        createItemProperties(ItemName.PICKAXE, ItemType.TOOL, 1);
        createItemProperties(ItemName.AXE, ItemType.TOOL, 1);
    }
    static void createItemProperties(ItemName name, ItemType type, int stack)
    {
        itemTypes.Add(name, type);
        stackable.Add(name, stack);
    }
    static void createItemProperties(ItemName name, ItemType type, int stack, FaceTexture texture)
    {
        itemTypes.Add(name, type);
        stackable.Add(name, stack);
        itemTextures.Add(name, texture);
    }

    
}
