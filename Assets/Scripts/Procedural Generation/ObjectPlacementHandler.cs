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
        List<Vector3Int> placedPositions = new List<Vector3Int>(); // List to track positions of placed objects

        for (int y = 0; y < wfcGenerator.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridWidth; x++)
            {
                // Check if we should place an object here, using the placed positions to ensure spacing
                if (ShouldPlaceObjectHere(x, y, placedPositions))
                {
                    PlaceSolidObject(x, y);
                    placedPositions.Add(new Vector3Int(x, y, 0)); // Store the position of the placed object
                }
            }
        }
    }
    private bool ShouldPlaceObjectHere(int x, int y, List<Vector3Int> placedPositions)
    {
        float totalWeight = solidObjectTiles.Sum(obj => obj.weight);
        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        foreach (var obj in solidObjectTiles)
        {
            cumulativeWeight += obj.weight;
            Debug.Log($"Object: {obj.name}, Weight: {obj.weight}, Cumulative: {cumulativeWeight}");

            if (randomValue <= cumulativeWeight)
            {
                Debug.Log($"Placing object: {obj.name} at position ({x}, {y})");

                // Spacing check
                foreach (var position in placedPositions)
                {
                    if (Vector3Int.Distance(new Vector3Int(x, y, 0), position) < 5f)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        return false;
    }

    private void PlaceSolidObject(int x, int y)
    {
        if (solidObjectTiles.Count > 0)
        {
            // Sum of all object weights
            float totalWeight = solidObjectTiles.Sum(obj => obj.weight);
            float randomValue = Random.Range(0, totalWeight);
            float cumulativeWeight = 0f;

            Debug.Log($"Total object weight for selection: {totalWeight}");
            Debug.Log($"Random value for selection: {randomValue}");

            foreach (var solidObject in solidObjectTiles)
            {
                cumulativeWeight += solidObject.weight;

                Debug.Log($"Checking Object: {solidObject.name}, Cumulative Weight: {cumulativeWeight}");

                if (randomValue <= cumulativeWeight)
                {
                    Debug.Log($"Selected Object: {solidObject.name} at {x}, {y}");

                    foreach (var tileData in solidObject.tiles)
                    {
                        Vector3Int tilePosition = new Vector3Int(x + tileData.position.x, y + tileData.position.y, 0);
                        solidObjectsTilemap.SetTile(tilePosition, tileData.tile);
                    }

                    return; // Stop once an object is placed
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


