using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace gmtk.AI
{
	public struct AIStateParams
	{
		public Transform Transform;
		public AIController Controller;
		public NavMeshAgent NavMeshAgent;
		public Animator Animator;
		public Renderer Renderer;

		public bool CanSeePlayer()
		{
			Vector3 dir = (Controller.CachedTarget.position - Transform.position).normalized;
			Ray ray = new Ray(Transform.position + Vector3.up, dir);
			RaycastHit hit;

			if (!Physics.Raycast(ray, out hit)) return false;

			return hit.transform == Controller.CachedTarget || hit.transform.IsChildOf(Controller.CachedTarget);
		}
	}
}