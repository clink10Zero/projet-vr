using UnityEngine;
using System.Collections;

public static class Noise
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector3 offset)
	{
		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (scale <= 0)
		{
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight)
				{
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight)
				{
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}

	public static float[,,] Noise3D(int mapWidth, int hauteur, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector3 offset)
    {
		float[,,] map = new float[mapWidth, hauteur, mapHeight];

		System.Random prng = new System.Random(seed);
		Vector3[] octaveOffsets = new Vector3[octaves];
		for (int i = 0; i < octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			float offsetZ = prng.Next(-100000, 100000) + offset.z;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHauteur = hauteur / 2f;
		float halfHeight = mapHeight / 2f;

		for (int x = 0; x < mapWidth; x++)
        {
			for(int y = 0; y < hauteur; y++)
            {
				for(int z = 0; z < mapHeight; z++)
                {
					float amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
						float sampleZ = (z - halfHauteur) / scale * frequency + octaveOffsets[i].z;
						float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

						float xy = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						float xz = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
						float yz = Mathf.PerlinNoise(sampleY, sampleZ) * 2 - 1;
						float yx = Mathf.PerlinNoise(sampleY, sampleX) * 2 - 1;
						float zx = Mathf.PerlinNoise(sampleZ, sampleX) * 2 - 1;
						float zy = Mathf.PerlinNoise(sampleZ, sampleY) * 2 - 1;

						noiseHeight += ((xy + xz + yz + yx + zx + zy) / 6) * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxNoiseHeight)
					{
						maxNoiseHeight = noiseHeight;
					}
					else if (noiseHeight < minNoiseHeight)
					{
						minNoiseHeight = noiseHeight;
					}
					map[x, y, z] = noiseHeight;
				}
            }
        }
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < hauteur; y++)
			{
				for (int z = 0; z < mapHeight; z++)
				{
					map[x, y, z] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y, z]);
				}
			}
		}
		return map;
    }
}
