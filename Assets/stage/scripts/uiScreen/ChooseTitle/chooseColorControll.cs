using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class chooseColorControll : MonoBehaviour
{
    public int playerID;

    [SerializeField]private int _colorID;
    public int colorID 
    {
        get => _colorID;
        set 
        {
            _colorID = value;
            if (value > 3) 
            {
                _colorID = 0;
            }

            if (value < 0) 
            {
                _colorID = 3;
            }
        }
    }

    private float Input;

    public Image[] pos;
    public bool startcontroll;
    public bool choose;
    public bool asking;

    public void moveToColor(InputAction.CallbackContext context)
    {
        if (startcontroll && !choose)
        {
            Input = context.ReadValue<Vector2>().x;

            if (Input > 0.9)
            {
                colorID++;
            }
            else if (Input < -0.9)
            {
                colorID--;
            }

            positionCheck();
        }
        else 
        {
            startcontroll = true;
        }
    }

    public void positionCheck() 
    {
        this.transform.position = pos[colorID].transform.position;
    }

    public void colorChose(InputAction.CallbackContext context)
    {
        float j = context.ReadValue<float>();

        if (j == 1  && startcontroll && !asking)
        {
            if (!choose)
            {
                asking = true;
                StartCoroutine(askTime(0.5f));

                if (PlayerChooseColorManager.instance.colorCheck(colorID))
                {
                    PlayerChooseColorManager.instance.playerColorList[playerID] = colorID;
                    choose = true;
                }
                else
                    Debug.Log("顏色已經被選走了");
            }
        }
    }

    public void cancelChose(InputAction.CallbackContext context) 
    {
        float j = context.ReadValue<float>();

        if (j == 1 && startcontroll && !asking)
        {
            if (choose)
            {
                asking = true;
                StartCoroutine(askTime(0.5f));
                PlayerChooseColorManager.instance.playerColorList[playerID] = -1;
                choose = false;
            }
        }
    }


    public void toGame(InputAction.CallbackContext context) 
    {
        float j = context.ReadValue<float>();

        if (startcontroll && choose && !asking) 
        {
            asking = true;
            StartCoroutine(askTime(0.5f));
            PlayerChooseColorManager.instance.readyToGame();
        }
    }

    private IEnumerator askTime(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        asking = false;
    }

}
