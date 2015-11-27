using UnityEngine;
using System.Collections;
using System.Threading;

public class ThreadingTest : MonoBehaviour {

	public void Start ()
	{
		var t1 = new Thread(MyThread);
		var t2 = new Thread(MyThread);
		t1.Start("1");
		t2.Start("2");
	}

	void MyThread(object obj)
	{
		var id = obj.ToString();
		for (var i = 0; i < 10; i++)
		{
			print("Stuff : " + id);
		}

	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			Start();
	}
}
