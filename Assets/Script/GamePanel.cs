using UnityEngine;
using System.Collections;

public class GamePanel : MonoBehaviour {

	public static GamePanel _instance;

	public static GamePanel Instance {
		get { return _instance; }
	}

	void Awark() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
