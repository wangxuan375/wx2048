using UnityEngine;
using System.Collections;

public class GamePanel : MonoBehaviour {

	static private GamePanel _instance;

	public static GamePanel Instance {
		get { return _instance; }
	}

	public GameObject prefabNumber;

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		var numcard = Instantiate(prefabNumber).GetComponent<RectTransform>();
		var script = numcard.GetComponent<NumberCard>();
		script.Init(2);
		numcard.SetParent(gameObject.transform);
		numcard.anchoredPosition = GameObject.Find("Bg_Obj1").GetComponent<RectTransform>().anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
