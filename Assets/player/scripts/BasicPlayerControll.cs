using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BasicPlayerControll : MonoBehaviour
{
    [Header("���ʬ���")]
    [Tooltip("���ʳt��")][SerializeField] float walkSpeed;
    [Tooltip("�q���Ҧ��t��")][SerializeField]public float tvMoveSpeed;

    [Header("���D����")]
    [Tooltip("���D�t�׷���")][SerializeField] float jumpSpeed;
    [Tooltip("�Y���t�׷���")][SerializeField] float fallSpeed;
    [Tooltip("���D�t�׮ɶ�����(�V��)")][SerializeField] int jumpSteps;

    [Header("���ˬ���")]
    [Tooltip("���˫�L�Įɶ�")][SerializeField] float recoveryTime;
    [Tooltip("���˫�w�t�ɶ�")][SerializeField] float dizzyTime;
    [Tooltip("���˫����h�O�D")][SerializeField] float knockDown;

    [Header("�a�������˴�")]
    [Tooltip("�g�u�˴��Z��(����)")][SerializeField] float groundCheckY; 
    [Tooltip("�����a������Layer")][SerializeField] LayerMask groundLayer;

    [Header("�Ĩ��˴�")]
    [Tooltip("�Ĩ�t��")][SerializeField] float dashSpeed;
    [Tooltip("�Ĩ�ɶ�")][SerializeField] float dashTime;
    [Tooltip("�Ĩ�N�o�һݮɶ�")][SerializeField] float dashCD;

    [Header("[�Ű�]����l�������")]
    [Tooltip("�q���Ҧ�����")][SerializeField]public GameObject tvModePart;


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



    public void derictionInput(InputAction.CallbackContext context) //��V��J
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



    //���D��J
    public void jumpInput(InputAction.CallbackContext context)
    {
        if (Grounded()) //�a������ �O�_�����b�a���W
        {
            pState.jumping = true; //�Ұʸ��D
        }
    }

    void jump() //Ĳ�o���D
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

        //���Y���t�צ��̤j�ȷ����Ӥ��O�L���[�t�Y��
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }
    }

    void StopJumpSlow() //���D���� ������w���׫� �|�}�l�Y��
    {
        stepsJumped = 0;
        pState.jumping = false;
    }

    public bool Grounded() //�a������ �O�_�����b�a���W
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
            Debug.Log("������w�t");
        }
        else
        {
            rb.AddForce(new Vector2(12, 5) * knockDown);
            Debug.Log("���k��w�t");
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
