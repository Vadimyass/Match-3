using System.IO;
using UnityEngine;

namespace MatchEngine
{
    public class Logger : MonoBehaviour
    {
        private string logFilePath;

        private void Start()
        {
            Debug.LogError(Application.dataPath + "/error_logs.txt");
            logFilePath = Application.dataPath + "/error_logs.txt";
            File.WriteAllText(logFilePath, "Error Logs\n"); 
            Application.logMessageReceived += LogErrorToFile; 
        }

        private void LogErrorToFile(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception)
            {
                string logEntry = $"{logString}\n";
                File.AppendAllText(logFilePath, logEntry);
            }
        }
    }
}