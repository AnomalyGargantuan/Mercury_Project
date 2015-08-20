using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HexTile : MonoBehaviour 
{
	public Vector3 gridPosition = Vector3.zero; //Position in the Grid
	public GameObject hex; //Hex Prefab

	private Renderer rend;
	private string distance = "";

	private HexGridController hexGrid;

	//The grid should be generated on game start
	void Start()
	{
		//Get the Renderer component from the Hex prefab
		rend = hex.GetComponent<Renderer>();
		hexGrid = HexGridController.instance;
	}

	void OnMouseEnter () 
	{
		rend.material.color = Color.gray;
		Debug.Log("My grid position is (" + gridPosition.x + ", " + gridPosition.y + ")");
		hexGrid.line.SetPosition(1, this.transform.position);
	}
	
	void OnMouseExit () 
	{
		rend.material.color = Color.white;
	}

	void setDistance(string distance)
	{
		this.distance = distance;
	}
}
