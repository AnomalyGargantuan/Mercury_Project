using UnityEngine;
using System.Collections;

public class HexagonalGrid : GridController 
{
	void Start()
	{
		HexRenderer = HexPrefab.GetComponent<Renderer>();
		SetSizes();
		GenerateMap();
	}

	void GenerateMap()
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
				
				hex.transform.position = CalcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;	
			}
		}
	}

	public Vector3 CalcWorldCoord(Vector2 gridPos)
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
