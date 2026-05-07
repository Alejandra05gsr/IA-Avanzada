using UnityEngine;

public class FuzzyEnemyController : MonoBehaviour
{

    public Transform player;
    public float health = 100;

    private MovementController movement;
   
    void Start()
    {
        movement = this.GetComponent<MovementController>();
        
    }

  
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.position);

        float ruleAttackStrong = AND(HighHealth(health), CloseDistance(distance));
        float ruleAttackMedium = AND(MediumHealth(health), CloseDistance(distance));
        float ruleFlee = AND(LowHealth(health), CloseDistance(distance));
        float rulePatrol = AND(HighHealth(health), FarDistance(distance));

        float attack = (ruleAttackStrong * 1.0f) + (ruleAttackMedium * 0.6f);
        float flee = ruleFlee * 1.0f;
        float patrol = rulePatrol * 1.0f;

        string debugMessage = string.Format(
            "<color = yellow>VALORES DIFUSOS:</color>" +
            "<color = green>Attack:{0:F2}</color>|" +
            "<color = red>Flee:{0:F2}</color>|" +
            "<color = cyan>Patrol:{0:F2}</color>|",
            attack, flee, patrol);
        Debug.Log(debugMessage);

        if(attack > flee && attack > patrol)
        {
            Chase();
        }
        else if(flee > attack && flee >patrol)
        {
            Flee();
        }
        else
        {
            Patrol();
        }
    }

    //Fuzzification (Conjuntos difusos)


    //------Salud------
    //Hombro Izquierdo <= 30 | 30 a 60
    float LowHealth(float health)
    {
        if (health <= 30) return 1f;
        if (health >= 60) return 0f;

        return (60 - health) / 30;
    }


    //Triangular 50 | 30 a 70
    float MediumHealth(float health)
    {
        if (health <= 30 || health >= 70) return 0;
        if (health == 50) return 1f;
        if (health < 50)
        {
            return (health - 30) / 20f;
        }
        else 
        {
            return (70 - health) / 20f;
        }
    }


    //Hombro Derecho >== 80 | 50 a 80
    float HighHealth(float health)
    {
        if (health <= 50) return 0;
        if (health >= 80) return 1f;

        return (health - 50) / 30f;
    }


    //------DistanciaJugador------
    //Hombro Izquierdo <= 5 | 5 a 15
    float CloseDistance(float distance)
    {
        if (distance <= 5) return 1f;
        if (distance >= 15) return 0f;

        return (15 - distance) / 10;
    }

    //Hombro Derecho >= 20 | 10 a 20
    float FarDistance(float distance)
    {
        if (distance <= 10) return 0;
        if (distance >= 20) return 1f;

        return (distance - 10) / 10f;
    }


    //Operaciones Difusas
    float AND(float a, float b)
    {
        return Mathf.Min(a, b);
    }

    float OR(float a, float b)
    {
        return Mathf.Max(a, b);
    }

    float NOT(float a)
    {
        return 1 - a;
    }


    void Patrol()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
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
        Gizmos.DrawWireSphere(transform.position, 20);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position + transform.forward * 3.0f);
    }

}
