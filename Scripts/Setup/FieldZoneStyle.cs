﻿using UnityEngine;

namespace My.Football.Fields.Setup
{
   [CreateAssetMenu( fileName = "Zone Style - ", menuName = "Field/Zone Style" )]
	public class FieldZoneStyle: ScriptableObject
	{
		[SerializeField] Material zoneLinesMaterial;
		[SerializeField] [Range( 0.1f, 0.5f )] float zoneBoarderWidth = .3f;
		[SerializeField] [Range( 0.0f, 0.5f )] float zoneDividerWidth = .2f;
		[SerializeField] Color zoneBoarderColor = new Color( 0, 0, 0, .6f );
		[SerializeField] Color zoneDividerColor = new Color( 0, 0, 0, .4f );

		public Material LinesMaterial => zoneLinesMaterial;
		public float BoarderWidth => zoneBoarderWidth;
		public float HalfBoarderWidth => BoarderWidth / 2f;

		public float DividerWidth => zoneDividerWidth;
		public Color BoarderColor => zoneBoarderColor;
		public Color DividerColor => zoneDividerColor;
	}
}