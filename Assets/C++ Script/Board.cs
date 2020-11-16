using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] dots;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
         for (int i = 0; i<width; i++)
        {
            for(int j = 0; j<height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition , Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + " , " + j + " )";
                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0;

                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100) //As long as MatchesAt is true it will keep choosing random dots.Untill it is not true
                {
                    dotToUse = Random.Range(0, dots.Length); //We keep choosing a random Dot untill MatchesAt(Boolean) Returns false
                    maxIterations++; //simple check that allows us to avoid an infinite loop
                }
                maxIterations = 0;//reset the counter of max iterations

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + " , " + j + " )";
                allDots[i, j] = dot;
            }
        }

    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column- 2,row].tag == piece.tag) //we check if the previous dot in our column is the same as the current dot.
            {
                return true;//if it is true we return true on our boolean
            }
            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)//we check if the previous dot in our Row is the same as the current dot.
            {
                return true;//if it is true we return true on our boolean
            }
        }else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allDots[column, row -1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;//If both checkes before are false we return false to our boolean
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int i=0; i < width ; i++)
        {
            for(int j =0; j < height; j++)
            {
                if(allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
    }

}
