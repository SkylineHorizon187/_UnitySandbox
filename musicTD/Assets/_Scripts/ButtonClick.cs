using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour {

    public void ButtonSelected(int val)
    {
        PlaceDefense.PD.UpgradeTo(val);
        Menu.buildMenu.MenuClose();
    }

}
