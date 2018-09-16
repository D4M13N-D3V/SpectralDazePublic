 using System.Collections;
using System.Collections.Generic;
using System.Linq;
 using Assets.Code.AI;
 using SpectralDaze.Characters;
 using SpectralDaze.Managers;
 using SpectralDaze.Player;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.AI.QuestNPC
{
    public class QuestNpc : BaseAI
    {
        public QuestNPCOptions Options;
        private UStateMachine<QuestNpcParams> stateMachine;
        private QuestNpcParams paramsInstance;


        public TimeInfo TimeInfo;
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
                Animator = GetComponent<Animator>(),
                CachedTargetPos = Vector3.zero,
                OriginPosistion = transform.position,
                MovementType = Options.MovementType,
                WanderDistance = Options.WanderDistance,
                IdleTime = Options.IdleTime,
                TimeLeftIdle = Options.IdleTime,
                PatrolPoints = Options.PatrolPoints,
                CurrentPatrolPoint = Options.StartingPatorlPoint,
                Conversation = Options.Conversation,
                Player = FindObjectOfType<PlayerController>()
        };
            stateMachine = new UStateMachine<QuestNpcParams>(paramsInstance, new Conversing(), new Idle(), new Move());
            stateMachine.SetState(typeof(Idle), paramsInstance);
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed*TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
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
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        private class Conversing : UState<QuestNpcParams>
        {
            DialogueManager _dialogueManager;

            public override void Enter(QuestNpcParams p)
            {
                //_dialogueManager = GameManager.Instance.DialogueManager;
            }

            public override void Update(QuestNpcParams p)
            {
                p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                if (Input.GetButtonDown("Interact"))
                {
                    if(_dialogueManager.IsQueueEmpty && _dialogueManager.DialogueParentObj.activeSelf == false)
                    {
                        _dialogueManager.StartDialogue(p.Conversation);
                    }
                    else
                    {
                        //GameManager.Instance.DialogueManager.CycleDialogue();
                    }
                }
            }

            public override void CheckForTransitions(QuestNpcParams p)
            {
                if (Vector3.Distance(p.NpcTransform.position, p.Player.transform.position) >= 4)
                {
                    Parent.SetState(typeof(Move), p);
                }
            }


        }

        private class Idle : UState<QuestNpcParams>
        {
            private float _timeLeftIdle = 0;

            public override void Enter(QuestNpcParams p)
            {
                _timeLeftIdle = p.IdleTime;
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

            public override void CheckForTransitions(QuestNpcParams p)
            {
                if (Vector3.Distance(p.NpcTransform.position, p.Player.transform.position)<4)
                {
                    p.NavAgent.SetDestination(p.NpcTransform.position);
                    p.NavAgent.isStopped = true;
                    p.NpcTransform.LookAt(p.Player.transform.position);
                    Parent.SetState(typeof(Conversing), p);
                }
            }
        }

        private class Move : UState<QuestNpcParams>
        {
            public override void Enter(QuestNpcParams p)
            {
                p.NavAgent.isStopped = false;
                if (p.MovementType == MovementType.Wander || p.MovementType == MovementType.NoLimitsWander)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * p.WanderDistance;
                    Vector3 randomWanderPosistion = randomOffset += p.NpcTransform.position;


                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false && Vector3.Distance(randomWanderPosistion, p.OriginPosistion) > p.WanderDistance || NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.WanderDistance;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;
                    }
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

            public override void CheckForTransitions(QuestNpcParams p)
            {
                if (Vector3.Distance(p.NpcTransform.position, p.Player.transform.position) < 4)
                {
                    p.NavAgent.SetDestination(p.NpcTransform.position);
                    p.NavAgent.isStopped = true;
                    p.NpcTransform.LookAt(p.Player.transform.position);
                    Parent.SetState(typeof(Conversing), p);
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
            public Animator Animator;
            public Vector3 CachedTargetPos;
            public Vector3 OriginPosistion;
            public MovementType MovementType;
            public float WanderDistance;
            public float IdleTime;
            public float TimeLeftIdle;
            public int CurrentPatrolPoint;
            public List<Vector3> PatrolPoints;
            public Conversation Conversation;
            public PlayerController Player;
            public float MovementModifier;
            public float MovementSpeed;
        }
    }

}