using UnityEngine;
using System.Collections;

public class Blocks : MonoBehaviour
{



	//Height multiplies the final noise output
	public float Height = 200.0f;

	//This divides the noise frequency
	public float NoiseSize = 10.0f;

	private GameObject root;

	//Public variable for the size of the terrain, width and heigth
	public Vector2 Size;
	public float terrainSize;


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

		//For loop for x-axis
		for(int i = 0; i <= Size.x; i++)
		{

			//For loop for z-axis
			for(int p = 0; p <= Size.y; p++)
			{

				GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);

				float noise = PerlinNoise (i, p);

				box.GetComponent<Renderer>().material.color = GenerateRandomColor();
				box.transform.position = new Vector3( i, noise , p);
				box.transform.parent = root.transform;

			}

		}

		//Move the root at the origin.
		root.transform.position = Vector3.zero;

	}
}