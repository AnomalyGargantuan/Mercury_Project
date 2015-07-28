using UnityEngine;
using System.Collections;

public class HexGridController : MonoBehaviour 
{
	//Referencia ao Prefab contendo o Hex
	public GameObject Hex;
	//Tamanho do Grid
	public int gridWidthInHexes = 10;
	public int gridHeightInHexes = 10;
	
	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;

	private Renderer HexRenderer;
	
	//The grid should be generated on game start
	void Start()
	{
		//Get the Renderer component from the Hex prefab
		HexRenderer = Hex.GetComponent<Renderer>(); 
		
		SetSides();
		//CreateGrid();
		CreateTriangleGrid();
	}

	//Method to initialise Hexagon width and height
	void SetSides()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth = HexRenderer.bounds.size.x;
		hexHeight = HexRenderer.bounds.size.z;
	}

	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 CalcInitPos()
	{
		Vector3 initPos;
		//the initial position will be in the left upper corner
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0, gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}
	
	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 CalcWorldCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = CalcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0f;
		if(gridPos.y % 2 != 0)
		{
			offset = hexWidth / 2;
		}
		
		float x = initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		
		return new Vector3(x, 0, z);
	}
	
	//Finally the method which initialises and positions all the tiles
	void CreateGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");
		
		for(float y = 0; y < gridHeightInHexes; y++)
		{
			for(float x = 0; x < gridWidthInHexes; x++)
			{
				//GameObject assigned to Hex public variable is cloned
				GameObject hex = (GameObject)Instantiate(Hex);
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				
				hex.transform.position = CalcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
			}
		}
	}	

	void CreateTriangleGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");

		for(float x = 0; x < gridHeightInHexes ; x++)
		{
			for(float y = 0; y < (gridWidthInHexes-x); y++)
			{
				//GameObject assigned to Hex public variable is cloned
				GameObject hex = (GameObject)Instantiate(Hex);
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				
				hex.transform.position = CalcWorldCoord(gridPos); //Repensar posiçao, tringulo ficou estranho
				hex.transform.parent = hexGridGO.transform;
			}
		}
	}
}
