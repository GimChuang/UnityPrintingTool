using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PrintingTool : MonoBehaviour {

    public string filePath = "";

    public PrintingTool printingTool;

    void Start()
    {
        printingTool.CmdPrintThreaded(filePath);
        printingTool.StartCheckIsPrintingDone();
    }

    void Update()
    {
        
    }
}
