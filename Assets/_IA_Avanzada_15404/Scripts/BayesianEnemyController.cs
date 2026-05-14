using UnityEngine;
using UnityEngine.AI;

public class BayesianEnemyController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Variables del jugador")]
    public float playerHealth = 100f;

    [Header("Percepción")]
    public float visionRange = 10f;

    [Header("Probabilidades")]
    [Range(0f, 1f)]
    public float priorAttackProbability = 0.5f;

    private float distanceToPlayer;
    private bool playerVisible;

    private float attackProbability;


    private void Update()
    {
        ObservePlayer();

        CalculateBayesianInference();

        DecideAction();
        
    }

    void ObservePlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Vector3 direction = (player.position - transform.position).normalized; //Hacia donde va a apuntar
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 1000f))
        {
            if (hit.transform == player)
            {
                playerVisible = true;
            }
            else
            {
                playerVisible = false;
            }

        }


        //playerVisible = distanceToPlayer <= visionRange;
    }

    void Attack()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        Debug.Log("Atacando");
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        Debug.Log("Persiguiendo");

    }

    void Patrol()
    {
        agent.isStopped = true;
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        Debug.Log("Esperando");
    }

    void CalculateBayesianInference()
    {
        //Probabilidades
        float pDistanceGivenAttack; //Probabilidad del ataque dada la distancia
        float pVisibleGivenAttack; //Probabilidad del ataque dada la visibilidad
        float pHealthGivenAttack; //Probabilidad del ataque dada la salud

        if(distanceToPlayer < 4f)
        {
            pDistanceGivenAttack = 0.8f;
        }
        else if(distanceToPlayer < 8f)
        {
            pDistanceGivenAttack = 0.5f;
        }
        else
        {
            pDistanceGivenAttack = 0.2f;
        }

        pVisibleGivenAttack = playerVisible ? 0.7f : 0.2f;

        if (playerHealth < 30f)
        {
            pHealthGivenAttack = 0.3f;
        }
        else
        {
            pHealthGivenAttack = 0.7f;
        }

        attackProbability = priorAttackProbability * pDistanceGivenAttack * pVisibleGivenAttack * pHealthGivenAttack;

        //Normalizar
        attackProbability = Mathf.Clamp01(attackProbability * 3f);

        Debug.Log("Probabilidad de ataque: " + attackProbability);

    }

    void DecideAction()
    {
        if(attackProbability > 0.65f)
        {
            Attack();
        }
        else if (attackProbability > 0.35)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);


        if (player == null) return;
        Gizmos.color = playerVisible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);

    }
}
