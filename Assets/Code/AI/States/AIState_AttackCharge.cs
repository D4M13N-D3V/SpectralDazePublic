using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpectralDaze.AI
{
	public class AIState_AttackCharge : UState<AIStateParams>
	{
		private const float TIME_TO_IDLE = 4f;
		private float idleTimer = 0f;
		private bool hasShot = false;
		private bool hasSetAnimTrigger = false;

		private const float INITIAL_DELAY = 0.5f;
		private const float QUART_INITIAL_DELAY = INITIAL_DELAY / 4f;
		private float iDelayTimer = 0f;

		private Color cachedColor;

		private Vector3 velocityReference;

		public override void Enter(AIStateParams ps)
		{
			// Start the charge up animation
			//Debug.Log(ps.Animator);

			GameObject go = ps.Transform.gameObject;

			LeanTween.scale(go, Vector3.one * 0.75f, 0.75f).setOnComplete(() =>
			{
				LeanTween.scale(go, Vector3.one, 0.5f).setOnComplete(() =>
				{
					GameObject bullet = UnityEngine.Object.Instantiate(ps.Controller.BulletPrefab, ps.Transform.position + (Vector3.up * 1.66f), Quaternion.identity);
					bullet.transform.rotation = Quaternion.Euler(0,ps.Transform.rotation.eulerAngles.y, 0);
					LeanTween.delayedCall(TIME_TO_IDLE, () =>
					{
						Parent.SetState(typeof(AIState_Idle), ps);
						return;
					});
				});
			});

			LeanTween.value(go,
				(f) =>
				{
					ps.Transform.rotation = Quaternion.LookRotation((ps.Controller.CachedTarget.position - ps.Transform.position).normalized, Vector3.up);
				},
				0f, 1f, 0.75f + 0.5f);

			cachedColor = ps.Renderer.material.color;
			ps.Renderer.material.color = Color.red;
		}

		public override void Exit(AIStateParams ps)
		{
			hasShot = false;
			hasSetAnimTrigger = false;
			idleTimer = 0f;
			iDelayTimer = 0f;
			ps.Renderer.material.color = cachedColor;
		}
	}
}