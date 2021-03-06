using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;



public class UIHandler : MonoBehaviour
{
    internal static UIHandler instance;


    [SerializeField]
    internal EventSystem eventSystem;

    [Header("Text Visualiser Variables")]
    [SerializeField]
    List<ButtonTextVisualiser> buttonTextVisualisers;

    [Header("Canvas Group Details")]
    [SerializeField]
    List<CanvasGroup> allcanvasGroups;
    [SerializeField]
    CanvasGroupDetails firstTimeCanvasGroup;
     [SerializeField]
    CanvasGroupDetails tutorialCanvasGroup;
     [SerializeField]
    CanvasGroupDetails pauseCanvasGroup;
     [SerializeField]
    CanvasGroupDetails gameOverCanvasGroup;
     [SerializeField]
    CanvasGroupDetails gameplayCanvasGroup;

    [Header("FEELS Feedbacks")]
    [SerializeField]
    internal MMFeedbacks flashFeedback;
    [SerializeField]
    internal MMFeedbacks scoreBounceFeedback;
    [SerializeField]
    internal MMFeedbacks timeBounceFeedback;

    //Called before start (game object does not need to be active)
    public void Awake()
    {
        //The if statement destroys this component if the instance already exists, stops multiple static instances
        if(instance ==null)
        {
            instance = this;
        }

        else
        {
            Destroy(this);
        }
        
    }

    //Called first frame of game object active
    private void Start()
    {
            SetMenu();
    }

    //This function is a switch that sets the alpha, interactable and raycast state of each of the canvas groups in the game scene. 
    //The switch also changes the button selected. 
    //Switch is used so fucnitons/variables can be affected depending on the switch argument (ie; change time scale when paused/playing)
    public void SetMenu()
    {
        switch (SceneLinker.instance.GameStateDestination)
        {
            case SceneLinker.TargetGameState.normalGameplay:
                Time.timeScale = 1;
               
                foreach (CanvasGroup item in allcanvasGroups)
                {
                    if(gameplayCanvasGroup != null)
                    {
                        if (item != gameplayCanvasGroup.canvasGroup)
                        {
                            item.interactable = false;
                            item.blocksRaycasts = false;
                            item.alpha = 0;
                        }

                        else
                        {
                            gameplayCanvasGroup.canvasGroup.alpha = 1;
                            gameplayCanvasGroup.canvasGroup.blocksRaycasts = true;
                            gameplayCanvasGroup.canvasGroup.interactable = true;
                        }
                    }
                }
                ChangeButtonSelection(gameplayCanvasGroup.firstButton);
                if(GameController.instance != null)
                {
                    GameController.instance.pauseTimer = false;
                    GameController.instance.BeginCountdown();
                }
               
                break;
            case SceneLinker.TargetGameState.firstTimeGameplay:
               
                Time.timeScale = 0;
                foreach (CanvasGroup item in allcanvasGroups)
                {
                    if (firstTimeCanvasGroup != null)
                    {
                        if (item != firstTimeCanvasGroup.canvasGroup)
                        {
                            item.interactable = false;
                            item.blocksRaycasts = false;
                            item.alpha = 0;
                        }

                        else
                        {
                            firstTimeCanvasGroup.canvasGroup.alpha = 1;
                            firstTimeCanvasGroup.canvasGroup.blocksRaycasts = true;
                            firstTimeCanvasGroup.canvasGroup.interactable = true;
                        }
                    }
                }
                if (GameController.instance != null)
                {
                    GameController.instance.pauseTimer = true;
                }
                ChangeButtonSelection(firstTimeCanvasGroup.firstButton);
                break;
            case SceneLinker.TargetGameState.tutorialGameplay:
              
                Time.timeScale = 0;
                foreach (CanvasGroup item in allcanvasGroups)
                {
                    if (tutorialCanvasGroup != null)
                    {
                        if (item != tutorialCanvasGroup.canvasGroup)
                        {
                            item.interactable = false;
                            item.blocksRaycasts = false;
                            item.alpha = 0;
                        }

                        else
                        {
                            tutorialCanvasGroup.canvasGroup.alpha = 1;
                            tutorialCanvasGroup.canvasGroup.blocksRaycasts = true;
                            tutorialCanvasGroup.canvasGroup.interactable = true;
                        }
                    }
                }
                if (GameController.instance != null)
                {
                    GameController.instance.pauseTimer = true;
                }
                ChangeButtonSelection(tutorialCanvasGroup.firstButton);
                break;
            case SceneLinker.TargetGameState.pauseGameplay:
               
                Time.timeScale = 0;
                foreach (CanvasGroup item in allcanvasGroups)
                {
                    if (pauseCanvasGroup != null)
                    {
                        if (item != pauseCanvasGroup.canvasGroup)
                        {
                            item.interactable = false;
                            item.blocksRaycasts = false;
                            item.alpha = 0;
                        }

                        else
                        {
                            pauseCanvasGroup.canvasGroup.alpha = 1;
                            pauseCanvasGroup.canvasGroup.blocksRaycasts = true;
                            pauseCanvasGroup.canvasGroup.interactable = true;
                        }
                    }
                    if (GameController.instance != null)
                    {
                        GameController.instance.pauseTimer = true;
                    }
                    ChangeButtonSelection(pauseCanvasGroup.firstButton);
                }
                break;
            case SceneLinker.TargetGameState.gameOverGameplay:
                Time.timeScale = 1;
                foreach (CanvasGroup item in allcanvasGroups)
                {
                    if (gameOverCanvasGroup != null)
                    {
                        if (item != gameOverCanvasGroup.canvasGroup)
                        {
                            item.interactable = false;
                            item.blocksRaycasts = false;
                            item.alpha = 0;
                        }

                        else
                        {
                            gameOverCanvasGroup.canvasGroup.alpha = 1;
                            gameOverCanvasGroup.canvasGroup.blocksRaycasts = true;
                            gameOverCanvasGroup.canvasGroup.interactable = true;
                        }
                    }
                }
                 if (GameController.instance != null)
                    {
                         GameController.instance.pauseTimer = true;
                    }
                ChangeButtonSelection(gameOverCanvasGroup.firstButton);
                break;
            case SceneLinker.TargetGameState.mainMenu:
                Time.timeScale = 1;
                break;
            default:
                break;
        }
        
    }

