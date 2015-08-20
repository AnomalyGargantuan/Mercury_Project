using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour 
{
	//Referencia ao Prefab contendo o Hex
	public GameObject HexPrefab;
	//Tamanho do Grid
	public int gridWidthInHexes = 10;
	public int gridHeightInHexes = 10;

	public string gridType = "HEX";
	
	//Hexagon tile width and height in game world
	public float hexWidth;
	public float hexHeight;
	
	//Renderer object of the Hex prefab
	public Renderer HexRenderer;
	
	//Map Storage
	List<List<HexTile>> map = new List<List<HexTile>>();

	void Start()
	{
		HexRenderer = HexPrefab.GetComponent<Renderer>();
		SetSizes();

	}

	//Method to initialise Hexagon width and height
	public void SetSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth = HexRenderer.bounds.size.x;
		hexHeight = HexRenderer.bounds.size.z;
	}

	//Method to calculate the position of the first hexagon tile. Center is at (0,0,0);
	Vector3 CalcInitPos()
	{
		Vector3 initPos;

		if(gridType.Equals("HEX"))
		{
			return new Vector3();
		}

		//the initial position will be in the left upper corner
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0, gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}

	Vector3 CalcWorldCoord(Vector2 gridPos)
	{
		Vector3 initPos = CalcInitPos();
		
		float offset = 0f;
		
		if (gridType.Equals("SQUARE"))
		{
			if(gridPos.y % 2 != 0)
			{
				offset = hexWidth / 2;
			}
		}
		else
		{
			offset = (hexWidth / 2) * gridPos.y;
		}
		
		
		float x = initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		
		return new Vector3(x, 0, z);
	}

	//Writes the coordinates on the HexField (Used for testing)
	public void setTextCoordinates(HexTile hex, string strCoordinates)
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


	void CreateTriangleGrid()
	{
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");
		
		
		for(float x = 0; x < gridHeightInHexes; x++)
		{
			List<HexTile> Row = new List<HexTile>();
			for(float y = 0; y < (gridWidthInHexes-x); y++)
			{
				HexTile hex = ((GameObject) Instantiate(HexPrefab)).GetComponent<HexTile>();
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				hex.gridPosition = new Vector3(x, y, 0);
				
				setTextCoordinates(hex, "(" + gridPos.x + ", " + gridPos.y + ")");
				
				hex.transform.position = CalcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;	
				Row.Add(hex);
			}
			map.Add(Row);
		}
	}

	void GenerateMap()
	{
	}


}
