using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumberCard : MonoBehaviour {

	public Text numtext;
	private uint number;

	public uint Number {
		set {
			if (value % 2 != 0) {
				throw new UnityException("nuber must be %2 == 0");
			}
			number = value;
		}
		get {
			return number;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(uint num) {
		this.Number = num;
		numtext.text = string.Format("{0}", this.Number);
	}
}
