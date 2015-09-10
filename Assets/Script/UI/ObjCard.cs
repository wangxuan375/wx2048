using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjCard : MonoBehaviour {

	public Text numtext;
	public NumberCard card;
	public uint posX;
	public uint posY;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(uint posx, uint posy, uint num = 2) {
		posX = posx;
		posY = posy;
		card = new NumberCard(num);
		numtext.text = string.Format("{0}", card.Number);
	}

	public void SetNumber(uint num) {
		card.Number = num;
		numtext.text = string.Format("{0}", card.Number);
	}
}
