using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ElementaryAlgorithms : MonoBehaviour {

	private List<VertexContainer> vertex;

	// BFS container
	private Queue<VertexContainer> queue;

	// \DFS container
	private Stack<VertexContainer> stack;

	// Common container
	List<bool> used;
	Algorithm algorithm;

	// Use this for initialization
	void Start () {
		queue = new Queue<VertexContainer>();
		stack = new Stack<VertexContainer>();
		used = new List<bool>();
	}

	public void RunBfs(VertexContainer start, VertexContainer finish, bool directed){
		if (start == null)
			return;

		// initialization
		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();

		algorithm.ResetAlgorithm();
		queue.Clear();
		used.Clear();
		for (int i=0; i<vertex.Count; i++)
			used.Add(false);
		Queue<EdgeContainer> prevEdge = new Queue<EdgeContainer>();

		queue.Enqueue(start);

		// process algorithm
		while (queue.Count > 0 && (finish == null || used[finish.Index] == false)){

			// if it is not start, mark the edge that we used to read this vertex
			if (prevEdge.Count > 0){
				EdgeContainer prev = prevEdge.Dequeue();
				algorithm.AddAction("Mark Edge", null, prev);
			}

			// visit this vertex
			VertexContainer current = queue.Dequeue();	
			algorithm.AddAction("Visit Vertex", current, null);
			used[current.Index] = true;

			for (int i=0; i<current.connectionForward.Count; i++){
				if (used[current.connectionForward[i].next.Index])
					continue;
				queue.Enqueue(current.connectionForward[i].next);
				prevEdge.Enqueue(current.connectionForward[i].edge);
			}

			if (!directed){
				for (int i=0; i<current.connectionBackward.Count; i++){
					if (used[current.connectionBackward[i].next.Index])
						continue;
					queue.Enqueue(current.connectionBackward[i].next);
					prevEdge.Enqueue(current.connectionBackward[i].edge);
				}
			}
		}
	}

	public void RunDfs(VertexContainer start, VertexContainer finish, bool directed){
		if (start == null)
			return;

		// initialization
		vertex = GameObject.Find("Graph Controller").GetComponent<GraphController>().vertex;
		algorithm = GameObject.Find("Algorithms").GetComponent<Algorithm>();

		algorithm.ResetAlgorithm();
		stack.Clear();
		used.Clear();
		for (int i=0; i<vertex.Count; i++)
			used.Add(false);
		
		stack.Push(start);

		Stack<EdgeContainer> prevEdge = new Stack<EdgeContainer>();
		
		// process algorithm
		while (stack.Count > 0 && (finish == null || used[finish.Index] == false)){

			// if it is not start, mark the edge that we used to read this vertex
			if (prevEdge.Count > 0){
				EdgeContainer prev = prevEdge.Pop();
				algorithm.AddAction("Mark Edge", null, prev);
			}

			// visit this vertex
			VertexContainer current = stack.Pop();
			used[current.Index] = true;
			algorithm.AddAction("Visit Vertex", current, null);
			
			for (int i=0; i<current.connectionForward.Count; i++){
				if (used[current.connectionForward[i].next.Index])
					continue;
				stack.Push(current.connectionForward[i].next);
				prevEdge.Push(current.connectionForward[i].edge);
			}
			
			if (!directed){
				for (int i=0; i<current.connectionBackward.Count; i++){
					if (used[current.connectionBackward[i].next.Index])
						continue;
					stack.Push(current.connectionBackward[i].next);
					prevEdge.Push(current.connectionBackward[i].edge);
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
