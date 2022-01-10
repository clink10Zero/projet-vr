using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int xSize, ySize, zSize;
    public int x, z;
    public Bloc blocPatron;

    public Bloc[,,] data;
    
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uv;

    Mesh mesh;

    public AnimationCurve modificateur;

    public MapGenerateur map;

    public void createChunk(int xSize, int ySize, int zSize, int x, int z, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector3 offset3D, Vector2 offset2D, float seuil, MapGenerateur mapGen)
    {
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
                    //data[x, y, z] = Instantiate<Bloc>(blocPatron, new Vector3(postionChunk.x + x, postionChunk.y + y, postionChunk.z + z), Quaternion.identity, this.transform);
                    this.data[x, y, z] = new Bloc();
                    if (y < yHeight) {
                            this.data[x, y, z].terre = true;
                        if (y<30)
                        {
                            this.data[x, y, z].blocType = ItemProperties.ItemName.STONE_BLOC;
                        }
                        else
                        {
                            if (y<50)
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
                        triangulationCube(x, z, y);
                }
            }
        }
    }

    public void triangulationCube(int x, int z, int y)
    {
        mesh = new Mesh();
        //top
        if (!data[x, y + 1, z].terre)
        {
             this.AddFace(new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f));
        }
       
        //bottom
        if(y > 0 && !data[x, y - 1, z].terre)
        {
            this.AddFace(new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
        }
        //pas de else, y a pas de truc sous le sol

        //north
        if(z != zSize -1)
        {
            if(!data[x, y, z + 1].terre)
            {
                this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f));
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x, this.z + 1)].data[x, y, 0].terre)
                {
                    this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f));
                }
            }
            catch(KeyNotFoundException){
                //this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 1f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 1f, y + 1f, z + 1f));
            }
        }

        //south
        if(z != 0)
        {
            if(!data[x, y, z - 1].terre)
            {
                this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x, this.z - 1)].data[x, y, zSize - 1].terre)
                {
                    this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f), new Vector3(x + 0f, y + 0f, z + 0f));
            }

        }

        //east
        if(x != xSize - 1)
        {
            if(!data[x + 1, y, z].terre)
            {
                this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f));
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x + 1, this.z)].data[0, y, z].terre)
                {
                    this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f));
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFace(new Vector3(x + 1f, y + 1f, z + 0f), new Vector3(x + 1f, y + 1f, z + 1f), new Vector3(x + 1f, y + 0f, z + 0f), new Vector3(x + 1f, y + 0f, z + 1f));
            }
           
        }

        //west
        if(x != 0)
        {
            if(!data[x - 1, y, z].terre)
            {
                this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f));
            }
        }
        else
        {
            try
            {
                if (!map.chunks[(this.x - 1, this.z)].data[xSize - 1, y, z].terre)
                {
                    this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f));
                }
            }
            catch (KeyNotFoundException)
            {
                //this.AddFace(new Vector3(x + 0f, y + 0f, z + 1f), new Vector3(x + 0f, y + 1f, z + 1f), new Vector3(x + 0f, y + 0f, z + 0f), new Vector3(x + 0f, y + 1f, z + 0f));
            }

        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();
        
        this.GetComponent<MeshFilter>().mesh = mesh;
        this.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void AddFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int index = this.vertices.Count;

        (int, int) indexV1 = getIndex(index,v1);
        (int, int) indexV2 = getIndex(indexV1.Item1, v2);
        (int, int) indexV3 = getIndex(indexV2.Item1, v3);
        (int, int) indexV4 = getIndex(indexV3.Item1, v4);

        this.triangles.Add(indexV1.Item2);
        this.triangles.Add(indexV2.Item2);
        this.triangles.Add(indexV3.Item2);
        this.triangles.Add(indexV2.Item2);
        this.triangles.Add(indexV4.Item2);
        this.triangles.Add(indexV3.Item2);
    }

    (int,int) getIndex(int index, Vector3 v1)
    {
        int indexV1 = index;
        int tmp = this.vertices.IndexOf(v1);
        if (tmp != -1)//s'il existe déjà
        {
            indexV1 = tmp;
        }
        else
        {
            this.vertices.Add(v1);
            index++;
        }
        return (index,indexV1);
    }
}
