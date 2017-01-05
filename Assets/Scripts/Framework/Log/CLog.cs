using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class CLog
    {
        public static bool enableLog = true;
        public static bool enableWarnLog = true;
        public static bool enableErrorLog = true;

        public static void Log(string msg,string color = "#ffffff")
        {
            if (enableLog)
            {
                StringBuilder sb = new StringBuilder();
                if (color.IsEmpty())
                {
                    sb.Append(msg.ToString());
                }
                else
                {
                    sb.Append("<color=" + color + ">");
                    sb.Append(msg.ToString());
                    sb.Append("</color>");
                }

                Debug.Log(sb.ToString());
            }
        }

        public static void LogError(object msg)
        {
            if (enableErrorLog)
            {
                UnityEngine.Debug.LogError(msg);
            }
        }

        public static void LogWarn(object msg)
        {
            if (enableWarnLog)
            {
                UnityEngine.Debug.LogWarning(msg);
            }
        }
    }

	public class CLogColor
	{
		public static string Yellow = "#ff7f00";
		public static string Red = "#ff0000";
	}
}
