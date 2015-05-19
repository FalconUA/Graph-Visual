using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpanningTree : MonoBehaviour {

	const int Infinity = 2000000000;

	private List<VertexContainer> vertex;
	private List<EdgeContainer> edge;
	Algorithm algorithm;

	// Cruscal data
	List< int > getTreeIndex;

	// Prima data
	List< bool > used;
	List< int > min_e;
	List< int > sel_e;

	// Common data
	List< EdgeContainer > ResultTree;

	// Use this for initialization
	void Start () {
		getTreeIndex = new List<int>();
		ResultTree = new List<EdgeContainer>();
		used = new List<bool>();
		min_e = new List<int>();
		sel_e = new List<int>();
	}

	public void RunCruscal(){
		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		edge = GameObject.Find("Graph Controller").GetComponent<GraphController>().edge;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();

		getTreeIndex.Clear();
		ResultTree.Clear();
		algorithm.ResetAlgorithm();

		for (int i=0; i<vertex.Count; i++){
			getTreeIndex.Add(i);
		}

		while ( ResultTree.Count < vertex.Count-1 ){
			int min = Infinity;
			EdgeContainer tmpEdge = null;
			for (int i=0; i<edge.Count; i++){
				if ((min > edge[i].Weight) && (getTreeIndex[edge[i].vertexBegin.Index] != getTreeIndex[edge[i].vertexEnd.Index])){
					min = edge[i].Weight;
					tmpEdge = edge[i];
				}
			}

			// if we found such edge, then connect two trees
			if (tmpEdge != null){
				ResultTree.Add(tmpEdge);
				algorithm.AddAction("Visit Vertex Immediately", tmpEdge.vertexBegin, null);
				algorithm.AddAction("Mark Edge", null, tmpEdge);
				algorithm.AddAction("Visit Vertex Immediately", tmpEdge.vertexEnd, null);
				int ValA = getTreeIndex[tmpEdge.vertexBegin.Index];
				int ValB = getTreeIndex[tmpEdge.vertexEnd.Index];
				int ValMin = (ValA > ValB)?(ValB):(ValA);
				for (int i=0; i<this.vertex.Count; i++)
					if (getTreeIndex[i] == ValA || getTreeIndex[i] == ValB)						
						getTreeIndex[i] = ValMin;					
			}
			else
				break;
		}
	}

	public void RunPrima(){
		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		edge = GameObject.Find("Graph Controller").GetComponent<GraphController>().edge;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();

		algorithm.ResetAlgorithm();

		used.Clear();
		ResultTree.Clear();
		min_e.Clear();
		sel_e.Clear();

		for (int i=0; i<vertex.Count; i++){
			used.Add(false);
		}

		used[0] = true;
		algorithm.AddAction("Visit Vertex", vertex[0], null);

		while (true){
			int min = Infinity;
			EdgeContainer tmpEdge = null;
			for (int i=0; i<this.edge.Count; i++){
				if (min > edge[i].Weight && (used[edge[i].vertexBegin.Index] ^ used[edge[i].vertexEnd.Index])){
					min = edge[i].Weight;
					tmpEdge = edge[i];
				}
			}

			if (tmpEdge != null){
				ResultTree.Add(tmpEdge);
				if (used[tmpEdge.vertexBegin.Index] == false){
					algorithm.AddAction("Mark Edge", null, tmpEdge);
					algorithm.AddAction("Visit Vertex", tmpEdge.vertexBegin, null);
				}
				else if (used[tmpEdge.vertexEnd.Index] == false){
					algorithm.AddAction("Mark Edge", null, tmpEdge);
					algorithm.AddAction("Visit Vertex", tmpEdge.vertexEnd, null);
				}
				used[tmpEdge.vertexBegin.Index] = true;
				used[tmpEdge.vertexEnd.Index] = true;
			}
			else 
				break;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}



}
