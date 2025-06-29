using UnityEngine;
using System.Collections.Generic;

public class ChameleonIdleMover : MonoBehaviour
{
    public Transform[] bodySegments; // 0 = tail, 1 = mid, 2 = head
    public float moveInterval = 0.4f;
    public float tileSize = 1f;

    private float moveTimer;
    private Queue<Vector3> previousPositions = new Queue<Vector3>();

    private Vector2Int[] path = new Vector2Int[]
    {
        Vector2Int.right, Vector2Int.right, Vector2Int.right, Vector2Int.right, Vector2Int.right, Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left, Vector2Int.left, Vector2Int.left,
        Vector2Int.down,
        Vector2Int.right, Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left, Vector2Int.left,
        Vector2Int.down,

        Vector2Int.right, Vector2Int.right, Vector2Int.right,
        Vector2Int.up,
        Vector2Int.left, Vector2Int.left, Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right, Vector2Int.right,
        Vector2Int.up,
        Vector2Int.left, Vector2Int.left, Vector2Int.left, Vector2Int.left, Vector2Int.left,
        Vector2Int.up,
    };




    private int pathIndex = 0;
    private Vector3Int gridPos;

    void Start()
    {
        gridPos = Vector3Int.FloorToInt(bodySegments[2].position); // Start with head's grid position

        for (int i = 0; i < bodySegments.Length; i++)
            previousPositions.Enqueue(bodySegments[i].position);
    }

    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval)
        {
            moveTimer = 0f;

            // Compute next position
            Vector2Int direction = path[pathIndex];
            pathIndex = (pathIndex + 1) % path.Length;

            gridPos += new Vector3Int(direction.x, direction.y, 0);
            Vector3 newHeadWorldPos = gridPos + new Vector3(0.5f, 0.5f, 0);

            // Add new head position to the queue
            previousPositions.Enqueue(newHeadWorldPos);

            // Pop tail's old position
            if (previousPositions.Count > bodySegments.Length)
                previousPositions.Dequeue();

            // Apply positions
            Vector3[] posArray = previousPositions.ToArray();
            for (int i = 0; i < bodySegments.Length; i++)
            {
                bodySegments[i].position = posArray[i];
            }

            // Flip sprite based on direction
            if (direction != Vector2Int.zero)
            {
                float scaleX = direction.x != 0 ? Mathf.Sign(direction.x) : Mathf.Sign(bodySegments[2].localScale.x);
                foreach (var seg in bodySegments)
                {
                    Vector3 s = seg.localScale;
                    s.x = Mathf.Abs(s.x) * scaleX;
                    seg.localScale = s;
                }
            }
        }
    }
}
