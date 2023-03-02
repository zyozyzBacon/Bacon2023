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
    [HideInInspector]public GameObject ItemPart;

    public GameObject PlayerNumPanel;
    public GameObject TutoUiPanel;
    public GameObject FoodPanel;
    public GameObject ItemPanel;

    public Transform uiPanelTransform;
    public Transform iconTransform;
    public Transform ItemTransform;

    private TextMeshProUGUI text;
    private Camera mCamera;

    private RectTransform panelrt;
    Vector2 panelPos;

    private RectTransform iconrt;
    Vector2 iconpos;

    private RectTransform itemrt;
    Vector2 itemPos;

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

        ItemPart = Instantiate(ItemPanel);
        itemrt = ItemPart.GetComponent<RectTransform>();
        ItemPart.transform.parent = GameObject.Find("Canvas").transform;
        ItemPart.GetComponent<Image>().enabled = false;

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
                text = uiPart.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                break;
        }

        active = true;
    }

    public void GetItemUI(Sprite image) 
    {
        ItemPart.GetComponent<Image>().enabled = true;
        ItemPart.GetComponent<Image>().sprite = image;
    }

    public void disableItemUI() 
    {
        ItemPart.GetComponent<Image>().sprite = null;
        ItemPart.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        if (active) 
        {
            iconpos = RectTransformUtility.WorldToScreenPoint(mCamera, iconTransform.position);
            iconrt.position = iconpos;

            itemPos = RectTransformUtility.WorldToScreenPoint(mCamera, ItemTransform.position);
            itemrt.position = itemPos;

            panelPos = RectTransformUtility.WorldToScreenPoint(mCamera, uiPanelTransform.position);
            panelrt.position = panelPos;

            text.SetText(pControll.bubbles.ToString());
        }
    }
}
