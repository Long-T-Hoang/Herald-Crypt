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
    
    public GameObject CheckSurrounding()
    {
        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);

        if (playerCollider != null)
        {
            lastSeenPos = playerCollider.transform.position;
            behaviorScript.currentState = EnemyBehavior.EnemyState.FOLLOW;
        }
        else if(lastSeenPos != transform.position)
        {
            behaviorScript.currentState = EnemyBehavior.EnemyState.FOLLOW;
        }

        if (playerCollider != null)
            return playerCollider.gameObject;
        else return null;
    }

    public float DistanceToPlayer()
    {
        float distance = 0.0f;

        playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerMask.value);

        if(playerCollider != null)
        {
            distance = Vector3.Distance(playerCollider.transform.position, transform.position);
        }

        return distance;
    }

    public float DistanceToPlayer(out GameObject player)
    {
        float distance = DistanceToPlayer();

        if (playerCollider != null) player = playerCollider.gameObject;
        else player = null;

        return distance;
    }

    // Execute follow state functions to be called in main behaviour script
    public void FollowState()
    {
        CheckSurrounding();

        // Look for player if the last path node pos is different from player current node pos
        // Or when path is null
        FindPath();

        if (path == null) Debug.Log("path is null");

        // Move if path is not null and consist of more than 1 tile
        if (path != null && path.Count > 1)
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
            // If path is empty assign path or player position change
            if (path == null || pathFinding.NodeGrid.WorldToCellPos(playerCollider.transform.position) != path[path.Count - 1].cellPos)
            {
                // Find and assign path to temporary list
                List<PathNode> temp = pathFinding.FindPath(transform.position, playerCollider.transform.position);

                SetPath(temp);
            }
        }
        // When player is outside vision
        // Use last seen position
        else if (lastSeenPos != null && lastSeenPos != transform.position)
        {
            // If path is empty assign path or last seen position change
            if (path == null || pathFinding.NodeGrid.WorldToCellPos(lastSeenPos) != path[path.Count - 1].cellPos)
            {
                // Find and assign path to temporary list
                List<PathNode> temp = pathFinding.FindPath(transform.position, lastSeenPos);

                SetPath(temp);
            }
        }
        else
        {
            path = null;
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
            currentNode++;

            if (currentNode >= path.Count - 1)
            {
                path = null;
                currentNode = 0;

                //behaviorScript.currentState = EnemyBehavior.EnemyState.ATTACK;

                return;
            }
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

#if (UNITY_EDITOR)
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
#endif
}
