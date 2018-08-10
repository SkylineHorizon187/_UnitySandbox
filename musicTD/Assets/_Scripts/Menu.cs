using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu buildMenu;

    public bool isOpen;
    public float radius;
    public GameObject spokePrefab;
    public GameObject[] menuButtons;
    public List<int> activeButtons;
    public GameObject closeButton;
    public float OpenCloseTime = 1f;

    private Transform spokeHolder;
    private List<GameObject> buttons;
    private List<GameObject> spokes;
    private bool menuActive;

    private List<Coroutine> CoRoutines;

    void Start()
    {
        buildMenu = this;

        spokeHolder = transform.GetChild(0);
        buttons = new List<GameObject>();
        activeButtons = new List<int>();
        spokes = new List<GameObject>();
        CoRoutines = new List<Coroutine>();

        menuActive = false;
        isOpen = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (buttons.Count < 1)
        {
            menuActive = false;
            isOpen = false;
            gameObject.SetActive(false);
        }
    }

    public void MenuOpen(Vector2 menuPos)
    {
        if (menuActive) return;

        isOpen = true;
        menuActive = true;
        gameObject.SetActive(true);

        transform.position = menuPos;
        float ang = 270f / activeButtons.Count;
        float offset = 225f + ang / 2;
        buttons.Clear();
        spokes.Clear();

        for (int i = 0; i < activeButtons.Count; i++)
        {
            // build a spoke
            spokes.Add(Instantiate(spokePrefab, transform.position, Quaternion.Euler(0, 0, offset + ang * i), spokeHolder));
            spokes[i].GetComponent<RectTransform>().sizeDelta = new Vector2(15, 1);

            // build a button
            Vector2 pos;
            pos.x = transform.position.x + radius * Mathf.Sin((offset + ang * i) * Mathf.Deg2Rad);
            pos.y = transform.position.y + radius * Mathf.Cos((offset + ang * i) * Mathf.Deg2Rad);
            buttons.Add(Instantiate(menuButtons[activeButtons[i]], transform.position, Quaternion.identity, transform));
            buttons[i].transform.localScale = new Vector3(.2f, .2f, .2f);

            CoRoutines.Add(StartCoroutine(ButtonFlyOut(buttons[i], spokes[i], pos, Vector3.one)));
        }
    }

    public void MenuClose()
    {
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            foreach (Coroutine c in CoRoutines)
            {
                StopCoroutine(c);
            }
            StartCoroutine(ButtonFlyIn(buttons[i], spokes[i], transform.position, new Vector3(.2f, .2f, .2f)));
        }
    }

    IEnumerator ButtonFlyOut(GameObject button, GameObject spoke, Vector3 pos, Vector3 scale)
    {
        float _timeStartedLerping = Time.time;
        while (button.transform.position != pos)
        {
            float percentageComplete = (Time.time - _timeStartedLerping) / OpenCloseTime;

            button.transform.position = Vector3.Lerp(button.transform.position, pos, percentageComplete);
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, scale, percentageComplete);
            spoke.GetComponent<RectTransform>().sizeDelta = new Vector2(15, Vector3.Distance(spoke.transform.position, button.transform.position));
            yield return null;
        }
    }

    IEnumerator ButtonFlyIn(GameObject button, GameObject spoke, Vector3 pos, Vector3 scale)
    {
        float _timeStartedLerping = Time.time;
        while (button.transform.position != pos)
        {
            float percentageComplete = (Time.time - _timeStartedLerping) / OpenCloseTime;

            button.transform.position = Vector3.Lerp(button.transform.position, pos, percentageComplete);
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, scale, percentageComplete);
            spoke.GetComponent<RectTransform>().sizeDelta = new Vector2(15, Vector3.Distance(spoke.transform.position, button.transform.position));

            yield return null;
        }

        buttons.Remove(button);
        Destroy(button);
        spokes.Remove(spoke);
        Destroy(spoke);
    }
}