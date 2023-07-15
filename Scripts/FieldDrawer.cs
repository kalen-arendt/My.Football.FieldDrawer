using System;
using UnityEngine;

namespace Football.Core.FieldDrawing
{
	/// <summary>
	/// Draw a Field based on a FieldConfig and FieldStyle
	/// </summary>
	[Serializable]
	public class FieldDrawer
	{
		[SerializeField] FieldConfig config;
		[SerializeField] FieldComponents components;
		[SerializeField] FieldStyle fieldStyle;
		[SerializeField] FieldZoneStyle zoneStyle;

		[SerializeField] Transform transform;

		private const string FIELD_SORTING_LAYER = "Field";
		private const int FIELD_LAYER = 8;

		private const int
			GRASS_ORDER = 0,
			LINES_ORDER = 10;

		private int
			width,
			length;

		public FieldDrawer( FieldConfig config, FieldComponents components,
			FieldStyle fieldStyle, FieldZoneStyle zoneStyle, Transform transform ) {
			this.config = config;			//!= null ? config		: throw new ArgumentNullException( nameof( config ) );
			this.components = components; //!= null ? components	: throw new ArgumentNullException( nameof( components ) );
			this.fieldStyle = fieldStyle; //!= null ? fieldStyle	: throw new ArgumentNullException( nameof( fieldStyle ) );
			this.zoneStyle = zoneStyle;	//!= null ? zoneStyle	: throw new ArgumentNullException( nameof( zoneStyle ) );
			this.transform = transform;	//!= null ? transform	: throw new ArgumentNullException( nameof( transform ) );

			width = config.Width;
			length = config.Length;
		}

		public void Draw() {
			if ( config && fieldStyle && components && transform ) {

				Erase();

				width = config.Width;
				length = config.Length;

				transform.position = Vector2.zero;


				DrawGrass();
				DrawFieldMarkings();
				DrawZoneBoundryLines();

				transform.gameObject.layer = FIELD_LAYER;

				foreach ( Transform item in transform.GetComponentsInChildren<Transform>() ) {
					item.gameObject.layer = FIELD_LAYER;
				}
			}
			else {
				Debug.LogWarning( "Field Drawer is missing components to draw field." );
			}
		}

		public void Erase() {
			while ( transform.childCount > 0 ) {
				GameObject.DestroyImmediate( transform.GetChild( 0 ).gameObject );
			}
		}


		#region Draw Grass
		
		private void DrawGrass() {
			Transform parent = CreateHeaderGameObject( "Zones", transform );

			DrawXZones( CreateHeaderGameObject( "X-Zones", parent ) );
			DrawYZones( CreateHeaderGameObject( "Y-Zones", parent ) );
		}

		/// <summary>
		/// draw the zones determined by the x position
		/// </summary>
		/// <param name="parent">the parent Transform of the instantiated zones</param>
		private void DrawXZones( Transform parent ) {
			int[] zoneWidths = config.HorizontalZoneWidths;
			float x = -(width / 2);

			for ( int i = 0; i < zoneWidths.Length; i++ ) {
				SetupSpriteRenderer(
					name: "XZone " + (i + 1),
					position: new Vector2( x + (zoneWidths[i] / 2f), 0 ),
					parent: parent,
					sprite: components.Grass,
					color: fieldStyle.WZoneColors[i % fieldStyle.WZoneColors.Length],
					size: new Vector2( zoneWidths[i], length ),
					layerName: FIELD_SORTING_LAYER,
					sortOrder: GRASS_ORDER + 2,
					drawMode: SpriteDrawMode.Sliced
				);

				x += zoneWidths[i];
			}
		}

		/// <summary>
		/// draw the zones determined by the y position
		/// </summary>
		/// <param name="parent">the parent Transform of the instantiated zones</param>
		private void DrawYZones( Transform parent ) {
			int[] zoneDepth = config.VerticalZoneDepths;
			float y = -(length / 2);

			for ( int i = 0; i < zoneDepth.Length; i++ ) {
				SetupSpriteRenderer(
					name:"YZone " + (i + 1),
					position:new Vector2( 0, y + (zoneDepth[i] / 2f) ),
					parent:parent,
					sprite: components.Grass,
					color:fieldStyle.DZoneColors[i % fieldStyle.DZoneColors.Length],
					size:new Vector2( width, zoneDepth[i] ),
					layerName:FIELD_SORTING_LAYER,
					sortOrder:GRASS_ORDER + 1,
					drawMode: SpriteDrawMode.Sliced
				);

				y += zoneDepth[i];
			}
		}

