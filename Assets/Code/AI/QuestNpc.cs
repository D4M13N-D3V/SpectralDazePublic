using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.ScriptableObjects.Time;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.AI.QuestNPC
{
    public class QuestNpc : MonoBehaviour
    {
        public QuestNPCOptions Options;
        private UStateMachine<QuestNpcParams> stateMachine;
        private QuestNpcParams paramsInstance;


        public Information TimeInfo;
        //private Animator _animator;
        private bool _timeBeingManipulated;
        private Manipulations _manipulationType;

        private void Start()
        {
            paramsInstance = new QuestNpcParams()
            {
                NpcTransform = transform,
                Npc = this,
                NavAgent = GetComponent<NavMeshAgent>(),
                CachedTargetPos = Vector3.zero,
                OriginPosistion = transform.position,
                MovementType = Options.MovementType,
                WanderDistance = Options.WanderDistance,
                IdleTime = Options.IdleTime,
                TimeLeftIdle = Options.IdleTime,
                PatrolPoints = Options.PatrolPoints,
                CurrentPatrolPoint = Options.StartingPatorlPoint
            };
            stateMachine = new UStateMachine<QuestNpcParams>(paramsInstance, new Idle(), new Move());
            stateMachine.SetState(typeof(Idle), paramsInstance);
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
        }

        private void Update()
        {
            stateMachine.Update(paramsInstance);
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate(paramsInstance);
        }


        /*
         * Time Bubble/Manipulation Code
         */
        public void StartTimeManipulation(int type)
        {
            _timeBeingManipulated = true;
            _manipulationType = (Manipulations)type;
            paramsInstance.NavAgent.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        private class Idle : UState<QuestNpcParams>
        {
            private float _timeLeftIdle = 0;

            public override void Enter(QuestNpcParams ps)
            {
                _timeLeftIdle = ps.IdleTime;
            }

            public override void FixedUpdate(QuestNpcParams p)
            {
                if (!p.NavAgent.pathPending && p.NavAgent.remainingDistance <= p.NavAgent.stoppingDistance &&
                    !p.NavAgent.hasPath || p.NavAgent.velocity.sqrMagnitude == 0f){
                    if (_timeLeftIdle > 0)
                    {
                        _timeLeftIdle = _timeLeftIdle - UnityEngine.Time.deltaTime;
                    }
                    else
                    {
                        Parent.SetState(typeof(Move), p);
                    }
                }
            }
        }

        private class Move : UState<QuestNpcParams>
        {
            public override void Enter(QuestNpcParams p)
            {
                if (p.MovementType == MovementType.Wander || p.MovementType == MovementType.NoLimitsWander)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * p.WanderDistance;
                    Vector3 randomWanderPosistion = randomOffset += p.NpcTransform.position;

                    if (p.MovementType == MovementType.Wander)
                        while (Vector3.Distance(randomWanderPosistion, p.OriginPosistion) > p.WanderDistance)
                        {
                            randomOffset = Random.insideUnitSphere * p.WanderDistance;
                            randomWanderPosistion = randomOffset += p.NpcTransform.position;
                        }

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomWanderPosistion, out hit, p.WanderDistance, NavMesh.AllAreas))
                    {
                        p.CachedTargetPos = hit.position;
                        if (p.NavAgent.SetDestination(randomWanderPosistion))
                        {
                            Parent.SetState(typeof(Idle), p);
                        }
                        else
                        {
                            Debug.LogError("Error occured when setting destination of navmesh agent.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when sampling posistion to get nearest navmesh point.");
                    }
                }
                else if (p.MovementType == MovementType.Patrol)
                {
                    p.CurrentPatrolPoint++;
                    if (p.CurrentPatrolPoint > p.PatrolPoints.Count-1)
                    {
                        p.CurrentPatrolPoint = 0;
                    }
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(p.PatrolPoints[p.CurrentPatrolPoint], out hit, p.WanderDistance, NavMesh.AllAreas))
                    {
                        p.CachedTargetPos = hit.position;
                        if (p.NavAgent.SetDestination(p.PatrolPoints[p.CurrentPatrolPoint]))
                        {
                            Parent.SetState(typeof(Idle), p);
                        }
                        else
                        {
                            Debug.LogError("Error occured when setting destination of navmesh agent.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when sampling posistion to get nearest navmesh point.");
                    }
                }
            }
        }

        public enum MovementType
        {
            Patrol,
            Wander,
            NoLimitsWander
        }

        private struct QuestNpcParams
        {
            public Transform NpcTransform;
            public QuestNpc Npc;
            public NavMeshAgent NavAgent;
            public Vector3 CachedTargetPos;
            public Vector3 OriginPosistion;
            public MovementType MovementType;
            public float WanderDistance;
            public float IdleTime;
            public float TimeLeftIdle;
            public int CurrentPatrolPoint;
            public List<Vector3> PatrolPoints;
        }
    }

    [CreateAssetMenu(menuName = "Spectral Daze/AI/QuestNPCSettings")]
    public class QuestNPCOptions : ScriptableObject
    {
        public QuestNpc.MovementType MovementType;
        public float WanderDistance;
        public float IdleTime;
        public int StartingPatorlPoint;
        public List<Vector3> PatrolPoints;
    }
}