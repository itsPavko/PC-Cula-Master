using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public int id;
    public Image holder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GameObject go = GameObject.Find("Manager");
        go.GetComponent<ObjectManager>().AnswerClicked(id, this.gameObject);
    }

    public void ChangeColor(Color color)
    {
        holder.color = color;
    }
}
