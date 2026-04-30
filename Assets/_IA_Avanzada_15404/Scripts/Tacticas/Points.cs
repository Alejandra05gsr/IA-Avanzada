using UnityEngine;

public class Points : MonoBehaviour
{
    public float coverValue; //0-1 ,, 0 no hay cobertura y 1 si hay cobertura
    public Transform player;
    public float maxDistance = 50f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateCover();
    }

    void EvaluateCover()
    {
        Vector3 direction = (player.position - transform.position).normalized; //Hacia donde va a apuntar
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            if (hit.transform == player)
            {
                coverValue = 0;
            }
            else
            {
                coverValue = 1;
            }

        }
    }


    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);
        Gizmos.color = (coverValue > 0.5f) ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);

    }


}
