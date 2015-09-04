using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	public static GameManager Instance {
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
