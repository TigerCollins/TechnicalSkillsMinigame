using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
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
}
