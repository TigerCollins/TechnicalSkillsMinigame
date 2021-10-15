using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{

    [SerializeField]
    GameObject wagonGlowObject;

    public void Start()
    {
        //Disables wagon glow if left enabled in edit mode
        WagonGlow(false);
    }

    public void AddPoints(int points)
    {
        //If game is not complete, add points and destroy object (Destoryed via HaveAddedToScore get/set)
        if(!GameController.instance.IsGameComplete)
        {
            GameController.instance.currentItemObject.HaveAddedToScore = true;
            GameController.instance.AddedScore = points;
            GameController.instance.onScoreChange.Invoke();
        }
       
    }

    public void WagonGlow(bool glowState)
    {
        wagonGlowObject.SetActive(glowState);
    }
}
