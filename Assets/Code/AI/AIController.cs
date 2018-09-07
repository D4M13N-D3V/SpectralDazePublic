using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.ScriptableObjects.Time;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.AI
{
	public class AIController : MonoBehaviour
	{
		public Transform CachedTarget;
		public Vector3? LastKnownPosition;

		public GameObject BulletPrefab;


		private UStateMachine<AIStateParams> stateMachine;

		private AIStateParams paramsInstance;

	    public Information TimeInfo;
        private Animator _animator;
        private bool _timeBeingManipulated;
	    private Manipulations _manipulationType;

		private void Start()
		{
		    _animator = GetComponent<Animator>();
            paramsInstance = new AIStateParams()
			{
				Controller = this,
				NavMeshAgent = GetComponent<NavMeshAgent>(),
				Animator = GetComponent<Animator>(),
				Renderer = GetComponentInChildren<Renderer>(),
				Transform = transform
			};
			stateMachine = new UStateMachine<AIStateParams>(paramsInstance,
				new AIState_Idle(),
				new AIState_Chase(),
				new AIState_AttackCharge(),
				new AIState_AttackShoot()
				);
			stateMachine.SetState(typeof(AIState_Idle), paramsInstance);
		}

		private void Update()
		{
			stateMachine.Update(paramsInstance);
		}

		private void OnGUI()
		{
			stateMachine.OnGUI(paramsInstance);
			GUI.Label(new Rect(0, 0, 100, 50), stateMachine.ActiveState.GetType()+"");
		}

		private void OnDrawGizmos()
		{
			if(stateMachine != null)
			stateMachine.OnDrawGizmos(paramsInstance);
		}

        /*
         * Time Bubble/Manipulation Code
         */
	    public void StartTimeManipulation(int type)
	    {
	        _timeBeingManipulated = true;
	        _manipulationType = (Manipulations) type;
	        _animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
	    }

	    public void StopTimeManipulation()
	    {
	        _timeBeingManipulated = true;
	        _manipulationType = Manipulations.Normal;
            _animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }
	}
}