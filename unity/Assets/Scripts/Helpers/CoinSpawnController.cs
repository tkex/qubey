using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinSpawnController : MonoBehaviour
{
    // Prefab of the coin to spawn
    public GameObject coinPrefab;

    // Number of coins to spawn in X direction
    public int numCoinsX = 3;

    // Number of coins to spawn in Y direction
    public int numCoinsY = 3;

    // Number of coins to spawn in Z direction
    public int numCoinsZ = 3; 

    public float coinSpacing = 1.0f; // Distance between each coin

    // X offset of the grid from the spawn point
    public float gridOffsetX = 0.0f;

    // Y offset of the grid from the spawn point
    public float gridOffsetY = 0.0f;

    // Z offset of the grid from the spawn point
    public float gridOffsetZ = 0.0f;

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int z = 0; z < numCoinsZ; z++)
        {
            for (int y = 0; y < numCoinsY; y++)
            {
                for (int x = 0; x < numCoinsX; x++)
                {
                    float xPos = x * coinSpacing + gridOffsetX;
                    float yPos = y * coinSpacing + gridOffsetY;
                    float zPos = z * coinSpacing + gridOffsetZ;

                    // Set spawn position of coin
                    Vector3 spawnPos = new Vector3(xPos, yPos, zPos);

                    // Spawn coins
                    Instantiate(coinPrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
