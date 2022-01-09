using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerateur : MonoBehaviour
{
    [Header("Parametre")]
    [Space]
    public float noiseScale;

    public int octaves;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    [Range(0, 1)]
    public float seuil;

    public int seed;

    public Vector2 offset2D;
    public Vector2 offset3D;

    [Space]
    public Transform map;
    public Chunk patronChunk;

   public Dictionary< (int,int), Chunk> chunks;

    public void Generation()
    {
        ItemProperties.GenerationDico();
        this.chunks = new Dictionary<(int,int), Chunk>();
        Vector3 postionMap = map.transform.position;
        for(int x = 0; x < 1; x++)
        {
            for(int z = 0; z < 1; z++)
            {
                Chunk courant = Instantiate<Chunk>(patronChunk, new Vector3(postionMap.x + (x * 16), postionMap.y, postionMap.z + (z * 16)), Quaternion.identity, map);
                courant.createChunk(16, 256, 16, x, z, seed, noiseScale, octaves, persistance, lacunarity, new Vector3(x * 16, 0, z * 16), new Vector2(x * 16, z * 16), seuil, this);
                this.chunks.Add((x, z), courant);
            }
        }
        
        for(int x = 0; x < 1; x++)
        {
            for(int z = 0; z < 1; z++)
            {
                Chunk courant = this.chunks[(x, z)];
                courant.refresh();
            }
        }
    }

    public void Clear()
    {
        for (int x = 0; x < 2; x++)
        {
            for (int z = 0; z < 2; z++)
            {
                Chunk courant = this.chunks[(x, z)];
                courant.clear();
            }
        }
    }
}