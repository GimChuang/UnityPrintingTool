using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public class PrintingTool : MonoBehaviour
{

    public string printerName = "Canon TS8100 series";

    public float interval_checkIsPrintingDone = 0.2f;

    Thread m_thread;

    void OnDisable()
    {
        if (m_thread != null)
            m_thread.Abort();
    }

    void OnDestroy()
    {
        if (m_thread != null)
            m_thread.Abort();
    }

    void OnApplicationQuit()
    {
        if (m_thread != null)
            m_thread.Abort();
    }

    public void CmdPrintThreaded(string _filePath)
    {
        if (File.Exists(_filePath) == false)
        {
            UnityEngine.Debug.LogError("File Not Exist: " + _filePath);
            UnityEngine.Debug.LogError("Printing Not Proceed");
        }
        else // File Exists
        {
            if (FileIOUtility.IsFileLocked(_filePath) == false)
            {
                string fullCommand = "rundll32 C:\\WINDOWS\\system32\\shimgvw.dll,ImageView_PrintTo " + "\"" + _filePath + "\"" + " " + "\"" + printerName + "\"";
                m_thread = new Thread(delegate () { CmdPrint(fullCommand); });
                m_thread.IsBackground = false;
                m_thread.Start();
            }
            else // File is locked
            {
                UnityEngine.Debug.LogError("File is Locked: " + _filePath);
                UnityEngine.Debug.LogError("Printing Not Proceed");
            }
        }
    }

    void CmdPrint(string _command)
    {
        try
        {
            Process myProcess = new Process();
            //myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.Arguments = "/c " + _command;
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            myProcess.WaitForExit();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
        }
    }

    int GetPrintJobCount()
    {
        try
        {
            Process myProcess = new Process();
            //myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.Arguments = "/c " + "cscript C:\\Windows\\System32\\Printing_Admin_Scripts\\en-US\\prnjobs.vbs -l";
            myProcess.StartInfo.RedirectStandardOutput = true; // capture the Standard Output stream
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();

            // Use a StringBuilder instead of string for better performance
            StringBuilder sb = new StringBuilder();
            while (!myProcess.HasExited)
                sb.Append(myProcess.StandardOutput.ReadToEnd());

            myProcess.WaitForExit();

            string resultString = sb.ToString();

            /*
             * resultString should be something like:
             * 
             * 
             * Microsoft (R) Windows Script Host Version 5.812
             * Copyright (C) Microsoft Corp. 1996-2006, 著作權所有，並保留一切權利
             *
             * Number of print jobs enumerated 0
             *
             * 
             * So we simply split the string with space(' ') 
             * and look at the last string, which is the count of print jobs.
            */
            string[] resultSringArray = resultString.Split(' ').ToArray();

            string jobCountString = resultSringArray[resultSringArray.Length - 1];
            UnityEngine.Debug.Log("Print job counts: " + jobCountString);
            return (int.Parse(jobCountString));

        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            return -1;
        }
    }

    public void StartCheckIsPrintingDone()
    {
        StartCoroutine(E_KeepCheckingIsPrintingDone());
    }

    /*
     * If the print job count becomes bigger than 0 and returns back to 0 again, we assume the print job is done.
     * You should adjust your check interval or checking method 
     * because every printer's performance and behavior varies.
    */
    IEnumerator E_KeepCheckingIsPrintingDone()
    {
        bool isPrintDone = false;
        bool hasPrintStarted = false;

        while (isPrintDone == false)
        {
            if(hasPrintStarted == false)
            {
                if (GetPrintJobCount() > 0)
                {
                    hasPrintStarted = true;
                    UnityEngine.Debug.LogWarning("Print has started");
                }
            }
            else
            {
                if (GetPrintJobCount() == 0)
                {
                    isPrintDone = true;
                    UnityEngine.Debug.LogWarning("Print job is done.");
                }
            }
                  
            // Keep checking until the print job is done
            yield return new WaitForSeconds(interval_checkIsPrintingDone);           
        }
    }

}