		#endregion


		#region Draw Field Markings & Goals

		private void DrawFieldMarkings() {
			Transform linesParent = CreateHeaderGameObject( "Lines", transform );

			DrawOutline( linesParent );
			DrawHalfLine( linesParent );
			DrawCenter( linesParent );
			DrawCornerArcs( linesParent );
			DrawPenaltyAreas( linesParent );

			foreach ( var sprite in linesParent.GetComponentsInChildren<SpriteRenderer>() ) {
				sprite.sortingLayerName = FIELD_SORTING_LAYER;
				sprite.sortingOrder = LINES_ORDER;

				sprite.color = fieldStyle.LineColor;
			}

			foreach ( var line in linesParent.GetComponentsInChildren<LineRenderer>() ) {
				line.sortingLayerName = FIELD_SORTING_LAYER;
				line.sortingOrder = LINES_ORDER;

				line.startColor = line.endColor = fieldStyle.LineColor;
				line.startWidth = line.endWidth = config.LineWidth;
			}

			DrawGoals();
		}

		private void DrawOutline( Transform parent ) {
			float
				halfLineWidth = config.LineWidth / 2,
				x = (width / 2) - halfLineWidth,
				y = (length / 2) - halfLineWidth;

			LineRenderer outline = GameObject.Instantiate( components.LineRenderer, parent );
			outline.name = "Outline";

			outline.loop = true;
			outline.positionCount = 4; //5 ;
			outline.SetPositions(
				new Vector3[4] {
					new Vector2(-x, -y),
					new Vector2(+x, -y),
					new Vector2(+x, +y),
					new Vector2(-x, +y)//,
											 //new Vector2(-x, -y)
				}
			);
		} // end DrawOutline()

		private void DrawHalfLine( Transform parent ) {
			LineRenderer halfLine = GameObject.Instantiate( components.LineRenderer, parent );
			halfLine.name = "Half Line";

			halfLine.positionCount = 2;
			halfLine.SetPositions(
				new Vector3[2] {
					new Vector2(-(width/2), 0),
					new Vector2(+(width/2), 0)
				}
			);
		}

		private void DrawCornerArcs( Transform parent ) {
			// Corner Arcs
			//	=> (location):	(scale) -> (position)
			//		(0,0):		(+,+)	->	(-,-)
			//		(0,1):		(+,-)	->	(-,+)
			//		(1,0):		(-,+)	->	(+,-)
			//		(1,1):		(-,-)	->	(+,+)
			//	
			//	0 -> +scale, -position
			//	1 -> -scale, +position

			//	Scale: i*2 - 1
			//	 >	( 0 -> +1 )	::	-((0 * 2) - 1) = +1
			//	 >	( 1 -> -1 )	::	-((1 * 2) - 1) = -1

			//	Position: 
			//	 >	(0 -> 0 + 0)
			//	 >	(1 -> 0 + 1)
			int
				initialX = 0 - (width / 2),
				initialY = 0 - (length / 2);

			Transform arcParent = CreateHeaderGameObject( "Corner Arcs", parent );

			for ( int xi = 0; xi < 2; xi++ ) {
				for ( int yj = 0; yj < 2; yj++ ) {
					// 1. Instantiate the GameObject
					Transform arc = GameObject.Instantiate( components.CornerArc, arcParent ).transform;

					// 2. Position the GameObject
					arc.position = new Vector2(
						initialX + (width * xi),
						initialY + (length * yj)
					);

					// 3. Scale (flip) the image
					arc.localScale = new Vector2(
						-((xi * 2) - 1),
						-((yj * 2) - 1)
					);
				}
			}
		} // end DrawCornerArcs()

		private void DrawCenter( Transform parent ) {
			Transform t = GameObject.Instantiate( components.CenterCircle, parent ).transform;
			t.position = Vector2.zero;
		}

