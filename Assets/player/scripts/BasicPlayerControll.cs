using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class BasicPlayerControll : MonoBehaviour
{
    [Header("辨識相關")]
    [Tooltip("玩家")] [SerializeField] public int ID;
    [Tooltip("顏色")] [SerializeField] public int Color;

    [Header("移動相關")]
    [Tooltip("移動速度")] [SerializeField] float walkSpeed;
    [Tooltip("電視模式速度")] [SerializeField] public float tvMoveSpeed;

    [Header("跳躍相關")]
    [Tooltip("跳躍力道")] [SerializeField] float jumpForce;
    [Tooltip("墜落速度極限")] [SerializeField] float fallSpeed;
    [Tooltip("是否允許多段跳躍")] [SerializeField] bool doubleJump;
    [Tooltip("多段跳躍次數")] [SerializeField] int jumpAir;

    [Header("受傷相關")]
    [Tooltip("受傷後無敵時間")] [SerializeField] float recoveryTime;
    [Tooltip("受傷後暈眩時間")] [SerializeField] float dizzyTime;
    [Tooltip("受傷後擊退力道")] [SerializeField] float knockDown;

    [Header("地面物件檢測")]
    [Tooltip("射線檢測距離(高度)")] [SerializeField] float groundCheckY;
    [Tooltip("偵測地面物件Layer")] [SerializeField] LayerMask groundLayer;

    [Header("衝刺檢測")]
    [Tooltip("衝刺速度")] [SerializeField] float dashSpeed;
    [Tooltip("衝刺時間")] [SerializeField] float dashTime;
    [Tooltip("衝刺冷卻所需時間")] [SerializeField] float dashCD;

    [Header("玩法相關")]
    [Tooltip("珍珠數量")] [SerializeField] public int bubbles;
    [Tooltip("珍珠換道具數量")] [SerializeField] public int bubblesforItem;
    [Tooltip("珍珠子彈速度")] [SerializeField] public float bubbleSpeed;
    [Tooltip("攻擊緩衝時間")] [SerializeField] public float attackTime;
    [Tooltip("是否允許攻擊")] [SerializeField] public bool allowAttack;
    [Tooltip("死亡後手上珍珠")] [SerializeField] public GameObject deadBubble;
    [Tooltip("珍珠顏色")] [SerializeField] public ItemManager.foodColor FoodColor;

    [Header("道具相關")]
    [Tooltip("手上道具ID")] [SerializeField] public GameObject CurrentItem;

    [Header("[勿動]抓取子物件相關")]
    [Tooltip("電視模式物件")] [SerializeField] public GameObject tvModePart;
    [Tooltip("衝刺撞人物件")] [SerializeField] private GameObject DashCollider;
    [Tooltip("幽靈模式物件")] [SerializeField] private GameObject GhostPlayer;
    [Tooltip("槍物件")] [SerializeField] private GameObject GunPower;
    [Tooltip("子彈物件")] [SerializeField] private GameObject BulletPrefabs;
    [Tooltip("死亡後手上珍珠子物件")] public GameObject deadBubbleOnHand;

    Vector2 moveInput;
    int jumpAirCurrent;
    public int bubblesforItemCurrent;

    float JumpBuffer;

    Rigidbody2D rb;
    PlayerUI pUI;
    PlayerStateList pState;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        pUI = GetComponent<PlayerUI>();
        anim = GetComponent<Animator>();
        jumpAirCurrent = 0;
        bubblesforItemCurrent = 0;
    }

    void Update()
    {
        if (!pState.pause)
        {
            if (!pState.tvModeOn)
            {
                walk();
                dash();
            }
            else
            {
                tvMove();
            }
        }
        pauseState();
        TryJump();
        jumpDetect();
        JumpBuffer -= Time.deltaTime;
    }
    void TryJump()
    {
        if (JumpBuffer > 0)
        {
            if (!pState.damaged)
            {

                if (IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
                else if (doubleJump && jumpAirCurrent < jumpAir)
                {
                    jumpAirCurrent++;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.75f);
                }
                JumpBuffer = 0;
            }
        }
    }

    //移動相關/////////////////////////////////////////////////////////
    public void derictionInput(InputAction.CallbackContext context) //方向輸入
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void walk()
    {
        if (!pState.dashing && !pState.damaged)
        {
            rb.velocity = new Vector2(moveInput.x == 0 ? 0 : (Mathf.Sign(moveInput.x) * walkSpeed), rb.velocity.y);

            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                GunPower.transform.rotation = Quaternion.Euler(0, 0, 0);
                pState.facingRight = true;
            }
            else if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                GunPower.transform.rotation = Quaternion.Euler(0, 180, 0);
                pState.facingRight = false;
            }

            if (moveInput.x != 0)
                anim.SetBool("Walk", true);
            else
                anim.SetBool("Walk", false);
        }
    }


    //移動相關/////////////////////////////////////////////////////////
    //跳躍相關/////////////////////////////////////////////////////////

    public void jumpInput(InputAction.CallbackContext context) //輸入跳躍事件
    {
        if (context.started == false) return;
        JumpBuffer =0.15f;

    }
    private void jumpDetect() //跳躍相關的狀態偵測用(Update())
    {
        if (IsGrounded())
        {
            jumpAirCurrent = 0;
            pState.jumping = false;
            anim.SetBool("Jump", false);
        }
        else
        {
            if (!pState.jumping)
            {
                anim.SetBool("Jump", true);
                anim.SetTrigger("JumpTrigger");
            }

            pState.jumping = true;

        }

        //讓墜落速度有最大值極限而不是無限加速墜落
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
    }

    private bool IsGrounded() //地面偵測 是否有站在地面上
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckY, groundLayer))
            return true;
        else
            return false;
    }

    //跳躍相關/////////////////////////////////////////////////////////
    //衝刺相關/////////////////////////////////////////////////////////


    public void dashInput(InputAction.CallbackContext context)
    {
        if (!pState.dashCoolDowning && !pState.damaged)
        {
            pState.dashing = true;
            pState.dashCoolDowning = true;
            StartCoroutine(dashCount(dashTime));
            StartCoroutine(dashCoodDownCount(dashTime + dashCD));
        }

    }

    void dash()
    {
        if (pState.dashing)
        {
            if (pState.facingRight)
                rb.velocity = Vector2.right * dashSpeed;
            else
                rb.velocity = Vector2.left * dashSpeed;
        }
    }

    public IEnumerator dashCount(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pState.dashing = false;
    }

    public IEnumerator dashCoodDownCount(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pState.dashCoolDowning = false;
    }

    //衝刺相關/////////////////////////////////////////////////////////
    //電視玩法相關/////////////////////////////////////////////////////

    public void frozenForTV() //會執行數次
    {
        StopAllCoroutines();
        StopAllCoroutines();

        pState.pause = true;

        pState.dashing = false;
        pState.dashCoolDowning = false;
    }

    public void unfrozen() //會執行數次
    {
        pState.pause = false;
    }

    public void inTvMode() //會執行一次
    {
        tvModePart.SetActive(true);
        DashCollider.SetActive(false);
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        rb.gravityScale = 0f;
        pState.tvModeOn = true;

        transform.localEulerAngles = Vector3.zero;

        if (this.gameObject.GetComponent<foodBattlePlayer>() != null)
            this.gameObject.GetComponent<foodBattlePlayer>().holdOn();
    }

    public void outTvMode() //會執行一次
    {
        tvModePart.SetActive(false);
        DashCollider.SetActive(true);
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        rb.gravityScale = 1f;
        pState.tvModeOn = false;

        transform.localEulerAngles = Vector3.zero;

        if (this.gameObject.GetComponent<foodBattlePlayer>() != null)
            this.gameObject.GetComponent<foodBattlePlayer>().keepGo();
    }

    /// <summary>
    /// 
    ///  對於沒搶到遙控器的普通玩家 => 
    ///  
    ///  frozenForTV() => inTvMode() => unfrozen() => frozenForTV() => outTvMode() => frozenForTV() => unfrozen()
    ///  
    ///  對於搶到遙控器的普通玩家 => 
    ///  
    /// frozenForTV() => inTvMode() => (unfrozen() => frozenForTV()) => unfrozen() => outTvMode() => frozenForTV() => unfrozen()
    /// 
    /// 對於搶到遙控器之前就已經出局的玩家 => 
    /// 
    /// frozenForTV() =>  =>  =>  =>  =>  => unfrozen()
    /// 
    /// </summary>

    private void tvMove() //update()
    {
        rb.velocity = moveInput * tvMoveSpeed;

        if (moveInput.x > 0.7)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            pState.facingRight = true;
        }
        else if (moveInput.x < -0.7)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            pState.facingRight = false;
        }
    }

    //電視玩法相關/////////////////////////////////////////////////////
    //受傷相關/////////////////////////////////////////////////////////

    public void damaged(Vector2 others)
    {
        pState.damaged = true;
        pState.recoverying = true;

        StartCoroutine(dizzyCount(dizzyTime));
        StartCoroutine(damagedCount(dizzyTime + recoveryTime));

        float p = this.transform.position.x - others.x;

        if (p < 0)
        {
            rb.AddForce(new Vector2(-12, 5) * knockDown);
            Debug.Log("往左邊暈眩");
        }
        else
        {
            rb.AddForce(new Vector2(12, 5) * knockDown);
            Debug.Log("往右邊暈眩");
        }

        StartCoroutine(holdCount(dizzyTime / 3));
        anim.SetTrigger("Hit");
    }

    IEnumerator holdCount(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    IEnumerator dizzyCount(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pState.damaged = false;
    }

    IEnumerator damagedCount(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        pState.recoverying = false;
    }

    //受傷相關/////////////////////////////////////////////////////////
    //退場相關/////////////////////////////////////////////////////////

    public void retired()
    {

        anim.SetTrigger("Retired");
        StartCoroutine(dead(2f));
    }

    IEnumerator dead(float seconds)
    {
        tvModePart.SetActive(false);
        DashCollider.SetActive(false);
        boxCollider.enabled = false;
        rb.gravityScale = 0f;
        pState.tvModeOn = true;
        tvMoveSpeed = walkSpeed;
        pState.dead = true;
        pState.pause = true;
        Destroy(pUI.uiPart.gameObject);

        yield return new WaitForSeconds(seconds);

        pState.pause = false;
        GhostPlayer.SetActive(true);
        spriteRenderer.enabled = false;

        if (MainGameManager.instance != null)
            MainGameManager.instance.gameOver();
    }

    //退場相關/////////////////////////////////////////////////////////
    //暫停相關/////////////////////////////////////////////////////////

    public void pauseState()
    {
        if (pState.pause)
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //暫停相關/////////////////////////////////////////////////////////
    //吃食物相關///////////////////////////////////////////////////////

    public void eating(GameObject foodObject)
    {
        pState.eating = true;

        //如果吃到顏色不對
        if (FoodColor != foodObject.GetComponent<foodpart>().FoodColor)
        {
            FoodColor = foodObject.GetComponent<foodpart>().FoodColor;
            bubbles = 0;
        }
        bubbles++;


        bubblesforItemCurrent++;

        if (bubblesforItemCurrent >= bubblesforItem)
        {
            bubblesforItemCurrent = 0;
            if (CurrentItem == null)
            {
                itemGet();
            }

        }

        StartCoroutine(eatTimer(0.15f));
    }

    IEnumerator eatTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pState.eating = false;
    }

    //吃食物相關///////////////////////////////////////////////////////
    //射擊相關/////////////////////////////////////////////////////////

    public void shootInput(InputAction.CallbackContext context) //輸入攻擊事件
    {
        float s = context.ReadValue<float>();

        if (s == 1 && allowAttack && !pState.attacking)
        {
            if (bubbles > 0)
            {
                bubbles--;
                pState.attacking = true;
                GameObject bullets = Instantiate(BulletPrefabs, GunPower.transform.position, GunPower.transform.rotation);
                bullets.GetComponent<bubbleBullet>().parent = this.gameObject;
                bullets.GetComponent<bubbleBullet>().speed = bubbleSpeed;
                StartCoroutine(shootCount(attackTime));
            }
        }

        IEnumerator shootCount(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            pState.attacking = false;
        }
    }


    //射擊相關/////////////////////////////////////////////////////////
    //新手教程相關/////////////////////////////////////////////////////

    bool asking;
    public void readyInput(InputAction.CallbackContext context)
    {
        if (TutoGameManager.instance != null && this.gameObject.GetComponent<tutoPlayer>() != null)
        {
            if (!asking && !pState.pause)
            {
                GetComponent<tutoPlayer>().readyInput();
                StartCoroutine(askTime(0.5f));
            }
        }
    }

    private IEnumerator askTime(float seconds)
    {
        asking = true;
        yield return new WaitForSeconds(seconds);
        asking = false;
    }


    //新手教程相關/////////////////////////////////////////////////////
    //取消說明相關/////////////////////////////////////////////////////

    public void readedInstructionInput(InputAction.CallbackContext context)
    {
        if (instruction.instance != null && MainGameManager.instance.InstructionBool == true)
        {
            if (instruction.instance.ActionAllow)
            {
                pState.readed = true;
                instruction.instance.playerReaded(ID);
            }
        }
    }

    //取消說明相關/////////////////////////////////////////////////////
    //隨機獲取道具相關/////////////////////////////////////////////////

    void itemGet()
    {
        int r = Random.Range(0, ItemManager.instance.ItemList.Length);
        CurrentItem = ItemManager.instance.ItemList[r];
        pUI.GetItemUI(CurrentItem.GetComponent<ShowItem>().Icon);
    }

    //隨機獲取道具相關/////////////////////////////////////////////////
    //使用道具相關/////////////////////////////////////////////////////

    public void itemUseInput(InputAction.CallbackContext context)
    {
        if (CurrentItem != null)
        {
            GameObject newItem = Instantiate(CurrentItem);
            IitemInterface iitemInterface = newItem.GetComponent<IitemInterface>();

            if (iitemInterface != null && !pState.usingItem)
            {
                pState.usingItem = true;
                iitemInterface.ItemTrigger(this.gameObject);
                CurrentItem = null;
                pUI.disableItemUI();
                StartCoroutine(itemTimer(1));
            }
        }

    }

    IEnumerator itemTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pState.usingItem = false;
    }


    //使用道具相關/////////////////////////////////////////////////////
}
