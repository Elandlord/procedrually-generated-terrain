using UnityEngine;
using System.Collections;

public class Blocks : MonoBehaviour
{



	//Height multiplies the final noise output
	public float Height = 10.0f;

	//This divides the noise frequency
	public float NoiseSize = 10.0f;

	public float bottomY = 30f;

	private GameObject root;

	//Public variable for the size of the terrain, width and heigth
	public Vector2 Size;
	public float terrainSize;

	public Camera character;

	void OnGUI ()
	{

		terrainSize = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(10, 50, 100, 30), terrainSize, 0.0F, 100.0F));

		GUI.Label(new Rect(10, 10, 250, 50), "Size:" + " X:" + terrainSize + " Y:" + terrainSize);

		Size = new Vector2 (terrainSize, terrainSize);

		//Make a button that generates when you press it
		if(GUI.Button( new Rect( 10, 100, 100, 30 ), "Generate" ))
		{

			//Generate!
			Generate();

		}

	}

	//Function that inputs the position and spits out a float value based on the perlin noise
	public float PerlinNoise(float x, float y)
	{
		//Generate a value from the given position, position is divided to make the noise more frequent.
		float noise = Mathf.PerlinNoise( x / NoiseSize, y / NoiseSize );

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

		//If we find a gameobject called terrain, there's a high
		//chance that we have the previous terrain still there
		//So, let's delete it
		Destroy(GameObject.Find("Terrain"));


		//Create a new empty gameobject that will store all the objects spawned for extra neatness
		root = new GameObject("Terrain");

		//Put the root object at the center of the boxes
		root.transform.position = new Vector3( Size.x/2, 0, Size.y/2 );
		character.transform.position = new Vector3 (Size.x / 2, 10, Size.y / 2);

		//For loop for x-axis
		for(int x = 0; x <= Size.x; x++)
		{

			//For loop for z-axis
			for(int z = 0; z <= Size.y; z++)
			{
				float noiseY = PerlinNoise (x, z);

				for(int depth = 0; depth <= bottomY; depth++)
				{
					if (depth > 20) {
						GenerateCube (x, noiseY - 20, z);
					} else {
						GenerateCube (x, noiseY - depth, z);
					}
				}
			}

		}

		//Move the root at the origin.
		root.transform.position = Vector3.zero;

	}

	void GenerateCube(float x, float y, float z) 
	{
		GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
		box.GetComponent<Renderer>().material.color = GenerateRandomColor();
		box.transform.position = new Vector3( Mathf.Round(x), Mathf.Round(y) , Mathf.Round(z));
		box.transform.parent = root.transform;
	}
}