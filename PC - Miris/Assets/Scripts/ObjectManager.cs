using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Miris
{
    public string ime;
    public int id;
    public Sprite sprite;
}
public class ObjectManager : MonoBehaviour
{
    public List <Miris> mirisi;
    [SerializeField]private Miris[] init;

    //pitanje
    public Miris selMiris;
    public Miris[] notSelMiris = {new Miris(), new Miris(), new Miris()};
    private GameObject correctMiris;

    //formular
    public GameObject preset;
    public GameObject holder;
    public TextMeshProUGUI instructionText;

    public Color red;
    public Color green;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        init = mirisi.ToArray();
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

			SelectRandom();
		}
    }

    public void SelectRandom()
    {
        Debug.Log(mirisi.Count);

        int n = Random.Range(0, mirisi.Count);
        selMiris.ime = mirisi[n].ime;
        selMiris.id = mirisi[n].id;
        selMiris.sprite = mirisi[n].sprite;

        SpawnObject(selMiris, true);

        mirisi.RemoveAt(n);

        instructionText.text = "Pomirši miris broj " + selMiris.id + ", i pogodi ga!";

        for (int i = 0; i < 3; i++)
        {
			n = Random.Range(0, mirisi.Count);
            notSelMiris[i].ime = mirisi[n].ime;
			notSelMiris[i].id = mirisi[n].id;
            notSelMiris[i].sprite = mirisi[n].sprite;

            SpawnObject(notSelMiris[i], false);

			mirisi.RemoveAt(n);
		}

        mirisi.Clear();
        mirisi.AddRange(init);

        ShuffleChildren(holder);
    }

    public void SpawnObject(Miris miris, bool correct)
    {
        GameObject go = Instantiate(preset, holder.transform);
        go.transform.GetChild(0).GetComponent<Image>().sprite = miris.sprite;
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = miris.ime;

        if(correct)
        {
            correctMiris = go;
        }
    }

    public void AnswerClicked(int id, GameObject go)
    {
        if (id == selMiris.id)
        {
            go.GetComponent<Option>().ChangeColor(green);
        }
		else
		{
			go.GetComponent<Option>().ChangeColor(red);
            correctMiris.GetComponent<Option>().ChangeColor(green);
		}
	}

	private void ShuffleChildren(GameObject parent)
	{
		if (parent.transform.childCount <= 1)
		{
			Debug.LogWarning("The selected GameObject must have at least two children to shuffle.");
			return;
		}

		Undo.RegisterFullObjectHierarchyUndo(parent, "Shuffle Children");

		// Get list of children
		List<Transform> children = new List<Transform>();
		foreach (Transform child in parent.transform)
		{
			children.Add(child);
		}

		// Shuffle the list
		for (int i = 0; i < children.Count; i++)
		{
			Transform temp = children[i];
			int randomIndex = Random.Range(i, children.Count);
			children[i] = children[randomIndex];
			children[randomIndex] = temp;
		}

		// Set sibling index to reorder
		for (int i = 0; i < children.Count; i++)
		{
			children[i].SetSiblingIndex(i);
		}

		Debug.Log($"Shuffled {children.Count} children of '{parent.name}'");
	}
}
