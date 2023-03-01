using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Professors
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("References")] 
    
        [SerializeField] private Transform player;

        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private LayerMask target;

        [SerializeField] private LayerMask obstacles;

        [Header("Statistics")]

        [SerializeField] private float walkSpeed;

        [SerializeField] private float chaseSpeed;
    
        [SerializeField] private float walkViewRadius;

        [SerializeField] private float chaseViewRadius;

        [SerializeField] private float walkViewAngle;

        [SerializeField] private float chaseViewAngle;

        [SerializeField] private float walkStoppingDistance;

        [SerializeField] private float distancePlayerCatch;

        [Header("Wandering parameters")] 
    
        [SerializeField] private float wanderingWaitTimeMin;

        [SerializeField] private float wanderingWaitTimeMax;

        [SerializeField] private float wanderingDistanceMin;
    
        [SerializeField] private float wanderingDistanceMax;
    
        [Header("Private parameters")]

        private float _viewRadius;

        private float _viewAngle;

        private bool _isInView;

        private bool _isCatch;

        private bool _hasDestination;
        
        private readonly Vector3[] _destinations = 
        {
            new (15, .5f, -15),
            new (-15, .5f, -15),
            new (-15, .5f, 15),
            new (15, .5f, 15)
        };

        private void Update()
        {
            _isCatch = false;
            _isInView = false;
            if (Vector3.Distance(transform.position, player.position) < _viewRadius)
            {
                Vector3 dirToTarget = (player.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
                {
                    float disToTarget = Vector3.Distance(transform.position, player.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacles))
                    {
                        _isInView = true;
                        if (Vector3.Distance(transform.position, player.position) < distancePlayerCatch)
                        {
                            _isCatch = true;
                        }
                    }
                }
            }
            
            Action();
        }

        private void Action()
        {
            if (_isCatch)
            {
                agent.isStopped = true;
            }
            else if (_isInView)
            {
                agent.isStopped = false;
                FollowPlayer();
            }
            else if (!agent.hasPath && !_hasDestination)
            { 
                Npc();
            }
        }
    
        private void FollowPlayer()
        {
            _viewRadius = chaseViewRadius;
            _viewAngle = chaseViewAngle;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
    
        private void Npc()
        {
            _viewRadius = walkViewRadius;
            _viewAngle = walkViewAngle;
            agent.speed = walkSpeed;
            agent.stoppingDistance = walkStoppingDistance;
            float delay = Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax);
            StartCoroutine(Random.Range(0, 2) == 0 ? GoToDestination(delay) : StayOnSite(delay));
        }

        private IEnumerator GoToDestination(float delay)
        {
            _hasDestination = true;
            yield return new WaitForSeconds(delay);
            
            agent.SetDestination(_destinations[Random.Range(0, _destinations.Length)]);
            _hasDestination = false;
        }

        private IEnumerator StayOnSite(float delay)
        {
            _hasDestination = true;
            yield return new WaitForSeconds(delay);

            Vector3 nextDestination;
            NavMeshHit hit;
            do
            {
                nextDestination = transform.position;
                nextDestination += Random.Range(wanderingDistanceMin, wanderingDistanceMax) *
                                   new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
            } while (!NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas));
            agent.SetDestination(hit.position);
            _hasDestination = false;
        }
        
        private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _viewRadius);
            
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, distancePlayerCatch);
            
            Handles.color = Color.yellow;
            Vector3 viewAngleA = DirFromAngle(-_viewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(_viewAngle / 2, false);
            Handles.DrawLine(transform.position, transform.position + viewAngleA * _viewRadius);
            Handles.DrawLine(transform.position, transform.position + viewAngleB * _viewRadius);
            ;
            

            if (_isInView)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, player.position);   
            }
        }
    }
}
