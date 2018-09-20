using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    
    public TileColor DesiredTileColor;
    public int col, row;
    public TileControl currentTile;

    public Cell[] neighbours;
    
    private void OnMouseDown()
    {
        FieldManager.Instance.SelectedCell = this;
        FieldManager.Instance.MoveTile();
    }
    
}