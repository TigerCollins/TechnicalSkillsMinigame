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
    AudioLibrary audioLibrary;

    [Space(5)]

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
    [SerializeField]
    float baseCountdownTime = 10;
    int previousTimeInt;
    int timeRemainingInt;

    [Space(5)]

    [SerializeField]
    internal UnityEvent onScoreChange;
    [SerializeField]
    internal UnityEvent onTimeIntegerChange;

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

    [Space(5)]

    [SerializeField]
    [Range(0, 1)]
    float standardColourThreshold = .5f;
    [SerializeField]
    [Range(0, 1)]
    float warningColourThreshold = .3f;

    [Space(5)]

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
        onScoreChange.AddListener(delegate { UIHandler.instance.scoreBounceFeedback.PlayFeedbacks(); });
        onScoreChange.AddListener(delegate { AudioPlayer.instance.PlaySoundEffect(audioLibrary.scoreChange); });
        onTimeIntegerChange.AddListener(delegate { UIHandler.instance.timeBounceFeedback.PlayFeedbacks(); });
        onTimeIntegerChange.AddListener(delegate { AudioPlayer.instance.PlaySoundEffect(audioLibrary.timeChange); });
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

    ///

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
            return (int)Mathf.Ceil(timeRemaining);
        }

        set
        {
            //Previous time int is required to run before the set. Functionality utilising it compares the two values and invokes the onTimeIntegerChange UnityEvent
            previousTimeInt = timeRemainingInt;
            timeRemainingInt = value;

            //Set the Text string when whole number changed
            timeRemainingTextForeground.text = timeRemainingInt.ToString();
            timeRemainingTextBackground.text = timeRemainingInt.ToString();

            //Sets the text Colour (argument needs to be whole number for switch)
            TimeRemainingTextColour(timeRemainingInt);

            if(timeRemainingInt != previousTimeInt)
            {
                onTimeIntegerChange.Invoke();
            }
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
            timeRemaining = Mathf.Clamp(value, 0, baseCountdownTime);
        }
    }

    internal IEnumerator GameCountdown()
    {
        //Pauses coroutine when bool changes
        while (pauseTimer)
        {
            yield return null;
        }

        //When coroutine isn't paused and timer is above 0, run this
        while (TimeFloat > 0)
        {
            //Set the Text string and foreground text colour
            TimeWholeNumbers = (int)Mathf.Ceil(TimeFloat);

            //Sets the Time remaining mask fill
            timeRemainingMask.fillAmount = TimeWholeNumbers / baseCountdownTime;

            //Sets the time (scaled with time so pausing takes affect)
            TimeFloat -= Time.deltaTime;

            yield return null;
        }

        //Triggers screen flash when time displays as 0 (0 should occur when while loop completes)
        UIHandler.instance.flashFeedback.PlayFeedbacks();

        //Forces timer to be 0 visually.
        timeRemainingMask.fillAmount = 0;
        timeRemainingTextForeground.text = TimeWholeNumbers.ToString();
        timeRemainingTextBackground.text = TimeWholeNumbers.ToString();

        //Stops game (interactions for points and dragging items)
        gameFinished = true;
        yield return new WaitForSeconds(1f);

        //Once the while loop is finished, run the following
        UIHandler.instance.GameOver();
    }

    //Only changes text colour (visual)
    void TimeRemainingTextColour(int time)
    {
        //Current time completion (in percent)
        float percentage = time/baseCountdownTime;

        //If above the threshold, display standard colour
        if(percentage >= standardColourThreshold )
        {
            SetTimeColour(standardColour);
        }

        //If above the threshold, display warning colour
        else if (percentage >= warningColourThreshold)
        {
            SetTimeColour(warningColour);
        }

        //Display imminent colour
        else
        {
            SetTimeColour(imminentColour);
        }
    }

    void SetTimeColour(Color color)
    {
        //Minimises the canvas needing to redraw due to the colour being changed itself
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
            "you shouldn't go on the Oregon Trail and help with the next wagon instead!",score);
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

        //Resets visuals
        UpdateScoreVisualPause();
        UpdateScoreVisualGameplay();

        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.normalGameplay;
    }
}

[System.Serializable]
public class ItemMaterial
{
    public Material itemMaterial;
    public Material glowMaterial;
}
