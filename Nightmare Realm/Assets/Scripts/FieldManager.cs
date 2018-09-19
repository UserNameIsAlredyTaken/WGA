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

	public GameObject winPanel;
	public Text resultText;
	public GameObject counterPanel;
	public Text counterText;
	
	private int[] spawnPositions = new int[15]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
	private List<Position> allPositions = new List<Position>();
	private static int MATRIX_SIZE = 5;
	private static int TILES_COUNT = 15;
	private int rightPositionTiles = 0;
	private int stepsCounter;
	private TileControl selectedTile;
	private Position selectedPosition;
	private Stopwatch timer = new Stopwatch();
	private TimeSpan resultTime;
	
	public TileControl SelectedTile
	{
		set { selectedTile = value; }
	}

	public Position SelectedPosition
	{
		set { selectedPosition = value; }
	}


	private void Awake()
	{
		Instance = this;
		CreatePositionsList();
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

	private void CreatePositionsList()
	{
		foreach (Transform child in transform)
		{
			allPositions.Add(child.GetComponent<Position>());
		}
	}

	private void ShuffleSpawnArr()
	{
		//Create random spawn of tiles
		Random random = new Random();
		for (var i = spawnPositions.Length - 1; i > 0; i--)
		{
			var t = random.Next(i + 1);
			var temp = spawnPositions[t];
			spawnPositions[t] = spawnPositions[i];
			spawnPositions[i] = temp;
		}
	}

	private void SpawnTiles()
	{
		GameObject[] tilesArr = GameObject.FindGameObjectsWithTag("tile");
		for (var i = 0; i < 15; i++)
		{
			var col = (spawnPositions[i] / MATRIX_SIZE) * 2;
			var row = spawnPositions[i] % MATRIX_SIZE;
			tilesArr[i].GetComponent<TileControl>().SetTilePosition(allPositions.Find(pos => pos.col == col && pos.row == row), false);
		}
	}
	
	public void MoveTile()
	{
		if (selectedTile != null && selectedPosition != null && PathCheck(selectedTile.currentPosition, selectedPosition, selectedTile.currentPosition))
		{
			selectedTile.SetTilePosition(selectedPosition, true);
			stepsCounter++;
		}
	}

	//Returns true if there is a path between start and finish
	private static bool PathCheck(Position start, Position finish, Position callerPosition)//PathCheck is recursive function and callerPosition is the position from where PathCheck was called
	{
		if (start == finish)
		{
			return true;
		}
		else
		{
			foreach (var neighbour in start.neighbours)
			{
				if (neighbour != null && neighbour != callerPosition && neighbour.currentColor == ColorOfPosition.EMPTY)
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
		rightPositionTiles += change;
		if (rightPositionTiles == TILES_COUNT)
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

	public void Quit()
	{
		Application.Quit();
	}
}