		private void DrawPenaltyAreas( Transform parent ) {
			// DrawPenaltyAreas
			//				scale		position
			// Defense	1,+1		0,-length/2 = -length/2 + 0*length
			// Attack	1,-1		0,+length/2 = -length/2 + 1*length

			int bottomCenterY = -(length / 2);

			for ( int i = 0; i < 2; i++ ) {
				Transform area = GameObject.Instantiate( components.PenaltyArea, parent ).transform;

				// 2. Position the GameObject
				area.position = new Vector2(
					0,
					bottomCenterY + (length * i)
				);

				// 3. Scale (flip) the image
				area.localScale = new Vector2(
					1,
					1 - (i * 2)
				);
			}
		} // end DrawPenaltyAreas()

		private void DrawGoals() {
			for ( int i = 0; i < 2; i++ ) {
				// Instantiate, Scale and Position the Prefab
				Transform area = GameObject.Instantiate( components.Goal, transform ).transform;
				area.position = new Vector2( 0, -(length / 2) + (length * i) );
				area.localScale = new Vector2( 1, -((i * 2) - 1) );

				// LineRenderer: goal POSTS & CROSSBAR
				var lr = area.GetComponent<LineRenderer>();
				lr.sortingLayerName = FIELD_SORTING_LAYER;
				lr.sortingOrder = LINES_ORDER;

				lr.startColor = lr.endColor = fieldStyle.LineColor;

				// SpriteRenderer: goal NETTING
				var sr = area.GetChild( 0 ).GetComponent<SpriteRenderer>();
				sr.sortingLayerName = FIELD_SORTING_LAYER;
				sr.sortingOrder = LINES_ORDER - 1;
				sr.color = fieldStyle.NetColor;
			}
		}

		#endregion


		#region Draw Zone Boundry Lines

		private void DrawZoneBoundryLines() {
			if ( !zoneStyle )
				return;

			Transform header = CreateHeaderGameObject( "Zone Boundries", transform );
			Transform xBoundries = CreateHeaderGameObject( "X Boundries", header );
			Transform yBoundries = CreateHeaderGameObject( "Y Boundries", header );




			if (zoneStyle.ZoneBoarderWidth > 0) {
				DrawXZoneBoarders( xBoundries );
				DrawYZoneBoarders( yBoundries );
			}

			if (zoneStyle.ZoneDividerWidth > 0) {
				DrawXZoneDividers( xBoundries );
				DrawYZoneDividers( yBoundries );
			}			
		}

		private void DrawXZoneBoarders( Transform parent ) {
			int[] zoneWidths = config.HorizontalZoneWidths;
			float
				x = -(width / 2),
				yBoarder = (length / 2),
				halfWidth = zoneStyle.ZoneBoarderWidth / 2;

			for ( int i = 0; i < zoneWidths.Length - 1; i++ ) {
				// POSITION
				x += zoneWidths[i];
				float xPos = InLinePos( x, halfWidth, true );

				SetupLineRenderer(
					parent,
					"Line - XBoarder - " + (i + 1),
					zoneStyle.ZoneLinesMaterial,
					zoneStyle.ZoneBoarderWidth,
					zoneStyle.ZoneBoarderColor,
					new Vector3[] {
						new Vector2( xPos, -yBoarder ),
						new Vector2( xPos, +yBoarder)
					}
				);
			}
		}

		private void DrawYZoneBoarders( Transform parent ) {
			int[] zoneWidths = config.VerticalZoneDepths;
			float
				y = -(length / 2f),
				xBoarder = (width / 2f),
				halfWidth = zoneStyle.ZoneBoarderWidth / 2f;

			for ( int i = 0; i < zoneWidths.Length - 1; i++ ) {
				// POSITION
				y += zoneWidths[i];
				float yPos = InLinePos( y, halfWidth, false );

				SetupLineRenderer(
					parent,
					"Line - XBoarder - " + (i + 1),
					zoneStyle.ZoneLinesMaterial,
					zoneStyle.ZoneBoarderWidth,
					zoneStyle.ZoneBoarderColor,
					new Vector3[] {
						new Vector2( -xBoarder, yPos ),
						new Vector2( +xBoarder, yPos )
					}
				);
			}
		}


