using System.Collections;
using UnityEngine;

public static class ColorMap {

	public static Color[] colorMap;
	public static int mapWidth;
	public static int mapHeight;

	public static Color[] GenerateColorMap(int mapW, int mapH, float noiseScale, float offsetX, float offsetY, float density) {
		mapWidth = mapW;
		mapHeight = mapH;

		colorMap = new Color[mapWidth * mapHeight];

		if (noiseScale == 0)
			noiseScale = 0.0001f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				colorMap [y * mapWidth + x] = (Mathf.PerlinNoise (x / noiseScale + offsetX, y / noiseScale + offsetY) <= density) ? Color.white : Color.clear;
			}
		}

		return colorMap;
	}
}
