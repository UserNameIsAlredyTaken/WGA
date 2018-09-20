using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TileControl : MonoBehaviour
{
	public Cell currentCell;
	public TileColor TileColor;
	public AudioSource audioSource;

	public void SetTilePosition(Cell newCell, bool playSound)
	{
		if (TileColor == newCell.DesiredTileColor && TileColor != currentCell.DesiredTileColor)//The tile was not on it's appropriate place, but now it is 
		{
			FieldManager.Instance.WinCheck(1);
		}
		else if(TileColor == currentCell.DesiredTileColor && TileColor != newCell.DesiredTileColor)//The tile was on it's appropriate place, but now it is not 
		{
			FieldManager.Instance.WinCheck(-1);
		}

		currentCell.currentTile = null;
		currentCell = newCell;
		currentCell.currentTile = this;
		transform.position = new Vector3(newCell.transform.position.x, newCell.transform.position.y, -0.1f);//Tile should always be higher then position to overlap it's collider
		if(playSound) audioSource.Play();
	}

	private void OnMouseDown()
	{
		FieldManager.Instance.SelectedTile = this;
	}
}
