using System;
using UnityEngine;

namespace PimpochkaGames.CoreLibrary
{
	public class EnumMaskFieldAttribute : PropertyAttribute 
	{
		public Type EnumType 
		{ 
			get; 
			private set; 
		}

		private EnumMaskFieldAttribute() {}

		public EnumMaskFieldAttribute(Type type)
		{
			EnumType	= type;
		}
	}
}