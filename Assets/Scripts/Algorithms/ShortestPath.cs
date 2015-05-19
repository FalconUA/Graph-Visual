using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShortestPath : MonoBehaviour {

	const int Infinity = 2000000000;

	private List<VertexContainer> vertex;
	private List<EdgeContainer> edge;
	private Algorithm algorithm;

	// data for Bellman-Ford

	// data for Dijkstra
	List< int > prev;
	List< bool > used;

	// data for Floyd Warshall
	List< List<int> > dist;

	// Common data
	List< int > distance;

	// Use this for initialization
	void Start () {
		distance = new List<int>();
		prev = new List<int>();
		used = new List<bool>();
		dist = new List< List<int> >();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RunBellmanFord(VertexContainer source){
		if (source == null)
			return;

		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		edge = GameObject.Find("Graph Controller").GetComponent<GraphController>().edge;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();

		algorithm.ResetAlgorithm();

		distance.Clear();
		prev.Clear();
		for (int i=0; i<this.vertex.Count; i++)
			distance.Add(Infinity);
			
		distance[source.Index] = 0;
		algorithm.AddAction("Visit Vertex", source, null);
		algorithm.AddAction("Update Vertex Text", source, null, "0");
		
		for (int i=0; i<this.vertex.Count; i++)
			for (int j=0; j<this.edge.Count; j++)
				if (distance[edge[j].vertexBegin.Index] < Infinity){
					distance[edge[j].vertexEnd.Index] = Math.Min(distance[edge[j].vertexEnd.Index], distance[edge[j].vertexBegin.Index] + edge[j].Weight);
					int new_t = distance[edge[j].vertexEnd.Index];
					algorithm.AddAction("Mark Edge", null, edge[j]);
					algorithm.AddAction("Visit Vertex", edge[j].vertexEnd, null);
					algorithm.AddAction("Update Vertex Text", edge[j].vertexEnd, null, new_t.ToString());
				}
	}

	public void RunDijkstra(VertexContainer source, bool directed){
		if (source == null)
			return;

		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		edge = GameObject.Find("Graph Controller").GetComponent<GraphController>().edge;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();
		
		algorithm.ResetAlgorithm();

		distance.Clear();
		prev.Clear();
		used.Clear();
		for (int i=0; i<this.vertex.Count; i++){
			distance.Add(Infinity);
			prev.Add(-1);
			used.Add(false);
		}

		distance[source.Index] = 0;
		algorithm.AddAction("Mark Vertex", source, null);
		algorithm.AddAction("Update Vertex Text", source, null, "0");

		for (int i=0; i<this.vertex.Count; i++){
			int v = -1;
			for (int j=0; j<this.vertex.Count; j++)
				if (!used[j] && (v == -1 || distance[j] < distance[v]))
					v = j;

			if (distance[v] == Infinity)
				break;

			used[v] = true;
			algorithm.AddAction("Visit Vertex", vertex[v], null);

			for (int j=0; j<vertex[v].connectionForward.Count; j++){
				VertexContainer to = vertex[v].connectionForward[j].next;
				algorithm.AddAction("Mark Edge", null, vertex[v].connectionForward[j].edge);
				if (distance[v] + vertex[v].connectionForward[j].weight < distance[to.Index]){
					distance[to.Index] = distance[v] + vertex[v].connectionForward[j].weight;
					prev[to.Index] = v;
					algorithm.AddAction("Mark Vertex", to, null);
					algorithm.AddAction("Update Vertex Text", to, null, distance[to.Index].ToString());
				}
			}

			if (!directed){
				for (int j=0; j<vertex[v].connectionBackward.Count; j++){
					VertexContainer to = vertex[v].connectionBackward[j].next;
					algorithm.AddAction("Mark Edge", null, vertex[v].connectionBackward[j].edge);
					if (distance[v] + vertex[v].connectionBackward[j].weight < distance[to.Index]){
						distance[to.Index] = distance[v] + vertex[v].connectionBackward[j].weight;
						prev[to.Index] = v;
						algorithm.AddAction("Mark Vertex", to, null);
						algorithm.AddAction("Update Vertex Text", to, null, distance[to.Index].ToString());
					}
				}
			}
		}
		// end dijkstra
	}

	public void RunFloydWarshall(){
		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		edge = GameObject.Find("Graph Controller").GetComponent<GraphController>().edge;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();
		
		algorithm.ResetAlgorithm();

		this.dist.Clear();
		for (int i=0; i<this.vertex.Count; i++){
			this.dist.Add(new List<int>());
			for (int j=0; j<this.vertex.Count; j++)
				dist[i].Add(0);
			dist[i][i] = 0;
		}

		for (int i=0; i<this.edge.Count; i++)
			dist[edge[i].vertexBegin.Index][edge[i].vertexEnd.Index] = edge[i].Weight;

		for (int k=0; k<this.vertex.Count; k++)
			for (int i=0; i<this.vertex.Count; i++)
				for (int j=0; j<this.vertex.Count; j++)
					if (dist[i][j] > dist[i][k] + dist[k][j])
						dist[i][j] = dist[i][k] + dist[k][j];
	}
}
