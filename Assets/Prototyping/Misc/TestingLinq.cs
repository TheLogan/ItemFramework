using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestingLinq : MonoBehaviour
{

	public int number;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			List<int> list = new List<int> { 2, 5, 7, 10 };
		
			int closest = list.Aggregate((x, y) => Mathf.Abs(x - number) < Mathf.Abs(y - number) ? x : y);

			print("closest : " + closest);
		}
	}
}
