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
    bool _checkGameStateEveryFrame = false;
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
