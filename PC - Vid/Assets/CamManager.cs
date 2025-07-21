
using UnityEngine;

public class CamManager : MonoBehaviour
{
	//WebCamDevice[] devices;
	public MeshRenderer [] planes;
	[SerializeField] public WebCamTexture[] textures;
	[SerializeField] public string[] cameras;
	private Vector2Int camToScreen = Vector2Int.zero;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		WebCamDevice[] devices = WebCamTexture.devices;

		// for debugging purposes, prints available devices to the console
		for (int i = 0; i < cameras.Length; i++)
		{
			print("Webcam available: " + devices[i].name);

			textures[i] = new WebCamTexture(cameras[i]);
			planes[i].material.mainTexture = textures[i];
			textures[i].Stop();
			textures[i].Play();
		}

		// assuming the first available WebCam is desired
		//WebCamTexture tex = new WebCamTexture(devices[0].name);
		//pas.material.mainTexture = tex;
		//tex.Stop();
		//tex.Play();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		return;
		// Set Screen
		if(Input.GetKeyDown(KeyCode.Q))
		{
			camToScreen = new Vector2Int(4, 0);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			camToScreen = new Vector2Int(2, 0);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			camToScreen = new Vector2Int(1, 0);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			camToScreen = new Vector2Int(3, 0);
		}


		if(camToScreen.x == 0)
		{
			return;
		}
		// Set Cam
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			camToScreen = new Vector2Int(camToScreen.x, 1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			camToScreen = new Vector2Int(camToScreen.x, 2);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			camToScreen = new Vector2Int(camToScreen.x, 3);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			camToScreen = new Vector2Int(camToScreen.x, 4);
		}

		if(camToScreen.y == 0)
		{
			return;
		}

		planes[camToScreen.x - 1].material.mainTexture = textures[camToScreen.y - 1];
		camToScreen = Vector2Int.zero;
	}
}
