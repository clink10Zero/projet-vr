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

    Dictionary< MapCoordonnee, Chunk> chunks;

    public void Start()
    {
        Vector3 postionMap = map.transform.position;
        for(int x = 0; x < 5; x++)
        {
            for(int z = 0; z < 5; z++)
            {
                Chunk courant = Instantiate<Chunk>(patronChunk, new Vector3(postionMap.x + (x * 16), postionMap.y, postionMap.z + (z * 16)), Quaternion.identity, map);
                courant.createChunk(16, 256, 16, x, z, seed, noiseScale, octaves, persistance, lacunarity, new Vector3(x * 16, 0, z * 16), new Vector2(x * 16, z * 16), seuil);
                
                this.chunks.Add(new MapCoordonnee(x, z), courant);
            }
        }
    }
}