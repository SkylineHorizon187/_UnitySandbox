using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
	[HideInInspector] public static MapGenerator MG;
	public GameObject map;

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;
	[Range(0f, 1f)] public float density;
	public float offsetX;
	public float offsetY;

	private Texture2D tex;
	private Sprite s;
	private SpriteRenderer sr;

	void Start() {
		MG = this;
		offsetX = Random.Range (0f, 999f);
		offsetY = Random.Range (0f, 999f);
		sr = map.GetComponent<SpriteRenderer> ();

		ColorMap.colorMap = ColorMap.GenerateColorMap (mapWidth, mapHeight, noiseScale, offsetX, offsetY, density);
		GenerateMap ();
	}

	public void GenerateMap() {
		tex = new Texture2D (mapWidth, mapHeight);

		tex.SetPixels (ColorMap.colorMap);
		tex.Apply ();

		s = Sprite.Create (tex, new Rect (0, 0, mapWidth, mapHeight), Vector2.zero, 1);
		sr.sprite = s;
	}

	public void UpdateMap() {
		tex.SetPixels (ColorMap.colorMap);
		tex.Apply ();
	}
}
