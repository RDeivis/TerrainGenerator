using UnityEngine;
using System.Collections;

public static class Noise {

	public static float[,] GenerateNoiseMap(int size, int levelOfDetail, int seed, float scale, Vector2 offset, int octaves, float frequencyInc, float amplitudeInc){

		Random.seed = seed;
		float val = Random.value;
		float[,] noiseMap = new float[size, size];


		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {

				float amplitude = 1;
				float frequency = 1;

				for (int i = 0; i < octaves; i++) {
					float posX = ((x * levelOfDetail) + offset.x + Mathf.Lerp (10000, 100000, val)) / scale * frequency;
					float posY = ((y * levelOfDetail) + offset.y + Mathf.Lerp (10000, 100000, val)) / scale * frequency;
					
					noiseMap [x, y] += (Mathf.PerlinNoise (posX, posY) * amplitude);
				
					amplitude *= amplitudeInc;
					frequency *= frequencyInc;
				}

				noiseMap [x, y] = Mathf.InverseLerp (0, 2, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}
}
