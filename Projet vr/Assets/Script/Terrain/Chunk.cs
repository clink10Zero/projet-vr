using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemProperties;

public class Chunk : MonoBehaviour
{
    public static int TextureAtlasSizeInBlocks = 4;
    public static float NormalizedBlockTextureSize = (float)(1f / (float)TextureAtlasSizeInBlocks);
    public int xSize, ySize, zSize;
    public int x, z;
    public Bloc blocPatron;

    public Bloc[,,] data;

    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uv;

    Mesh mesh;

    public AnimationCurve modificateur;

    public MapGenerateur map;

    public void createChunk(int xSize, int ySize, int zSize, int x, int z, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector3 offset3D, Vector2 offset2D, float seuil, MapGenerateur mapGen)
    {
        //rend = GetComponent<Renderer>();
        //rend.sharedMaterial.shader = Shader.Find("Shader Graphs/TerrainLit");
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uv = new List<Vector2>();

        this.xSize = xSize;
        this.ySize = ySize;
        this.zSize = zSize;

        this.data = new Bloc[xSize, ySize, zSize];

        this.x = x;
        this.z = z;

        this.map = mapGen;

        float[,] height = Noise.GenerateNoiseMap(xSize, zSize, seed, noiseScale, octaves, persistance, lacunarity, offset2D);
        float[,,] map = Noise.Noise3D(xSize, ySize, zSize, seed, noiseScale, octaves, persistance, lacunarity, offset3D);

        setData(height, map, seuil);

        Debug.Log("chunk : " + x + " : " + z  + "\n" +
            "vertices : " + this.vertices.Count + "\n" +
            "triangles : " + this.triangles.Count + "\n" +
            "uv : " + this.uv.Count + "\n");
    }

    private void setData(float[,] height, float[,,] map, float seuil)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                int yHeight = (int)(32 + (32 * modificateur.Evaluate(height[x, z])));
                for (int y = 0; y < ySize; y++)
                {
                    Vector3 postionChunk = this.transform.position;
                    this.data[x, y, z] = new Bloc();

                    if (y < yHeight)
                    {
                        data[x, y, z].terre = true;

                        if (y < 30)
                        {
                            this.data[x, y, z].blocType = ItemProperties.ItemName.STONE_BLOC;
                        }
                        else
                        {
                            if (y < 40)
                            {
                                this.data[x, y, z].blocType = ItemProperties.ItemName.DIRT_BLOC;
                            }
                            else
                            {
                                this.data[x, y, z].blocType = ItemProperties.ItemName.SNOW_BLOC;
                            }
                        }
                    }
                    else
                        this.data[x, y, z].terre = false;
                }
            }
        }
    }

    public void clear()
    {
        vertices.Clear();
        triangles.Clear();
        uv.Clear();
    }

    public void refresh()
    {
        vertices.Clear();
        triangles.Clear();
        uv.Clear();
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if(this.data[x, y, z].terre)
                        triangulationCube(x, y, z);
                }
            }
        }
    }
    public void triangulationCube(int x, int y, int z)
    {
        mesh = new Mesh();
        FaceTexture text = ItemProperties.itemTextures[data[x, y, z].blocType];
        //top
        if (!data[x, y + 1, z].terre)
        {
            this.AddFaceAndTexture(new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), text.GetTextureID(FACE.TOP),FACE.TOP);
        }

        //bottom
        if (y > 0 && !data[x, y - 1, z].terre)
        {
            this.AddFaceAndTexture(new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f), text.GetTextureID(FACE.BOTTOM),FACE.BOTTOM);
        }
        //pas de else, y a pas de truc sous le sol

        //north
        if (z != zSize - 1)
        {
            if (!data[x, y, z + 1].terre)
            {
                this.AddFaceAndTexture(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f), text.GetTextureID(FACE.BACK),FACE.BACK);
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x, this.z + 1)].data[x, y, 0].terre)
                {
                    this.AddFaceAndTexture(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f), text.GetTextureID(FACE.BACK),FACE.BACK);
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFaceAndTexture(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f), text.GetTextureID(FACE.BACK),FACE.BACK);
            }
        }

        //south
        if (z != 0)
        {
            if (!data[x, y, z - 1].terre)
            {
                this.AddFaceAndTexture(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f), text.GetTextureID(FACE.FRONT),FACE.FRONT);
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x, this.z - 1)].data[x, y, zSize - 1].terre)
                {
                    this.AddFaceAndTexture(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f), text.GetTextureID(FACE.FRONT),FACE.FRONT);
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFaceAndTexture(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f), text.GetTextureID(FACE.FRONT),FACE.FRONT);
            }

        }

        //east
        if (x != xSize - 1)
        {
            if (!data[x + 1, y, z].terre)
            {
                this.AddFaceAndTexture(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f), text.GetTextureID(FACE.RIGHT),FACE.RIGHT);
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x + 1, this.z)].data[0, y, z].terre)
                {
                    this.AddFaceAndTexture(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f), text.GetTextureID(FACE.RIGHT),FACE.RIGHT);
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFaceAndTexture(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f), text.GetTextureID(FACE.RIGHT),FACE.RIGHT);
            }

        }

        //west
        if (x != 0)
        {
            if (!data[x - 1, y, z].terre)
            {
                this.AddFaceAndTexture(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), text.GetTextureID(FACE.LEFT),FACE.LEFT);
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x - 1, this.z)].data[xSize - 1, y, z].terre)
                {
                    this.AddFaceAndTexture(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), text.GetTextureID(FACE.LEFT),FACE.LEFT);
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFaceAndTexture(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), text.GetTextureID(FACE.LEFT),FACE.LEFT);
            }

        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();

        this.GetComponent<MeshFilter>().mesh = mesh;
        this.GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    public void AddFaceAndTexture(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, int textureID, FACE face)
    {
        int index = this.vertices.Count - 1;

        float y = textureID / TextureAtlasSizeInBlocks;
        float x = textureID - (y * TextureAtlasSizeInBlocks);

        x *= NormalizedBlockTextureSize;
        y *= NormalizedBlockTextureSize;

        y = 1f - y - NormalizedBlockTextureSize;

        this.vertices.Add(v1);
        this.vertices.Add(v2);
        this.vertices.Add(v3);
        this.vertices.Add(v4);

        switch (face)
        {
            case FACE.FRONT:
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y + NormalizedBlockTextureSize));
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y));
                uv.Add(new Vector2(x, y + NormalizedBlockTextureSize));
                uv.Add(new Vector2(x, y));
                break;
            case FACE.RIGHT:
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y + NormalizedBlockTextureSize));
                uv.Add(new Vector2(x, y + NormalizedBlockTextureSize));
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y));
                uv.Add(new Vector2(x, y));
                break;
            case FACE.BACK:
                uv.Add(new Vector2(x, y));
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y));
                uv.Add(new Vector2(x, y + NormalizedBlockTextureSize));
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y + NormalizedBlockTextureSize));
                break;
            default:
                uv.Add(new Vector2(x, y));
                uv.Add(new Vector2(x, y + NormalizedBlockTextureSize));
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y));
                uv.Add(new Vector2(x + NormalizedBlockTextureSize, y + NormalizedBlockTextureSize));
                break;
        }

        this.triangles.Add(index + 1);
        this.triangles.Add(index + 2);
        this.triangles.Add(index + 3);
        this.triangles.Add(index + 2);
        this.triangles.Add(index + 4);
        this.triangles.Add(index + 3);
    }

}

