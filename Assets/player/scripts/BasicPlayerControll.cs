using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BasicPlayerControll : MonoBehaviour
{
    [Header("移動相關")]
    [Tooltip("移動速度")][SerializeField] float walkSpeed;
    [Tooltip("電視模式速度")][SerializeField]public float tvMoveSpeed;

    [Header("跳躍相關")]
    [Tooltip("跳躍速度極限")][SerializeField] float jumpSpeed;
    [Tooltip("墜落速度極限")][SerializeField] float fallSpeed;
    [Tooltip("跳躍速度時間長度(幀數)")][SerializeField] int jumpSteps;

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

    [Header("[勿動]抓取子物件相關")]
    [Tooltip("電視模式物件")][SerializeField]public GameObject tvModePart;


    int stepsJumped = 0;
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
                if (!pState.damaged) 
                {
                    jump();
                    walk();
                    dash();
                }

            }
            else 
            {
                tvMove();
            }
        }
        pauseState();
    }



    public void derictionInput(InputAction.CallbackContext context) //方向輸入
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void walk()
    {
        if (!pState.dashing) 
        {
            rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);

            if (moveInput.x > 0.1)
                spriteRenderer.flipX = true;
            else if (moveInput.x < -0.1)
                spriteRenderer.flipX = false;

            pState.facingRight = spriteRenderer.flipX;
        }

    }



    //跳躍輸入
    public void jumpInput(InputAction.CallbackContext context)
    {
        if (Grounded()) //地面偵測 是否有站在地面上
        {
            pState.jumping = true; //啟動跳躍
        }
    }

    void jump() //觸發跳躍
    {
        if (pState.jumping)
        {
            if (stepsJumped < jumpSteps)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                stepsJumped++;
            }
            else
            {
                StopJumpSlow();
            }
        }

        //讓墜落速度有最大值極限而不是無限加速墜落
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }
    }

    void StopJumpSlow() //跳躍結束 跳到指定高度後 會開始墜落
    {
        stepsJumped = 0;
        pState.jumping = false;
    }

    public bool Grounded() //地面偵測 是否有站在地面上
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void dashInput(InputAction.CallbackContext context)
    {
        if (!pState.dashCoolDowning) 
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

    public void frozenForTV() 
    {
        StopAllCoroutines();
        StopAllCoroutines();

        pState.pause = true;

        pState.dashing = false;
        pState.dashCoolDowning = false;
    }

    public void unfrozen() 
    {
        pState.pause = false;
    }

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

    public void inTvMode() 
    {
        tvModePart.SetActive(true);
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        rb.gravityScale = 0f;
        pState.tvModeOn = true;
    }

    public void outTvMode()
    {
        tvModePart.SetActive(false);
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        rb.gravityScale = 1f;
        pState.tvModeOn = false;
    }

    private void tvMove() 
    {
        rb.velocity = moveInput * tvMoveSpeed;
    }

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
    


}
