using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MaskEnum))]
public class MaskEnumDrawer : PropertyDrawer
{
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		_property.intValue = EditorGUI.MaskField( _position, _label, _property.intValue, _property.enumNames );
	}
}