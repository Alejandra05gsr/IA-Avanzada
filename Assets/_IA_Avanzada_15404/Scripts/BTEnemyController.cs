using Panda;
using UnityEngine;

public class BTEnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float fleeRange = 3f;
    public float health = 100f;

    Vector3 patrolPoint;

    public float patrolRange = 15f;
    public float arrivalDistance = 0.5f;
    private bool hasPatrolPoint = false;

    private MovementController movement;

    void Start()
    {
        movement = this.GetComponent<MovementController>();
    }


    //Elegir un punto aleatorio para patrullar
    //Si el punto aleatorio es valido, mover al enemigo hacia ese punto
    //Si el punto aleatorio no es valido, elegir otro punto aleatorio

    [Task]
    void ChoosePatrolPoint()
    {
        patrolPoint = transform.position + new Vector3(Random.Range(-patrolRange, patrolRange),0,
        Random.Range(-patrolRange, patrolRange));

        hasPatrolPoint = true;

        Task.current.Succeed();
    }




    [Task]
    bool IsLowHealth()
    {
        return health < 30f;
    }

    [Task]
    bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance < detectionRange;
    }

    [Task]
    void Patrol()
    {
        movement.MoveTowards(patrolPoint);

        if (Vector3.Distance(transform.position, patrolPoint) < arrivalDistance)
        {
            hasPatrolPoint = false;
            Task.current.Succeed();
        }
    }


    [Task]
    void Chase()
    {
        movement.MoveTowards(player.position);
        Task.current.Succeed();
    }

    [Task]
    void Flee()
    {
        movement.MoveAway(player.position);
        Task.current.Succeed();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position + transform.forward * 3.0f);
    }

}
