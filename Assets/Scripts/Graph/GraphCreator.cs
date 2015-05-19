using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class GraphCreator : MonoBehaviour {
	
	private LineRenderer newEdge = null;
	private VertexContainer firstVertex = null;
	private VertexContainer movingVertex = null;
	private EdgeContainer selectedEdge = null;
	private InputField inputField = null;
	private bool toggleMove = false;

	public bool PlacingStartFlag = false;
	public bool PlacingFinishFlag = false;

	public GameObject textVertex;
	public GameObject textEdge;

	// Use this for initialization
	void Start () {
		newEdge = gameObject.AddComponent<LineRenderer> ();
		newEdge.enabled = false;

		newEdge.useWorldSpace = true;
		newEdge.material = new Material (Shader.Find("Particles/Additive"));
		newEdge.SetColors (Color.yellow, Color.yellow);
		newEdge.SetVertexCount (2);

		firstVertex = null;
		textVertex = Instantiate (Resources.Load ("Prefabs/TextVertex")) as GameObject;
		textEdge = Instantiate (Resources.Load ("Prefabs/TextEdge")) as GameObject;

		inputField = GameObject.Find("InputField").GetComponent<InputField>();
		inputField.gameObject.SetActive(false);
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
		submitEvent.AddListener(SubmitValue);
		inputField.onEndEdit = submitEvent;
	}
	
	// Update is called once per frame
	void Update () {	
		if (EventSystem.current.IsPointerOverGameObject())
			return ;

		if (PlacingStartFlag){

			Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			GameObject.Find("Begin Point").GetComponent<Transform>().position = new Vector3(p.x+0.1f, p.y+0.8f, 5.0f);

			if (Input.GetMouseButtonUp (0)){
				VertexContainer currentVertex = GameObject.Find("Graph Controller").GetComponent<GraphController> ().VertexAtPoint(p, 1.0f);
				GameObject.Find("Algorithms").GetComponent<Algorithm>().start = currentVertex;
				PlacingStartFlag = false;
			}

			return ;
		}
		if (PlacingFinishFlag){

			Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			GameObject.Find("End Point").GetComponent<Transform>().position = new Vector3(p.x+0.1f, p.y+0.8f, 5.0f);
			if (Input.GetMouseButtonUp (0)){
				VertexContainer currentVertex = GameObject.Find("Graph Controller").GetComponent<GraphController> ().VertexAtPoint(p, 1.0f);
				GameObject.Find("Algorithms").GetComponent<Algorithm>().finish = currentVertex;
				PlacingFinishFlag = false;
			}
			return ;
		}

		if (Input.GetMouseButtonUp (0)) { 
			movingVertex = GameObject.Find ("Graph Controller").GetComponent<GraphController> ().AddVertex (
				new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
			if (movingVertex != null)
				toggleMove = !toggleMove;
			if (toggleMove == false)
				movingVertex = null;
		}
		if (Input.GetMouseButtonUp(1) && !toggleMove){
			Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			VertexContainer currentVertex = GameObject.Find("Graph Controller").GetComponent<GraphController> ().VertexAtPoint(p, 1.0f);
			if (currentVertex != null){
				if (firstVertex == null){					
					firstVertex = currentVertex;

					newEdge.enabled = true;				
					newEdge.SetPosition(0, new Vector3(firstVertex.vertex.transform.position.x, firstVertex.vertex.transform.position.y, 15.0f));
					newEdge.SetWidth(0.25f, 0.25f);
				}
				else if (currentVertex != firstVertex){
					GameObject.Find("Graph Controller").GetComponent<GraphController>().AddEdge(firstVertex, currentVertex, 1);

					newEdge.enabled = false;
					firstVertex = null;
				}

				if (selectedEdge != null){
					// if adding new edge, then unselect all other edges
					selectedEdge.marked = false;
					selectedEdge = null;
					inputField.gameObject.SetActive(false);
				}
			}
			else {
				newEdge.enabled = false;
				firstVertex = null;

				EdgeContainer currentEdge = GameObject.Find("Graph Controller").GetComponent<GraphController>().EdgeAtPoint(p);

				if (selectedEdge != null)
					selectedEdge.marked = false;
				if (currentEdge != null){
					currentEdge.marked = true;
					if (currentEdge.weighted){
						inputField.gameObject.SetActive(true);
						EventSystem.current.SetSelectedGameObject(inputField.gameObject);
						inputField.ActivateInputField();
						inputField.transform.position = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
					}
				}
				else {
					inputField.gameObject.SetActive(false);
				}
				selectedEdge = currentEdge;
			}
		}

		if (firstVertex != null) {
			Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			newEdge.SetPosition (1, new Vector3 (p.x, p.y, 15.0f));
		}

		if (toggleMove){
			if (movingVertex != null)
				GameObject.Find ("Graph Controller").GetComponent<GraphController> ().MoveVertex(
					movingVertex, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			else 
				toggleMove = false;
		}
			
	}

	void SubmitValue(string value){
		GameObject.Find("Graph Controller").GetComponent<GraphController>().EditEdge(selectedEdge, inputField.text);
		selectedEdge.marked = false;
		selectedEdge = null;
		inputField.text = "";
		inputField.gameObject.SetActive(false);
	}

}
