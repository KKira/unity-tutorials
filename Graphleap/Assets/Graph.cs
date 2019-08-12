using UnityEngine;

// class extendes MonoBehaviour so it can be used as a component for game objects
public class Graph : MonoBehaviour {

	// public field to hold reference to a prefab for creating points
	// we need access to the trasnform component to positions the points, make that the field
	public Transform pointPrefab;
	// [Range] instructs the inspector to enforce a range for resolution
	[Range(10,100)]
	public int resolution = 10;

	void Awake () {

		//store in a variable to avoid calculating in the loop everytime as the value is the same
		//2f is the range
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		Vector3 position;
		position.z = 0f;

		for (int i = 0; i < resolution; i++) {
			//keep track of the referenced transform component with a variable
			Transform point = Instantiate(pointPrefab);
			//to put the cubes in a row along the X axis, multiply by i
			//diving by 5f makes them cover the 0-2 range
			//tp turn that into the -1-1 range, subtract 1 before scaling the vector
			//adding 0.5f before diving will shift the position of the cubes to fit -1-1 range
			position.x = (i + 0.5f) * step -1f;
			position.y = position.x * position.x;
			point.localPosition = position;
			// scale down cubes to appropriate domain
			point.localScale = scale;
			// set up each cube as a child of Graph game object
			point.SetParent(transform, false);
		}
	}
}
