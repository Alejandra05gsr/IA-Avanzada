using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float fleeRange = 3f;

    public float health = 100f;

    private MovementController movement;


    private void Start()
    {
        movement = this.GetComponent<MovementController>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(health < 30f && distance < detectionRange)
        {
            Flee();
        }
        else if(distance < detectionRange)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.Rotate(0,50*Time.deltaTime,0);
        health = health < 100 ? health + 10 : health;
    }

    void Chase()
    {
        movement.MoveTowards(player.position);
    }

    void Flee()
    {
        movement.MoveAway(player.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position + transform.forward * 3.0f);
    }

}
