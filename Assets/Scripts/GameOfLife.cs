using UnityEngine;
using System.Collections;
using System;

public class GameOfLife : MonoBehaviour {

	public enum States {
		
		Idle, Running
	}

	public Cell cellPrefab;

	public float updateInterval = 0.1f;  


	 public Cell[,] cells; 
	public States state = States.Idle;

	 public int sizeX; 
 	 public int sizeY; 

	 private Action cellUpdates; 
	private Action cellApplyUpdates; 

	private IEnumerator coroutine; 

	void Awake () {
		Init (50, 50); 

		Run (); 
	}

	public void Init (int x, int y) {
		
		if (cells != null) {
			for (int i = 0; i < sizeX; i++) {
				
				for (int j = 0; j < sizeY; j++) {
					GameObject.Destroy (cells [i, j].gameObject);

				      }
			}
		}

	
		cellUpdates = null;
		cellApplyUpdates = null;

		coroutine = null;

		sizeX = x;
		sizeY = y;
		SpawnCells (sizeX, sizeY);
	}


	public void UpdateCells () {
		cellUpdates ();
		cellApplyUpdates ();

	 }

	public void SpawnCells (int x, int y) {
		cells = new Cell[x, y];
		for (int i = 0; i < x; i++) {
			
			for (int j = 0; j < y; j++) {
				
				Cell c = Instantiate (cellPrefab, new Vector3 ((float)i, (float)j, 0f), Quaternion.identity) as Cell; 
				cells [i, j] = c;
				c.Init (this, i, j); 
				c.SetRandomState (); 

			
				cellUpdates += c.CellUpdate;
				cellApplyUpdates += c.CellApplyUpdate;
			}
		}

	
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				cells [i, j].neighbours = GetNeighbours (i, j);
			}
		}
	}


	public Cell[] GetNeighbours (int x, int y) {
		
		Cell[] result = new Cell[8];
		result[0] = cells[x, (y + 1) % sizeY];
		result[1] = cells[(x + 1) % sizeX, (y + 1) % sizeY];
		result[2] = cells[(x + 1) % sizeX, y % sizeY]; 
		result[3] = cells[(x + 1) % sizeX, (sizeY + y - 1) % sizeY]; 
		result[4] = cells[x % sizeX, (sizeY + y - 1) % sizeY]; 
		result[5] = cells[(sizeX + x - 1) % sizeX, (sizeY + y - 1) % sizeY]; 
		result[6] = cells[(sizeX + x - 1) % sizeX, y % sizeY]; 
		result[7] = cells[(sizeX + x - 1) % sizeX, (y + 1) % sizeY];

		return result;
	}


	public void Run () {
		
		state = States.Running;
		if (coroutine != null)
			StopCoroutine (coroutine);
		
		coroutine = RunCoroutine ();
		StartCoroutine (coroutine);
	}

	private IEnumerator RunCoroutine () {
		while (state == States.Running) {
			UpdateCells (); 
			yield return new WaitForSeconds (updateInterval); 
		}
	}
}
