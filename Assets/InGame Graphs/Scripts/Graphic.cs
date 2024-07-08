 /// <summary>
/// Graphic - Ideal for monitoring a parameter in real time
/// Code by João Ramiro
/// </summary>
/// v1.1 - fixed bug where axis and graphlines wouldn't display correctyl if rotation was different from Quaternion.identity
/// v2.0 - added autorange and editor script
/// 
using System.Collections;
using System.Collections.Generic;

 //necessary to use lists
using UnityEngine;
using UnityEngine.UI;



public class Graphic : MonoBehaviour
{

	//graphic adapts to points
	public bool autoRange;

	//how many lines do you want to see
	[Tooltip ("How many lines you want to display")]
	public int lines;

	//minimum and maximum values
	public Vector2 yLimits;

	//relation betwwen actual size and graphic coordinates
	public Vector2 scale = Vector2.one;

	//how much time
	public float interval = 5;

	//lines
	public LineRenderer[] lrs;

	//axisX line renderer
	public LineRenderer lrx;

	//axisY line renderer
	public LineRenderer lry;

	//step between each tick mark on the y axis
	public float tickMarksStep = 1;

	//display x axis?
	public bool axisX;

	//display y axis?
	public bool axisY;

	//scale of the labels
	public float labelsScale = 0.04f;

	//axis element
	public GameObject axisFab;

	//line prefab
	public GameObject lineFab;

	//axis ticks prefab
	public GameObject axisMarkFab;

	//axis labels prefab
	public GameObject labelFab;

	//axis x label
	public Text labelXtxt;

	//axis y label
	public Text labelYtxt;

	//current time since graph started rolling
	float[] yCurOffset;

	//time when the graph started
	float[] tstart;

	//color of the axis
	public Color axiscolor = Color.black;

	//text elements' color
	public Color textcolor = Color.black;

	//all ticks currently on graphic
	[SerializeField] 
	public List<GameObject> ticksIngraph = new List<GameObject> ();

	//color for each graphic
	public List<Color> colors;

	//used in autorange
	float prevStep;

	void Start ()
	{	
		//initializes graph
		IniGraph ();
	}

	//function that runs one time as the scene starts
	void IniGraph ()
	{

		//sets conversion between local scale and graphic scale
		SetScales ();

		//warning if there are not enough colors
		if (lines > colors.Count)
			Debug.LogWarning ("There are more lines than colors, some line will be black");

		//assigns line renderers
		lrs = new LineRenderer[lines];


		//creates linefab for everyline
		for (int i = 0; i < lines; i++) {
			
			//create line renderer 
			GameObject go = (GameObject)Instantiate (lineFab, transform.position, transform.rotation);
			go.transform.SetParent (transform);

			//moves lines to origin of graph
			go.transform.localPosition = Vector3.zero;

			//centers line renderer on zero of the graphic scale (this equation does it with some dark magic)
			go.transform.position += Vector3.down * scale.y * (yLimits.x + yLimits.y) / 2; 

			//scales lines' scale to the graph
			go.transform.localScale = new Vector3 (go.transform.localScale.x / go.transform.lossyScale.x, go.transform.localScale.y / go.transform.lossyScale.y); 

			//sets the material
			lrs [i] = go.GetComponent<LineRenderer> ();
			lrs [i].material = new Material (Shader.Find ("Sprites/Default"));

			//sets colors, if no colors is defined then sets it to black
			if (i < colors.Count) {
				
				lrs [i].material.color = colors [i];
			} else
				lrs [i].material.color = Color.black;
		}

		//moves the lines offsets with time, so that for instance t=5s now means t=4.7s for example
		yCurOffset = new float[lines];

		//sets start time to 'unassigned'
		tstart = new float[lines];
		for (int i = 0; i < tstart.Length; i++) {
			tstart [i] = -1;
		}
	}



