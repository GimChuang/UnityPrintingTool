# UnityPrintingTool
Simple scripts to control printers with Unity. 🖨

How to Use
---
- Add PrintTool.cs to a gameobject in your scene.
- Type in the name of your printer (you can find it from Contol Panel - Devices and Printers)
- (optional) Type in the interval (in seconds). This will be used to check if the printing is done.
- Then you can get access to the PrintTool class and call methods as mentioned below. There is an example scene Test_PrintingTool.unity in this project.

Scripting Interface
---
`CmdPrintThreaded(string _filePath)` asks for path of the file which you want to print. It opens cmd.exe and calls `rundll32 C:\WINDOWS\system32\shimgvw.dll,ImageView_PrintTo "c:\MyDir\MyPicture.png" "My Printer"` to print your file!
(e.g. rundll32 C:\WINDOWS\system32\shimgvw.dll,ImageView_PrintTo "C:\Users\Gim\Desktop\Test.png" "Canon TS8100 series")

`StartCheckIsPrintingDone()` starts a coroutine. It opens cmd.exe and calls [prnjobs.vbs -l](http://www.windowscommandline.com/prnjobs-vbs/) and checks the number of print jobs. The coroutine does this check every `interval_checkIsPrintingDone` seconds. If the print job count becomes bigger than 0 and returns back to 0 again, we assume the print job is done and the coroutine will stop.

You should adjust the check interval, or figure out another appropriate way for you to check if the printing is done because every printer's performance and behavior vary. Of course if you have a better idea please share it :)

Reference
---
[How to get the output of a System.Diagnostics.Process?](https://stackoverflow.com/questions/1390559/how-to-get-the-output-of-a-system-diagnostics-process)
