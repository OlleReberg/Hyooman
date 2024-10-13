using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacementHandler : MonoBehaviour
{
    [SerializeField] private WFCGenerator wfcGenerator; // Reference to the WFCGenerator
    [SerializeField] private Tilemap solidObjectsTilemap; // Tilemap for solid objects
    [SerializeField] private List<SolidObjectTile> solidObjectTiles; // List of solid objects

    public void ProcessAndPlaceObjects()
    {
        List<Vector3Int> placedPositions = new List<Vector3Int>(); // Track placed positions

        // Iterate over each type of solid object
        foreach (var solidObject in solidObjectTiles)
        {
            // Calculate number of placements based on grid size and weight
            int numberOfPlacements = Mathf.RoundToInt(wfcGenerator.gridWidth * wfcGenerator.gridHeight * solidObject.weight);

            for (int i = 0; i < numberOfPlacements; i++)
            {
                // Try to find a valid position for this object
                for (int attempt = 0; attempt < 10; attempt++) // Give 10 attempts to find a valid position
                {
                    int x = Random.Range(0, wfcGenerator.gridWidth);
                    int y = Random.Range(0, wfcGenerator.gridHeight);

                    // Make sure the position respects spacing and is not too close to others
                    if (ShouldPlaceObjectHere(x, y, placedPositions))
                    {
                        PlaceSolidObject(x, y, solidObject);
                        placedPositions.Add(new Vector3Int(x, y, 0));
                        break; // Exit the attempt loop after successful placement
                    }
                }
            }
        }
    }

    private bool ShouldPlaceObjectHere(int x, int y, List<Vector3Int> placedPositions)
    {
        // Ensure spacing logic (e.g., 5 tiles apart from other objects)
        foreach (var position in placedPositions)
        {
            if (Vector3Int.Distance(new Vector3Int(x, y, 0), position) < 5f) // Adjust distance threshold as needed
            {
                return false; // Skip placement if too close to another object
            }
        }
        return true; // Place the object if the position is valid
    }

    private void PlaceSolidObject(int x, int y, SolidObjectTile selectedObject)
    {
        int clusterSize = Random.Range(selectedObject.minClusterSize, selectedObject.maxClusterSize);

        for (int i = 0; i < clusterSize; i++)
        {
            // Generate random positions nearby for cluster placement
            int offsetX = Random.Range(-2, 2); // Adjust for proximity
            int offsetY = Random.Range(-2, 2); // Adjust for proximity

            Vector3Int tilePosition = new Vector3Int(x + offsetX, y + offsetY, 0);

            // Ensure the position is valid and within bounds of the grid
            if (tilePosition.x >= 0 && tilePosition.y >= 0 &&
                tilePosition.x < wfcGenerator.gridWidth && tilePosition.y < wfcGenerator.gridHeight)
            {
                foreach (var tileData in selectedObject.tiles)
                {
                    Vector3Int adjustedPosition = new Vector3Int(tilePosition.x + tileData.position.x, tilePosition.y + tileData.position.y, 0);
                    solidObjectsTilemap.SetTile(adjustedPosition, tileData.tile);
                }
            }
        }
    }

    public void ClearObjects()
    {
        if (solidObjectsTilemap != null)
        {
            solidObjectsTilemap.ClearAllTiles();
        }
        else
        {
            Debug.LogError("SolidObjects Tilemap is not assigned in ObjectPlacementHandler!");
        }
    }
}




