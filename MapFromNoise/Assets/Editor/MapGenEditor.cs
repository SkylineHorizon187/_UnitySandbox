﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGenEditor : Editor {

	public override void OnInspectorGUI() {
		MapGenerator mapGen = (MapGenerator)target;

		if (DrawDefaultInspector ()) {
			mapGen.GenerateMap ();
		}
	}
}
