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
    public class ShootingAI : MonoBehaviour
    {
        public Renderer Renderer;

        [HideInInspector]
        public float _localTimeScale = 1.0f;
        [HideInInspector]
        public float localTimeScale
        {
            get
            {
                return _localTimeScale;
            }
            set
            {
                float multiplier = value / _localTimeScale;
                paramsInstance.RigidBody.angularDrag *= multiplier;
                paramsInstance.RigidBody.drag *= multiplier;
                paramsInstance.RigidBody.mass /= multiplier;
                paramsInstance.RigidBody.velocity *= multiplier;
                paramsInstance.RigidBody.angularVelocity *= multiplier;

                _localTimeScale = value;
            }
        }
        [HideInInspector]
        public float localDeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime * UnityEngine.Time.timeScale * _localTimeScale;
            }
        }

        public ShootingAIOptions Options;
        private UStateMachine<ShootingAIParams> stateMachine;
        private ShootingAIParams paramsInstance;

        public Information TimeInfo;
        private bool _timeBeingManipulated;
        private Manipulations _manipulationType;

        private void Start()
        {
            paramsInstance = new ShootingAIParams()
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
                Player = FindObjectOfType<PlayerController>(),
                AggroDistance = Options.AggroDistance,
                RigidBody = GetComponent<Rigidbody>(),
                MovementSpeed = Options.MovementSpeed,
                TimeBetweenAttacks = Options.TimeBetweenAttacks,
                BulletPrefab = Options.BulletPrefab,
                AttackChargeAmount = Options.AttackChargeAmount,
                Renderer = Renderer
            };
            stateMachine = new UStateMachine<ShootingAIParams>(paramsInstance, new Attacking(), new Idle(), new Move());
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
        }

        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
        }

        private class Attacking : UState<ShootingAIParams>
        {
            private float _timeLeftUntilAttack;
            private bool _chargingShotInProgress;
            private Color _originalColor;
            private float t = 0;
            private GameObject _currentProjectile;

            public override void Enter(ShootingAIParams p)
            {
                _originalColor = p.Renderer.material.color;
                p.NavAgent.isStopped = true;
                _timeLeftUntilAttack = p.TimeBetweenAttacks;
            }

            public override void FixedUpdate(ShootingAIParams p)
            {
                p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);

                _timeLeftUntilAttack -= p.Npc.localDeltaTime;
                if (_timeLeftUntilAttack <= 0)
                {
                    if (!_chargingShotInProgress)
                    {
                        if (p.Renderer.material.color != _originalColor)
                        {
                            t += p.Npc.localDeltaTime;
                            p.Renderer.material.color = Color.Lerp(p.Renderer.material.color, _originalColor, t);
                        }
                        else
                        {
                            t = 0;
                            _chargingShotInProgress = true;
                        }
                    }
                    else
                    {
                        if (p.Renderer.material.color == Color.red)
                        {
                            t = 0;
                            CreateProjectile(p);
                            _timeLeftUntilAttack = p.TimeBetweenAttacks;
                            _chargingShotInProgress = false;
                        }
                        else
                        {
                            t += p.Npc.localDeltaTime;
                            p.Renderer.material.color = Color.Lerp(p.Renderer.material.color, Color.red, t);
                        }
                    }
                }
                else
                {
                    if (p.Renderer.material.color != _originalColor)
                    {
                        t += p.Npc.localDeltaTime;
                        p.Renderer.material.color = Color.Lerp(p.Renderer.material.color, _originalColor, t);
                    }
                }
            }

            public override void CheckForTransitions(ShootingAIParams p)
            {
            }

            private void CreateProjectile(ShootingAIParams p)
            {
                _currentProjectile = Instantiate(p.BulletPrefab, (p.NpcTransform.position + p.NpcTransform.forward),Quaternion.LookRotation(p.Player.transform.position-p.NpcTransform.position));
            }
        }

        private class Idle : UState<ShootingAIParams>
        {
            private float _timeLeftIdle = 0;

            public override void Enter(ShootingAIParams p)
            {
                _timeLeftIdle = p.IdleTime;
            }

            public override void FixedUpdate(ShootingAIParams p)
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

            public override void CheckForTransitions(ShootingAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        private class Move : UState<ShootingAIParams>
        {
            public override void Enter(ShootingAIParams p)
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

            public override void CheckForTransitions(ShootingAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
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

        private struct ShootingAIParams
        {
            public Transform NpcTransform;
            public ShootingAI Npc;
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
            public float MovementModifier;
            public Rigidbody RigidBody;
            public float MovementSpeed;
            public GameObject BulletPrefab;
            public float TimeBetweenAttacks;
            public float AttackChargeAmount;
            public Renderer Renderer;
        }
    }
}
