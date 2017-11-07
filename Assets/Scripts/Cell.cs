using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public enum States {
		Dead, Alive
	}

	public Material livingMaterial;

	public Material deadMaterial;

	 [HideInInspector] public GameOfLife gameOfLife;
	[HideInInspector] public int x, y;
	[HideInInspector] public Cell[] neighbours;


	[HideInInspector] public States state;
	private States nextState;

	private MeshRenderer meshRenderer;


	void Awake () {
		
		meshRenderer = GetComponent <MeshRenderer> ();
	}


	public void CellUpdate () {
		
		nextState = state;
		int aliveCells = GetAliveCells ();
		if (state == States.Alive) { 
			
			if (aliveCells != 2 && aliveCells != 3) 
				nextState = States.Dead;
		} else {
			if (aliveCells == 3)
				
				nextState = States.Alive;
		}
	}


	public void CellApplyUpdate () {
		
		state = nextState;
		UpdateMaterial ();
	}


	public void Init (GameOfLife gol, int x, int y) {
		gameOfLife = gol;
		transform.parent = gol.transform;


		this.x = x;
		this.y = y;
	}


	public void SetRandomState () {
		state = (Random.Range (0, 2) == 0) ? States.Dead : States.Alive;

		UpdateMaterial ();
	}


	private void UpdateMaterial () {
		
		if (state == States.Alive)
			meshRenderer.sharedMaterial = livingMaterial;
		else
			meshRenderer.sharedMaterial = deadMaterial;
	}

	private int GetAliveCells () {
		int ret = 0;

		for (int i = 0; i < neighbours.Length; i++) {
			
			if (neighbours[i] != null && neighbours [i].state == States.Alive)
					ret++;
		}
		return ret;
	}
}
