using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelLogic : MonoBehaviour {

	public static void CarveOutCircle(Vector2 pos, int r) {
		int cx = (int)pos.x;
		int cy = (int)pos.y;
		int r2 = r*r;

		for (int x = cy - r; x <= cy + r; x++) {
			int di2 = (x - cy) * (x - cy);
			for (int y = cx-r; y <= cx+r; y++) {
				// test if in-circle
				if ((y-cx)*(y-cx) + di2 <= r2) {
					int colorMapIdx = x * AllTerrain.AT.mapWidth + y;
					if (colorMapIdx >= 0 && colorMapIdx < AllTerrain.AT.colorMap.Length) {
						AllTerrain.AT.colorMap [colorMapIdx] = Color.clear;
					}
				}
			}
		}
		AllTerrain.AT.UpdateMap ();
	}

	public static bool HitTest(Vector2 pos, int r) {
		int cx = (int)pos.x;
		int cy = (int)pos.y;
		int r2 = r * r;

		for (int x = cy - r; x <= cy + r; x++) {
			int di2 = (x - cy) * (x - cy);
			for (int y = cx-r; y <= cx+r; y++) {
				// test if in-circle
				if ((y-cx)*(y-cx) + di2 <= r2) {
					int point = x * AllTerrain.AT.mapWidth + y;
					if (point >= 0 && point < AllTerrain.AT.colorMap.Length) {
						if (AllTerrain.AT.colorMap [point] != Color.clear)
							return true;
					}
				}
			}
		}
		return false;
	}
}
