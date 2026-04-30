using UnityEngine;

public class GoalsEnemyController : MonoBehaviour
{
    public Transform player;
    public float health = 100f;

    public float detectionRange = 10f;

    private MovementController movement;


    void Start()
    {
        movement = this.GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        float patrolScore = EvaluatePatrol();
        float chaseScore = EvaluateChase();
        float fleeScore = EvaluateFlee();

        Debug.Log($"Patrol: {patrolScore} | Chase: {chaseScore} | Flee: {fleeScore}");

        //Seleccionar una acción
        float maxScore = Mathf.Max(patrolScore, chaseScore, fleeScore);

        if (maxScore == fleeScore)
        {
            Flee();
        }
        else if (maxScore == chaseScore)
        {
            Chase();
        }
        else
        {
            Patrol();
        }

    }

    float GetNormalizedDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        return Mathf.Clamp01(distance/detectionRange);
    }

    //Flee
    float EvaluateFlee()
    {
        //0 - cerca , 1 - lejos
        float distance = GetNormalizedDistance();

        //1 poca vida
        float healthFactor = 1 - (health / 100f);

        return healthFactor * (1 - distance);
    }


    //Chase
    float EvaluateChase()
    {
        float distance = GetNormalizedDistance();

        float healthFactor = health / 100f;

        return (1 - distance) * healthFactor;
    }


    //Patrol
    float EvaluatePatrol()
    {
        float distance = GetNormalizedDistance();

        return distance;
    }


    void Patrol()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
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

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position + transform.forward * 3.0f);
    }

}
