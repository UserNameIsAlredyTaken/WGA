using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TileControl : MonoBehaviour
{
	public Position currentPosition;
	public ColorOfPosition tileColor;
	public AudioSource audioSource;

	public void SetTilePosition(Position newPosition, bool playSound)
	{
		if (tileColor == newPosition.desiredColor && tileColor != currentPosition.desiredColor)
		{
			FieldManager.Instance.WinCheck(1);
		}
		else if(tileColor == currentPosition.desiredColor && tileColor != newPosition.desiredColor)
		{
			FieldManager.Instance.WinCheck(-1);
		}
		
		currentPosition.currentColor = ColorOfPosition.EMPTY;
		currentPosition = newPosition;
		currentPosition.currentColor = tileColor;
		transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, -0.1f);//Tile should always be higher then position to overlap it's collider
		if(playSound) audioSource.Play();
	}

	private void OnMouseDown()
	{
		FieldManager.Instance.SelectedTile = this;
	}
}
