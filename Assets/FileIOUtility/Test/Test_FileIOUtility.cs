using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FileIOUtility : MonoBehaviour {

	
	void Start () {
        Debug.Log(FileIOUtility.GenerateFileName("MyFile", 12, FileIOUtility.FileExtension.PNG));
	}
	
	void Update () {
		
	}
}
