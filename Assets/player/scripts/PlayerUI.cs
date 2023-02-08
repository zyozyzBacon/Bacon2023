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

    public GameObject Panel;

    public Transform uiPos;

    public Image bubbleColor;
    public TextMeshProUGUI text;

    [SerializeField] private Sprite[] bubble;

    Camera mCamera;
    private RectTransform rt;
    Vector2 pos;

    PlayerStateList pState;
    private BasicPlayerControll pControll;
    private foodBattlePlayer pFood;


    // Start is called before the first frame update
    public void init()
    {
        uiPart = Instantiate(Panel);
        uiPart.transform.parent = GameObject.Find("Canvas").transform;
        mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rt = uiPart.GetComponent<RectTransform>();
        bubbleColor = uiPart.transform.GetChild(0).GetComponent<Image>();

        pFood = GetComponent<foodBattlePlayer>();
        text = uiPart.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        pControll = GetComponent<BasicPlayerControll>();
        pState = GetComponent<PlayerStateList>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pState.dead) 
        {
            pos = RectTransformUtility.WorldToScreenPoint(mCamera, uiPos.position);
            rt.position = pos;

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

        }

    }
}
