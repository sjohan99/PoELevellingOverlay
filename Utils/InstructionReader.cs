using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoELevellingOverlay
{
    public class InstructionReader
    {

        List<List<string>> Acts = new List<List<string>>();
        public int Act = 0;
        public int Instruction = 0;

        public void init()
        {
            var pathProperty = Properties.Settings.Default["InstructionPath"];
            string path = "";
            if (pathProperty != null)
            {
                path = pathProperty.ToString();
                string[] files = Directory.GetFiles(path);
                List<string> filesList = new List<string>();
                filesList.AddRange(files);
                filesList.Sort((x, y) => ExtractNumber(x).CompareTo(ExtractNumber(y)));

                foreach (string file in filesList)
                {
                    List<string> instructions = new List<string>();
                    foreach(string line in System.IO.File.ReadAllLines(file))
                    {
                        instructions.Add(line);
                    }
                    Acts.Add(instructions);
                }
                var savedAct = Properties.Settings.Default["SavedAct"];
                var savedInstruction = Properties.Settings.Default["SavedStep"];
                Trace.WriteLine("Looked for settings");
                if (savedAct != null)
                {
                    Act = (int) savedAct;
                    Trace.WriteLine(savedAct.ToString());
                    Trace.WriteLine("Enter savedact if");
                }
                if (savedInstruction != null)
                {
                    Instruction = (int) savedInstruction;
                    Trace.WriteLine("Entered savedinstruction if");
                }
            }
        }

        public string getNextInstruction()
        {
            if (Acts[Act].Count == Instruction + 1)
            {
                if (!(Acts.Count == Act + 1))
                {
                    Act++;
                    Instruction = 0;
                }
            }
            else
            {
                Instruction++;
            }
            string newInstruction = Acts[Act][Instruction];
            return newInstruction;
        }

        public string getPreviousInstruction()
        {
            if (Instruction == 0)
            {
                if (Act == 0)
                {
                    return Acts[Act][Instruction];
                }
                Act--;
                Instruction = Acts[Act].Count - 1;
            }
            Instruction--;
            return Acts[Act][Instruction];
        }

        public string getCurrentInstruction()
        {
            return Acts[Act][Instruction];
        }

        public string getProgress()
        {
            int a = Act + 1;
            int s = Instruction + 1;
            return "Act " + a + ", Step " + s;
        }


        static int ExtractNumber(string text)
        {
            Match match = Regex.Match(text, @"(\d+)");
            if (match == null)
            {
                return 0;
            }

            int value;
            if (!int.TryParse(match.Value, out value))
            {
                return 0;
            }

            return value;
        }


    }
}
