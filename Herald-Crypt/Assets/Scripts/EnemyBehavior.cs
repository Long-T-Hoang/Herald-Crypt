using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public Pathfinding pathFinding;
    private List<PathNode> path;
    private int currentNode;

    // Player attributes
    private Collider2D playerCollider;
    private Vector3 lastSeenPos;

    // Possible states of enemy
    private enum EnemyState
    {
        IDLE,
        FOLLOW,
        ATTACK,
        ROAM  
    }

    private EnemyState currentState;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = null;

        currentNode = 0;

        lastSeenPos = transform.position;

        currentState = EnemyState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.IDLE:
                playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);

                if (playerCollider != null)
                {
                    lastSeenPos = playerCollider.transform.position;
                    currentState = EnemyState.FOLLOW;
                }

                break;

            case EnemyState.FOLLOW:
                FollowState();
                break;

            default:
                break;
        }
    }

    private void FollowState()
    {
        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);

        if (playerCollider != null) lastSeenPos = playerCollider.transform.position;

        // Look for player if the last path node pos is different from player current node pos
        // Or when path is null
        LookForPlayer();

        if (path != null)
        {
            FollowPlayer();
        }
    }

    // Check radius for player and assign path if detected
    private void LookForPlayer()
    {
        // When player is in vision
        if (playerCollider != null)
        {
            // Find and assign path to temporary list
            List<PathNode> temp = pathFinding.FindPath(transform.position, playerCollider.transform.position);
            
            // If path is empty assign path
            if (path == null)
            {
                SetPath(temp);
            }
            // If player grid position and destination grid position is different, assign path
            else if(pathFinding.NodeGrid.WorldToCellPos(playerCollider.transform.position) != path[path.Count - 1].cellPos)
            {
                SetPath(temp);
            }
        }
        // When player is outside vision
        // Use last seen position
        else if (lastSeenPos != null && lastSeenPos != transform.position)
        {
            // Find and assign path to temporary list
            List<PathNode> temp = pathFinding.FindPath(transform.position, lastSeenPos);

            // If path is empty assign path
            if (path == null)
            {
                SetPath(temp);
            }
            // If last seen grid position and destination grid position is different, assign path
            else if (pathFinding.NodeGrid.WorldToCellPos(lastSeenPos) != path[path.Count - 1].cellPos)
            {
                SetPath(temp);
            }
        }
    }

    private void SetPath(List<PathNode> temp)
    {
        path = temp;
        currentNode = 1;
    }

    // Follow player based on path
    private void FollowPlayer()
    {
        float distanceToNextNode = Vector3.Distance(transform.position, path[currentNode].GetPos());
        
        if(distanceToNextNode >= 0.01f)
        {
            MoveTo(path[currentNode].GetPos());
        }
        else
        {
            if (currentNode >= path.Count - 1)
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
        //Handles.color = Color.red;
        //Handles.DrawWireDisc(transform.position, transform.forward, detectionRange);
        
        if(path != null)
        {
            for(int i = 0; i < path.Count - 1; i++)
            {
                Vector2 start = path[i].GetPos();
                Vector2 end = path[i + 1].GetPos();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(start, end);
            }
        }
    }

    /*
    IEnumerator FollowPlayer()
    {

    }
    */
}
