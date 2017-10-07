using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public abstract class Step
    {
        public string Text { get; private set; }
        public string MultiLineTextArg { get; private set; }
        public Table TableArg { get; }

        public Step(string text)
            : this(text, null, null)
        {
            
        }

        public Step(string text, string multiLineArg)
            : this(text, multiLineArg, null)
        {

        }

        public Step(string text, Table tableArg)
            : this(text, null, tableArg)
        {

        }

        public Step(string text, string multiLineTextArg, Table tableArg)
        {
            Text = text;
            MultiLineTextArg = multiLineTextArg;
            TableArg = tableArg;
        }

        public abstract void Run(ITestRunner runner);

        protected abstract Step Clone();

        public Step ApplyExampleParameters(TableRow examples)
        {
            var ret = Clone();

            foreach (KeyValuePair<string,string> kvp in examples)
            {
                ret.Text = ret.Text.Replace($"<{kvp.Key}>", kvp.Value);
                ret.MultiLineTextArg = ret.MultiLineTextArg.Replace($"<{kvp.Key}>", kvp.Value);
                foreach (var row in ret.TableArg.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        row[key] = row[key].Replace($"<{kvp.Key}>", kvp.Value);
                    }
                }
            }
            return ret;
        }

        public Step ApplyMacroParameters(Table macroParameters, int instance)
        {
            var ret = Clone();

            foreach (var param in macroParameters.Rows)
            {
                ret.Text = ret.Text.Replace($"[{param[0]}]", param[1+instance]);
                ret.MultiLineTextArg = ret.MultiLineTextArg.Replace($"[{param[0]}]", param[1 + instance]);
                foreach (var row in ret.TableArg.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        row[key] = row[key].Replace($"[{param[0]}]", param[1 + instance]);
                    }
                }
            }
            return ret;
        }

        protected static Table Clone(Table table)
        {
            var ret = new Table(table.Header.ToString());
            foreach (var row in table.Rows)
            {
                ret.AddRow(row.Values.ToArray());
            }
            return ret;
        }
    }
}