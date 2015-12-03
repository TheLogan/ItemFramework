using UnityEngine;
using System.Collections;
using ItemFramework.Db;

public class Manager : MonoBehaviour
{
	void Start()
	{
		DbManager.Instance.Handler = new DbFileHandler("test.json");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
