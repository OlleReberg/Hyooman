using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawWorld : MonoBehaviour
{
    [SerializeField] private WFCGenerator wfcGenerator;
    [SerializeField] private Tilemap backgroundTilemap;

    public void InitializeWorld()
    {
        if (backgroundTilemap == null)
        {
            Debug.LogError("Background Tilemap is not assigned in DrawWorld!");
            return;
        }

        // Clear any existing tiles before drawing new ones
        backgroundTilemap.ClearAllTiles();

        // Draw the world based on the collapsed tiles
        DrawTiles();
    }

    public void DrawTiles()
    {
        for (int y = 0; y < wfcGenerator.gridManager.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridManager.gridWidth; x++)
            {
                int tileIndex = wfcGenerator.gridManager.GetTileIndex(x, y);
               // Debug.Log($"Tile Index at ({x}, {y}): {tileIndex}");

                if (tileIndex < 0 || tileIndex >= wfcGenerator.tileConstraints.Count) continue;

                TileConstraint tileConstraint = wfcGenerator.tileConstraints[tileIndex];
                TileBase tile = tileConstraint.tile;
               // Debug.Log($"Tile at ({x}, {y}): {tileConstraint.tileType}");

                if (tile == null) continue;

                Vector3Int position = new Vector3Int(x, y, 0);
                backgroundTilemap.SetTile(position, tile);
            }
        }
    }
}