	public void AddPointXY (float x, float y, int line = 0)
	{
		//auxiliar offset, for first point to zero
		float offset;

		//sets time this function was called
		float time = Time.time;

		if (line > lines - 1) {
			Debug.LogWarning ("You are trying to add points to a line that doensn't exist");
			return;
		}

		//assigns starting time if it was unassigned
		if (tstart [line] == -1) {
			tstart [line] = time;
		}

		//time is the time since the start of the graphic
		time = time - tstart [line];

		//limit y values in normalmode
		if (autoRange == false)
			y = Mathf.Clamp (y, yLimits.x, yLimits.y);

		
		//adds one point to the line renderer
		lrs [line].positionCount++;

		//sets new point
		lrs [line].SetPosition (lrs [line].positionCount - 1, new Vector2 (x * scale.x, scale.y * (y)));

		//redefines limits and scales and adapts points to new range
		if (autoRange) {		
			AutoRange ();
		}

	}


	/// <summary>
	/// Adds point to a specific line
	/// </summary>
	/// <param name="y">The y coordinate.</param>
	/// <param name="line">Line.</param>
	public void AddPoint (float y, int line = 0)
	{
		//auxiliar offset, for first point to zero
		float offset;

		//sets time this function was called
		float time = Time.time;

		if (line > lines - 1) {
			Debug.LogWarning ("You are trying to add points to a line that doensn't exist");
			return;
		}

		//assigns starting time if it was unassigned
		if (tstart [line] == -1) {
			tstart [line] = time;
		}

		//time is the time since the start of the graphic
		time = time - tstart [line];

		//limit y values in normalmode
		if (autoRange == false)
			y = Mathf.Clamp (y, yLimits.x, yLimits.y);

		//if theres passed more time than the one shown in the graphic, start moving the graphic
		if (time > interval) {

			//shifts points to the left
			offset = ShiftLeft (line);

			//offsets in time according to amount shifted
			yCurOffset [line] += offset;
				
			//sets new point
			lrs [line].SetPosition (lrs [line].positionCount - 1, new Vector2 (time * scale.x - yCurOffset [line], scale.y * (y)));

		} else {

			//adds one point to the line renderer
			lrs [line].positionCount++;

			//sets new point
			lrs [line].SetPosition (lrs [line].positionCount - 1, new Vector2 (time * scale.x, scale.y * (y)));
		}

		//redefines limits and scales and adapts points to new range
		if (autoRange) {		
			AutoRange ();
		}

	}



	
	/// <summary>
	/// Shifts to the left all points
	/// </summary>
	/// <returns>How much was the offset to the left</returns>
	/// <param name="line">Line.</param>
	float ShiftLeft (int line = 0)
	{
		//saves initial offset
		float offset = lrs [line].GetPosition (0).x;

		//shifts position of points in the linerenderer by the offset amount
		for (int i = 0; i < lrs [line].positionCount - 1; i++) {
			lrs [line].SetPosition (i, lrs [line].GetPosition (i + 1) - new Vector3 (offset, 0));
		}

		return offset;
	}

	//sets scale or ratio between local scale and graphic scale
	public void SetScales ()
	{
		//Sets relative scales between graphic and local scale
		//for instance of local scale is 10 and graphic range is(-2,2) -> scale= 10*2/(2-(-2))=5  (on the y axis)
		scale.x = transform.localScale.x / interval;
		scale.y = transform.localScale.y / Mathf.Abs (yLimits.x - yLimits.y);
	}

