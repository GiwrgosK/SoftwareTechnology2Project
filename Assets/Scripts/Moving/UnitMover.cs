using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls unit movement (e.g., soldiers, vehicles).
/// Uses NavMeshAgent for smart pathfinding and keeps units on a flat plane.
/// Adds acceleration for Bots tagged "Bot".
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class UnitMover : MonoBehaviour
{
    #region Inspector Variables
    private NavMeshAgent _agent;    // Handles pathfinding and movement
    private Rigidbody _rigidbody;   // Manages physics (kept kinematic to avoid conflicts)
    #endregion

    #region Tracking Internal Variables
    // Tracking variables
    private Vector3 _targetDestination;  // Where the unit is heading
    private float _originalHeight;       // Keeps unit at starting height
    private Vector3 _lastPosition;       // Tracks position for stuck detection
    private float _stuckTimer;           // Timer for checking if unit is stuck
    private bool _isMoving;              // Is the unit currently moving?
    private float _lastPathAdjustTime;   // Cooldown for path adjustments
    #endregion

    #region Adjustable Settings for each prefab of a unit
    // Adjustable settings (change these in Unity Inspector)
    [Header("Movement Settings")]
    [SerializeField, Tooltip("How fast the unit moves.")]
    private float speed = 3.5f;
    [SerializeField, Tooltip("How fast the unit turns.")]
    private float angularSpeed = 360f;
    [SerializeField, Tooltip("Height of the unit for pathfinding.")]
    private float agentHeight = 2.0f;

    [Header("Acceleration Settings")]
    [SerializeField, Tooltip("Acceleration for bots.")]
    private float botAcceleration = 10f;

    [SerializeField, Tooltip("Default acceleration for other units.")]
    private float defaultAcceleration = 8f;
    #endregion

    #region Non-adjustable settings (fixed values)
    private float agentRadius = 0.7f;           // Size of the unit for avoiding others
    private float stoppingDistance = 0.1f;      // How close to the target before stopping
    private float stuckCheckInterval = 0.2f;    // How often to check if the unit is stuck (seconds)
    private float stuckMovementThreshold = 0.05f; // Minimum distance moved to not be 'stuck'
    private float detourMultiplier = 3f;        // How far to detour around obstacles (multiplier)
    private float pathResumeDelay = 0.3f;       // Delay before returning to original path (seconds)
    private float pathAdjustCooldown = 0.2f;    // Cooldown between path changes (seconds)
    #endregion

    private void Awake()
    {
        // Set up components
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();

        // Configure NavMeshAgent with our settings
        _agent.speed = speed;
        _agent.angularSpeed = angularSpeed;
        _agent.radius = agentRadius;
        _agent.height = agentHeight;
        _agent.stoppingDistance = stoppingDistance;

        // Set acceleration based on tag "Bot"
        if (CompareTag("Bot"))
        {
            _agent.acceleration = botAcceleration;
        }
        else
        {
            _agent.acceleration = defaultAcceleration;
        }

        // Extra NavMeshAgent settings
        _agent.updatePosition = true;  // Let agent control position
        _agent.updateRotation = true;  // Let agent control rotation
        _agent.autoTraverseOffMeshLink = false; // No automatic off-mesh jumps
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        _agent.autoRepath = true;      // Recalculate path if blocked

        // Rigidbody setup: no physics interference
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        // Lock unit to starting height (flat movement only)
        _originalHeight = transform.position.y;
        _lastPosition = transform.position;
    }

    /// <summary>
    /// Tells the unit to move to a new location.
    /// </summary>
    public void MoveTo(Vector3 destination)
    {
        _originalHeight = transform.position.y;

        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            float searchRadius = 3f;
            int attempts = 8;

            bool found = false;
            for (int i = 0; i < attempts; i++)
            {
                float angle = i * Mathf.PI * 2 / attempts;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * searchRadius;
                Vector3 testPoint = destination + offset;

                if (NavMesh.SamplePosition(testPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    found = true;
                    break;
                }
            }
            if (!found) return;
        }

        _targetDestination = hit.position;
        _agent.SetDestination(_targetDestination);
        _isMoving = true;
        _lastPosition = transform.position;
        _stuckTimer = 0f;
    }

    // Main update method that tracks the path moving of the agent
    private void Update()
    {
        if (!_isMoving) return; // Do nothing if not moving

        // Keep unit on flat plane
        transform.position = new Vector3(transform.position.x, _originalHeight, transform.position.z);

        // Check if unit is stuck
        _stuckTimer += Time.deltaTime;
        if (_stuckTimer >= stuckCheckInterval)
        {
            float distanceMoved = Vector3.Distance(transform.position, _lastPosition);
            if (distanceMoved < stuckMovementThreshold && _agent.hasPath)
            {
                // Try to detour
                AdjustPathAroundObstacle();
            }
            _lastPosition = transform.position;
            _stuckTimer = 0f;
        }

        // Stop when close to target
        if (_agent.remainingDistance <= _agent.stoppingDistance && _agent.hasPath)
        {
            _isMoving = false;
        }
    }

    // If the unit collides with an obstactle , dynamically adjust the path
    private void OnCollisionEnter(Collision collision)
    {
        // On hitting an obstacle, adjust path (with a cooldown)
        if (collision.gameObject.GetComponent<NavMeshObstacle>() != null &&
            Time.time - _lastPathAdjustTime > pathAdjustCooldown)
        {
            AdjustPathAroundObstacle();
            _lastPathAdjustTime = Time.time;
        }
    }

    /// <summary>
    /// Tries to find a way around an obstacle by checking multiple directions.
    /// </summary>
    private void AdjustPathAroundObstacle()
    {
        // Search for a clear detour point in 8 directions, or fall back to a nearby valid spot
        Vector3 currentPos = transform.position;
        float detourDistance = _agent.radius * detourMultiplier;
        int directions = 8; // Check 8 directions around unit
        float angleStep = 360f / directions;

        for (int i = 0; i < directions; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 detourPoint = currentPos + direction * detourDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(detourPoint, out hit, detourDistance, NavMesh.AllAreas))
            {
                // Check if detour is free of obstacles
                Collider[] obstacles = Physics.OverlapSphere(hit.position, _agent.radius, LayerMask.GetMask("Obstacles"));
                if (obstacles.Length == 0)
                {
                    _agent.SetDestination(hit.position);
                    CancelInvoke(nameof(ResumeOriginalDestination));
                    Invoke(nameof(ResumeOriginalDestination), pathResumeDelay);
                    return;
                }
            }
        }

        // If no detour works, try nearest valid spot
        if (NavMesh.SamplePosition(currentPos, out NavMeshHit fallbackHit, 5.0f, NavMesh.AllAreas))
        {
            _agent.SetDestination(fallbackHit.position);
            CancelInvoke(nameof(ResumeOriginalDestination));
            Invoke(nameof(ResumeOriginalDestination), pathResumeDelay);
        }
        else
        {
            Debug.LogWarning($"Unit {gameObject.name} is trapped!");
            _isMoving = false; // Give up if no path exists
        }
    }

    /// <summary>
    /// Goes back to the original target after a detour.
    /// </summary>
    private void ResumeOriginalDestination()
    {
        if (_isMoving)
        {
            _agent.SetDestination(_targetDestination);
        }
    }

    public void Stop() {
        _agent.ResetPath();
        _isMoving = false;
    }
}
