#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Graphic))]
public class GraphicEditor : Editor {

	//bool for foldout
	public bool showAxis=true;



	void OnEnable()
	{

	}


	//overrides default inspector
	public override void OnInspectorGUI()
	{
		Undo.RecordObject(target, "Do Something ");

		//gets the script
		Graphic graph = (Graphic)target;

		//adds spacing
		EditorGUILayout.Space ();
	

		EditorGUI.BeginChangeCheck ();

		//sets linenumber fiels
		int lineaux = EditorGUILayout.IntField (new GUIContent("Line Number","How many lines you want to display"), graph.lines);

		//cant change it if game is running
		if (Application.isPlaying == false) {
			
			graph.lines = lineaux;
		}
		if (EditorGUI.EndChangeCheck ()) {

			//adds or removes colors	

			while (graph.colors.Count < graph.lines)
			{
				graph.colors.Add (Color.blue);
			}

			while (graph.colors.Count > graph.lines)
			{
				graph.colors.RemoveAt (graph.colors.Count - 1);

			}				
		}





		//interval field
		float interval= EditorGUILayout.FloatField (new GUIContent("Time Interval","time interval that is displayed"), graph.interval);

		//cant change it if game is running
		if (Application.isPlaying == false)
			graph.interval = interval;


		//axis foldout
		showAxis = EditorGUILayout.Foldout(showAxis, "See Axis");
		if (showAxis)
		{
			GUILayout.BeginHorizontal();

			EditorGUI.BeginChangeCheck ();

			GUILayout.Label("X");
			graph.axisX=EditorGUILayout.Toggle(graph.axisX);


			if (EditorGUI.EndChangeCheck ()) {

				//adds or removes x axis
				if (graph.axisX) {
					
					graph.AddAxisX ();
					Undo.RegisterCreatedObjectUndo (graph.lrx.gameObject, "Created x Axis");

				}
				else if(graph.lrx!=null)
				//	DestroyImmediate (graph.lrx.gameObject);
				Undo.DestroyObjectImmediate (graph.lrx.gameObject);
			}

			EditorGUI.BeginChangeCheck ();

			GUILayout.Label("Y");
			graph.axisY=EditorGUILayout.Toggle(graph.axisY);


			if (EditorGUI.EndChangeCheck ()) {

				//add or removes y axis and ticks
				if (graph.axisY) {

					//Undo.RecordObject(target,"Ticks changed");

					graph.AddAxisY ();
					Undo.RegisterCreatedObjectUndo (graph.lry.gameObject, "Created y Axis");
					//Undo.RecordObjects (graph.ticksIngraph.ToArray(), "change ticks");

					EditorUtility.SetDirty( target );

				}
				else if (graph.lry != null) {

					//DestroyImmediate (graph.lry.gameObject);
					Undo.DestroyObjectImmediate (graph.lry.gameObject);

					//Undo.RecordObject (target,"list");
					graph.DeleteAxisTicks ();
					//EditorUtility.SetDirty( target );

				}
			}

			GUILayout.EndHorizontal();

		}


		EditorGUI.BeginChangeCheck ();
		string txtAux= EditorGUILayout.TextField (new GUIContent("Label X","Label on X axis"), graph.labelXtxt.text);
		if (EditorGUI.EndChangeCheck ()) {

			Undo.RecordObject( graph.labelXtxt, "Do Something ");
			graph.labelXtxt.text = txtAux;

			EditorUtility.SetDirty( graph.labelXtxt );
	
		}

		EditorGUI.BeginChangeCheck ();
		txtAux= EditorGUILayout.TextField (new GUIContent("Label Y","Label on Y axis"), graph.labelYtxt.text);
		if (EditorGUI.EndChangeCheck ()) {

			Undo.RecordObject( graph.labelYtxt, "Do Something ");
			graph.labelYtxt.text = txtAux;

			EditorUtility.SetDirty( graph.labelYtxt );

		}




		EditorGUI.BeginChangeCheck ();

		graph.labelsScale= EditorGUILayout.FloatField (new GUIContent("Label Size","Size of the Label"), graph.labelsScale);

		if (EditorGUI.EndChangeCheck ()) {

			Undo.RecordObject( graph.labelYtxt.transform, "Do Something ");
			Undo.RecordObject( graph.labelXtxt.transform, "Do Something ");


			//edits label scale on realtime
			graph.labelXtxt.transform.localScale = new Vector3 (graph.labelsScale,graph.labelsScale,graph.labelsScale); 
			graph.labelYtxt.transform.localScale = new Vector3 (graph.labelsScale,graph.labelsScale,graph.labelsScale); 

			EditorUtility.SetDirty( graph.labelYtxt.transform );
			EditorUtility.SetDirty( graph.labelXtxt.transform );
		}


		//autoRange toggle
		graph.autoRange= EditorGUILayout.Toggle(new GUIContent("Auto Range","if set to true graphic y bounds will update"),graph.autoRange);


		//yLimits stuff
		if (graph.autoRange == false) {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Y Bounds:");

			EditorGUI.BeginChangeCheck ();

			GUILayout.Label("Min");
			graph.yLimits.x=EditorGUILayout.FloatField(graph.yLimits.x);
			GUILayout.Label("Max");
			graph.yLimits.y=EditorGUILayout.FloatField(graph.yLimits.y);

			if (EditorGUI.EndChangeCheck ()) {

				//if max or min changes, change scales
				//redraw ticks
				//re add x axis

				graph.SetScales ();
				graph.RedrawAxisTicks (graph.tickMarksStep);


				if (graph.axisX && graph.lrx != null) {
					DestroyImmediate (graph.lrx.gameObject);
					graph.AddAxisX ();
				}
			}


			GUILayout.EndHorizontal();


			EditorGUI.BeginChangeCheck ();

			graph.tickMarksStep= EditorGUILayout.FloatField (new GUIContent("Tick Marks Step","Intervel between each tickmark"), graph.tickMarksStep);


			if (EditorGUI.EndChangeCheck ()) {

				//redraw tick on y axis
				if(graph.axisY)
				graph.RedrawAxisTicks (graph.tickMarksStep);
			}
		}





		EditorGUI.BeginChangeCheck ();
		graph.axiscolor=EditorGUILayout.ColorField ("Axis Color", graph.axiscolor);
		if (EditorGUI.EndChangeCheck ()) {
			
			//change colors of x y axis and its arrows (childs)


			if (graph.lrx != null) {

				Undo.RecordObject(graph.lrx.sharedMaterial,"Change color");
				Undo.RecordObject(graph.lrx.transform.GetChild (0).GetComponent<LineRenderer> ().sharedMaterial,"Change color");
				graph.lrx.sharedMaterial.color = graph.axiscolor;
				graph.lrx.transform.GetChild (0).GetComponent<LineRenderer> ().sharedMaterial.color = graph.axiscolor;

				EditorUtility.SetDirty( graph.lrx );
			}
			if (graph.lry != null) {

				Undo.RecordObject(graph.lry.sharedMaterial,"Change color");
				Undo.RecordObject(graph.lry.transform.GetChild (0).GetComponent<LineRenderer> ().sharedMaterial,"Change color");

				graph.lry.sharedMaterial.color = graph.axiscolor;
				graph.lry.transform.GetChild (0).GetComponent<LineRenderer> ().sharedMaterial.color = graph.axiscolor;

				EditorUtility.SetDirty( graph.lrx );

				graph.ColorTicks ();
			}

			//Undo.RecordObject (target, "axis color changed");

		}

		EditorGUI.BeginChangeCheck ();

		graph.textcolor=EditorGUILayout.ColorField ("Text Color", graph.textcolor);

		if (EditorGUI.EndChangeCheck ()) {

			//RecolorText
			graph.ColorText();

		}

		//What is this cluster of stuff?
		//shows colors for every line

		EditorGUI.BeginChangeCheck ();

		serializedObject.Update ();
		SerializedProperty colors = serializedObject.FindProperty ("colors");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(colors, true);
		if(EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();

		if (EditorGUI.EndChangeCheck ()) {

			while (graph.colors.Count < graph.lines) {

				//if line is added, its color is blue by default
				graph.colors.Add (Color.blue);

			}

			//removes colors if line was removed
			while (graph.colors.Count > graph.lines) {
				graph.colors.RemoveAt (graph.colors.Count - 1);

			}

		}



		EditorUtility.SetDirty( target );

		//Draws inspector like if there was no editor
		//DrawDefaultInspector ();

	}


}


#endif