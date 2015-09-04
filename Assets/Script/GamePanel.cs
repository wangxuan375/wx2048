using UnityEngine;
using System.Collections;

public class GamePanel : MonoBehaviour {

	static private GamePanel _instance;

	public static GamePanel Instance {
		get { return _instance; }
	}

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
