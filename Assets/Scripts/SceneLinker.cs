using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLinker : MonoBehaviour
{
    internal static SceneLinker instance;



    [Header("Target Menu")]
    [SerializeField]
    bool _checkGameStateEveryFrame = false; //Set true when debugging using the inspector, as general gameplay should NOT rely on Update to change the menu
    [SerializeField]
    TargetGameState targetGameState;

    [Space(10)]

    [SerializeField]
    UnityEvent OnTargetGameStateChange; //Functions by programmers can be added by programmers, designers and others can add via inspector

    //Hidden
    TargetGameState previousTargetState;

    public enum TargetGameState
    {
        normalGameplay,
        firstTimeGameplay,
        tutorialGameplay,
        pauseGameplay,
        gameOverGameplay,
        mainMenu
    }

    public void Awake()
    {
        //If statement destroys this gameobject if the instance already exists, stops multiple static instances
        
        if (instance == null)
        {
            instance = this;

            //This object won't destroy on sceneload
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Adds fucntion to the OnTargetGameStateChange event - when the event is called, the function is called.
        OnTargetGameStateChange.AddListener(delegate { TargetGameStateActivater(); });
    }


    /// <summary>
    /// This function is merely a switch that changes scenes according to the enum.
    /// Each case contains an if statement to avoid loading the already active scene.
    /// 
    /// At the end of the switch, SetMenu from the UIHandler is called to open the desired menu according to the enum
    /// </summary>
    public void TargetGameStateActivater()
    {
        switch (targetGameState)
        {
            case TargetGameState.normalGameplay:
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    SceneManager.LoadSceneAsync(1);
                }
                break;
            case TargetGameState.firstTimeGameplay:
                if(SceneManager.GetActiveScene().buildIndex != 1)
                {
                    SceneManager.LoadSceneAsync(1);
                }
                
                break;
            case TargetGameState.tutorialGameplay:
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    SceneManager.LoadSceneAsync(1);
                }
                break;
            case TargetGameState.pauseGameplay:
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    SceneManager.LoadSceneAsync(1);
                }
                break;
            case TargetGameState.gameOverGameplay:
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    SceneManager.LoadSceneAsync(1);
                }
                break;
            case TargetGameState.mainMenu:
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    SceneManager.LoadSceneAsync(0);
                }
                break;
            default:
                break;
        }
        UIHandler.instance.SetMenu();
    }

    //Runs check everyframe - poor optimisation, should be event based - UPDATE
    private void Update()
    {
        //If target game state is different to the prior one (last frame), run code
        if(targetGameState != previousTargetState && _checkGameStateEveryFrame)
        {
            previousTargetState = targetGameState;
            OnTargetGameStateChange.Invoke();
        }
    }


    //Get Set is setup so other components can reference the game state enum and when set, the UnityEvent is invoked so designers do not need to interact too much with code.
    protected internal TargetGameState GameStateDestination
    {
        get
        {
            return targetGameState;
        }

        set
        {
            targetGameState = value;
            OnTargetGameStateChange.Invoke();
        }
    }
        
}
