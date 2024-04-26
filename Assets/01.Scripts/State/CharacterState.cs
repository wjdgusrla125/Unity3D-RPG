using UnityEngine;

public enum CH_STATE
{
    IDLE,
    RUN,
    DEAD,
    BASEATTACK = 10,
    DOUBLEATTACK,
    SPINATTACK,
    BLOCK,
    MS_IDLE = 20,
    MS_CHASE,
    MS_ATTACK,
    MS_COMBOATTACK,
    MS_SPINATTACK,
    MS_HEAVYATTACK,
    MS_DEFENCE,
    MS_DEAD
}

public class CharacterState : MonoBehaviour
{
    //플레이어
    public float horizontal;
    public float vertical;
    public float playerSpeed = 5f;
    public float playerRotationSpeed = 600f;
    public float yVelocity;

    //몬스터
    public float MS_attackRange = 2f;
    public float MS_ChaseRange = 10f;
    public float MS_RunSpeed = 2f;
    public float MS_AttackDamage = 5f;
}
