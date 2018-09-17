using System;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Player;
using SpectralDaze.Managers;
using SpectralDaze.Managers.InputManager;
using UnityEngine;

namespace SpectralDaze.Managers.PowerManager
{
    public class PowerManager : MonoBehaviour
	{
	    public Power1 Power1;
	    public Power2 Power2;
        public DashPower DashPower;

	    private PlayerPower _power1Cache;
	    private PlayerPower _power2Cache;
	    private PlayerPower_Dash _dashCache;

	    public PowerInventory PowerInventory;
	    private PlayerController pc;

	    public Control Power1Control;
	    public Control Power2Control;
	    public Control DashControl;

	    private void Start()
	    {
	        pc = GetComponent<PlayerController>();
	        Power1 = Resources.Load<Power1>("Managers/PowerManager/Power1");
	        Power2 = Resources.Load<Power2>("Managers/PowerManager/Power2");
	        DashPower = Resources.Load<DashPower>("Managers/PowerManager/DashPower");

	        PowerInventory = Resources.Load<PowerInventory>("Managers/PowerManager/PowerInventory");
            Power1Control = Resources.Load<Control>("Managers/InputManager/Power1");
	        Power2Control = Resources.Load<Control>("Managers/InputManager/Power2");
	        DashControl = Resources.Load<Control>("Managers/InputManager/Dash");

            /*
             * Disable these during testing by commenting them out
             */
	        Power1.Power = null;
	        Power2.Power = null;
	        //DashPower.Power = null;

	        if (Power1 != null && Power1.Power != null)
	        {
	            Power1.Power.Init(pc);
	            Power1.Power.Control = Power1Control;
	        }

	        if (Power2 != null && Power2.Power !=null)
	        {
	            Power2.Power.Init(pc);
	            Power2.Power.Control = Power2Control;
            }

	        if (DashPower != null && DashPower.Power != null)
	        {
	            DashPower.Power.Init(pc);
	            DashPower.Power.Control = DashControl;
	        }
	    }

	    private void Update()
	    {
            /*
	        if (Power1.Power != _power1Cache)
	            Power1.Power.Init(pc);

	        if (Power2.Power != _power1Cache)
	            Power2.Power.Init(pc);

	        if (DashPower.Power != _power1Cache)
	            DashPower.Power.Init(pc);
            */

	        if (Power1.Power != null)
	        {
	            if (Power1.Power != _power1Cache)
	                Power1.Power.Init(pc);
                _power1Cache = Power1.Power;
	            Power1.Power.OnUpdate(pc);
            }

	        if (Power2.Power != null)
	        {
	            if (Power2.Power != _power2Cache)
	                Power2.Power.Init(pc);
                _power2Cache = Power2.Power;
	            Power2.Power.OnUpdate(pc);
            }

	        if (DashPower.Power != null)
	        {
	            if (DashPower.Power != _dashCache)
	                DashPower.Power.Init(pc);
                _dashCache = DashPower.Power;
	            DashPower.Power.OnUpdate(pc);
            }
        }

	    private void OnGUI()
	    {
	        if (Power1.Power != null)
                Power1.Power.OnGUI();
	        if (Power2.Power != null)
                Power2.Power.OnGUI();
	        if (DashPower.Power != null)
                DashPower.Power.OnGUI();
        }

        /*
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
        */
	}
}