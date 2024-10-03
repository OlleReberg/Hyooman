using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFCGenerator : MonoBehaviour
{
    public static WFCGenerator Instance { get; private set; } // Singleton instance

    public int gridWidth = 10;
    public int gridHeight = 10;
    public TileConstraint[] tileConstraints;

    public Dictionary<TileType, List<int>> tileTypeToIndex; // Public for access in TileCollapseHandler
    private NativeArray<int> grid;
    private TileCollapseHandler tileCollapseHandler; // Reference to TileCollapseHandler

    void Awake()
    {
        // Implement the singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize TileCollapseHandler in Awake to avoid null references
        tileCollapseHandler = new TileCollapseHandler(this);
    }

    void Start()
    {
        InitializeTileTypeLookup();
        InitializeGrid();
        RunWaveFunctionCollapse();
    }

    public void RunWaveFunctionCollapse()
    {
        // Check if tileCollapseHandler is properly initialized
        if (tileCollapseHandler == null)
        {
            Debug.LogError("TileCollapseHandler is not initialized.");
            return;
        }

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int tileIndex = tileCollapseHandler.CollapseTileAt(x, y);
                grid[x + y * gridWidth] = tileIndex;
            }
        }
        ApplyGrid();
    }

    void InitializeTileTypeLookup()
    {
        tileTypeToIndex = new Dictionary<TileType, List<int>>(); // Use List<int> as the value type

        for (int i = 0; i < tileConstraints.Length; i++)
        {
            TileType currentType = tileConstraints[i].tileType;

            if (!tileTypeToIndex.ContainsKey(currentType))
            {
                tileTypeToIndex[currentType] = new List<int>(); // Initialize a new list for this TileType
            }

            // Add the index of the current TileConstraint to the list for this TileType
            tileTypeToIndex[currentType].Add(i);
        }
    }

    void InitializeGrid()
    {
        if (grid.IsCreated)
        {
            grid.Dispose();
        }

        grid = new NativeArray<int>(gridWidth * gridHeight, Allocator.Persistent);

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = -1;
        }
    }

    public TileType GetNeighborTileType(int x, int y)
    {
        // Ensure x and y are within bounds of the grid
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) 
            return TileType.None; // Return a default TileType if out of bounds

        int index = x + y * gridWidth;

        // Check if the index is valid within the grid array
        if (index < 0 || index >= grid.Length)
            return TileType.None;

        int neighborIndex = grid[index];

        // Return TileType.None if the tile is not yet collapsed or the index is invalid
        if (neighborIndex < 0 || neighborIndex >= tileConstraints.Length) 
            return TileType.None;

        return tileConstraints[neighborIndex].tileType;
    }

    public TileDirection GetNeighborTileDirection(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) 
            return TileDirection.Middle;

        int index = x + y * gridWidth;

        if (index < 0 || index >= grid.Length)
            return TileDirection.Middle;

        int neighborIndex = grid[index];

        if (neighborIndex < 0 || neighborIndex >= tileConstraints.Length) 
            return TileDirection.Middle;

        return tileConstraints[neighborIndex].tileDirection;
    }

    public TransitionType GetNeighborTransitionType(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) 
            return TransitionType.None;

        int index = x + y * gridWidth;

        if (index < 0 || index >= grid.Length)
            return TransitionType.None;

        int neighborIndex = grid[index];

        if (neighborIndex < 0 || neighborIndex >= tileConstraints.Length) 
            return TransitionType.None;

        return tileConstraints[neighborIndex].transitionType;
    }

    public TileConstraint GetTileConstraintByType(TileType tileType)
    {
        // Get the list of indices for the specified TileType
        if (tileTypeToIndex.TryGetValue(tileType, out List<int> indices))
        {
            // Return the first matching TileConstraint (adjust logic if needed for specific constraints)
            return tileConstraints[indices[0]];
        }
        
        return null;
    }

    void ApplyGrid()
    {
        FindObjectOfType<DrawWorld>().UpdateWorld();
    }

    public int GetTileIndex(int x, int y)
    {
        int index = x + y * gridWidth;
        return (index >= 0 && index < grid.Length) ? grid[index] : -1;
    }

    void OnDestroy()
    {
        if (grid.IsCreated)
        {
            grid.Dispose();
        }
    }
}



