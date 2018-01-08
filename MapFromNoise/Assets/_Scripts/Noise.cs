using System.Collections;
using UnityEngine;

public static class Noise {

	public static int[] noiseMap;
	public static int mapWidth;
	public static int mapHeight;

	public static int[] GenerateNoiseMap(int mapW, int mapH, float noiseScale, float offsetX, float offsetY, float density) {
		mapWidth = mapW;
		mapHeight = mapH;

		noiseMap = new int[mapWidth * mapHeight];

		if (noiseScale == 0)
			noiseScale = 0.0001f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [y * mapWidth + x] = (Mathf.PerlinNoise (x / noiseScale + offsetX, y / noiseScale + offsetY) <= density) ? 1 : 0;
			}
		}

		return noiseMap;
	}
}
