using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjCard : MonoBehaviour {

	public Text numtext;
	public NumberCard card;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(uint num = 2) {
		card = new NumberCard(num);
		numtext.text = string.Format("{0}", card.Number);
	}
}
