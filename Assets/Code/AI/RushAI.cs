using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Managers;
using SpectralDaze.Player;
using SpectralDaze.ScriptableObjects.AI;
using SpectralDaze.ScriptableObjects.Time;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.AI
{
    public class RushAI : MonoBehaviour
    {
        public float _localTimeScale = 1.0f;
        public float localTimeScale
        {
            get
            {
                return _localTimeScale;
            }
            set
            {
                //            if (r == null) r = GetComponent<Rigidbody>();
                //            if (r != null)
                //            {
                float multiplier = value / _localTimeScale;
                paramsInstance.RigidBody.angularDrag *= multiplier;
                paramsInstance.RigidBody.drag *= multiplier;
                paramsInstance.RigidBody.mass /= multiplier;
                paramsInstance.RigidBody.velocity *= multiplier;
                paramsInstance.RigidBody.angularVelocity *= multiplier;
                //            }

                _localTimeScale = value;
            }
        }
        public float localDeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime * UnityEngine.Time.timeScale * _localTimeScale;
            }
        }

        public RushAIOptions Options;
        private UStateMachine<RushAIParams> stateMachine;
        private RushAIParams paramsInstance;

        public Information TimeInfo;
        private bool _timeBeingManipulated;
        private Manipulations _manipulationType;
        
        private void Start()    
        {
            paramsInstance = new RushAIParams()
            {
                NpcTransform = transform,
                Npc = this,
                NavAgent = GetComponent<NavMeshAgent>(),
                Animator = GetComponent<Animator>(),
                CachedTargetMoveToTargetPos = Vector3.zero,
                OriginPosistion = transform.position,
                MovementType = Options.MovementType,
                WanderDistance = Options.WanderDistance,
                IdleTime = Options.IdleTime,
                TimeLeftIdle = Options.IdleTime,
                PatrolPoints = Options.PatrolPoints,
                CurrentPatrolPoint = Options.StartingPatorlPoint,
                CachedTarget = null,
                Player = FindObjectOfType<PlayerController>(),
                AggroDistance = Options.AggroDistance,
                TimeBetweenCharges = Options.TimeBetweenCharges,
                RigidBody = GetComponent<Rigidbody>(),
                LaunchVelocity = Options.LaunchVelocity,
                MovementSpeed = Options.MovementSpeed
            };
            stateMachine = new UStateMachine<RushAIParams>(paramsInstance, new Attacking(), new Idle(), new Move());
            stateMachine.SetState(typeof(Idle), paramsInstance);
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
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
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed*TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        private class Attacking : UState<RushAIParams>
        {
            private float _remainderChargeCooldown;
            private bool _chargeInProgress = false;
            public override void Enter(RushAIParams p)
            {
                p.NavAgent.isStopped = true;
                _remainderChargeCooldown = p.TimeBetweenCharges;
            }

            public override void FixedUpdate(RushAIParams p)
            {
                _remainderChargeCooldown -= UnityEngine.Time.deltaTime;
                if (_remainderChargeCooldown <= 0 && !_chargeInProgress)
                {
                    p.NpcTransform.LookAt(p.Player.transform);
                    _chargeInProgress = true;
                    p.RigidBody.velocity= p.NpcTransform.forward * p.LaunchVelocity * p.MovementModifier;
                    _remainderChargeCooldown = p.TimeBetweenCharges;
                    _chargeInProgress = false;
                }
            }

            public override void CheckForTransitions(RushAIParams p)
            {
            }
        }

        private class Idle : UState<RushAIParams>
        {
            private float _timeLeftIdle = 0;

            public override void Enter(RushAIParams p)
            {
                _timeLeftIdle = p.IdleTime;
            }

            public override void FixedUpdate(RushAIParams p)
            {
                if (!p.NavAgent.pathPending && p.NavAgent.remainingDistance <= p.NavAgent.stoppingDistance &&
                    !p.NavAgent.hasPath || p.NavAgent.velocity.sqrMagnitude == 0f)
                {
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

            public override void CheckForTransitions(RushAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    p.CachedTarget = p.Player.gameObject;
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        private class Move : UState<RushAIParams>
        {
            public override void Enter(RushAIParams p)
            {
                p.NavAgent.isStopped = false;
                if (p.MovementType == MovementType.Wander || p.MovementType == MovementType.NoLimitsWander)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * p.WanderDistance;
                    Vector3 randomWanderPosistion = randomOffset += p.NpcTransform.position;


                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.WanderDistance;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;

                        if (p.MovementType == MovementType.Wander)
                            while (Vector3.Distance(randomWanderPosistion, p.OriginPosistion) > p.WanderDistance)
                            {
                                randomOffset = Random.insideUnitSphere * p.WanderDistance;
                                randomWanderPosistion = randomOffset += p.NpcTransform.position;
                            }
                        p.CachedTargetMoveToTargetPos = hit.position;
                        if (p.NavAgent.SetDestination(randomWanderPosistion))
                        {
                            Parent.SetState(typeof(Idle), p);
                        }
                        else
                        {
                            Debug.LogError("Error occured when setting destination of navmesh agent.");
                        }
                    }
                }
                else if (p.MovementType == MovementType.Patrol)
                {
                    p.CurrentPatrolPoint++;
                    if (p.CurrentPatrolPoint > p.PatrolPoints.Count - 1)
                    {
                        p.CurrentPatrolPoint = 0;
                    }
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(p.PatrolPoints[p.CurrentPatrolPoint], out hit, p.WanderDistance, NavMesh.AllAreas))
                    {
                        p.CachedTargetMoveToTargetPos = hit.position;
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

            public override void CheckForTransitions(RushAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    p.CachedTarget = p.Player.gameObject;
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        public enum MovementType
        {
            Patrol,
            Wander,
            NoLimitsWander
        }

        private struct RushAIParams
        {
            public Transform NpcTransform;
            public RushAI Npc;
            public NavMeshAgent NavAgent;
            public Animator Animator;
            public Vector3 CachedTargetMoveToTargetPos;
            public Vector3 OriginPosistion;
            public MovementType MovementType;
            public float WanderDistance;
            public float IdleTime;
            public float TimeLeftIdle;
            public int CurrentPatrolPoint;
            public List<Vector3> PatrolPoints;
            public GameObject CachedTarget;
            public PlayerController Player;
            public float AggroDistance;
            public float TimeBetweenCharges;
            public float MovementModifier;
            public Rigidbody RigidBody;
            public float LaunchVelocity;
            public float MovementSpeed;
        }
    }
}
