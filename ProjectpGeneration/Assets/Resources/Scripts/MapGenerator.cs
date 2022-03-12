using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private GameObject[] tilePrefabs;
    [Space(2)]
    [Header("Parameters")]
    [SerializeField] private string seed = "generator";
    [Header("Distribution")]
    [SerializeField] [Range(1,30)] private int grassDistribution = 10;
    [SerializeField] [Range(1,15)] private int mountainDistribution = 3;
    [SerializeField] [Range(1,15)] private int waterDistribution = 5;
    [Header("Clustering")]
    [SerializeField] [Range(1,10)] private int grassClustering = 2;
    [SerializeField] [Range(1,10)] private int mountainClustering = 4;
    [SerializeField] [Range(1,10)] private int waterClustering = 3;

    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;
    [SerializeField] private int chunkXID = 0;
    [SerializeField] private int chunkYID = 0;



    private GameObject[,] tiles;
    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
        GenerateTiles();
    }

    private void InitializeReferences()
    {
        //tilePrefabs = Resources.LoadAll<GameObject>("Prefabs/Tiles");
    }
    private void GenerateTiles()
    {
        tiles = new GameObject[width,height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //yield return new WaitForSeconds(0);
                int tileID = GenerateTileID(chunkXID, chunkYID, i, j);
                tiles[i,j] = Instantiate(tilePrefabs[tileID]);
                tiles[i,j].transform.parent=gameObject.transform;
                tiles[i,j].transform.localPosition=new Vector3(i+0.5f,j+0.5f,0);
            }
        }
    }

    private int GenerateTileID(int chunkX, int chunkY, int tileX, int tileY)
    {
        int randomNumber = GetRandomNumber(chunkX, chunkY, tileX, tileY);
        Debug.Log("Random Number" + randomNumber);
        float currGrass = grassDistribution;
        float currMountain = mountainDistribution;
        float currWater = waterDistribution;
        for(int i = tileX - 1; i <= tileX + 1; i++)
        {
            if(i>=0&&i<width)
            {
                for (int j = tileY-1; j < tileY + 1; j++)
                {
                    if (j >= 0 && j < height)
                    {
                        if (tiles[i, j] != null)
                        {
                            //Debug.Log("AA");
                            if (tiles[i, j].tag == "Grass")
                            {
                                currGrass += grassClustering;
                                //Debug.Log("Grass: " + currGrass);
                            }
                            else if (tiles[i, j].tag == "Mountain")
                            {
                                currMountain += mountainClustering;
                                //Debug.Log("Mountain: " + currMountain);
                            }
                            else if (tiles[i, j].tag == "Water")
                            {
                                currWater += waterClustering;
                                //Debug.Log("Water: " + currWater);
                            }
                        }
                    }
                }
            }
        }
        float scaleToPercentage = 100 / (currGrass+currMountain+currWater);
        currGrass *= scaleToPercentage;
        currMountain *= scaleToPercentage;
        currWater *= scaleToPercentage;
        //Debug.Log("Random: "+randomNumber);
        //Debug.Log("Grass: "+currGrass);
        //Debug.Log("Mountain: "+currMountain);
        //Debug.Log("Water: "+currWater);
        if (randomNumber < currGrass) return 0;
        else if (randomNumber < currGrass+currMountain) return 1;
        else return 2;
    }

    private int GetRandomNumber(int chunkX, int chunkY, int tileX, int tileY)
    {
        int temp1,temp2;
        int seedNum = Mathf.Abs(seed.GetHashCode());//get a number from seed
        int tileOffset = Mathf.Abs((("CX" + chunkX.ToString()).GetHashCode()//get a number from tile location
            * ("TX" + tileX.ToString()).GetHashCode()
            + ("CY" + chunkY.ToString()).GetHashCode() )
            * ("TY" + tileY.ToString()).GetHashCode());
        //Debug.Log(tileOffset);
        int length = 0;
        temp1 = seedNum;
        while (temp1 > 0)//calc length of seednum
        {
            length++;
            temp1 /= 10;
        }
        temp1 = 0;
        temp2 = tileOffset;
        while (temp2 > 0)//calc sum of tileoffset digits
        {
            temp1+=temp2%10;
            temp2 /= 10;
        }
        //choose a digit from seednum using prev calc in temp
        temp1 %= length;//make sure temp is not bigger than length
        seedNum /= 10 ^ temp1;
        seedNum = seedNum % 10;//seednum holds chosen digit
        length = 0;
        temp1 = tileOffset;
        while (temp1 > 0)//calc length of seednum
        {
            length++;
            temp1 /= 10;
        }
        seedNum %= length;//make sure sednum(digit) is not bigger than length
        tileOffset /= 10 ^ (seedNum - 1);//make tileoffset to chosen digits
        int RandomNum = tileOffset % 10 + (((tileOffset / 10) % 10) * 10);//get last 2 digits from tileoffset
        return Mathf.Abs(RandomNum);
    }

    public void Remap()
    {
        foreach(GameObject tile in tiles)
        {
            if(tile!=null)
            Destroy(tile);
        }
        GenerateTiles();
    }
}
