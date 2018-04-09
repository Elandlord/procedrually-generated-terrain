using UnityEngine;
using System.Collections;

public class Blocks : MonoBehaviour
{



	//Height multiplies the final noise output
	public float Height = 10.0f;

	//This divides the noise frequency
	public float NoiseSize = 10.0f;

	public float bottomY = 15f;

	private bool generated;
	private float randomFloat;

	private GameObject root;

	//Public variable for the size of the terrain, width and heigth
	public Vector2 Size;
	public float terrainSize;

	// FPS Controller
	public GameObject character;

	// Main camera
	public GameObject mainCamera;

	public Material grass;
	public Material dirt;
	public Material water;

	void Awake() {
		randomFloat = Random.Range (0.0f, 5.0f);
	}

	void OnGUI ()
	{
		if (!generated) {
			terrainSize = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(10, 50, 100, 30), terrainSize, 0.0F, 100.0F));

			GUI.Label(new Rect(10, 10, 250, 50), "Size:" + " X:" + terrainSize + " Y:" + terrainSize);
			Size = new Vector2 (terrainSize, terrainSize);

			//Make a button that generates when you press it
			if(GUI.Button( new Rect( 10, 100, 100, 30 ), "Generate" ))
			{
				Generate();
			}
		}
	}

	//Function that inputs the position and spits out a float value based on the perlin noise
	public float PerlinNoise(float x, float y)
	{
		//Generate a value from the given position, position is divided to make the noise more frequent.
		float noise = Mathf.PerlinNoise( (x + randomFloat) / NoiseSize, y / NoiseSize );

		//Return the noise value
		return noise * Height;

	}

	public Color GenerateRandomColor()
	{
		return new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), 1f);
	}

	//Call this function to generate the terrain
	void Generate ()
	{
		generated = true;
		//If we find a gameobject called terrain, there's a high
		//chance that we have the previous terrain still there
		//So, let's delete it
		Destroy(GameObject.Find("Terrain"));


		//Create a new empty gameobject that will store all the objects spawned for extra neatness
		root = new GameObject("Terrain");

		//Put the root object at the center of the boxes
		root.transform.position = new Vector3( Size.x/2, 0, Size.y/2 );

		// Set character to Active to get it to spawn
		character.SetActive (true);

		// Set MainCamera to false to prevent it to interfere with the camera of the character
		mainCamera.SetActive (false);


		//For loop for x-axis
		for(int x = 0; x <= Size.x; x++)
		{

			//For loop for z-axis
			for(int z = 0; z <= Size.y; z++)
			{
				float noiseY = PerlinNoise (x, z);

				for(int depth = 0; depth <= bottomY; depth++)
				{
					if (depth == 0) {
						if (Mathf.Round(noiseY) == 3) {
							GenerateCube (x, noiseY, z, water);
						} else {
							GenerateCube (x, noiseY , z, grass);
						}
					} else if (depth <= 10){
						GenerateCube (x, noiseY - depth, z, dirt);
					}

				}
			}

		}

		//Move the root at the origin.
		root.transform.position = Vector3.zero;

	}

	void GenerateCube(float x, float y, float z, Material material = default(Material)) 
	{
		GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);

		// if material is not set, use default
		if(material != null) {
			box.GetComponent<Renderer> ().material = material;
		}else {
			box.GetComponent<Renderer>().material.color = GenerateRandomColor();
		}

		box.transform.position = new Vector3( Mathf.Round(x), Mathf.Round(y) , Mathf.Round(z));
		box.transform.parent = root.transform;
	}
}
