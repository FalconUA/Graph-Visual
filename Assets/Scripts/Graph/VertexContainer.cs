using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConnectionInformation {
	public VertexContainer next;
	public EdgeContainer edge;
	public int weight {
		get { return edge.Weight; }
	}
	public ConnectionInformation(VertexContainer nextVertex, EdgeContainer edgeConnector){
		next = nextVertex;
		edge = edgeConnector;
	}
}

public class VertexContainer
{
	public int Index { get; private set; }
	public List<ConnectionInformation> connectionForward;
	public List<ConnectionInformation> connectionBackward;

	private bool isVisited;
	public bool visited { 
		get { return isVisited; }
		set { 
			if (value == false)
				this.vertex.GetComponent<Renderer>().material.color = Color.white;
			else 
				this.vertex.GetComponent<Renderer>().material.color = Color.red;
			isVisited = value;
		}
	}

	public void Mark(){
		if (this.isVisited) 
			return;
		this.vertex.GetComponent<Renderer>().material.color = Color.blue;
	}

	public GameObject vertexText;
	private String text;
	public GameObject vertex { get; private set; }
	
	public VertexContainer(GameObject textObject, Vector2 position, int index){
		vertex = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		vertex.transform.SetParent (GameObject.Find ("Graph Controller").transform);
		vertex.GetComponent<SphereCollider> ().radius = 0.5f;
		vertex.transform.position = new Vector3(position.x, position.y, 10.0f);
		vertex.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		vertex.GetComponent<Renderer>().material = new Material(Shader.Find("Sprites/Default"));
		
		visited = false;
		connectionForward = new List<ConnectionInformation>();
		connectionBackward = new List<ConnectionInformation>();

		this.vertexText = textObject;
		textObject.transform.SetParent(vertex.gameObject.transform);

		UpdateText("");
		Index = index;
	}

	public void UpdateText(string textString){

		Vector3 position = vertex.transform.position;
		Vector3 textCoordinates = new Vector3 (position.x, position.y, 9.0f);	

		vertexText.transform.position = textCoordinates;
		vertexText.GetComponent<TextMesh>().text = this.text = textString;
	}

	public void ChangePosition(Vector2 position){
		Vector3 InternalPosition = Camera.main.ScreenToWorldPoint (new Vector3 (position.x, position.y, 0f));
		vertex.transform.position = new Vector3(InternalPosition.x, InternalPosition.y, 10.0f);
	}
}