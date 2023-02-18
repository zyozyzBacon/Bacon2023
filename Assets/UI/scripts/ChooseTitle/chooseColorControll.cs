using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
    public void moveToColor(InputAction.CallbackContext context)
    {

        if (startcontroll && !choose && context.performed)
        {
            Input = context.ReadValue<Vector2>().x;

            int before = 0;

            if (Input >= 0.4)
            {
                before = colorID;
                colorID++;
            }
            else if (Input <= -0.4)
            {
                before = colorID;
                colorID--;
            }

            positionCheck();
            colorPanelCheck(before, colorID);
        }
        else 
        {
            startcontroll = true;
        }
    }

    public void positionCheck() 
    {
        this.transform.position = pos[colorID].transform.position;
        this.transform.parent = pos[colorID].transform;
    }

    public void colorPanelCheck(int before,int after) 
    {
        if (before != -1) 
            pos[before].transform.parent.GetComponent<colorPanel>().phaseChage();


        pos[after].transform.parent.GetComponent<colorPanel>().phaseChage();
    }

    public void colorChose(InputAction.CallbackContext context)
    {
        if (context.performed && startcontroll)
        {
            if (!choose)
            {

                if (PlayerChooseColorManager.instance.colorCheck(colorID))
                {
                    PlayerChooseColorManager.instance.playerColorList[playerID] = colorID;
                    pos[colorID].transform.parent.GetComponent<colorPanel>().select = true;
                    GetComponent<Image>().color = new Color
                        (
                        PlayerChooseColorManager.instance.playerIconColor[colorID].x / 255,
                        PlayerChooseColorManager.instance.playerIconColor[colorID].y / 255,
                        PlayerChooseColorManager.instance.playerIconColor[colorID].z / 255
                        );
                    colorPanelCheck(-1, colorID);
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

        if (j == 1 && startcontroll)
        {
            if (choose)
            {
                PlayerChooseColorManager.instance.playerColorList[playerID] = -1;
                pos[colorID].transform.parent.GetComponent<colorPanel>().select = false;
                colorPanelCheck(-1, colorID);
                GetComponent<Image>().color = Color.white;
                choose = false;
            }
        }
    }


    public void toGame(InputAction.CallbackContext context) 
    {
        if (startcontroll && choose) 
        {
            PlayerChooseColorManager.instance.readyToGame();
        }
    }


}
