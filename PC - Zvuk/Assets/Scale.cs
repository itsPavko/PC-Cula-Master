using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObjectPair
{
    public GameObject icon;
    public float ammount;
}

public class Scale : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float noise;
    private float noiseSmooth;
    public float scaledNoise;

    public Image scaleImage;
    public GameObject knobHolder;

    public Image[] images;
    public Gradient gradient;

    public float dbNumber;

    public ObjectPair[] objects;

    private AudioClip micClip;
    private const int sampleWindow = 128;
    public float noiseMulti;

    public TextMeshProUGUI dbAmmount;
    public TextMeshProUGUI dbCategory;
    public GameObject sensText;

    public int record;
    public TextMeshProUGUI recordText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        micClip = Microphone.Start(null, true, 1, 44100);
        sensText.SetActive(false);



		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    // Update is called once per frame
    void Update()
    {
        float volume = GetMicVolume();

        noiseSmooth = Mathf.Lerp(noiseSmooth, noise, Time.deltaTime * 5);
        

        noise = volume * noiseMulti;

        scaledNoise = noiseSmooth * 0.006f;

        scaleImage.fillAmount = Mathf.Clamp(scaledNoise, 0.0f, 0.6f);

        float z = noiseSmooth * 2.16f;
        
        z = Mathf.Clamp(z, 0, 216);

        knobHolder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -z));

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = gradient.Evaluate(noiseSmooth / 100);
        }

        dbNumber = scaledNoise / 0.003636f;

		dbAmmount.text = Mathf.RoundToInt(dbNumber).ToString() + "dB";
        dbAmmount.color = gradient.Evaluate(noiseSmooth / 100);
		dbCategory.color = gradient.Evaluate(noiseSmooth / 100);


		if (Mathf.RoundToInt(dbNumber) > record)
        {
            recordText.text = "Današnji rekord je: " + record + "dB";
            record = Mathf.RoundToInt(dbNumber);

		}

		for (int i = 0; i < objects.Length; i++)
        {
            if(noiseSmooth > objects[i].ammount)
            {
                objects[i].icon.transform.GetComponent<Icons>().Pass();

                if(i == 0 && objects[i+1].icon.transform.GetComponent<Icons>().state == 0)
                {
					dbCategory.text = objects[i].icon.transform.GetComponent<Icons>().Focus(gradient.Evaluate(noiseSmooth / 100));
                }
               
                if (i == objects.Length - 1)
                {
					dbCategory.text = objects[i].icon.transform.GetComponent<Icons>().Focus(gradient.Evaluate(noiseSmooth / 100));
                }
            }
            else
            {
                if(i != 0)
                {
                    if (objects[i - 1].icon.transform.GetComponent<Icons>().state == 2)
                    {
						dbCategory.text = objects[i - 1].icon.transform.GetComponent<Icons>().Focus(gradient.Evaluate(noiseSmooth / 100));
                    }
                }
                
                objects[i].icon.transform.GetComponent<Icons>().Gray();
            }

        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            noiseMulti = 100;
        }

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			noiseMulti = 150;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			noiseMulti = 200;
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			noiseMulti = 250;
		}

		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			noiseMulti = 300;
		}

		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			noiseMulti = 350;
		}

		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			noiseMulti = 400;
		}

		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			noiseMulti = 450;
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			noiseMulti = 500;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			noiseMulti += 10;
		}

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			noiseMulti -= 10;
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			noiseMulti += 1;
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			noiseMulti -= 1;
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
            sensText.SetActive(!sensText.activeInHierarchy);
		}

        sensText.GetComponent<TextMeshProUGUI>().text = noiseMulti.ToString();
	}

	float GetMicVolume()
	{
		float[] samples = new float[sampleWindow];
		int micPosition = Microphone.GetPosition(null) - sampleWindow + 1;
		if (micPosition < 0) return 0;

		micClip.GetData(samples, micPosition);

		float levelMax = 0;
		for (int i = 0; i < sampleWindow; ++i)
		{
			float wavePeak = Mathf.Abs(samples[i]);
			if (wavePeak > levelMax)
				levelMax = wavePeak;
		}
		return levelMax;
	}
}
