using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelLogic : MonoBehaviour {
	public Transform player;

	public static void CarveOutCircle(Vector2 pos, int r) {
		int cx = (int)pos.x;
		int cy = (int)pos.y;
		int r2 = r*r;

		for (int x = cy - r; x <= cy + r; x++) {
			int di2 = (x - cy) * (x - cy);
			for (int y = cx-r; y <= cx+r; y++) {
				// test if in-circle
				if ((y-cx)*(y-cx) + di2 <= r2) {
					int colorMapIdx = x * ColorMap.mapWidth + y;
					if (colorMapIdx >= 0 && colorMapIdx < ColorMap.colorMap.Length) {
						ColorMap.colorMap [colorMapIdx] = Color.clear;
					}
				}
			}
		}
		MapGenerator.MG.UpdateMap ();
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
					int pointInNoisemap = x * ColorMap.mapWidth + y;
					if (pointInNoisemap >= 0 && pointInNoisemap < ColorMap.colorMap.Length) {
						if (ColorMap.colorMap [pointInNoisemap] == Color.white)
							return true;
					}
				}
			}
		}
		return false;
	}
}
