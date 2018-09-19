using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    
    public ColorOfPosition desiredColor;
    public int col, row;
    public ColorOfPosition currentColor = ColorOfPosition.EMPTY;

    public Position[] neighbours;
    
    private void OnMouseDown()
    {
        FieldManager.Instance.SelectedPosition = this;
        FieldManager.Instance.MoveTile();
    }
    
}