using UnityEngine;
using UnityEngine.UI;

public class SoundBarLogic : MonoBehaviour
{
    public float desHeight;
    private Image img;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

		img = GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
        img.fillAmount = Mathf.Lerp(img.fillAmount, desHeight, Time.deltaTime * 5);
    }

    public void ChangeData(float volume)
    {
        desHeight = volume;
    }
}
