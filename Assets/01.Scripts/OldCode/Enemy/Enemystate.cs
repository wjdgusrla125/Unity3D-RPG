/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState
{
    public abstract void EnterState(EnemyController enemyController);
    public abstract void UpdateState(EnemyController enemyController);
    public abstract void ExitState(EnemyController enemyController);
}

public class IdleState : EnemyState
{
    public override void EnterState(EnemyController enemyController)
    {
        
    }

    public override void UpdateState(EnemyController enemyController)
    {
        float distanceToPlayer = Vector3.Distance(enemyController.transform.position, GameManager.Instance.Player.transform.position);
        
        if (distanceToPlayer <= enemyController.chaseRange)
        {
            enemyController.ChangeState(new ChaseState());
        }
        
        if (enemyController.currentHP == 0)
        {
            enemyController.ChangeState(new DeadState());
        }
        
        enemyController._enemyanimator.Play("Idle");
    }

    public override void ExitState(EnemyController enemyController)
    {
        enemyController.navMeshAgent.ResetPath();
    }
}

public class ChaseState : EnemyState
{
    public override void EnterState(EnemyController enemyController)
    {
        enemyController.navMeshAgent.speed = enemyController.chaseSpeed;
        enemyController.navMeshAgent.destination = GameManager.Instance.Player.transform.position;
        enemyController._enemyanimator.Play("Chase");
    }

    public override void UpdateState(EnemyController enemyController)
    {
        float distanceToPlayer = Vector3.Distance(enemyController.transform.position, GameManager.Instance.Player.transform.position);
        
        enemyController.navMeshAgent.destination = GameManager.Instance.Player.transform.position;
        
        if (distanceToPlayer <= enemyController.attackRange)
        {
            enemyController.ChangeState(new AttackState());
        }
        else if(distanceToPlayer > enemyController.chaseRange)
        {
            enemyController.ChangeState(new IdleState());
        }
        
        if (enemyController.currentHP == 0)
        {
            enemyController.ChangeState(new DeadState());
        }
        
        enemyController._enemyanimator.Play("Chase");
    }

    public override void ExitState(EnemyController enemyController)
    {
        enemyController.navMeshAgent.ResetPath();
    }
}

public class AttackState : EnemyState
{
    public override void EnterState(EnemyController enemyController)
    {
        enemyController._enemyanimator.Play("Attack");
    }

    public override void UpdateState(EnemyController enemyController)
    {
        float distanceToPlayer = Vector3.Distance(enemyController.transform.position, GameManager.Instance.Player.transform.position);
        
        if (distanceToPlayer > enemyController.attackRange)
        {
            enemyController.ChangeState(new ChaseState());
        }
        
        if (enemyController.currentHP == 0)
        {
            enemyController.ChangeState(new DeadState());
        }
    }

    public override void ExitState(EnemyController enemyController)
    {
        enemyController.navMeshAgent.ResetPath();
    }
}

public class DeadState : EnemyState
{
    public override void EnterState(EnemyController enemyController)
    {
        enemyController._enemyanimator.Play("Dead");
        enemyController.DropItem();
        enemyController._capsuleCollider.enabled = false;
    }

    public override void UpdateState(EnemyController enemyController)
    {
        
    }

    public override void ExitState(EnemyController enemyController)
    {
        
    }
}*/