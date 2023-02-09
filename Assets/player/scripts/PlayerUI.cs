using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUI : MonoBehaviour
{
    [HideInInspector]public GameObject uiPart;
    [HideInInspector]public GameObject iconPart;
    [HideInInspector]public Sprite PlayerIcon;

    public GameObject PlayerNumPanel;
    public GameObject TutoUiPanel;
    public GameObject FoodPanel;

    public Transform uiPanelTransform;
    public Transform iconTransform;

    [SerializeField] private Sprite[] bubble;
    private Image bubbleColor;
    private TextMeshProUGUI text;
    private Camera mCamera;

    private RectTransform panelrt;
    Vector2 panelPos;

    private RectTransform iconrt;
    Vector2 iconpos;

    private PlayerStateList pState;
    private BasicPlayerControll pControll;
    private foodBattlePlayer pFood;


    bool active;
    
    public void init()
    {
        pFood = GetComponent<foodBattlePlayer>();
        pControll = GetComponent<BasicPlayerControll>();
        pState = GetComponent<PlayerStateList>();

        mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        iconPart = Instantiate(PlayerNumPanel);
        iconrt = iconPart.GetComponent<RectTransform>();
        iconPart.transform.parent = GameObject.Find("Canvas").transform;

        iconPart.GetComponent<Image>().sprite = PlayerIcon;
        iconPart.GetComponent<Image>().color = new Color
        (
            MainGameManager.instance.playerIconColor[pControll.Color].x / 255,
            MainGameManager.instance.playerIconColor[pControll.Color].y / 255,
            MainGameManager.instance.playerIconColor[pControll.Color].z / 255
        );

        switch (MainGameManager.instance.GameMode) 
        {
            case MainGameManager.gameMode.tuto:

                uiPart = Instantiate(TutoUiPanel);
                panelrt = uiPart.GetComponent<RectTransform>();
                uiPart.transform.parent = GameObject.Find("Canvas").transform;

                break;

            case MainGameManager.gameMode.foodBattle:

                uiPart = Instantiate(FoodPanel);
                panelrt = uiPart.GetComponent<RectTransform>();
                uiPart.transform.parent = GameObject.Find("Canvas").transform;

                bubbleColor = uiPart.transform.GetChild(0).GetComponent<Image>();
                text = uiPart.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                break;
        }

        active = true;
    }

    void Update()
    {
        if (active) 
        {
            iconpos = RectTransformUtility.WorldToScreenPoint(mCamera, iconTransform.position);
            iconrt.position = iconpos;

            if (!pState.dead)
            {

                panelPos = RectTransformUtility.WorldToScreenPoint(mCamera, uiPanelTransform.position);
                panelrt.position = panelPos;

                switch (MainGameManager.instance.GameMode)
                {
                    case MainGameManager.gameMode.foodBattle:

                        if (pFood != null)
                            text.SetText(pFood.food.ToString());

                        if (pControll.FoodColor != ItemManager.foodColor.none)
                        {
                            if (pControll.FoodColor == ItemManager.foodColor.white)
                                bubbleColor.sprite = bubble[0];
                            else if (pControll.FoodColor == ItemManager.foodColor.black)
                                bubbleColor.sprite = bubble[1];

                            bubbleColor.gameObject.SetActive(true);
                        }
                        else
                        {
                            bubbleColor.gameObject.SetActive(false);
                        }

                        break;
                }
            }
        }
    }
}
