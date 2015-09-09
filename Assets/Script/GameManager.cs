using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static private GameManager _instance;

	public static GameManager Instance {
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
