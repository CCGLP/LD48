using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationController : MonoBehaviour
{

    public static GenerationController Instance;

    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private int depthGeneration = 20;
    [SerializeField]
    private int width = 30, height = 30;

    [SerializeField]
    private List<GameObject> prefabLevelList = null;
    [SerializeField]
    private GameObject betaLevel = null;
    private GameObject lastParent;
    private int positionY = 0;

    [SerializeField]
    private GameObject heartPrefab;

    [SerializeField]
    private GameObject levelDefault;
    public List<GameObject> hearts = null;
    private int actualNumberOfHearts = 2;
    void Start()
    {
        CubeController.actualDifficulty = 2; 
        hearts = new List<GameObject>();
        Instance = this;
        GenerateLayer();
    }

    private void GenerateLayer(float x = 0, float y = -11)
    {
        CubeController.actualDifficulty++;
        var originX = x - width / 2;
        var originY = y - height / 2;
        positionY -= depthGeneration - 10;
        var goParent = new GameObject("Layer" + positionY.ToString());


        List<Vector3Int> heartList = new List<Vector3Int>();
        for (int i = 0; i < actualNumberOfHearts; i++)
        {
            heartList.Add(new Vector3Int(Random.Range(3, width - 3), Random.Range(3, depthGeneration - 3), Random.Range(3, height)));
        }


        for (int z = 0; z < depthGeneration; z++)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!InstantiateHearts(heartList, j, i, z))
                    {
                        var go = Instantiate(cubePrefab, new Vector3(originX, positionY, originY), Quaternion.identity);


                        if (z == depthGeneration - 1)
                        {
                            go.tag = "Finish";
                        }
                        go.transform.parent = goParent.transform;
                        if (z > 0)
                        {
                            go.GetComponent<Renderer>().enabled = false;
                        }
                        originX++;
                    }
                    else
                    {
                        var heart = Instantiate(heartPrefab, new Vector3(originX, positionY, originY), Quaternion.identity);
                        heart.transform.parent = goParent.transform;
                        hearts.Add(heart);
                    }
                }
                originY++;
                originX = x - width / 2;

            }
            originY = y - height / 2;
            positionY--;
        }

        lastParent = goParent;
    }




    private bool CheckHeart(List<Vector3Int> hearts, int x, int y, int depth)
    {
        bool value = false;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (x > hearts[i].x - 3 && x < hearts[i].x + 3 && depth > hearts[i].y - 3 && depth < hearts[i].y + 3 && y > hearts[i].z - 3 && y < hearts[i].z + 3)
            {

            }
        }


        return value;
    }


    private bool InstantiateHearts(List<Vector3Int> hearts, int x, int y, int depth)
    {
        bool value = false;
        for (int i = 0; i < hearts.Count; i++)
        {
            if (x == hearts[i].x && y == hearts[i].z && depth == hearts[i].y)
            {
                return true;
            }
        }

        return value;
    }


    public void InstantiateLevel(Vector3 lastCubePosition)
    {
        GameObject go = null;

        if (prefabLevelList.Count > 0)
        {
            int aux = Random.Range(0, prefabLevelList.Count);
            go = Instantiate(prefabLevelList[aux]);
            prefabLevelList.RemoveAt(aux);
        }
        else
        {
            go = Instantiate(levelDefault);
        }
        positionY -= depthGeneration - 10;
        go.transform.position = new Vector3(lastCubePosition.x, positionY, lastCubePosition.z);
        positionY -= depthGeneration;
        Destroy(lastParent);
        actualNumberOfHearts += 2;
        GenerateLayer(lastCubePosition.x - 20, lastCubePosition.z);
        PlayerController.Instance.NextDebuff();
    }

    public void InstantiateBetaTesterLevel(Vector3 position)
    {

        betaLevel.transform.position = position + Vector3.down * 30;
    }

    void Update()
    {

    }
}
