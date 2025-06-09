using UnityEngine;

public class CamManager : MonoBehaviour
{
	WebCamDevice[] devices;
	public MeshRenderer pas;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		WebCamDevice[] devices = WebCamTexture.devices;

		// for debugging purposes, prints available devices to the console
		for (int i = 0; i < devices.Length; i++)
		{
			print("Webcam available: " + devices[i].name);
		}

		// assuming the first available WebCam is desired
		WebCamTexture tex = new WebCamTexture(devices[0].name);
		pas.material.mainTexture = tex;
		tex.Stop();
		tex.Play();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
