using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

class LogicPos {
	public uint posX;
	public uint posY;

	public LogicPos(uint x, uint y) {
		posX = x;
		posY = y;
	}
}

public class GamePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	static private GamePanel _instance;
	
	public static GamePanel Instance {
		get { return _instance; }
	}

	Dictionary<LogicPos, RectTransform> CardList = new Dictionary<LogicPos, RectTransform>();
	//List<RectTransform> CardList = new List<RectTransform>();
	
	public GameObject prefabNumber;

	private Vector2 pressPosition;

	public GamePanel() {
	}

	public void ResetPanel() {
		foreach(KeyValuePair<LogicPos, RectTransform> p in CardList) {
			GameObject.Destroy(p.Value.gameObject);
		}
		CardList.Clear();
		for(int i = 0; i < 2; i++) {
			uint posX, posY;
			if(GameManager.Instance.ProduceCard(out posX, out posY)) {
				var numcard = Instantiate(prefabNumber).GetComponent<RectTransform>();
				var script = numcard.GetComponent<ObjCard>();
				script.Init(posX, posY);
				numcard.SetParent(gameObject.transform);
				numcard.anchoredPosition = GetObjPosition(posX, posY);
				var logicpos = new LogicPos(posX, posY);
				CardList[logicpos] = numcard;
			}
		}
	}

	public Vector2 GetObjPosition(uint PosX, uint PosY) {
		var index = (PosX - 1) * GameManager.Width + PosY;
		var gameobj = GameObject.Find(string.Format("Bg_Obj{0}", index));
		if (gameobj == null) {
			throw new UnityException(string.Format("gameobj Bg_Obj{0} cannot found!", index));
		}
		return gameobj.GetComponent<RectTransform>().anchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventdata) {
		pressPosition = eventdata.position;
	}

	public void OnPointerUp(PointerEventData eventdata) {
		Vector2 pointerUpPosition = eventdata.position;
		GameManager.Instance.OnPointerMove(pressPosition, pointerUpPosition);
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
