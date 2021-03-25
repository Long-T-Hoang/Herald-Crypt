using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyPathfinding : MonoBehaviour
{
    EnemyBehavior behaviorScript;

    [Header("Enemy attributes")]
    [SerializeField]
    [Range(1.0f, 5.0f)]
    private float speed;
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float detectionRange;

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

    // Start is called before the first frame update
    void Start()
    {
        behaviorScript = GetComponent<EnemyBehavior>();

        // Setting up player detection and pathfinding
        playerCollider = null;
        currentNode = 0;
        lastSeenPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void CheckSurrounding()
    {
        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);

        if (playerCollider != null)
        {
            lastSeenPos = playerCollider.transform.position;
            behaviorScript.currentState = EnemyBehavior.EnemyState.FOLLOW;
        }
        else if(lastSeenPos == transform.position)
        {
            behaviorScript.currentState = EnemyBehavior.EnemyState.IDLE;
        }
    }

    // Execute follow state functions to be called in main behaviour script
    public void FollowState()
    {
        CheckSurrounding();

        // Look for player if the last path node pos is different from player current node pos
        // Or when path is null
        FindPath();

        if (path != null)
        {
            MoveToPlayer();
        }
    }

    // Check radius for player and assign path if detected
    private void FindPath()
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
            else if (pathFinding.NodeGrid.WorldToCellPos(playerCollider.transform.position) != path[path.Count - 1].cellPos)
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
    private void MoveToPlayer()
    {
        float distanceToNextNode = Vector3.Distance(transform.position, path[currentNode].GetPos());

        if (distanceToNextNode >= 0.01f)
        {
            MoveTo(path[currentNode].GetPos());
        }
        else
        {
            if (currentNode >= path.Count - 1)
            {
                path = null;
                currentNode = 0;

                behaviorScript.currentState = EnemyBehavior.EnemyState.ATTACK;

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
        newPos.y += direction.y * speed * Time.deltaTime;

        transform.position = newPos;
    }

    // Debug info
    private void OnDrawGizmos()
    {
        if (!debugOn) return;

        // Detection range gizmo
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, detectionRange);

        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 start = path[i].GetPos();
                Vector2 end = path[i + 1].GetPos();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
