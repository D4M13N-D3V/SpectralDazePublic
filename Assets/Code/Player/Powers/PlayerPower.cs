using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers.InputManager;
using UnityEngine;

namespace SpectralDaze.Player
{
    public enum PowerTypes { Dash, Power }

	public class PlayerPower : ScriptableObject
	{
		public Texture2D Icon;
		public string Name;

	    public PowerTypes Type;

        public Control Control;
		public virtual void Init(PlayerController pc) { }
		public virtual void OnUpdate(PlayerController pc) { }
		public virtual void OnGUI() { }
		public virtual void OnGizmos(PlayerController pc) { }
		public virtual void Reset() { }

		public virtual void AnimationTrigger(string param) { }
	}
}