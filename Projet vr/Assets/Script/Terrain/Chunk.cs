using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int xSize, ySize, zSize;
    public int x, z;
    public Bloc blocPatron;

    Bloc[,,] data;
    
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uv;

    Mesh mesh;

    public void createChunk(int xSize, int ySize, int zSize, int x, int z, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector3 offset3D, Vector2 offset2D, float seuil)
    {
        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uv = new List<Vector2>();

        this.xSize = xSize;
        this.ySize = ySize;
        this.zSize = zSize;

        this.x = x;
        this.z = z;

        float[,] height = Noise.GenerateNoiseMap(xSize, zSize, seed, noiseScale, octaves, persistance, lacunarity, offset2D);
        float[,,] map = Noise.Noise3D(xSize, ySize, zSize, seed, noiseScale, octaves, persistance, lacunarity, offset3D);

        setData(height, map, seuil);
        refresh();

        Debug.Log("chunk : " + x + " : " + z  + "\n" +
            "vertices : " + vertices.Count + "\n" +
            "triangles : " + triangles.Count + "\n" +
            "uv : " + uv.Count + "\n");
    }

    private void setData(float[,] height, float[,,] map, float seuil)
    {
        data = new Bloc[xSize, ySize, zSize];
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                int yHeight = (int)(32 + (32 * height[x, z]));
                for (int y = 0; y < ySize; y++)
                {
                    Vector3 postionChunk = this.transform.position;
                    data[x, y, z] = Instantiate<Bloc>(blocPatron, new Vector3(postionChunk.x + x, postionChunk.y + y, postionChunk.z + z), Quaternion.identity, this.transform);
                    if (y < yHeight) {
                        if (map[x, y, z] < seuil)
                        {
                            data[x, y, z].terre = true;
                        }
                        else
                        {
                            data[x, y, z].terre = false;
                        }
                    }
                    else
                        data[x, y, z].terre = false;
                }
            }
        }
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
                    if(data[x, y, z].terre)
                        triangulationCube(x, z, y);
                }
            }
        }
    }

    public void triangulationCube(int x, int z, int y)
    {
        mesh = new Mesh();
        if (y < ySize - 1)
        {
            if (!data[x, y + 1, z].terre)
            {
                this.AddFace(new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f));
            }
        }
        else
        {
            this.AddFace(new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f));
        }

        if(y > 0)
        {
            if (!data[x, y - 1, z].terre)
            {
                this.AddFace(new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
            }
        }
        else
        {
            this.AddFace(new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
        }

        if(z < zSize - 1)
        {
            if(!data[x, y, z + 1].terre)
            {
                this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f));
            }
        }
        else
        {
            this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f));
        }

        if(z > 0)
        {
            if(!data[x, y, z - 1].terre)
            {
                this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
            }
        }
        else
        {
            this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
        }

        if(x < xSize - 1)
        {
            if(!data[x + 1, y, z].terre)
            {
                this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f));
            }
        }
        else
        {
            this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f));
        }

        if(x > 0)
        {
            if(!data[x - 1, y, z].terre)
            {
                this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f));
            }
        }
        else
        {
            this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f));
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();
        
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void AddFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int index = this.vertices.Count;
        
        int indexV1 = index;
        int tmp = this.vertices.IndexOf(v1);
        if (tmp != -1)
        {
            indexV1 = tmp;
        }
        else
        {
            this.vertices.Add(v1);
            index++;
        }

        int indexV2 = index;
        tmp = this.vertices.IndexOf(v2);
        if (tmp != -1)
        {
            indexV2 = tmp;
        }
        else
        {
            this.vertices.Add(v2);
            index++;
        }
        
        int indexV3 = index;
        tmp = this.vertices.IndexOf(v3);
        if (tmp != -1)
        {
            indexV3 = tmp;
        }
        else
        {
            this.vertices.Add(v3);
            index++;
        }

        int indexV4 = index;
        tmp = this.vertices.IndexOf(v4);
        if (tmp != -1)
        {
            indexV4 = tmp;
        }
        else
        {
            this.vertices.Add(v4);
            index++;
        }

        this.triangles.Add(indexV1);
        this.triangles.Add(indexV2);
        this.triangles.Add(indexV3);
        this.triangles.Add(indexV2);
        this.triangles.Add(indexV4);
        this.triangles.Add(indexV3);
    }

    /*
    case BlocDirection.Haut:
        
    break;
    case BlocDirection.Avant:
        
    break;
    case BlocDirection.Droite:
    break;
    case BlocDirection.Bas:

    break;
    case BlocDirection.Arrier:
    break;
    case BlocDirection.Gauche:
    break;
    */
}
