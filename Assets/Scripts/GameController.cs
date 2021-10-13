using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    internal static GameController instance;

    [SerializeField]
    InputAction pauseInput;
    [SerializeField]
    public readonly int maxScoreIncrease = 50;

    [Header("Player Performance")]
    [SerializeField]
    int score;
    [SerializeField]
    float timeRemaining = 10;

    [Header("Item Handler")]
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    List<Material> itemMatList;
    [SerializeField]
    List<Transform> itemSpawnPoints;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        SetupInput();
        SpawnItem();
    }

    void SetupInput()
    {
        //Adds function to input. Only calls when performed (pressed, not held or up)
        pauseInput.performed += UIHandler.instance.PauseInput;

        //Input functions enabled (this is needed or the input wont work)
        pauseInput.Enable();
    }

    internal void SpawnItem()
    {
        //Makes local variables randomly choosing a mat and spawn point within the list
        Transform selectedSpawnPoint = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count)];
        Material selectedMat = itemMatList[Random.Range(0, itemMatList.Count)];

        //Creates the item and initiates the variables for position and material
        GameObject itemObject = Instantiate(itemPrefab);
        if(itemObject.TryGetComponent(out Item item))
        {
            item.transform.position = selectedSpawnPoint.transform.position;
            item.transform.localScale = selectedSpawnPoint.transform.localScale;
            item.SetMaterial(selectedMat);
        }
    }

    protected internal int AddedScore
    {
        set
        {
            //Score is clamped to avoid cheating.
            //The value being passed through is the score adding the new value
            //The min is the current score
            //The max is the current score add the max score increase variable (a readonly variable of 50 to communicate to designers and follow the brief)
            score = Mathf.Clamp(GetScore + value, GetScore, GetScore + maxScoreIncrease);
        }
    }

    protected internal int GetScore
    {
        get
        {
            return score;
        }
    }


}
