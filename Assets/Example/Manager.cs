using UnityEngine;
using System.Collections;
using ItemFramework.Db;

public class Manager : MonoBehaviour {
    private IDbHandler dbHandler = new DbFileHandler("test.json");

    // Use this for initialization
    void Start () {
        dbHandler.Load();
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
