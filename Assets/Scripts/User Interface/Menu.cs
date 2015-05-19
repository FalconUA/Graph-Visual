using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Menu : MonoBehaviour {

	private GameObject panel;

	// Use this for initialization
	void Start () {
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, -100, t.position.z);
	}

	public void OnDirectedToggle(Boolean value){
		bool newValue = GameObject.Find("ToggleDirected").GetComponent<Toggle>().isOn;
		GameObject.Find("Graph Controller").GetComponent<GraphController>().directed = newValue;
	}
	public void OnWeightedToggle(Boolean value){
		bool newValue = GameObject.Find("ToggleWeighted").GetComponent<Toggle>().isOn;
		GameObject.Find("Graph Controller").GetComponent<GraphController>().weighted = newValue;
	}

	public void OnBFSClick(){
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, 10, t.position.z);	
		VertexContainer start = GameObject.Find("Algorithms").GetComponent<Algorithm>().start;
		VertexContainer finish = GameObject.Find("Algorithms").GetComponent<Algorithm>().finish;
		bool directed = GameObject.Find("Graph Controller").GetComponent<GraphController>().directed;
		GameObject.Find("Algorithms").GetComponent<ElementaryAlgorithms>().RunBfs(start, finish, directed);
	}

	public void OnDFSClick(){		
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, 10, t.position.z);	
		VertexContainer start = GameObject.Find("Algorithms").GetComponent<Algorithm>().start;
		VertexContainer finish = GameObject.Find("Algorithms").GetComponent<Algorithm>().finish;
		bool directed = GameObject.Find("Graph Controller").GetComponent<GraphController>().directed;
		GameObject.Find("Algorithms").GetComponent<ElementaryAlgorithms>().RunDfs(start, finish, directed);
	}

	public void OnCruscalClick(){
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, 10, t.position.z);	
		GameObject.Find("Algorithms").GetComponent<SpanningTree>().RunCruscal();
	}

	public void OnPrimaClick(){
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, 10, t.position.z);		
		GameObject.Find("Algorithms").GetComponent<SpanningTree>().RunPrima();
	}

	public void OnBellmanFordClick(){
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, 10, t.position.z);		
		VertexContainer start = GameObject.Find("Algorithms").GetComponent<Algorithm>().start;
		GameObject.Find("Algorithms").GetComponent<ShortestPath>().RunBellmanFord(start);
	}

	public void OnDijkstraClick(){
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, 10, t.position.z);	
		VertexContainer start = GameObject.Find("Algorithms").GetComponent<Algorithm>().start;
		bool directed = GameObject.Find("Graph Controller").GetComponent<GraphController>().directed;
		GameObject.Find("Algorithms").GetComponent<ShortestPath>().RunDijkstra(start, directed);
	}

	public void OnFloydWarshallClick(){

	}

	public void OnGreenFlagClick(){
		GameObject.Find("Algorithms").GetComponent<Algorithm>().start = null;

		if (GameObject.Find("Algorithms").GetComponent<Algorithm>().finish == null)
			GameObject.Find("End Point").GetComponent<Renderer>().enabled = false;

		GameObject.Find("Begin Point").GetComponent<Renderer>().enabled = true;
		GameObject.Find("Graph Controller").GetComponent<GraphCreator>().PlacingStartFlag = true;
		GameObject.Find("Graph Controller").GetComponent<GraphCreator>().PlacingFinishFlag = false;
		Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GameObject.Find("Begin Point").GetComponent<Transform>().position = new Vector3(p.x+0.1f, p.y+0.8f, 5.0f);
	}

	public void OnRedFlagClick(){
		GameObject.Find("Algorithms").GetComponent<Algorithm>().finish = null;

		if (GameObject.Find("Algorithms").GetComponent<Algorithm>().start == null)
			GameObject.Find("Begin Point").GetComponent<Renderer>().enabled = false;

		GameObject.Find("End Point").GetComponent<Renderer>().enabled = true;
		GameObject.Find("Graph Controller").GetComponent<GraphCreator>().PlacingStartFlag = false;
		GameObject.Find("Graph Controller").GetComponent<GraphCreator>().PlacingFinishFlag = true;
		Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GameObject.Find("End Point").GetComponent<Transform>().position = new Vector3(p.x+0.1f, p.y+0.8f, 5.0f);
	}

	public void OnResetClick(){
		Transform t = GameObject.Find("ResetAlgorithm").GetComponent<Transform>();
		t.position = new Vector3(t.position.x, -100, t.position.z);	
		GameObject.Find("Algorithms").GetComponent<Algorithm>().ResetAlgorithm();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
