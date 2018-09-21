using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace SpectralDaze.Player
{
	[CreateAssetMenu(fileName = "Dimension_Peek", menuName = "Spectral Daze/PlayerPower/Dimension Peek")]
	public class PlayerPower_DimensionPeek : PlayerPower
	{
		[TabGroup("Test")]
		public AnimationCurve GrowthCurve;
		public AnimationCurve DecayCurve;

		public float MaxSphereScale = 6f;

		private float sphereScale = 0f;
		private Vector3 spherePos;
		private float t = 0f;

		private Material drawMaterial;
		private Mesh drawMesh;

		public override void OnUpdate(PlayerController pc)
		{
			Debug.Log(sphereScale + " " + t);
			if (Input.GetMouseButton(0))
			{
				t += UnityEngine.Time.deltaTime;
				sphereScale = GrowthCurve.Evaluate(t) * MaxSphereScale;
			}
			else
			{
				t -= UnityEngine.Time.deltaTime;
				sphereScale = DecayCurve.Evaluate(t) * MaxSphereScale;
			}

			t = math.clamp(t, 0, 1);

			if (Vector3.Distance(spherePos, pc.transform.position) > 4f)
				spherePos = pc.transform.position;

			spherePos = Vector3.Lerp(spherePos, pc.transform.position, UnityEngine.Time.deltaTime * 4f);

			Matrix4x4 meshMtx = Matrix4x4.TRS(spherePos, Quaternion.identity, Vector3.one * sphereScale);
			Graphics.DrawMesh(drawMesh, meshMtx, drawMaterial, 0);
			//Graphics.DrawMeshNow(drawMesh, meshMtx);
		}

		public override void OnGizmos(PlayerController pc)
		{
			Gizmos.DrawWireSphere(pc.transform.position, sphereScale / 2f);
		}

		public override void Reset()
		{
			t = 0f;
			sphereScale = 0f;
		}

		public override void Init(PlayerController pc)
		{
			drawMaterial = Resources.Load("fx/PeekQuad") as Material;

			// Make a Sphere primitive and grab it's mesh
			GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			drawMesh = newSphere.GetComponent<MeshFilter>().sharedMesh;
			Destroy(newSphere);
		}
	}
}