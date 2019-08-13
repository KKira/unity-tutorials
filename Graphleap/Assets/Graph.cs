using UnityEngine;

// class extendes MonoBehaviour so it can be used as a component for game objects
public class Graph : MonoBehaviour {

	// public field to hold reference to a prefab for creating points
	// we need access to the trasnform component to positions the points, make that the field
	public Transform pointPrefab;
	// [Range] instructs the inspector to enforce a range for resolution
	[Range(10,100)]
	public int resolution = 10;
	Transform[] points;

	[Range(0,1)]
	public int function;

	void Awake () {

		//store in a variable to avoid calculating in the loop everytime as the value is the same
		//2f is the range
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		Vector3 position;
		position.y = 0f;
		position.z = 0f;
		points = new Transform[resolution];

		for (int i = 0; i < points.Length; i++) {
			//keep track of the referenced transform component with a variable
			Transform point = Instantiate(pointPrefab);
			//to put the cubes in a row along the X axis, multiply by i
			//diving by 5f makes them cover the 0-2 range
			//tp turn that into the -1-1 range, subtract 1 before scaling the vector
			//adding 0.5f before diving will shift the position of the cubes to fit -1-1 range
			position.x = (i + 0.5f) * step -1f;
			point.localPosition = position;
			// scale down cubes to appropriate domain
			point.localScale = scale;
			// set up each cube as a child of Graph game object
			point.SetParent(transform, false);
			points[i] = point;
		}
	}

	void Update() {
		float t = Time.time;
		//by using delegate, it is possible to invoke f as a function
		//instead of if else, use an array's index to determine which function
		static GraphFunction[] functions = {
			SineFunction, MultiSineFunction
		};
		if (function == 0) {
			f = SineFunction;
		} else {
			f = MultiSineFunction;
		}

		for (int i = 0; i < points.Length; i++) {
			//get reference of current array element
			Transform point = points[i];
			//retrieve that point's position
			Vector3 position = point.localPosition;
			//derive y coordinate
			position.y = f(position.x, t);

			//old way to derive y coordinate
			//to animate this function add current time to x
			// we use f(x,t) = sin(pi(x+t)) where t is the elapsed time
			//position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
			//vector are not objects
			//we adjusted the local variable
			//to apply to the point we set its position again
			point.localPosition = position;
		}
	}

	//extracting math from the code that loops through the graph points
	static float SineFunction (float x, float t) {
		return Mathf.Sin(Mathf.PI * (x + t));
	}

	static float MultiSineFunction (float x, float t) {
		float y = Mathf.Sin(Mathf.PI * (x + t));
		//dpouble the frequency, and half it to keep the shape of the sine wave
		y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t)) / 2f;
		//to guarantee that the values stay within -1-1 range we multiply by 2/3
		y *= 2f / 3f;
		return y;
	}
}
