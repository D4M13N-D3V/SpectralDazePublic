using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpectralDaze.Player
{
	public class PlayerPowerController : MonoBehaviour
	{
        //
        public PlayerPower_Dash DashPower;
		public PlayerPower Power1;
		public PlayerPower Power2;

		private PlayerController pc;

		public void AnimationTrigger(string param)
		{
			Power1.AnimationTrigger(param);
		}

		private void Start()
		{
			pc = GetComponent<PlayerController>();

		    DashPower.Init(pc);


            if (Power1 != null)
		    {
		        Power1.Init(pc);
		        Power1.Reset();
            }

		    if (Power2 != null)
		    {
		        Power2.Init(pc);
		        Power2.Reset();
            }
		}

		private void Update()
		{
		    DashPower.OnUpdate(pc);


            if (Power1 != null)
		    {
		        Power1.OnUpdate(pc);
		    }
		    if (Power2 != null)
		    {
		        Power2.OnUpdate(pc);
		    }
		}

		private void OnGUI()
		{
		    DashPower.OnGUI();

            if (Power1 != null)
			    Power1.OnGUI();

			if (Power2 != null)
			    Power2.OnGUI();
		}
	}
}