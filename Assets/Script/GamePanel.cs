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
		script.Init();
		numcard.SetParent(gameObject.transform);
		numcard.anchoredPosition = GameObject.Find("Bg_Obj1").GetComponent<RectTransform>().anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public Vector2 GetPositionByIndex(uint index) {
		var gameobj = GameObject.Find(string.Format("Bg_Obj{0}", index));
		if (gameobj == null) {
			throw new UnityException(string.Format("gameobj Bg_Obj{0} cannot found!", index));
		}
		return gameobj.GetComponent<RectTransform>().anchoredPosition;
	}
}
