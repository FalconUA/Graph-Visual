using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Algorithm : MonoBehaviour {

	public class ActionRecord
	{
		public String action;
		public VertexContainer vertex;
		public EdgeContainer edge;
		public String text;
		public ActionRecord(string action, VertexContainer v, EdgeContainer e, String t){
			this.action = action;
			this.vertex = v;
			this.edge = e;
			this.text = t;		
		}
	}
	

	List<VertexContainer> vertex;
	List<EdgeContainer> edge;
	List<int> InitialWeight;
	Queue<ActionRecord> actionQueue;

	float ActionDuration;
	float TotalDeltaTime;

	private VertexContainer startVertex;
	public VertexContainer start
	{
		get { return startVertex; }
		set {
			if (value == null){
				GameObject.Find("Begin Point").GetComponent<Renderer>().enabled = false;
				startVertex = value;
				return;
			};
			GameObject.Find("Begin Point").GetComponent<Renderer>().enabled = true;
			Vector3 p = value.vertex.transform.position;
			GameObject.Find("Begin Point").GetComponent<Transform>().position = new Vector3(p.x+0.1f, p.y+0.8f, 5.0f);
			GameObject.Find("Begin Point").GetComponent<Transform>().SetParent(value.vertex.transform);
			startVertex = value;
		}
	}

	private VertexContainer finishVertex;
	public VertexContainer finish
	{
		get { return finishVertex; }
		set {
			if (value == null){
				GameObject.Find("End Point").GetComponent<Renderer>().enabled = false;
				finishVertex = value;
				return;
			};
			GameObject.Find("End Point").GetComponent<Renderer>().enabled = true;
			Vector3 p = value.vertex.transform.position;
			GameObject.Find("End Point").GetComponent<Transform>().position = new Vector3(p.x+0.1f, p.y+0.8f, 5.0f);
			GameObject.Find("End Point").GetComponent<Transform>().SetParent(value.vertex.transform);
			finishVertex = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		actionQueue = new Queue<ActionRecord> ();
		InitialWeight = new List<int> ();

		start = null;
		finish = null;
	}

	public void ResetAlgorithm(){
		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		edge = GameObject.Find("Graph Controller").GetComponent<GraphController>().edge;

		actionQueue.Clear();
		for (int i=0; i<vertex.Count; i++){
			if (vertex[i] != null){
				vertex[i].visited = false;
				vertex[i].UpdateText("");
			}
		}

		for (int i=0; i<edge.Count; i++)
			if (edge[i] != null)
				edge[i].marked = false;
				
		TotalDeltaTime = 0;
		ActionDuration = 0;
	}

	public void AddAction(string action, VertexContainer v, EdgeContainer e){
		actionQueue.Enqueue(new ActionRecord(action, v, e, ""));
	}
	public void AddAction(string action, VertexContainer v, EdgeContainer e, string t){
		actionQueue.Enqueue(new ActionRecord(action, v, e, t));
	}

	public void ProcessAction(ActionRecord Action){
		if (Action.action == "Update Vertex Text")
			if (Action.vertex != null)
				Action.vertex.UpdateText(Action.text);
		if (Action.action == "Visit Vertex")
			if (Action.vertex != null)
				Action.vertex.visited = true;			
		if (Action.action == "Mark Vertex")
			if (Action.vertex != null)
				Action.vertex.Mark();
		if (Action.action == "Mark Edge")
			if (Action.edge != null)
				Action.edge.marked = true;
		if (Action.action == "Visit Vertex Immediately")
			if (Action.vertex != null)
				Action.vertex.visited = true;
		if (Action.action == "Mark Edge Immediately")
			if (Action.edge != null)
				Action.edge.marked = true;
	}
	
	float GetDuration(ActionRecord Action){
		if (Action.action == "Update Vertex Text")
			return 0.5f;
		if (Action.action == "Visit Vertex")
			return 0.5f;		
		if (Action.action == "Mark Vertex")
			return 0.5f;
		if (Action.action == "Mark Edge")
			return 0.5f;		
		if (Action.action == "Visit Vertex Immediately")
			return 0.0f;		
		if (Action.action == "Mark Edge Immediately")
			return 0.0f;
		return 0.5f;
	}

	// Update is called once per frame
	void Update () {
		TotalDeltaTime += Time.deltaTime;
		if (actionQueue.Count == 0)
			return;
		if (TotalDeltaTime >= ActionDuration){
			TotalDeltaTime = 0.0f;
			ActionRecord lastAction = actionQueue.Dequeue();		
			ActionDuration = GetDuration(lastAction);
			ProcessAction(lastAction);
		}
	}
}