	//adds markings to YAxis (only happens if it exists an Y axis)
	void AxisSteps (float step)
	{	
		//holds biggest digit count (used to move Ylabel in case numbers get to long like 0.222)
		int longest = 0;

		//holds number of digits after decimal point (used because floating point is stored in weird maner
		//which will sometimes make a number like 3.4 turn into 3.40001
		int afterdot = 0;

		//calculates digits after decimal point if there IS a decimal point
		if (step.ToString ().IndexOf (".") > -1) {
			afterdot = step.ToString ().Substring (step.ToString ().IndexOf (".")).Length - 1;
		}
	
		//i goes from 0 to full range and adds a tick at every step
		for (float i = 0; i <= yLimits.y - yLimits.x; i += step) {

			//creates a tick
			GameObject go = (GameObject)Instantiate (axisMarkFab, transform);
			#if UNITY_EDITOR
			UnityEditor.Undo.RegisterCreatedObjectUndo (go, "Created tick");
			#endif

			//adds to ticklist
			ticksIngraph.Add (go);

			//scales tick marks apropriately
			go.transform.localScale = new Vector3 (go.transform.localScale.x / transform.localScale.x, go.transform.localScale.y / transform.localScale.y, go.transform.localScale.z);

			//sets color
			LineRenderer l = go.GetComponent<LineRenderer> ();
			l.sharedMaterial = new Material (Shader.Find ("Sprites/Default"));
			l.sharedMaterial.color = axiscolor;

			//centers it on center of the graphic
			go.transform.localPosition = Vector3.zero;

			//starts on bottom to the top (moves it half of graphic to the left, adds at the correct scale ticks)
			go.transform.position += Vector3.left * 0.5f + Vector3.up * scale.y * (i - (yLimits.y - yLimits.x) / 2);

			//gets text componet for a tickmark
			Text nTxt = go.GetComponentInChildren<Text> ();

			//sets text to graphic y value at that position
			nTxt.text = ((i + yLimits.x)).ToString ();

			//limit digits after comma, trims the end of big numbers (if step is 0.2 and number is 3.4102 it makes it 3.4)
			nTxt.text = processTickNumber (nTxt.text, afterdot);

			//changes color of the text
			go.transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = textcolor;

			//finds longest digit count on tick text
			if (nTxt.text.Length > longest)
				longest = nTxt.text.Length;
		} 

		//move Ylabel if there are to many digits
		labelYtxt.transform.localPosition = Vector3.left * 1.3f + Vector3.left * 0.079f * longest; 

	} 


	/// <summary>
	/// Trims tick numbers if they have to many digits
	/// </summary>
	/// <returns>The tick number.</returns>
	/// <param name="tickTxt">Tick text.</param>
	/// <param name="afterdot">Afterdot.</param>
	string processTickNumber (string tickTxt, int afterdot)
	{
		//if step has decimal point
		if (afterdot > 0) {

			//if tick number has decimal point and number has too many digits
			if (tickTxt.IndexOf (".") > -1 && tickTxt.Substring (tickTxt.IndexOf (".")).Length - 1 > afterdot)

				//trim exta at the end
				tickTxt = tickTxt.Substring (0, tickTxt.IndexOf (".") + afterdot + 1);			
		}

		return tickTxt;
	}


	/// <summary>
	/// recalculates scales, position of point in line renderers, and the axisticks positions
	/// </summary>
	void AutoRange ()
	{
		//to the power of 10
		int exp;

		//auxiliary variable
		float value;

				//hold scale before this change happens
		float prevScale = 0;

		//stores minimum value point right now
		float min = Mathf.Infinity;
		float max = Mathf.NegativeInfinity;

		//in every line search max and min point
		for (int i = 0; i < lines; i++) {

			for (int k = 0; k < lrs [i].positionCount; k++) {

				if (lrs [i].GetPosition (k).y > max) {
					max = lrs [i].GetPosition (k).y;
				}

				if (lrs [i].GetPosition (k).y < min) {
					min = lrs [i].GetPosition (k).y;
				}

			}
		}

		//converts from world coordinates to value (graphic) coordinates 
		max /= scale.y;
		min /= scale.y;

		//if graphic max or min changed autorange will happen
		if (yLimits.x != min || yLimits.y != max) {

			//gets the highest exponent of range -> [-2,100] -2 has exponent 0 -2+10^0 and 100 has exponent 2 , 1*10^2
			if (Mathf.Abs (max) > Mathf.Abs (min)) {

				exp=exponent (max);
			}else
				exp=exponent (min);

			//calculates ranges normalized between [0,10[
			value = (max - min) / Mathf.Pow(10,exp);

			//gets approximate value of a tick (5 ticks almost always)
			value /= 5;

			//gets actual value of tick
			if (value > 1)
				value = 2;
			else if (value > 0.4f)
				value = 1;
			else
				value = 0.5f;


			//rounds limits up, say step is 1 and graph is between 0 and 2, (2-0)/5=0.4f  0.4-->0.5 then graph bound are -0.5 and 2.5
			yLimits.y = (Mathf.RoundToInt( (max / Mathf.Pow(10,exp))+value)*Mathf.Pow(10,exp));
			yLimits.x = (Mathf.RoundToInt( (min / Mathf.Pow(10,exp))-value)*Mathf.Pow(10,exp));

			//save previous scale scale
			prevScale = scale.y;

			//set new scales
			SetScales ();


			//refreshes position of X axis
			if(axisX==true)
			CenterOnZero (lrx.transform);

			if ((yLimits.x > 0 || yLimits.y < 0 ) && axisX==true)
				lrx.transform.localPosition = Vector3.down * 0.5f;


			//center lines on zero according to new scales
			for (int i = 0; i < lines; i++) {

				CenterOnZero (lrs [i].transform);
			}


			//converts points to new scale
			//position= value in old scale	|	position/prevScale = realValue	|	realValue*scale.y = value in new scale
			for (int k = 0; k < lines; k++)
				for (int i = 0; i < lrs [k].positionCount; i++) {
					lrs [k].SetPosition (i, new Vector3 (lrs [k].GetPosition (i).x, scale.y * (lrs [k].GetPosition (i).y / prevScale)));
				}

			//recalculate ticks amount an position
			if (axisY) {
				
				//redraw axisTicks
				RedrawAxisTicks (value);
			}

		}
	}

