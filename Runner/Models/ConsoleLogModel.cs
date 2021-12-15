using System;
using System.Collections.Generic;
using System.ComponentModel;
using ReactiveUI;

namespace Runner.Models;

public class ConsoleLogModel : ReactiveObject
{
    private string _consoleLog = $"[{DateTime.Now:HH:mm:ss}] Welcome!" + Environment.NewLine;
    public string ConsoleLog
    {
        get => _consoleLog;
        set => this.RaiseAndSetIfChanged(ref _consoleLog, value); 
    }
    
    public void AppendLog(string text)
    {
        ConsoleLog = $"[{DateTime.Now:HH:mm:ss}]" + text + Environment.NewLine + ConsoleLog;
    }
}