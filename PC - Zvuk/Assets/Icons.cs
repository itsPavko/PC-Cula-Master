using UnityEngine;
using UnityEngine.UI;

public class Icons : MonoBehaviour
{
    public float initSize;
    public float desSize;

    public Color desColor;
    public Color initColor;

    public string category;

    public int state; // 0 - gray, 1 - focused, 2 - passed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initSize = transform.GetChild(0).transform.GetComponent<RectTransform>().sizeDelta.x;
        initColor = transform.GetChild(0).transform.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).transform.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(transform.GetChild(0).transform.GetComponent<RectTransform>().sizeDelta, new Vector2(desSize, desSize), Time.deltaTime * 10);
        transform.GetChild(0).transform.GetComponent<Image>().color = Color.Lerp(transform.GetChild(0).transform.GetComponent<Image>().color, desColor, Time.deltaTime * 10f);

    }

    public string Focus(Color color)
    {
        state = 1;
        desSize = initSize + 35;
        desColor = color;
        return category;
    }

    public void Pass()
    {
        state = 2;
        desSize = initSize;
    }

    public void Gray()
    {
        state = 0;
        desSize = initSize;
        desColor = initColor;
    }
}
