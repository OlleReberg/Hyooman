using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile Constraint")]
public class TileConstraint : ScriptableObject
{
    public GameObject tilePrefab;
    public int[] allowedTop;
    public int[] allowedBottom;
    public int[] allowedLeft;
    public int[] allowedRight;
}
