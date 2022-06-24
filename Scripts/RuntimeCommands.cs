using System.Collections.Generic;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class RuntimeCommands
    {
        public void PrintList(List<object> list)
        {
            RuntimeConsole.Header("Start - Printing list information");

            if (list == null)
            {
                RuntimeConsole.Error("List is null.");
                RuntimeConsole.Header("End");
                return;
            }

            if (list.Count > 0)
            {
                int counter = -1;
                foreach (object listItem in list)
                {
                    counter++;
                    RuntimeConsole.Log("Itm:" + counter + " " + listItem.ToString());
                }

                RuntimeConsole.Log("List length: " + list.Count);

                RuntimeConsole.Header("End");
            }
            else
            {
                RuntimeConsole.Error("Couldn't print list information because it's empty.");
                RuntimeConsole.Header("End");
            }
        }

        public void PrintList(List<object> list, bool isSerialized)
        {
            RuntimeConsole.Header("Start - Printing list information");

            if (list == null)
            {
                RuntimeConsole.Error("List is null.");
                RuntimeConsole.Header("End");
                return;
            }

            if (list.Count > 0)
            {
                int counter = -1;
                foreach (object listItem in list)
                {
                    counter++;

                    if (!isSerialized)
                    {
                        RuntimeConsole.Log("Itm:" + counter + " " + listItem.ToString());
                    }
                    else
                    {

                        string json = JsonUtility.ToJson(listItem);
                        RuntimeConsole.Log("Itm:" + counter + " " + json);
                    }
                }

                RuntimeConsole.Log("List length: " + list.Count);

                RuntimeConsole.Header("End");
            }
            else
            {
                RuntimeConsole.Header("Couldn't print list information because it's empty.");
                RuntimeConsole.Header("End");
            }
        }
    }
}