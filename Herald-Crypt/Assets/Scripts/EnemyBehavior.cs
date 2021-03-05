using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy attributes")]
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float detectionRange;
    [SerializeField]
    [Range(1.0f, 5.0f)]
    private float speed;

    [Header("Misc")]
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private bool debugOn;

    // Pathfinding
    private Pathfinding pathFinding;
    private List<PathNode> path;
    private int currentNode;

    // Player attributes
    private Collider2D playerCollider;
    private Vector3 lastSeenPos;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = null;

        pathFinding = new Pathfinding(20, 20, 1);
        currentNode = 0;

        lastSeenPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);

        if(playerCollider != null) lastSeenPos = playerCollider.transform.position;

        if (path == null)
        {
            LookForPlayer();
        }
        else
        {
            FollowPlayer();
        }
    }

    // Check radius for player
    private void LookForPlayer()
    {
        if (playerCollider != null)
        {
            path = pathFinding.FindPath(transform.position, playerCollider.transform.position);
        }
        else if (lastSeenPos != null && lastSeenPos != transform.position)
        {
            path = pathFinding.FindPath(transform.position, lastSeenPos);
        }

        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].GetPos(), path[i + 1].GetPos(), Color.green, 100f);
            }
        }
    }

    // Follow player
    private void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, path[currentNode].GetPos());
        
        if(distance >= 0.01f)
        {
            MoveTo(path[currentNode].GetPos());
        }
        else
        {
            if (currentNode == path.Count - 1)
            {
                path = null;
                currentNode = 0;
                return;
            }

            currentNode++;
        }
    }

    // Move to position
    private void MoveTo(Vector3 pos)
    {
        Vector2 newPos = transform.position;
        Vector2 direction = pos - transform.position;
        direction.Normalize();

        newPos.x += direction.x * speed * Time.deltaTime;
        newPos.y += direction.y *speed * Time.deltaTime;

        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Projectiles"))
        {
            Destroy(gameObject);
        }
    }


    // Debug info
    private void OnDrawGizmos()
    {
        if (!debugOn) return;

        // Detection range gizmo
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, detectionRange);
    }

    /*
    IEnumerator FollowPlayer()
    {

    }
    */
}
