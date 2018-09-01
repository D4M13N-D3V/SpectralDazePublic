using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gmtk.Player
{
	public class PlayerPowerController : MonoBehaviour
	{
		public PlayerPower CurrentPower;

		private PlayerController pc;

		public void AnimationTrigger(string param)
		{
			CurrentPower.AnimationTrigger(param);
		}

		private void Start()
		{
			pc = GetComponent<PlayerController>();
			CurrentPower.Init(pc);
			CurrentPower.Reset();
		}

		private void Update()
		{
			if (CurrentPower != null)
				CurrentPower.OnUpdate(pc);
		}

		private void OnGUI()
		{
			if (CurrentPower != null)
				CurrentPower.OnGUI();
		}
	}
}