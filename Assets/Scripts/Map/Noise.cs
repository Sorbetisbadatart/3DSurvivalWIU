using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

#region Finished Code
public static class Noise
{
    // Public static float to return 2D arrays
    public static float[,] GenerateNoiseMap(int MapWidth, int MapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] NoiseMap = new float[MapWidth, MapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float halfWidth = MapWidth / 2;
        float halfHeight = MapHeight / 2;

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    // Higher frequency makes points further apart
                    float SampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x * frequency;
                    float SampleY = (y - halfHeight) / scale * frequency - octaveOffsets[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(SampleX, SampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    // Amplitude decreases over time with persistance
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight < minValue)
                    minValue = noiseHeight;
                if (noiseHeight > maxValue)
                    maxValue = noiseHeight;

                NoiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize values
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                NoiseMap[x, y] = Mathf.InverseLerp(minValue, maxValue, NoiseMap[x, y]);
            }
        }
        return NoiseMap;
    }
}
#endregion

#region Fresh Code
//public static class Noise
//{
//    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
//    {
//        float[,] noiseMap = new float[mapWidth, mapHeight];
//        if (scale <= 0)
//        {
//            scale = 0.0001f;
//        }

//        for (int y = 0; y < mapHeight; y++)
//        {
//            for (int x = 0; x < mapWidth; x++)
//            {
//                float sampleX = x / scale;
//                float sampleY = y / scale;

//                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
//                noiseMap[x, y] = perlinValue;
//            }
//        }
//        return noiseMap;
//    }
//}
#endregion