		private void DrawXZoneDividers( Transform parent ) {
			int[] zoneWidths = config.HorizontalZoneWidths;
			float
				x = -(width / 2f),
				yBoarder = (length / 2f),
				halfWidth = zoneStyle.ZoneDividerWidth / 2f;

			for ( int i = 0; i < zoneWidths.Length; i++ ) {
				// POSITION
				float halfZoneWidth = zoneWidths[i] / 2f;
				float xPos = InLinePos( x + halfZoneWidth, halfWidth, true );

				SetupLineRenderer(
					parent,
					"Line - XDivider - " + (i + 1),
					zoneStyle.ZoneLinesMaterial,
					zoneStyle.ZoneDividerWidth,
					zoneStyle.ZoneDividerColor,
					new Vector3[] {
						new Vector2( xPos, -yBoarder ),
						new Vector2( xPos, +yBoarder)
					}
				);

				x += zoneWidths[i];
			}
		}

		private void DrawYZoneDividers( Transform parent ) {
			int[] zoneDepths = config.VerticalZoneDepths;
			float
				y = -(length / 2f),
				xBoarder = (width / 2f),
				halfWidth = zoneStyle.ZoneBoarderWidth / 2f;

			for ( int i = 0; i < zoneDepths.Length; i++ ) {
				// POSITION
				float halfZoneDepth = zoneDepths[i] / 2f;
				float yPos = InLinePos( y + halfZoneDepth, halfWidth, false );

				SetupLineRenderer(
					parent,
					"Line - YDivider - " + (i + 1),
					zoneStyle.ZoneLinesMaterial,
					zoneStyle.ZoneDividerWidth,
					zoneStyle.ZoneDividerColor,
					new Vector3[] {
						new Vector2( -xBoarder, yPos ),
						new Vector2( +xBoarder, yPos )
					}
				);
				y += zoneDepths[i];
			}
		}

		#endregion

		// ----------------------------------------------
		// Helper Functions
		// ----------------------------------------------

		private Transform CreateHeaderGameObject( string name, Transform parent ) {
			var t = new GameObject( name ).transform;
			t.SetParent( parent );

			return t;
		}

		private LineRenderer SetupLineRenderer
		( Transform parent, string name, Material mat, float width, Color color, Vector3[] points ) {
			var obj = new GameObject( name );
			obj.transform.SetParent( parent );

			LineRenderer line = obj.AddComponent<LineRenderer>();

			line.receiveShadows = false;
			line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			line.allowOcclusionWhenDynamic = false;
			line.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
			line.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

			// COLOR, SIZE
			line.material = mat;
			line.textureMode = LineTextureMode.Tile;

			line.startColor = line.endColor = color;
			line.startWidth = line.endWidth = width;

			// SORTING ORDER
			line.sortingLayerName = FIELD_SORTING_LAYER;
			line.sortingOrder = LINES_ORDER - 1;

			line.positionCount = points.Length;
			line.SetPositions( points );

			return line;
		}


		private SpriteRenderer SetupSpriteRenderer
			( string name, Vector2 position, Transform parent,
			Sprite sprite, Color color, Vector2 size,
			string layerName, int sortOrder, SpriteDrawMode drawMode = SpriteDrawMode.Sliced ) {

			GameObject obj = new GameObject( name );
			obj.transform.SetParent( parent );
			obj.transform.position = position;

			SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
			sr.sprite = sprite;
			sr.color = color;

			sr.drawMode = drawMode;
			sr.size = size;

			sr.sortingLayerName = layerName;
			sr.sortingOrder = sortOrder;

			return sr;
		}

		/// <summary>
		/// Align the position of a line renderer point so the edge of the line is flush with z
		/// </summary>
		/// <param name="z">the variable to align</param>
		/// <param name="halfWidth">half the width of the line</param>
		/// <param name="towardZero">inline towards zero? or away from zero?</param>
		/// <returns></returns>
		private float InLinePos( float z, float halfWidth, bool towardZero = true )
			=> (z == 0) ? 0 : ((z < 0) == towardZero) ? z + halfWidth : z - halfWidth;
	}
}