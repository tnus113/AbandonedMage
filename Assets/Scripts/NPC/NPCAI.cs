using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCAI : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        FollowPlayer,
        GuideToBoss,
        FightBoss,
        Celebrate
    }

    [Header("State")]
    [SerializeField] private NPCState currentState = NPCState.Idle;
    public NPCState CurrentState => currentState;

    [Header("References")]
    [SerializeField] private Transform bossTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 5.5f;
    [SerializeField] private float followDistance = 3.0f;
    [SerializeField] private float maxPlayerDistance = 10.0f;
    [SerializeField] private float resumeDistance = 5.0f;
    [SerializeField] private float arriveAtBossRange = 15.0f;

    [Header("Combat Settings")]
    [SerializeField] private float minCombatRange = 6.0f;
    [SerializeField] private float maxCombatRange = 12.0f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private string attackSoundName = "Fireball";

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private float attackTimer;
    private bool isWaitingForPlayer = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Try to find boss if not assigned
        FindBoss();

        // Subscribe to boss defeated event
        HealthSystem.OnBossDefeated += HandleBossDefeated;
    }

    private void OnDestroy()
    {
        HealthSystem.OnBossDefeated -= HandleBossDefeated;
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        switch (currentState)
        {
            case NPCState.Idle:
                HandleIdle();
                break;
            case NPCState.FollowPlayer:
                HandleFollowPlayer();
                break;
            case NPCState.GuideToBoss:
                HandleGuideToBoss();
                break;
            case NPCState.FightBoss:
                HandleFightBoss();
                break;
            case NPCState.Celebrate:
                HandleCelebrate();
                break;
        }

        UpdateAnimator();
    }

    private void FindBoss()
    {
        if (bossTransform == null)
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Enemy");
            if (bossObj != null)
            {
                bossTransform = bossObj.transform;
            }
        }
    }

    private void HandleIdle()
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        // Look at player if player is nearby
        if (player != null)
        {
            LookAtTarget(player.position);
        }
    }

    private void HandleFollowPlayer()
    {
        if (player == null)
        {
            currentState = NPCState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            if (distanceToPlayer > followDistance)
            {
                agent.isStopped = false;
                agent.speed = walkSpeed;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
                LookAtTarget(player.position);
            }
        }
    }

    private void HandleGuideToBoss()
    {
        FindBoss();

        if (bossTransform == null)
        {
            // Fallback to following player if no boss is present in the scene
            currentState = NPCState.FollowPlayer;
            return;
        }

        if (player == null)
        {
            currentState = NPCState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToBoss = Vector3.Distance(transform.position, bossTransform.position);

        // Check if player is too far behind
        if (distanceToPlayer > maxPlayerDistance)
        {
            isWaitingForPlayer = true;
        }
        else if (isWaitingForPlayer && distanceToPlayer <= resumeDistance)
        {
            isWaitingForPlayer = false;
        }

        if (isWaitingForPlayer)
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            LookAtTarget(player.position);
        }
        else
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.speed = walkSpeed;
                agent.SetDestination(bossTransform.position);
            }

            // Transition to fighting boss if within combat arrival range
            if (distanceToBoss <= arriveAtBossRange)
            {
                currentState = NPCState.FightBoss;
                attackTimer = 0f;
            }
        }
    }

    private void HandleFightBoss()
    {
        FindBoss();

        if (bossTransform == null)
        {
            currentState = NPCState.Celebrate;
            return;
        }

        float distanceToBoss = Vector3.Distance(transform.position, bossTransform.position);
        LookAtTarget(bossTransform.position);

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            if (distanceToBoss > maxCombatRange)
            {
                // Boss is too far, run closer
                agent.isStopped = false;
                agent.speed = runSpeed;
                agent.SetDestination(bossTransform.position);
            }
            else if (distanceToBoss < minCombatRange)
            {
                // Boss is too close, back away
                Vector3 retreatDir = (transform.position - bossTransform.position).normalized;
                Vector3 targetPos = transform.position + retreatDir * 5f;

                // Make sure target position is valid on NavMesh
                if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                {
                    agent.isStopped = false;
                    agent.speed = runSpeed;
                    agent.SetDestination(hit.position);
                }
            }
            else
            {
                // Sweet spot, stop moving and attack
                agent.isStopped = true;
            }
        }

        // Attack on cooldown
        if (distanceToBoss <= maxCombatRange)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    private void HandleCelebrate()
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        if (projectilePrefab != null && firePoint != null && bossTransform != null)
        {
            // Aim slightly upwards towards the boss's center
            Vector3 targetPosition = bossTransform.position + Vector3.up * 1f;
            Vector3 direction = (targetPosition - firePoint.position).normalized;

            Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

            if (SoundManager.Instance != null && !string.IsNullOrEmpty(attackSoundName))
            {
                SoundManager.Instance.PlaySound3D(attackSoundName, firePoint.position);
            }
        }
    }

    private void HandleBossDefeated()
    {
        currentState = NPCState.Celebrate;
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("victory");
        }
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f; // Keep rotation horizontal

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime);
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        bool isRunning = false;
        if (agent.isActiveAndEnabled && agent.isOnNavMesh && !agent.isStopped)
        {
            isRunning = agent.velocity.magnitude > 0.15f;
        }

        animator.SetBool("isRunning", isRunning);
    }

    // Public API to trigger state transitions from Dialogue / Events
    public void StartGuiding()
    {
        FindBoss();
        if (bossTransform != null)
        {
            currentState = NPCState.GuideToBoss;
            isWaitingForPlayer = false;
        }
        else
        {
            Debug.LogWarning("NPCAI: Cannot guide, no boss found in scene.");
        }
    }

    public void StartFollowing()
    {
        currentState = NPCState.FollowPlayer;
    }

    public void StopFollowing()
    {
        currentState = NPCState.Idle;
    }
}
