using UnityEngine;
using UnityEngine.AI;
using Panda;

public class EnemyControllerTactic : MonoBehaviour
{
    public Transform player, goal;
    public float health = 100f;

    public NavMeshAgent agent;
    public Tactics tactics;

    public float lowHealth = 30f;

    [Task]
    public bool IsLowHealth()
    {
        return health < lowHealth;
    }

    [Task]
    public void Idle()
    {
        Task.current.Succeed();
    }

    [Task]
    public void Attack()
    {
        Debug.Log("Wooooosh");
        Task.current.Succeed();
    }

    [Task]
    public void MoveToAttackPoint()
    {
        if (goal == null)
        {
            goal = tactics.GetBestPoint(IAEstado.Attack);
        }


        if (goal == null)
        {
            Task.current.Fail();
            return;
        }

        agent.SetDestination(goal.position);

        if(!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            goal = null;
            Task.current.Succeed();
        }

    }

    [Task]
    public void MoveToSafePoint()
    {
        if (goal == null)
        {
            goal = tactics.GetBestPoint(IAEstado.Flee);
        }


        if (goal == null)
        {
            Task.current.Fail();
            return;
        }

        agent.SetDestination(goal.position);

        if (!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            goal = null;
            Task.current.Succeed();
        }

    }
}
