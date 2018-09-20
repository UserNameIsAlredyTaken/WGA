using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = System.Random;


public class FieldManager : MonoBehaviour
{
	public static  FieldManager Instance { get; private set; } // Making FieldManager singleton
	private const int MATRIX_SIZE = 5;
	private const int TILES_COUNT = 15;

	public GameObject winPanel;
	public Text resultText;
	public GameObject counterPanel;
	public Text counterText;
	
	private int[] spawnCells = new int[15]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
	private List<Cell> cellsArr;
	private GameObject[] tilesArr;
	private int rightCellsTiles = 0;
	private int stepsCounter;
	private TileControl selectedTile;
	private Cell selectedCell;
	private Stopwatch timer = new Stopwatch();
	private TimeSpan resultTime;
	
	public TileControl SelectedTile{set { selectedTile = value; }}
	public Cell SelectedCell{set { selectedCell = value; }}

	private void Awake()
	{
		Instance = this;
		FindAllCellsAndTiles();
		ShuffleSpawnArr();
		winPanel.SetActive(false);
	}

	private void Start()
	{
		SpawnTiles();
		timer.Start();
	}
	
	private void Update()
	{
		resultTime = timer.Elapsed;
		counterText.text = "Steps: " + stepsCounter + 
		                   "\nTime: " + String.Format("{0:00}:{1:00}", resultTime.Minutes, resultTime.Seconds);
	}

	private void FindAllCellsAndTiles()
	{
		cellsArr = new List<Cell>();
		foreach (Transform child in transform)
		{
			cellsArr.Add(child.GetComponent<Cell>());
		}
		
		tilesArr = GameObject.FindGameObjectsWithTag("tile");
	}

	private void ShuffleSpawnArr()
	{
		//Create random spawn of tiles
		Random random = new Random();
		for (var i = spawnCells.Length - 1; i > 0; i--)
		{
			var t = random.Next(i + 1);
			var temp = spawnCells[t];
			spawnCells[t] = spawnCells[i];
			spawnCells[i] = temp;
		}
	}

	private void SpawnTiles()
	{
		for (var i = 0; i < 15; i++)
		{
			var col = (spawnCells[i] / MATRIX_SIZE) * 2;
			var row = spawnCells[i] % MATRIX_SIZE;
			tilesArr[i].GetComponent<TileControl>().SetTilePosition(cellsArr.Find(pos => pos.col == col && pos.row == row), false);
		}
	}
	
	public void MoveTile()
	{
		if (selectedTile != null && 
		    selectedCell != null && 
		    PathCheck(selectedTile.currentCell, selectedCell, selectedTile.currentCell))
		{
			selectedTile.SetTilePosition(selectedCell, true);
			stepsCounter++;
		}
	}

	//Returns true if there is a path between start and finish
	private static bool PathCheck(Cell start, Cell finish, Cell callerCell)//PathCheck is recursive function and callerPosition is the position from where PathCheck was called
	{
		if (start == finish)
		{
			return true;
		}
		else
		{
			foreach (var neighbour in start.neighbours)
			{
				if (neighbour != null && neighbour != callerCell && neighbour.currentTile == null)
				{
					if (PathCheck(neighbour, finish, start)) return true;
				}
			}
		}
		return false;
	}

	//Check the win condition and show result
	public void WinCheck(int change)//If tile has come onto it's right position change = 1, if tile has gone from it's position change = -1
	{
		rightCellsTiles += change;
		Debug.Log(rightCellsTiles);
		if (rightCellsTiles == TILES_COUNT)
		{
			resultText.text = "Your steps: " + stepsCounter + 
			                  "\nYour time: " + String.Format("{0:00}:{1:00}.{2:00}", resultTime.Minutes, resultTime.Seconds, resultTime.Milliseconds);
			counterPanel.SetActive(false);
			winPanel.SetActive(true);
		}
	}

	public void RestartGame()
	{
		ShuffleSpawnArr();
		winPanel.SetActive(false);
		counterPanel.SetActive(true);
		SpawnTiles();
		stepsCounter = 0;
		timer.Reset();
		timer.Start();
	}
}
