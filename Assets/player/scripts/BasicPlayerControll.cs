using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class BasicPlayerControll : MonoBehaviour
{
    [Header("���Ѭ���")]
    [Tooltip("���a")] [SerializeField] public int ID;
    [Tooltip("�C��")] [SerializeField] public int Color;

    [Header("���ʬ���")]
    [Tooltip("���ʳt��")] [SerializeField] float walkSpeed;
    [Tooltip("�q���Ҧ��t��")] [SerializeField] public float tvMoveSpeed;

    [Header("���D����")]
    [Tooltip("���D�O�D")] [SerializeField] float jumpForce;
    [Tooltip("�Y���t�׷���")] [SerializeField] float fallSpeed;
    [Tooltip("�O�_���\�h�q���D")] [SerializeField] bool doubleJump;
    [Tooltip("�h�q���D����")] [SerializeField] int jumpAir;

    [Header("���ˬ���")]
    [Tooltip("���˫�L�Įɶ�")] [SerializeField] float recoveryTime;
    [Tooltip("���˫�w�t�ɶ�")] [SerializeField] float dizzyTime;
    [Tooltip("���˫����h�O�D")] [SerializeField] float knockDown;

    [Header("�a�������˴�")]
    [Tooltip("�g�u�˴��Z��(����)")] [SerializeField] float groundCheckY;
    [Tooltip("�����a������Layer")] [SerializeField] LayerMask groundLayer;

    [Header("�Ĩ��˴�")]
    [Tooltip("�Ĩ�t��")] [SerializeField] float dashSpeed;
    [Tooltip("�Ĩ�ɶ�")] [SerializeField] float dashTime;
    [Tooltip("�Ĩ�N�o�һݮɶ�")] [SerializeField] float dashCD;

    [Header("���k����")]
    [Tooltip("�ï]�ƶq")] [SerializeField] public int bubbles;
    [Tooltip("�ï]���D��ƶq")] [SerializeField] public int bubblesforItem;
    [Tooltip("�ï]�l�u�t��")] [SerializeField] public float bubbleSpeed;
    [Tooltip("�����w�Įɶ�")] [SerializeField] public float attackTime;
    [Tooltip("�O�_���\����")] [SerializeField] public bool allowAttack;
    [Tooltip("���`���W�ï]")] [SerializeField] public GameObject deadBubble;
    [Tooltip("�ï]�C��")] [SerializeField] public ItemManager.foodColor FoodColor;

    [Header("�D�����")]
    [Tooltip("��W�D��ID")] [SerializeField] public GameObject CurrentItem;

    [Header("[�Ű�]����l�������")]
    [Tooltip("�q���Ҧ�����")] [SerializeField] public GameObject tvModePart;
    [Tooltip("�Ĩ뼲�H����")] [SerializeField] private GameObject DashCollider;
    [Tooltip("���F�Ҧ�����")] [SerializeField] private GameObject GhostPlayer;
    [Tooltip("�j����")] [SerializeField] private GameObject GunPower;
    [Tooltip("�l�u����")] [SerializeField] private GameObject BulletPrefabs;
    [Tooltip("���`���W�ï]�l����")] public GameObject deadBubbleOnHand;

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

    //���ʬ���/////////////////////////////////////////////////////////
    public void derictionInput(InputAction.CallbackContext context) //��V��J
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


    //���ʬ���/////////////////////////////////////////////////////////
    //���D����/////////////////////////////////////////////////////////

    public void jumpInput(InputAction.CallbackContext context) //��J���D�ƥ�
    {
        if (context.started == false) return;
        JumpBuffer =0.15f;

    }
    private void jumpDetect() //���D���������A������(Update())
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

        //���Y���t�צ��̤j�ȷ����Ӥ��O�L���[�t�Y��
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
    }

    private bool IsGrounded() //�a������ �O�_�����b�a���W
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckY, groundLayer))
            return true;
        else
            return false;
    }

    //���D����/////////////////////////////////////////////////////////
    //�Ĩ����/////////////////////////////////////////////////////////


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

    //�Ĩ����/////////////////////////////////////////////////////////
    //�q�����k����/////////////////////////////////////////////////////

    public void frozenForTV() //�|����Ʀ�
    {
        StopAllCoroutines();
        StopAllCoroutines();

        pState.pause = true;

        pState.dashing = false;
        pState.dashCoolDowning = false;
    }

    public void unfrozen() //�|����Ʀ�
    {
        pState.pause = false;
    }

    public void inTvMode() //�|����@��
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

    public void outTvMode() //�|����@��
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
    ///  ���S�m�컻���������q���a => 
    ///  
    ///  frozenForTV() => inTvMode() => unfrozen() => frozenForTV() => outTvMode() => frozenForTV() => unfrozen()
    ///  
    ///  ���m�컻���������q���a => 
    ///  
    /// frozenForTV() => inTvMode() => (unfrozen() => frozenForTV()) => unfrozen() => outTvMode() => frozenForTV() => unfrozen()
    /// 
    /// ���m�컻�������e�N�w�g�X�������a => 
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

    //�q�����k����/////////////////////////////////////////////////////
    //���ˬ���/////////////////////////////////////////////////////////

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
            Debug.Log("������w�t");
        }
        else
        {
            rb.AddForce(new Vector2(12, 5) * knockDown);
            Debug.Log("���k��w�t");
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

    //���ˬ���/////////////////////////////////////////////////////////
    //�h������/////////////////////////////////////////////////////////

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

    //�h������/////////////////////////////////////////////////////////
    //�Ȱ�����/////////////////////////////////////////////////////////

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

    //�Ȱ�����/////////////////////////////////////////////////////////
    //�Y��������///////////////////////////////////////////////////////

    public void eating(GameObject foodObject)
    {
        pState.eating = true;

        //�p�G�Y���C�⤣��
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

    //�Y��������///////////////////////////////////////////////////////
    //�g������/////////////////////////////////////////////////////////

    public void shootInput(InputAction.CallbackContext context) //��J�����ƥ�
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


    //�g������/////////////////////////////////////////////////////////
    //�s��е{����/////////////////////////////////////////////////////

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


    //�s��е{����/////////////////////////////////////////////////////
    //������������/////////////////////////////////////////////////////

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

    //������������/////////////////////////////////////////////////////
    //�H������D�����/////////////////////////////////////////////////

    void itemGet()
    {
        int r = Random.Range(0, ItemManager.instance.ItemList.Length);
        CurrentItem = ItemManager.instance.ItemList[r];
        pUI.GetItemUI(CurrentItem.GetComponent<ShowItem>().Icon);
    }

    //�H������D�����/////////////////////////////////////////////////
    //�ϥιD�����/////////////////////////////////////////////////////

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


    //�ϥιD�����/////////////////////////////////////////////////////
}
