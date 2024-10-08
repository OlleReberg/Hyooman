using UnityEngine;

[CreateAssetMenu(menuName = "Tile/Decorative")]
public class DecorativeTile : BaseTile
{
    public int minClusterSize = 1; // Minimum size of the cluster
    public int maxClusterSize = 3; // Maximum size of the cluster

    // Method to get a random cluster size within the specified range
    public int GetRandomClusterSize()
    {
        return Random.Range(minClusterSize, maxClusterSize + 1); // +1 because max is exclusive in Random.Range
    }
}
