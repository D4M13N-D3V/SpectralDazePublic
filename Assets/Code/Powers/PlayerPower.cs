using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gmtk.Player
{
	public class PlayerPower : ScriptableObject
	{
		public Texture2D Icon;
		public string Name;

		public virtual void Init(PlayerController pc) { }
		public virtual void OnUpdate(PlayerController pc) { }
		public virtual void OnGUI() { }
		public virtual void Reset() { }

		public virtual void AnimationTrigger(string param) { }
	}
}