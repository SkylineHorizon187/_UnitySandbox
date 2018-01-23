using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaskEnum : PropertyAttribute
{
	public MaskEnum() { }
}


public class General : MonoBehaviour {

	[System.Flags]
	public enum TargetTypes {
		Player 	= (1 << 0),
		Card 	= (1 << 1),
		Field 	= (1 << 2),
	}

}
