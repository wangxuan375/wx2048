using UnityEngine;
using System.Collections;

public class NumberCard {

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

	public NumberCard(uint num = 2) {
		Number = num;
	}
}
