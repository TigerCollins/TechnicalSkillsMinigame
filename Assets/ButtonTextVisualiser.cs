using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


[RequireComponent(typeof(EventTrigger))]
public class ButtonTextVisualiser : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _buttonText;
    [SerializeField]
    Button _button;
    [SerializeField]
    GameObject _buttonSelect;
    bool _isSelected;

    [Header("Visualiser Settings")]
    [SerializeField]
    bool _canModifyTextColour = false;
    [SerializeField]
    float _deselectColourChangeCountdown = .3f;

    [Header("Colour Settings")]
    [SerializeField]
    Color _unselectedColour = new Color(255, 255, 255, 255);
    [SerializeField]
    Color _selectedColour = new Color(0, 0, 0, 255);

    /// <summary>
    /// Purpose of this script is to handle visuals with the Unity 
    /// Button component. The script can find null references and 
    /// warn other developers of the missing component for debugging.
    /// </summary>

    private void Start()
    {
        NullCheck();
        SelectionCheck();
    }

    void NullCheck()
    {
        if (_buttonText == null)
        {
            Debug.LogWarning("The variable 'buttonText' on " + gameObject.name + " is missing, attempting to assign variable now. Please assign missing variable before shipping for performance.");

            //Attempts to find text mesh pro component in the buttons child.
            //TryGetComponent is more effecient than GetComponent
            if (transform.GetChild(0).TryGetComponent(out TextMeshProUGUI text))
            {
                _buttonText = text;
            }
        }

        if (_button == null)
        {
            Debug.LogWarning("The variable 'button' on " + gameObject.name + " is missing, attempting to assign variable now. Please assign missing variable before shipping for performance.");

            //Attempts to find text mesh pro component in the buttons child.
            //TryGetComponent is more effecient than GetComponent
            if (TryGetComponent(out Button newButton))
            {
                _button = newButton;
            }
        }
    }

    //To be used within other functions such as Start, but not with the EventTrigger
    public void SelectionCheck()
    {
        if (UIHandler.instance.eventSystem.currentSelectedGameObject == _button.gameObject)
        {
            _isSelected = true;
        }

        else
        {
            _isSelected = false;
        }
        IsSelected(_isSelected);
    }

    //Used with EventTrigger, bool can be modified in the inspector to modify text colour
    public void IsSelected(bool selectState)
    {
        if (selectState && _canModifyTextColour)
        {
            _buttonText.color = _selectedColour;
        }

        else if(_canModifyTextColour)
        {
            _buttonText.color = _unselectedColour;
        }
    }

    //Hides/Shows the pointer selector depending on bool
    public void PointerCheck(bool selectState)
    {
        if (_buttonSelect != null)
        {
            _buttonSelect.SetActive(selectState);
        }
    }

    //Used with EventTrigger (specifically pointer enter), bool can be modified in the inspector to modify activate state
    public void PointerEnter(bool selectState)
    {
        _isSelected = selectState;
        PointerCheck(selectState);

        //Selects UI element and triggers the select event
        _button.Select();
    }

    //Used with EventTrigger (specifically on select), bool can be modified in the inspector to modify activate state
    public void OnSelect(bool selectState)
    {
        _isSelected = selectState;
        PointerCheck(selectState);

        //Disables the other ButtonTextVisualiser elements
        UIHandler.instance.ToggleOtherPointerIndicators(this);
    }

    

    //When this variable is set, the IsSelect function is called to change visual components. THIS DOES NOT JUST CHANGE THE BOOL
    //This get/set is done so the private variables can be used within the solution without exposing them to the memory
    internal bool IsButtonSelected
    {
        get
        {
            return _isSelected;
        }

        set
        {
            _isSelected = value;
            IsSelected(value);
        }
    }


    //This function is called via the Unity inspector. It will start the TextColourCountdown coroutine and change the color to unselected after the desired time
    public void UnselectColourCountdown()
    {
        StartCoroutine(TextColourCountdown(_unselectedColour));
    }

    IEnumerator TextColourCountdown(Color desiredColor)
    {
        yield return new WaitForSecondsRealtime(_deselectColourChangeCountdown);
        _buttonText.color = desiredColor;

    }
    
}