	/// <summary>
	/// Redraws the axis ticks.
	/// </summary>
	/// <param name="tickstep">Tickstep.</param>
	public void RedrawAxisTicks (float tickstep)
	{
		//checks if tick step is invalid
		if (tickstep <= 0)
			return;
		
		//destroy previous ticks
		DeleteAxisTicks ();
	
		//add new ticks
		AxisSteps (tickstep);
	}
	public void DeleteAxisTicks ()
	{

		//converts list to array
		GameObject[] gos = ticksIngraph.ToArray ();

		#if UNITY_EDITOR
		//record the script
		UnityEditor.Undo.RecordObject(this,"Stuff");
		#endif
		//clears list
		ticksIngraph.Clear ();


		//iterates through array
		for (int i = 0; i < gos.Length; i++) {

			//deletes it
			if (gos [i] != null)
			if (Application.isPlaying)
				Destroy (gos [i]);
			else {
				//in case this is doing without the game running (throught the editor), DestroyObjectImmediate must be used
				#if UNITY_EDITOR

				GameObject currentGoToDestroy = gos[i];

				UnityEditor.Undo.DestroyObjectImmediate(currentGoToDestroy);



				#else
				DestroyImmediate (ticksIngraph [i]);
				#endif


			}
		}

	}

	//transforms to input must be childs of the graphic
	void CenterOnZero (Transform t)
	{
		//simple function that does so much

		//	---
		//	X--
		//	---

		//reposition line on graphics zero, which is  the center left of the sprite (X marks the spot)
		t.localPosition = Vector3.zero;

		//centers line renderer on zero
		//moves it up or down to the exact place where zero is (dark magic equation)
		t.position += Vector3.down * scale.y * (yLimits.x + yLimits.y) / 2; 

	}

	//adds an X axis (on editor mode and playmode)
	public void AddAxisX ()
	{
		
		//sets conversion between local scale and graphic scale
		SetScales ();

		//instantiantes x axis
		GameObject go = (GameObject)Instantiate (axisFab, transform.position, transform.rotation);

		//sets its parent
		go.transform.SetParent (transform);

		//moves it to zero
		go.transform.localPosition = Vector3.zero;

		//moves it to the graphics zero with dark magic
		go.transform.position += Vector3.down * scale.y * (yLimits.x + yLimits.y) / 2; 

		//if axis  should not be displayed because there is no zero on graph, show it below
		if (yLimits.x > 0 || yLimits.y < 0)
			go.transform.localPosition = Vector3.down * 0.5f;

		//sets the axis material and colors
		lrx = go.GetComponent<LineRenderer> ();
		lrx.sharedMaterial = new Material (Shader.Find ("Sprites/Default"));
		lrx.sharedMaterial.color = axiscolor;

		//puts the end of the axis where its supposed to be (the full lenght of the graphic to the left)
		lrx.SetPosition (1, lrx.GetPosition (1) * transform.localScale.x);

		//sets arrow material
		LineRenderer l = go.GetComponent<LineRenderer> ();

		go.transform.GetChild (0).transform.localPosition = lrx.GetPosition (1);
		l = go.transform.GetChild (0).GetComponent<LineRenderer> ();
		l.sharedMaterial = new Material (Shader.Find ("Sprites/Default"));
		l.sharedMaterial.color = axiscolor;


	
	
	}

