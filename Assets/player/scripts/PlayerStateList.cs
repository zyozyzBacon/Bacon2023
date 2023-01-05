using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    public MainGameManager.playerTeam playerTeam;
    public tvPartGameplay.answer currentAnswer;
    public bool pause;
    public bool walking;
    public bool jumping;
    public bool facingRight;
    public bool dashing;
    public bool dashCoolDowning;
    public bool tvModeOn;
    public bool damaged;
    public bool recoverying;
}
