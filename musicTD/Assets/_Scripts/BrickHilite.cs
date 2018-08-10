using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrickHilite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    private cakeslice.Outline myOutline;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlaceDefense.PD.selectedBrick = gameObject.transform.parent.gameObject;
            if (PlaceDefense.PD.selectedBrick.transform.childCount < 2)
            {
                Menu.buildMenu.activeButtons.Clear();
                Menu.buildMenu.activeButtons.Add(0);
                Menu.buildMenu.activeButtons.Add(1);
            } else
            {
                Menu.buildMenu.activeButtons.Clear();
                Menu.buildMenu.activeButtons.Add(2);
                Menu.buildMenu.activeButtons.Add(3);
            }
            Menu.buildMenu.MenuOpen(eventData.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myOutline.enabled = true;

        GameObject go = transform.parent.gameObject;
        if (go.transform.childCount > 1)
        {
            go.transform.GetChild(1).GetComponent<TowerAI>().SelectUnit(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myOutline.enabled = false;

        GameObject go = transform.parent.gameObject;
        if (go.transform.childCount > 1)
        {
            go.transform.GetChild(1).GetComponent<TowerAI>().SelectUnit(false);
        }
    }

    void Start () {
        myOutline = GetComponent<cakeslice.Outline>();
	}
}