using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EuclidianGeometry : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float sqr(float x){ return x*x; }
	public float length_squared(Vector2 v, Vector2 w){
		return sqr(w.x - v.x) + sqr(w.y - v.y);
	}
	public float distance(Vector2 p, Vector2 v){
		return sqr(p.x - v.x) + sqr(p.y - v.y);
	}
	public float dot(Vector2 a, Vector2 b){
		return (a.x*b.x + a.y*b.y);
	}
	
	public float minimum_squared_distance(Vector2 v, Vector2 w, Vector2 p) {
		// Return minimum distance between line segment vw and point p
		float l2 = length_squared(v, w);  // i.e. |w-v|^2 -  avoid a sqrt
		if (l2 == 0.0) return distance(p, v);   // v == w case
		// Consider the line extending the segment, parameterized as v + t (w - v).
		// We find projection of point p onto the line.
		// It falls where t = [(p-v) . (w-v)] / |w-v|^2
		float t = dot(new Vector2(p.x - v.x, p.y - v.y),
		              new Vector2(w.x - v.x, w.y - v.y)) / l2;
		if (t < 0.0) return distance(p, v);       // Beyond the 'v' end of the segment
		else if (t > 1.0) return distance(p, w);  // Beyond the 'w' end of the segment
		Vector2 projection = new Vector2(v.x + t * (w.x - v.x),
		                                 v.y + t * (w.y - v.y));  // Projection falls on the segment
		return distance(p, projection);
	}

}
