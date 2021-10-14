using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    internal static DragDrop instance;

     GameObject target;
     Vector3 screenSpace;

    [Header("Debug")]
    public Vector2 mousePos;
    [SerializeField]
    Vector3 curScreenSpace;
    [SerializeField]
    Vector3 curPosition;

    [Header("Pickup Settings")]
    [SerializeField]
    Vector3 pickupOffset = new Vector3(0,8,0);
    [SerializeField]
    float dropDivider = 2.5f; //When dropping items, the offset is divided but this value


    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void SetMousePos(InputAction.CallbackContext context)
    {
        //Updates mouse position - only updates when mouse moved (performant)
        mousePos = context.ReadValue<Vector2>();

        //keep track of the mouse position
        curScreenSpace = new Vector3(mousePos.x, mousePos.y, screenSpace.z);

        //convert the screen mouse position to world point and adjust with offset
        curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);

        //update the position of the object in the world
        if(target != null)
        {
            target.transform.position = curPosition + pickupOffset;
        }

        if(GameController.instance.IsGameComplete)
        {
            Drop();
        }
       
    }

    public void Grab(InputAction.CallbackContext context)
    {
        //Runs code if game is not complete
        if(!GameController.instance.IsGameComplete)
        {
            RaycastHit hitInfo;
            target = GetClickedObject(out hitInfo);
            if (target != null)
            {
                //Offsets item position to matrch offset
                screenSpace = Camera.main.WorldToScreenPoint(target.transform.position);
                target.transform.position = curPosition + pickupOffset;

                //Sets held items held state to tru, stopping the AddPoints function from being called
                GameController.instance.currentItemObject.CurrentlyHeld = true;
            }
        }
       
    }

    public void Drop(InputAction.CallbackContext context)
    {
        Drop();
    }

    void Drop()
    {
        if (target != null)
        {
            //Drops object (position)
            target.transform.position = curPosition - (pickupOffset / dropDivider);

            //Sets held items held state to false, allowing AddPoints function to be called on the ItemReceiver component
            GameController.instance.currentItemObject.CurrentlyHeld = false;

            //Stops moving object
            target = null;
        }

    }

    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
}
