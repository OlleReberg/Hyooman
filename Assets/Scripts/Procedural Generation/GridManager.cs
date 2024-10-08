using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int gridWidth { get; set; }
    public int gridHeight { get; set; }

    private NativeArray<int> grid;
    private TileConstraint[] tileConstraints;

    // Dictionary to map tile types to their corresponding indices
    public Dictionary<TileType, List<int>> tileTypeToIndex { get; private set; }

    // Initializes the GridManager from the Unity inspector
    public void Initialize(TileConstraint[] constraints)
    {
        tileConstraints = constraints;
        InitializeTileTypeLookup(); // Populate the tileTypeToIndex dictionary
        InitializeGrid();
    }

    // Initializes the tile type to index dictionary
    private void InitializeTileTypeLookup()
    {
        tileTypeToIndex = new Dictionary<TileType, List<int>>();

        for (int i = 0; i < tileConstraints.Length; i++)
        {
            TileType currentType = tileConstraints[i].tileType;

            if (!tileTypeToIndex.ContainsKey(currentType))
            {
                tileTypeToIndex[currentType] = new List<int>();
            }

            tileTypeToIndex[currentType].Add(i);
        }
    }

    // Initializes the grid
    public void InitializeGrid()
    {
        if (grid.IsCreated)
        {
            grid.Dispose();
        }

        grid = new NativeArray<int>(gridWidth * gridHeight, Allocator.Persistent);

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = -1; // Unassigned tile indicator
        }
    }

    // Clears the grid by resetting tile values
    public void ClearGrid()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = -1; // Reset to unassigned tile
        }
    }

    // Gets the tile type at the given coordinates
    public TileType GetTileTypeAt(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return TileType.None;

        int index = x + y * gridWidth;
        int tileIndex = grid[index];

        if (tileIndex >= 0 && tileIndex < tileConstraints.Length)
        {
            return tileConstraints[tileIndex].tileType;
        }

        return TileType.None;
    }

    // Gets the tile index at the given coordinates
    public int GetTileIndex(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return -1;

        return grid[x + y * gridWidth];
    }

    // Sets the tile index at the specified coordinates
    public void SetTileIndex(int x, int y, int tileIndex)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return;

        grid[x + y * gridWidth] = tileIndex;
    }

    // New method to set a tile's type during biome generation
    public void SetTileType(int x, int y, TileType tileType)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return;

        // Find the list of indices for this TileType
        if (tileTypeToIndex.TryGetValue(tileType, out List<int> indices))
        {
            // Pick the first available index (or implement your own logic to select from multiple)
            if (indices.Count > 0)
            {
                SetTileIndex(x, y, indices[0]);
            }
        }
    }

    // Disposes of the grid's memory
    public void DisposeGrid()
    {
        if (grid.IsCreated)
        {
            grid.Dispose();
        }
    }
}





