using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameController : MonoBehaviour
{
    internal static GameController instance;
    Coroutine countdownCoroutine;

    [SerializeField]
    public readonly int maxScoreIncrease = 50;
    [SerializeField]
    bool gameFinished;
    [SerializeField]
    internal bool pauseTimer;

    [Header("Player Performance")]
    [SerializeField]
    int score;
    [SerializeField]
    float timeRemaining = 10;

    [Space(5)]

    [SerializeField]
    internal UnityEvent onScoreChange;

    [Header("Item Handler")]
    [SerializeField]
    internal Item currentItemObject; //Internal used so variable can be refenced within solution easily

    [Space(5)]

    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    List<ItemMaterial> itemMat;
    [SerializeField]
    List<Transform> itemSpawnPoints;

    [Header("UI - Time")]
    [SerializeField]
    TextMeshProUGUI timeRemainingTextBackground;
    [SerializeField]
    TextMeshProUGUI timeRemainingTextForeground;
    [SerializeField]
    Image timeRemainingMask;
    [SerializeField]
    Color standardColour;
    [SerializeField]
    Color warningColour;
    [SerializeField]
    Color imminentColour;

    [Header("UI - Score")]
    [SerializeField]
    TextMeshProUGUI scoreText;

    [Space(10)]

    [SerializeField]
    string pauseScoreString = "Score:";
    [SerializeField]
    TextMeshProUGUI pauseScoreText;

    [Space(10)]

    [SerializeField]
    TextMeshProUGUI gameOverText;

    [Header("Inputs")]
    [SerializeField]
    InputAction pauseInput;
    [SerializeField]
    InputAction grabDropInput;
    [SerializeField]
    InputAction mousePos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        SetupInput();
        SpawnItem();
        onScoreChange.AddListener(delegate { UpdateScoreVisualGameplay(); });
        onScoreChange.AddListener(delegate { UpdateScoreVisualPause(); });
    }


    public void BeginCountdown()
    {
        //Stops coroutine being started ontop of itself causing unintended speeds
        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(GameCountdown());
        }
    }

    void SetupInput()
    {
        //Adds function to input. Only calls when performed (pressed, not held or up)
        pauseInput.performed += UIHandler.instance.PauseInput;
        grabDropInput.performed += DragDrop.instance.Grab;
        grabDropInput.canceled += DragDrop.instance.Drop;
        mousePos.performed += DragDrop.instance.SetMousePos;

        //Input functions enabled (this is needed or the input wont work)
        pauseInput.Enable();
        grabDropInput.Enable();
        mousePos.Enable();
    }

    //If destroying an object is not required, this runs the function without destroying anything
    internal void SpawnItem()
    {
        SpawnItem(null);
    }

    internal void SpawnItem(GameObject currentObject)
    {
        //Makes local variables randomly choosing a mat and spawn point within the list
        Transform selectedSpawnPoint = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count)];
        int randomNum = Random.Range(0, itemMat.Count);
        Material selectedMat = itemMat[randomNum].itemMaterial;
        Material glowMat = itemMat[randomNum].glowMaterial;

        //Destroys current object if passed through as an argument
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        //Creates the item and initiates the variables for position and material
        GameObject itemObject = Instantiate(itemPrefab);
        if (itemObject.TryGetComponent(out Item item))
        {
            item.transform.position = selectedSpawnPoint.transform.position;
            item.transform.localScale = selectedSpawnPoint.transform.localScale;
            item.SetMaterial(selectedMat,glowMat);
            currentItemObject = item;
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

            //Spawn a new item
            SpawnItem();
        }
    }

    protected internal int GetScore
    {
        get
        {
            return score;
        }
    }

    int TimeWholeNumbers
    {
        get
        {
            return (int)Mathf.Round(timeRemaining);
        }

        set
        {
            //The +1 compensates for the rounding down of the int struct
            int newVal = value;

            //Set the Text string when whole number changed
            timeRemainingTextForeground.text = newVal.ToString();
            timeRemainingTextBackground.text = newVal.ToString();
        }
    }

    float TimeFloat
    {
        get
        {
            return timeRemaining;
        }

        set
        {
            //Clamps the time remaining from going below 0
            timeRemaining = Mathf.Clamp(value, 0, 10);
        }
    }

    internal IEnumerator GameCountdown()
    {
        float timeProgressed = 0;
        //Pauses coroutine when bool changes
        while (pauseTimer)
        {
            yield return null;
        }

        //When coroutine isn't paused and timer is above 0, run this
        while (TimeFloat > 0)
        {
            //Set the Text string
            TimeWholeNumbers = (int)Mathf.Round(TimeFloat);

            //Sets the text Colour (argument needs to be whole number for switch)
            TimeRemainingTextColour(TimeWholeNumbers);

            //Sets the Time remaining mask fill
            timeRemainingMask.fillAmount = Mathf.Round((TimeFloat / 10) * 10.0f) * 0.1f;

            //Sets the time (scaled with time so pausing takes affect)
            TimeFloat -= Time.deltaTime;
            timeProgressed += Time.deltaTime;

            yield return null;
        }


        //Stops game (interactions for points and dragging items)
        timeRemainingTextForeground.text = TimeWholeNumbers.ToString();
        timeRemainingTextBackground.text = TimeWholeNumbers.ToString();
        gameFinished = true;
        yield return new WaitForSeconds(1f);

        //Once the while loop is finished, run the following
        UIHandler.instance.GameOver();
    }



    //Only changes text colour (visual)
    void TimeRemainingTextColour(int time)
    {
        switch (time)
        {
            case 10:
                SetTimeColour(standardColour);
                break;
            case 9:
                SetTimeColour(standardColour);
                break;
            case 8:
                SetTimeColour(standardColour);
                break;
            case 7:
                SetTimeColour(standardColour);
                break;
            case 6:
                SetTimeColour(standardColour);
                break;
            case 5:
                SetTimeColour(standardColour);
                break;
            case 4:
                SetTimeColour(warningColour);
                break;
            case 3:
                SetTimeColour(warningColour);
                break;
            case 2:
                SetTimeColour(imminentColour);
                break;
            case 1:
                SetTimeColour(imminentColour);
                break;
            case 0:
                SetTimeColour(imminentColour);
                break;
            default:
                break;
        }
    }

    void SetTimeColour(Color color)
    {
        //Minimises the canvas needing to redraw due to the colour beingt changed itself
        if (timeRemainingTextForeground.color != color)
        {
            timeRemainingTextForeground.color = color;
        }
    }

    public bool IsGameComplete
    {
        get
        {
            return gameFinished;
        }
    }

    //Updates score, called when score changes - Listener on onScoreChange UnityEvent
    void UpdateScoreVisualGameplay()
    {
        scoreText.text = "$" + score.ToString();
    }

    //Updates score in the pause menu, called when score changes - Listener on onScoreChange UnityEvent
    void UpdateScoreVisualPause()
    {
        pauseScoreText.text = pauseScoreString + " $" + score.ToString();
    }

    //Updates the entire string. Dialogue currently hardcoded.
    internal void UpdateScoreGameOver()
    {
        gameOverText.SetText("Hmmm y'know what? Not too shabby buddy!" + "<br><br>You helped pack a heap of supplies for the trail! You packed $<color=#FA3029>{0}</color> worth of items, what a score! Maybe " +
            "you shouldn't go on the Oregon Trail and help with the next wagon instead! ",score);
    }

    public void ResetMinigame()
    {
        //Changes variables
        timeRemaining = 10;
        score = 0;
        gameFinished = false;
        pauseTimer = false;

        //Resets timer coroutine
        countdownCoroutine = null;
        countdownCoroutine = StartCoroutine(GameCountdown());

        //Spawn new object and destroy previous one
        Destroy(currentItemObject.gameObject);
        SpawnItem();

        //Calls events
        onScoreChange.Invoke();

        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.normalGameplay;
    }
}

[System.Serializable]
public class ItemMaterial
{
    public Material itemMaterial;
    public Material glowMaterial;
}
