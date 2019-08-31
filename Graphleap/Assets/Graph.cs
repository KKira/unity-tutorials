using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// class extendes MonoBehaviour so it can be used as a component for game objects
public class Graph : MonoBehaviour {
	List<string> names = new List<string>(){
		"None",
		"Sine",
		 "Sine 2D",
		 "Multi Sine",
		 "Multi Sine 2D",
		 "Ripple",
		 "Cylinder",
		 "Sphere",
		 "Torus"
	};

	public Dropdown dropdown;

	// public field to hold reference to a prefab for creating points
	// we need access to the trasnform component to positions the points, make that the field
	public Transform pointPrefab;
	// [Range] instructs the inspector to enforce a range for resolution
	[Range(10,100)]
	public int resolution = 10;
	Transform[] points;

	public GraphFunctionName function;

	//by using delegate, it is possible to invoke f as a function
	//instead of if else, use an array's index to determine which function
	static GraphFunction[] functions = {
			None,
			SineFunction,
			Sine2DFunction,
			MultiSineFunction,
			MultiSine2DFunction,
			Ripple,
			Cylinder,
			Sphere,
			Torus
		};

	const float pi = Mathf.PI;

	GraphFunction f = functions[0];

	void Start () {
		PopulateList();
	}

	void Awake () {

		//store in a variable to avoid calculating in the loop everytime as the value is the same
		//2f is the range
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		/*Vector3 position;
		position.y = 0f;
		position.z = 0f;
		*/
		points = new Transform[resolution * resolution];

		// keep track of x explicitly
		/*for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			//after a row is completed, row = resolution, reset x
			if (x == resolution) {
				x = 0;
				z += 1;
			}
		*/

		/*for (int i = 0, z = 0; z < resolution; z++) {
			
			position.z = (z + 0.5f) * step - 1f;

			for (int x = 0; x < resolution; x++, i++) {
				//keep track of the referenced transform component with a variable
				Transform point = Instantiate(pointPrefab);
				//to put the cubes in a row along the X axis, multiply by i
				//diving by 5f makes them cover the 0-2 range
				//tp turn that into the -1-1 range, subtract 1 before scaling the vector
				//adding 0.5f before diving will shift the position of the cubes to fit -1-1 range
				position.x = (x + 0.5f) * step -1f;
				point.localPosition = position;
				// scale down cubes to appropriate domain
				point.localScale = scale;
				// set up each cube as a child of Graph game object
				point.SetParent(transform, false);
				points[i] = point;
			}
		}*/

		for (int i = 0; i < points.Length; i++) {
			Transform point = Instantiate(pointPrefab);
			point.localScale = scale;
			point.SetParent(transform, false);
			points[i] = point;
		}
	}

	void Update() {
		float t = Time.time;

		//enumaration cannot be implicitly cast to an integer (int)
		//GraphFunction f = functions[(int)function];

		/*for (int i = 0; i < points.Length; i++) {
			//get reference of current array element
			Transform point = points[i];
			//retrieve that point's position
			Vector3 position = point.localPosition;
			//derive y coordinate
			position.y = f(position.x, position.z, t);

			//old way to derive y coordinate
			//to animate this function add current time to x
			// we use f(x,t) = sin(pi(x+t)) where t is the elapsed time
			//position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
			//vector are not objects
			//we adjusted the local variable
			//to apply to the point we set its position again
			point.localPosition = position;
		}*/

		float step = 2f / resolution;
		for (int i = 0, z = 0; z  < resolution; z++) {
			float v = (z + 0.5f) * step - 1f;
			for (int x = 0; x < resolution; x++, i++) {
				float u = (x + 0.5f) * step - 1f;
				points[i].localPosition = f(u,v,t);
			}
		}
	}

	void PopulateList() {
		dropdown.AddOptions(names);
	}

	public void GraphChange(int index){
		f = functions[index];
	}

	//extracting math from the code that loops through the graph points
	static Vector3 SineFunction (float x, float z, float t) {
		//old method
		//return Mathf.Sin(pi * (x + t));

		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.z = z;
		return p;
	}

	static Vector3 MultiSineFunction (float x, float z, float t) {
		/*float y = Mathf.Sin(pi * (x + t));
		//dpouble the frequency, and half it to keep the shape of the sine wave
		y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
		//to guarantee that the values stay within -1-1 range we multiply by 2/3
		y *= 2f / 3f;
		return y;
		*/

		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
		p.y *= 2f / 3f;
		p.z = z;
		return p;
	}

	static Vector3 Sine2DFunction (float x, float z, float t) {
		//return Mathf.Sin(pi * (x + z + t));
		/* old method not using 3d functions
		float y = Mathf.Sin(pi * (x + t));
		y += Mathf.Sin(pi * (z + t));
		y *= 0.5f;
		return y;
		*/
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(pi * (z + t));
		p.y *= 0.5f;
		p.z = z;
		return p;
	}

	static Vector3 MultiSine2DFunction (float x, float z, float t) {
		/*float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
		y += Mathf.Sin(pi * (x + t));
		y += Mathf.Sin(2f * pi * (z + 2f *t)) * 0.5f;
		y *= 1f/5.5f;
		return y;*/

		Vector3 p;
		p.x = x;
		p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
		p.y += Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(2f * pi * (z + 2f *t)) * 0.5f;
		p.y *= 1f / 5.5f;
		p.z = z;
		return p;
	}

	static Vector3 Ripple (float x, float z, float t) {
		Vector3 p;
		float d = Mathf.Sqrt(x * x + z * z);
		p.x = x;
		p.y = Mathf.Sin(pi * (4f *d - t));
		p.y /= 1f + 10F * d;
		p.z = z;
		return p;
	}

	static Vector3 Cylinder (float u, float v, float t) {
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
		p.x = r * Mathf.Sin(pi * u);
		p.y = v;
		p.z = r * Mathf.Cos(pi * u);
		return p;
	}

	//pulstaing sphere
	static Vector3 Sphere (float u, float v, float t) {
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
		float s = r * Mathf.Cos(pi * 0.5f * v);
		p.x = s * Mathf.Sin(pi * u);
		p.y = r * Mathf.Sin(pi * 0.5f * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	static Vector3 Torus (float u, float v, float t) {
		Vector3 p;
		float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	static Vector3 None(float u, float v, float t) {
		Vector3 p;
		p.x = 0f;
		p.y = 0f;
		p.z = 0f;
		return p;
	}		
}
