using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridController : MonoBehaviour 
{
	public static HexGridController instance;
	//Referencia ao Prefab contendo o Hex
	public GameObject HexPrefab;
	//Tamanho do Grid
	public int gridWidthInHexes = 10;
	public int gridHeightInHexes = 10;

	public LineRenderer line;
	
	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;

	//Renderer object of the Hex prefab
	private Renderer HexRenderer;

	//Map Storage
	List<List<HexTile>> map = new List<List<HexTile>>();
	
	//The grid should be generated on game start
	void Start()
	{
		instance = this;
		//Get the Renderer component from the Hex prefab
		HexRenderer = HexPrefab.GetComponent<Renderer>(); 

		SetSizes();
		CreateRetangleGrid();
		//CreateTriangleGrid();
		//CreateHexagonalGrid();



		GameObject lineRenderer;

		
		lineRenderer = this.transform.Find("LineRenderer").gameObject;
		
		if(lineRenderer != null)
		{
			line = lineRenderer.GetComponent<LineRenderer>();
			line.SetPosition(0, map[0][0].transform.position);
			line.SetWidth(0.3f, 0.3f);
		}
		else
		{
			Debug.LogError("Could not find TextMesh object.");
		}


		for(int i = 0; i < map.Count; i ++)
		{
			List<HexTile> row = map[i];
			for(int j = 0; j < row.Count; j++)
			{
				HexTile tile = row[j];
				string message = "i: " + i + "j: " + j + " gridPos: " + tile.gridPosition;
				Debug.Log(message);
			}
		}
	}

	//Method to initialise Hexagon width and height
	void SetSizes()
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
	void CreateRetangleGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");
		
		for(float y = 0; y < gridHeightInHexes; y++)
		{
			List<HexTile> Row = new List<HexTile>();
			for(float x = 0; x < gridWidthInHexes; x++)
			{
				//GameObject assigned to Hex public variable is cloned
				HexTile hex = ((GameObject)Instantiate(HexPrefab)).GetComponent<HexTile>();
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				hex.gridPosition = gridPos;

				setTextCoordinates(hex, "(" + gridPos.x + ", " + gridPos.y + ")");

				hex.transform.position = CalcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				Row.Add(hex);
			}
			map.Add(Row);
		}
	}	

	void CreateTriangleGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");


		for(float x = 0; x < gridHeightInHexes ; x++)
		{
			List<HexTile> Row = new List<HexTile>();
			for(float y = 0; y < (gridWidthInHexes-x); y++)
			{
				//GameObject assigned to Hex public variable is cloned
				//GameObject hex = (GameObject)Instantiate(Hex);
				HexTile hex = ((GameObject) Instantiate(HexPrefab)).GetComponent<HexTile>();
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				hex.gridPosition = new Vector3(x, y, 0);

				setTextCoordinates(hex, "(" + gridPos.x + ", " + gridPos.y + ")");

				hex.transform.position = CalcTriangleWordCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;	
				Row.Add(hex);
			}
			map.Add(Row);
		}
	}

	public Vector3 CalcTriangleWordCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = CalcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0f;
		offset = (hexWidth / 2) * gridPos.y;

		float x = initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		
		return new Vector3(x, 0, z);
	}	

	//Writes the coordinates on the HexField (Used for testing)
	void setTextCoordinates(HexTile hex, string strCoordinates)
	{
		GameObject positionText;
		TextMesh text;

		positionText = hex.transform.Find("Position").gameObject;
		
		if(positionText != null)
		{
			text = positionText.GetComponent<TextMesh>();
			text.text = strCoordinates;
		}
		else
		{
			Debug.LogError("Could not find TextMesh object.");
		}
	}

	void CreateHexagonalGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");
		int mapRadius = 3;

		for(int i = -mapRadius; i <= mapRadius; i++)
		{
			int r1 = Mathf.Max(-mapRadius, -i - mapRadius);
			int r2 = Mathf.Min(mapRadius, -i + mapRadius);
			for(int r = r1; r <= r2; r++)
			{
				HexTile hex = ((GameObject) Instantiate(HexPrefab)).GetComponent<HexTile>();
				Vector2 gridPos = new Vector2(i, r);
				hex.gridPosition = new Vector3(i, r, 0);

				setTextCoordinates(hex, "(" + gridPos.x + ", " + gridPos.y + ")");

				hex.transform.position = CalcHexWordCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;	
			}
		}
	}

	public Vector3 CalcHexWordCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = new Vector3();
		//Every second row is offset by half of the tile width
		float offset = 0f;
		offset = (hexWidth / 2) * gridPos.y;
		
		float x = initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		
		return new Vector3(x, 0, z);
	}
}
