using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GraphController : MonoBehaviour {
	
	public List<VertexContainer> vertex;
	public List<EdgeContainer> edge;
	
	private bool isDirected;
	public bool directed {
		get { return isDirected; }
		set { 
			for (int i=0; i<edge.Count; i++)
				edge[i].directed = value;
			isDirected = value;
		}
	}

	private bool isWeighted;
	public bool weighted {
		get { return isWeighted; }
		set { 
			for (int i=0; i<edge.Count; i++)
				edge[i].weighted = value;
			isWeighted = value;
		}
	}

	// Use this for initialization
	void Start () {
		this.vertex = new List<VertexContainer> ();
		this.edge = new List<EdgeContainer> ();
		this.directed = true;
		this.weighted = true;
	}
	
	// checking methods
	public VertexContainer VertexAtPoint(Vector3 p, float koefficient){
		for (int i = 0; i < vertex.Count; i++){
			if (vertex[i] == null) 
				continue;
			float radius = vertex[i].vertex.GetComponent<SphereCollider>().radius;
			if (Math.Abs(vertex[i].vertex.transform.position.x - p.x) <= koefficient * radius &&
				    Math.Abs(vertex[i].vertex.transform.position.y - p.y) <= koefficient * radius)
				return vertex[i];
			}
		return null;
	}
	
	public EdgeContainer EdgeAtPoint(Vector3 p){
		Vector2 point = new Vector2(p.x, p.y);
		for (int i=0; i<edge.Count; i++){
			if (edge[i] == null)
				continue;
			float thickness = 0.5f;
			Vector2 v = edge[i].begin;
			Vector2 w = edge[i].end;
			if (gameObject.GetComponent<EuclidianGeometry>().minimum_squared_distance(v, w, point) < thickness + float.Epsilon)
				return edge[i];			
		}
		return null;
	}

	public VertexContainer AddVertex(Vector2 p){
		Vector3 InternalPosition = Camera.main.ScreenToWorldPoint (new Vector3 (p.x, p.y, 0f));
		VertexContainer currentVertex;
		if ((currentVertex = VertexAtPoint (InternalPosition, 2.0f)) == null) {
			// if mouse is pointing at empty space, then add a vertex in that position
			GameObject textCopy = Instantiate (gameObject.GetComponent<GraphCreator> ().textVertex) as GameObject;
			VertexContainer newVertex = new VertexContainer (textCopy, new Vector2 (InternalPosition.x, InternalPosition.y), vertex.Count);
			vertex.Add (newVertex);
		}
		return currentVertex;
	}

	public void MoveVertex(VertexContainer v, Vector2 position){
		if (v == null)
			return;
		v.ChangePosition (position);
		for (int i=0; i<edge.Count; i++)
			edge [i].UpdatePosition ();
	}

	public void EditEdge(EdgeContainer e, String newValue){
		int value = int.Parse(newValue);
		e.Weight = value;
	}


	public void AddEdge (VertexContainer vertex1, VertexContainer vertex2, int weight){
		for (int i=0; i<edge.Count; i++)
			if ((edge [i].vertexBegin == vertex1 && edge [i].vertexEnd == vertex2) ||
					(edge [i].vertexEnd == vertex1 && edge [i].vertexBegin == vertex2))
				return ;

		GameObject textCopy = Instantiate (gameObject.GetComponent<GraphCreator> ().textEdge) as GameObject;
		EdgeContainer newEdge = new EdgeContainer (vertex1, vertex2, textCopy );

		newEdge.directed = this.directed;

		vertex1.connectionForward.Add(new ConnectionInformation(vertex2, newEdge));
		vertex2.connectionBackward.Add(new ConnectionInformation(vertex1, newEdge));

		newEdge.weighted = this.weighted;
		edge.Add (newEdge);
	}



	// Update is called once per frame
	void Update () {
	
	}
}
