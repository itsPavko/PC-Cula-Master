using UnityEngine;

public class MicVisualizer : MonoBehaviour
{
	public GameObject[] bars; // Assign your 80 UI bar GameObjects
	public int sampleSize = 1024;
	public float visualScale = 10f;

	private AudioSource audioSource;
	private float[] spectrum;
	public float sensitivity = 2f; // Default is 2x boost

	void Start()
	{
		spectrum = new float[sampleSize];

		// Set up microphone input
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
		audioSource.loop = true;
		//audioSource.outputAudioMixerGroup.name = "Mic";
		while (!(Microphone.GetPosition(null) > 0)) { }
		audioSource.Play();
	}

	void Update()
	{
		audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

		int minIndex = FrequencyToIndex(300);
		int maxIndex = FrequencyToIndex(3400);
		int range = maxIndex - minIndex;

		int binsPerBar = Mathf.Max(1, range / bars.Length);

		for (int i = 0; i < bars.Length; i++)
		{
			int start = minIndex + i * binsPerBar;
			float avg = 0f;

			for (int j = 0; j < binsPerBar; j++)
			{
				int index = Mathf.Clamp(start + j, 0, spectrum.Length - 1);
				avg += spectrum[index];
			}

			avg /= binsPerBar;
			float normalized = Mathf.Clamp01(avg * visualScale * sensitivity);
			bars[i].GetComponent<SoundBarLogic>().ChangeData(normalized);
		}
	}

	int FrequencyToIndex(float freq)
	{
		float f = Mathf.Clamp(freq, 20, AudioSettings.outputSampleRate / 2);
		return Mathf.FloorToInt(f / (AudioSettings.outputSampleRate / 2f) * sampleSize);
	}
}
