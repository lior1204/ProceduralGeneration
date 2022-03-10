using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private GameObject[] tilePrefabs;
    private int chankXID = 0;
    private int chankYID = 0;
    [SerializeField] private string seed="generator";
    [SerializeField] private int width=20;
    [SerializeField] private int height=20;
    private GameObject[][] tiles;
    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
         tilePrefabs= Resources.LoadAll<GameObject>("Prefabs/Tiles");
    }
    private void GenerateTiles()
    {
        tiles = new GameObject[width][];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //int tileID = GenerateTileID(, int chunkY, int tileX, int tileY);
            }
        }
    }

    private int GenerateTileID(int chunkX, int chunkY, int tileX, int tileY)
    {
        return 0;
    }

    private float getRandomNumber(int chunkX,int chunkY,int tileX,int tileY)
    {
        float randomNumber = seed.GetHashCode();
        randomNumber += (tileX + tileY+ chunkX + chunkY) /100;
        randomNumber /= (tileX + tileY + chunkX + chunkY);
        randomNumber -= (int)randomNumber;
        return (int)(randomNumber * 100);
    }
}
