using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace Parsers
{
	internal enum Token
	{
		error = 1, EOF = 2, NUMBER = 3
	};

	internal partial struct ValueType
	{
		public int n;
		public string s;
	}
	// Abstract base class for GPLEX scanners
	internal abstract class ScanBase : AbstractScanner<ValueType, LexLocation>
	{
		private LexLocation __yylloc = new LexLocation();
		public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
		protected virtual bool yywrap() { return true; }
	}

	// Utility class for encapsulating token information
	internal class ScanObj
	{
		public int token;
		public ValueType yylval;
		public LexLocation yylloc;
		public ScanObj(int t, ValueType val, LexLocation loc)
		{
			this.token = t; this.yylval = val; this.yylloc = loc;
		}
	}

	internal partial class TinyCParser : ShiftReduceParser<ValueType, LexLocation>
	{
#pragma warning disable 649
		private static Dictionary<int, string> aliasses;
#pragma warning restore 649
		private static Rule[] rules = new Rule[5];
		private static State[] states = new State[5];
		private static string[] nonTerms = new string[] {
      "main", "$accept", "number", };

		static TinyCParser()
		{
			states[0] = new State(new int[] { 3, 4, 2, -3 }, new int[] { -1, 1, -3, 3 });
			states[1] = new State(new int[] { 2, 2 });
			states[2] = new State(-1);
			states[3] = new State(-2);
			states[4] = new State(-4);

			for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

			rules[1] = new Rule(-2, new int[] { -1, 2 });
			rules[2] = new Rule(-1, new int[] { -3 });
			rules[3] = new Rule(-3, new int[] { });
			rules[4] = new Rule(-3, new int[] { 3 });
		}

		protected override void Initialize()
		{
			this.InitSpecialTokens((int)Token.error, (int)Token.EOF);
			this.InitStates(states);
			this.InitRules(rules);
			this.InitNonTerminals(nonTerms);
		}

		protected override void DoAction(int action)
		{
#pragma warning disable 162, 1522
			switch (action)
			{
				case 4: // number -> NUMBER
					{ Console.WriteLine("Rule -> number: {0}", ValueStack[ValueStack.Depth - 1].n); }
					break;
			}
#pragma warning restore 162, 1522
		}

		protected override string TerminalToString(int terminal)
		{
			if (aliasses != null && aliasses.ContainsKey(terminal))
				return aliasses[terminal];
			else if (((Token)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
				return ((Token)terminal).ToString();
			else
				return CharToString((char)terminal);
		}

	}
}
