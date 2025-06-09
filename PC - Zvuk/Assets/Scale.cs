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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        micClip = Microphone.Start(null, true, 1, 44100);
    }

    // Update is called once per frame
    void Update()
    {
        float volume = GetMicVolume();
        noiseSmooth = Mathf.Lerp(noiseSmooth, noise, Time.deltaTime * 25);

        noise = volume * noiseMulti;

        scaledNoise = noiseSmooth * 0.006f;
        scaleImage.fillAmount = scaledNoise;

        float z = noiseSmooth * 2.16f;

        knobHolder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -z));

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = gradient.Evaluate(noiseSmooth / 100);
        }

        dbNumber = scaledNoise / 0.003636f;

        for (int i = 0; i < objects.Length; i++)
        {
            if(noiseSmooth > objects[i].ammount)
            {
                objects[i].icon.transform.GetComponent<Icons>().Pass();

                if(i == 0 && objects[i+1].icon.transform.GetComponent<Icons>().state == 0)
                {
                    objects[i].icon.transform.GetComponent<Icons>().Focus(gradient.Evaluate(noiseSmooth / 100));
                }
               
                if (i == objects.Length - 1)
                {
                    objects[i].icon.transform.GetComponent<Icons>().Focus(gradient.Evaluate(noiseSmooth / 100));
                }
            }
            else
            {
                if(i != 0)
                {
                    if (objects[i - 1].icon.transform.GetComponent<Icons>().state == 2)
                    {
                        objects[i - 1].icon.transform.GetComponent<Icons>().Focus(gradient.Evaluate(noiseSmooth / 100));
                    }
                }
                
                objects[i].icon.transform.GetComponent<Icons>().Gray();
            }
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
}
