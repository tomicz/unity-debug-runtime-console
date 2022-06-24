using System.Collections.Generic;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class RuntimeCommands
    {
        public void PrintList(List<object> list)
        {
            RuntimeConsole.PrintMessage(MessageType.Header, "Start - Printing list information");

            if (list == null)
            {
                RuntimeConsole.PrintMessage(MessageType.Error, "List is null.");
                return;
            }

            if (list.Count > 0)
            {
                int counter = -1;
                foreach (object listItem in list)
                {
                    counter++;
                    RuntimeConsole.PrintMessage(MessageType.Log, "Itm:" + counter + " " + listItem.ToString());
                }

                RuntimeConsole.PrintMessage(MessageType.Log, "List length: " + list.Count);

                RuntimeConsole.PrintMessage(MessageType.Header, "End");
            }
            else
            {
                RuntimeConsole.PrintMessage(MessageType.Error, "Couldn't print list information because it's empty.");
            }
        }

        public void PrintList(List<object> list, bool isSerialized)
        {
            RuntimeConsole.PrintMessage(MessageType.Header, "Start - Printing list information");

            if (list == null)
            {
                RuntimeConsole.PrintMessage(MessageType.Error, "List is null.");
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
                        RuntimeConsole.PrintMessage(MessageType.Log, "Itm:" + counter + " " + listItem.ToString());
                    }
                    else
                    {

                        string json = JsonUtility.ToJson(listItem);
                        RuntimeConsole.PrintMessage(MessageType.Log, "Itm:" + counter + " " + json);
                    }
                }

                RuntimeConsole.PrintMessage(MessageType.Log, "List length: " + list.Count);

                RuntimeConsole.PrintMessage(MessageType.Header, "End");
            }
            else
            {
                RuntimeConsole.PrintMessage(MessageType.Error, "Couldn't print list information because it's empty.");
            }
        }
    }
}