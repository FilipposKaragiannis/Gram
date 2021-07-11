using System;
using System.IO;
using System.Text;
using Gram.Rpg.Client.Core;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Initialisation
{
    public class Logging : InitialisableBase
    {
        protected override void Initialise()
        {
            Console.SetOut(new GTextWriter(Debug.Log));
            Console.SetError(new GTextWriter(Debug.LogError));

            G.Configure(Debug.Log,
                Debug.Log,
                Debug.LogWarning,
                Debug.LogError,
                (s, e) => Debug.LogError($"{s}\n{e}"));
        }

        private class GTextWriter : TextWriter
        {
            private readonly StringBuilder  buffer = new StringBuilder();
            private readonly Action<string> unityLogger;

            public GTextWriter(Action<string> unityLogger)
            {
                this.unityLogger = unityLogger;
            }

            public override Encoding Encoding => Encoding.Default;

            public override void Flush()
            {
                unityLogger(buffer.ToString());
                buffer.Length = 0;
            }

            public override void Write(string value)
            {
                buffer.Append(value);

                if (value == null)
                    return;

                var len = value.Length;
                if (len == 0)
                    return;


                var lastChar = value[len - 1];
                if (lastChar == '\n')
                    Flush();
            }

            public override void Write(char value)
            {
                buffer.Append(value);

                if (value == '\n')
                    Flush();
            }

            public override void Write(char[] value, int index, int count)
            {
                Write(new string(value, index, count));
            }
        }
    }
}
