using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BasicPlayerControll : MonoBehaviour
{
    [Header("辨識相關")]
    [Tooltip("玩家")][SerializeField] public int ID;
    [Tooltip("顏色")][SerializeField] public int Color;

    [Header("移動相關")]
    [Tooltip("移動速度")][SerializeField] float walkSpeed;
    [Tooltip("電視模式速度")][SerializeField]public float tvMoveSpeed;

    [Header("跳躍相關")]
    [Tooltip("跳躍力道")][SerializeField] float jumpForce;
    [Tooltip("墜落速度極限")][SerializeField] float fallSpeed;

    [Header("受傷相關")]
    [Tooltip("受傷後無敵時間")][SerializeField] float recoveryTime;
    [Tooltip("受傷後暈眩時間")][SerializeField] float dizzyTime;
    [Tooltip("受傷後擊退力道")][SerializeField] float knockDown;

    [Header("地面物件檢測")]
    [Tooltip("射線檢測距離(高度)")][SerializeField] float groundCheckY; 
    [Tooltip("偵測地面物件Layer")][SerializeField] LayerMask groundLayer;

    [Header("衝刺檢測")]
    [Tooltip("衝刺速度")][SerializeField] float dashSpeed;
    [Tooltip("衝刺時間")][SerializeField] float dashTime;
    [Tooltip("衝刺冷卻所需時間")][SerializeField] float dashCD;

    [Header("玩法相關")]
    [Tooltip("珍珠數量")][SerializeField] public int bubbles;
    [Tooltip("珍珠顏色")][SerializeField] public ItemManager.foodColor FoodColor;

    [Header("[勿動]抓取子物件相關")]
    [Tooltip("電視模式物件")][SerializeField] public GameObject tvModePart;
    [Tooltip("衝刺撞人物件")][SerializeField] private GameObject DashCollider;
    [Tooltip("幽靈模式物件")][SerializeField] private GameObject GhostPlayer;

    Vector2 moveInput;

    Rigidbody2D rb;
    PlayerStateList pState;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        jumpDetect();
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
            rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);

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

    }


    //移動相關/////////////////////////////////////////////////////////
    //跳躍相關/////////////////////////////////////////////////////////

    public void jumpInput(InputAction.CallbackContext context) //輸入跳躍事件
    {
        float j = context.ReadValue<float>();

        if (j != 0 && IsGrounded() && !pState.damaged)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //anim.SetTrigger("jumpTrigger");
        }

        if (j == 0 && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

    }
    private void jumpDetect() //跳躍相關的狀態偵測用(Update())
    {
        if (IsGrounded())
        {
            if (pState.jumping)
                StartCoroutine(jumpCoolDown(0.05f));

            pState.jumping = false;
        }
        else
        {
            if (!pState.jumping)
            {
                //anim.SetTrigger("jumpTrigger");
            }

            pState.jumping = true;
        }

        //pState.grounded = IsGrounded();
        //anim.SetBool("jump", pState.jumping);

        //讓墜落速度有最大值極限而不是無限加速墜落
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
    }


    private IEnumerator jumpCoolDown(float seconds) //跳躍冷卻(0.03秒)
    {
        pState.jumped = true;
        yield return new WaitForSeconds(seconds);
        pState.jumped = false;
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

        transform.localEulerAngles= Vector3.zero;

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
            rb.AddForce(new Vector2(-12,5) * knockDown);
            Debug.Log("往左邊暈眩");
        }
        else
        {
            rb.AddForce(new Vector2(12, 5) * knockDown);
            Debug.Log("往右邊暈眩");
        }
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
        tvModePart.SetActive(false);
        DashCollider.SetActive(false);
        GhostPlayer.SetActive(true);
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        rb.gravityScale = 0f;
        pState.tvModeOn = true;
        tvMoveSpeed = tvMoveSpeed * 10;
        pState.dead = true;

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

}