    //This function runs through the list of the Button Visualisers and checks if they're selected. If selected they show the pointer.
    internal void ToggleOtherPointerIndicators(ButtonTextVisualiser visualiser)
    {
        foreach (ButtonTextVisualiser item in buttonTextVisualisers)
        {
            if(visualiser == item)
            {
                item.PointerCheck(true);
            }

            else
            {
                item.PointerCheck(false);
            }    
        }
    }


    //This function sets the selected button component. Called when a game menu state is changed.
    //If the argument is null, no UI element is selected - stops null reference.
    public void ChangeButtonSelection(Button button)
    {
        if(button!=null)
        {
            eventSystem.SetSelectedGameObject(button.gameObject);
        }

        else
        {
            eventSystem.SetSelectedGameObject(null);
        }
       
    }


    /// <summary>
    /// The following functions named after game state enums are so they can be easily called and used within inspector UnityEvents
    /// </summary>
    public void StartGameFirstTime()
    {
        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.firstTimeGameplay;
    }

    public void Gameplay()
    {
        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.normalGameplay;
    }

    public void Pause()
    {
            SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.pauseGameplay;
    }

    public void GameOver()
    {
        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.gameOverGameplay;
        GameController.instance.UpdateScoreGameOver();
    }

    public void Tutorial()
    {
        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.tutorialGameplay;
    }

    public void QuitToMenu()
    {
        SceneLinker.instance.GameStateDestination = SceneLinker.TargetGameState.mainMenu;
    }


    //Calls the pause function, but for functionality with the new input system
    public void PauseInput(InputAction.CallbackContext context)
    {
        //Stops pause menu from opening if the current game state is not regular gameplay
        if (SceneLinker.instance.GameStateDestination == SceneLinker.TargetGameState.normalGameplay)
        {
            Pause();
        }

        //If on the main menu, the game will quit
        else if(SceneLinker.instance.GameStateDestination == SceneLinker.TargetGameState.mainMenu)
        {
            QuitGame();
        }
       
    }

    //If the game is within the engine, the editor will stop playing but if the project is packaged, the application is closed
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}

[System.Serializable]
public class CanvasGroupDetails
{
    public CanvasGroup canvasGroup;
    public Button firstButton;
}