	public void AddAxisY ()
	{
		//sets conversion between local scale and graphic scale
		SetScales ();


		//instantiantes axis
		GameObject go = (GameObject)Instantiate (axisFab, transform.position, transform.rotation);

		//sets its parent
		go.transform.SetParent (transform);

		//moves it down to the end of the graph
		go.transform.localPosition = -Vector3.up * 0.5f;

		//rotates it to make it horizontal
		go.transform.Rotate (0, 0, 90);

		//sets the axis materials and color
		lry = go.GetComponent<LineRenderer> ();
		lry.sharedMaterial = new Material (Shader.Find ("Sprites/Default"));
		lry.sharedMaterial.color = axiscolor;

		//scale axis correctly
		lry.SetPosition (1, lry.GetPosition (1) * transform.localScale.x);



		LineRenderer l = go.GetComponent<LineRenderer> ();
		//sets arrows position and color
		go.transform.GetChild (0).transform.localPosition = lry.GetPosition (1);
		l = go.transform.GetChild (0).GetComponent<LineRenderer> ();
		l.sharedMaterial = new Material (Shader.Find ("Sprites/Default"));
		l.sharedMaterial.color = axiscolor;


		//Adds tick marks
		AxisSteps (tickMarksStep);
	}
	#if UNITY_EDITOR
	//changes the color of the axes' ticks
	public void ColorTicks ()
	{
		//for every tick on the ticklist
		for (int i = 0; i < ticksIngraph.Count; i++) {

			UnityEditor.Undo.RecordObject(ticksIngraph [i].GetComponent<LineRenderer> ().sharedMaterial,"Change color");

			//color it
			ticksIngraph [i].GetComponent<LineRenderer> ().sharedMaterial.color = axiscolor;


			UnityEditor.EditorUtility.SetDirty( ticksIngraph [i].GetComponent<LineRenderer> ().sharedMaterial);
		}
	}


	//changes the color of the labels and tick text
	public void ColorText ()
	{
		//used to undo textcolors
		UnityEditor.Undo.RecordObject(labelXtxt,"Change color");
		UnityEditor.Undo.RecordObject(labelYtxt,"Change color");

		//actually unos textcolors
		labelXtxt.color = textcolor;
		labelYtxt.color = textcolor;

		//used to undo textcolors
		UnityEditor.EditorUtility.SetDirty( labelYtxt);
		UnityEditor.EditorUtility.SetDirty( labelXtxt);

		//change steps color
		for (int i = 0; i < ticksIngraph.Count; i++) {

			//used to undo tick textcolors
			UnityEditor.Undo.RecordObject(ticksIngraph [i].transform.GetChild (0).GetChild (0).GetComponent<Text> (),"Change color");

			//navigates to the text component of an gameobject inside the canvas inside the tick gameobject and changes its color
			ticksIngraph [i].transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = textcolor;

			//used to undo tick textcolors
			UnityEditor.EditorUtility.SetDirty( ticksIngraph [i].transform.GetChild (0).GetChild (0).GetComponent<Text> ());

		}
	}

	#endif


	//gets exponent of a number, 15 -> exponent is 1 because 1.5*10^1 = 15
	int exponent (float num)
	{
		return num == 0 ? 0 : (int)Mathf.Floor ((Mathf.Log10 (Mathf.Abs (num))));
	}
}
