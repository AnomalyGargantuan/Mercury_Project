using UnityEngine;
using System.Collections;

public class SquareTile : MonoBehaviour 
{
	public Vector2 gridPosition = Vector2.zero;
	private Renderer rend;
	
	void Start() 
	{
		rend = GetComponent<Renderer>();
	}
	
	void OnMouseEnter () 
	{
		rend.material.color = Color.red;
		Debug.Log("My grid position is (" + gridPosition.x + ", " + gridPosition.y + ")");
	}
	
	void OnMouseExit () 
	{
		rend.material.color = Color.white;
	}
	
	void OnMouseDown()
	{
		//GameController.instance.moveCurrentPlayer(this);
	}
}
