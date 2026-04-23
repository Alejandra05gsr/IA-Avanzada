using Panda;
using UnityEngine;

public class BTEnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float fleeRange = 3f;
    public float health = 100f;

    private MovementController movement;

    void Start()
    {
        movement = this.GetComponent<MovementController>();
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
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        Task.current.Succeed();
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
