using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class WFCGenerator : MonoBehaviour
{
    public static WFCGenerator Instance { get; private set; }

    public int gridWidth = 10;
    public int gridHeight = 10;
    public TileConstraint[] tileConstraints;

    public Tilemap backgroundTilemap; // Reference to the Background Tilemap
    public Dictionary<TileType, List<int>> tileTypeToIndex;
    private NativeArray<int> grid;
    private TileCollapseHandler tileCollapseHandler;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
                SetTileAt(x, y, tileIndex); // Use SetTileAt method
            }
        }
        ApplyGrid();
    }

    void InitializeTileTypeLookup()
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
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return TileType.None;

        int index = x + y * gridWidth;

        if (index < 0 || index >= grid.Length)
            return TileType.None;

        int neighborIndex = grid[index];

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
        if (tileTypeToIndex.TryGetValue(tileType, out List<int> indices))
        {
            return tileConstraints[indices[0]];
        }

        return null;
    }

    public void ApplyGrid()
    {
        if (backgroundTilemap == null)
        {
            Debug.LogError("Background Tilemap reference is missing!");
            return;
        }

        // Draw base terrain on the Background Tilemap
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int tileIndex = grid[x + y * gridWidth];
                TileBase tile = tileConstraints[tileIndex].tile;
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    public void SetTileAt(int x, int y, int tileIndex)
    {
        int index = x + y * gridWidth;
        if (index >= 0 && index < grid.Length)
        {
            grid[index] = tileIndex;
        }
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
    
    public void ClearWorld()
    {
        // Clear the terrain tiles on the background tilemap
        backgroundTilemap.ClearAllTiles();
        // Reset any other necessary data (e.g., grid, constraints)
    }
    public TileType GetTileTypeAt(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return TileType.None;

        int index = x + y * gridWidth;
        int tileIndex = grid[index];

        // Check if tileIndex is valid
        if (tileIndex < 0 || tileIndex >= tileConstraints.Length)
            return TileType.None;

        return tileConstraints[tileIndex].tileType;
    }


}


