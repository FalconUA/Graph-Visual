using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class EdgeContainer
{
	private Vector3 togglePointTrue;
	private Vector3 togglePointFalse;
	public Vector2 begin { get; private set; }
	public Vector2 end { get; private set; }
	private GameObject edgeText;

	private bool isDirected;
	public bool directed {
		get { return isDirected; }
		set {
			if (value == true){
				head.GetComponent<LineRenderer> ().enabled = true;
				body.GetComponent<LineRenderer> ().SetPosition(1, togglePointTrue);
			}
			else {
				head.GetComponent<LineRenderer> ().enabled = false;
				body.GetComponent<LineRenderer> ().SetPosition(1, togglePointFalse);
			}
			isDirected = value;
		}
	}

	private bool isMarked;
	public bool marked { 
		get { return isMarked; }
		set { 
			if (value == false){
				body.GetComponent<LineRenderer>().SetColors (Color.yellow, Color.yellow);
				head.GetComponent<LineRenderer>().SetColors (Color.yellow, Color.yellow);
			}
			else {
				body.GetComponent<LineRenderer>().SetColors (Color.red, Color.red);
				head.GetComponent<LineRenderer>().SetColors (Color.red, Color.red);
			}
			isMarked = value;
		}
	}

	private bool isWeighted;
	public bool weighted {
		get { return isWeighted; }
		set {
			edgeText.SetActive(value);
			isWeighted = value;
		}
	}

	private int edgeWeight;
	public int Weight {
		get { return edgeWeight; }
		set {
			this.UpdateText(value.ToString());
			edgeWeight = value;
		}
	}

	private String text;

	public VertexContainer vertexBegin { get; private set; }
	public VertexContainer vertexEnd { get; private set; }
	private GameObject body = null;
	private GameObject head = null;
	
	public EdgeContainer(VertexContainer v1, VertexContainer v2, GameObject textObject){
		body = new GameObject ();
		head = new GameObject ();
		
		vertexBegin = v1;
		vertexEnd = v2;
		begin = new Vector2 (v1.vertex.transform.position.x, v1.vertex.transform.position.y);
		end = new Vector2 (v2.vertex.transform.position.x, v2.vertex.transform.position.y);
		Vector2 delta = end - begin;
		delta.Normalize ();	
		
		LineRenderer bodyLine = body.AddComponent<LineRenderer> ();
		bodyLine.SetWidth (0.25f, 0.25f);
		bodyLine.material = new Material (Shader.Find("Particles/Additive"));
		bodyLine.SetVertexCount (2);
		bodyLine.SetPosition(0, new Vector3(begin.x, begin.y, 20.0f));
		bodyLine.SetPosition(1, togglePointFalse = new Vector3(end.x, end.y, 20.0f));		               
		
		LineRenderer headLine = head.AddComponent<LineRenderer> ();
		headLine.enabled = false;
		headLine.SetWidth (0.69f, 0.0f);
		headLine.material = new Material (Shader.Find("Particles/Additive"));
		headLine.SetVertexCount (2);
		headLine.SetPosition(0, togglePointTrue = new Vector3((end-1.4f*delta).x, (end-1.4f*delta).y, 20.0f));	
		headLine.SetPosition(1, new Vector3((end-0.49f*delta).x, (end-0.49f*delta).y, 20.0f));	

		edgeText = textObject;
		UpdateText ("");

		marked = false;
		directed = false;
		weighted = false;

		Weight = 1;
	}

	public void UpdateText(string textString){
		Vector2 norm = end - begin;
		norm.Normalize ();

		float sinNormal = norm.y;
		float cosNormal = norm.x;
		if (cosNormal < float.Epsilon && cosNormal > -float.Epsilon)
			cosNormal += float.Epsilon * 2;	

		Vector3 position = begin + (end - begin)/2;
		Vector3 textCoordinates = new Vector3 (position.x, position.y, 19.0f);	

		edgeText.transform.Rotate(-edgeText.transform.rotation.eulerAngles);
		edgeText.transform.Rotate(new Vector3(0.0f,0.0f,180 * Mathf.Atan (sinNormal / cosNormal) / Mathf.PI));
		edgeText.transform.position = textCoordinates;
		edgeText.GetComponent<TextMesh>().text = text = textString;
	}

	public void UpdatePosition(){
		Vector2 beginPoint = new Vector2 (vertexBegin.vertex.transform.position.x, vertexBegin.vertex.transform.position.y);
		Vector2 endPoint = new Vector2 (vertexEnd.vertex.transform.position.x, vertexEnd.vertex.transform.position.y);
		if (beginPoint == begin && endPoint == end)
			return;
		LineRenderer bodyLine = body.GetComponent<LineRenderer> ();
		LineRenderer headLine = head.GetComponent<LineRenderer> ();
		begin = beginPoint;
		end = endPoint;

		Vector2 delta = end - begin;
		delta.Normalize ();

		togglePointFalse = new Vector3 (end.x, end.y, 20.0f);
		togglePointTrue = new Vector3 ((end - 1.4f * delta).x, (end - 1.4f * delta).y, 20.0f);

		bodyLine.SetPosition (0, new Vector3 (beginPoint.x, beginPoint.y, 20f));
		if (directed)
			bodyLine.SetPosition (1, togglePointTrue);
		else
			bodyLine.SetPosition(1, togglePointFalse);

		headLine.SetPosition(0, togglePointTrue);	
		headLine.SetPosition(1, new Vector3((end-0.49f*delta).x, (end-0.49f*delta).y, 20.0f));	

		UpdateText(text);
	}
}