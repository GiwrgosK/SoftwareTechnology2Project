using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class BotTracker : MonoBehaviour
{
    public float followRange = 100f;
    public string targetTag = "Unit";
    public string hqTag = "PlayerHQ";

    private UnitMover mover;
    private Transform target;

    private float followTimer = 0f;
    private const float maxFollowDuration = 20f;
    private const float stopDistance = 0.1f;

    private float idleTimer = 0f;
    private const float idleDuration = 1f;
    private bool isIdle = false;

    private readonly int damage = 30;
    private readonly float damageCooldown = 0.1f;
    private float lastDamageTime = -999f;

    private void Awake()
    {
        mover = GetComponent<UnitMover>();
    }

    private void Update()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                idleTimer = 0f;
                isIdle = false;
            }
            return;
        }

        if (target != null && (!target.gameObject.activeInHierarchy || target == null))
        {
            target = null;
        }

        if (target == null)
        {
            followTimer = 0f;
            Collider[] hits = Physics.OverlapSphere(transform.position, followRange);
            foreach (var hit in hits)
            {
                if (hit.CompareTag(targetTag) && hit.gameObject.activeInHierarchy)
                {
                    target = hit.transform;
                    break;
                }
            }

            if (target == null)
            {
                GameObject hq = GameObject.FindGameObjectWithTag("PlayerHQ");
                if (hq != null && hq.activeInHierarchy)
                {
                    target = hq.transform;
                }
            }
        }

        if (target == null)
        {
            mover.MoveTo(transform.position);
            isIdle = true;
            idleTimer = 0f;
            return;
        }

        followTimer += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target.position);

        if (followTimer > maxFollowDuration || distance <= stopDistance)
        {
            target = null;
            mover.MoveTo(transform.position);
            isIdle = true;
            idleTimer = 0f;
            return;
        }
        mover.MoveTo(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        if (other.CompareTag(targetTag) || other.CompareTag(hqTag))
        {
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        if (other.CompareTag(targetTag) || other.CompareTag(hqTag))
        {
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}