using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Solar;

namespace SolarForge
{

	public class ProgramOutputHandler : EngineOutputHandler
	{

		public void BeginErrorSuppression()
		{
			if (this.suppressedErrors != null)
			{
				throw new InvalidOperationException();
			}
			this.suppressedErrors = new List<string>();
		}


		public List<string> EndErrorSuppression()
		{
			if (this.suppressedErrors == null)
			{
				throw new InvalidOperationException();
			}
			List<string> result = this.suppressedErrors;
			this.suppressedErrors = null;
			return result;
		}


		public override void OnTrace(string fileName, int lineNumber, string message)
		{
			Trace.WriteLine(message);
		}


		public override ShowErrorResult OnAlert(string fileName, int lineNumber, string message)
		{
			if (this.suppressedErrors != null)
			{
				this.suppressedErrors.Add(message);
				return ShowErrorResult.Skip;
			}
			DialogResult dialogResult = MessageBox.Show(message, "ALERT", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
			if (dialogResult == DialogResult.Abort)
			{
				return ShowErrorResult.TriggerBreakpoint;
			}
			if (dialogResult == DialogResult.Ignore)
			{
				return ShowErrorResult.SkipAll;
			}
			return ShowErrorResult.Skip;
		}


		public override ShowErrorResult OnAssertFailed(string fileName, int lineNumber, string expression)
		{
			string text = string.Format("ASSERT({0}) @ {1}({2})", expression, fileName, lineNumber);
			if (this.suppressedErrors != null)
			{
				this.suppressedErrors.Add(text);
				return ShowErrorResult.Skip;
			}
			DialogResult dialogResult = MessageBox.Show(text, "ASSERT", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
			if (dialogResult == DialogResult.Abort)
			{
				return ShowErrorResult.TriggerBreakpoint;
			}
			if (dialogResult == DialogResult.Ignore)
			{
				return ShowErrorResult.SkipAll;
			}
			return ShowErrorResult.Skip;
		}


		private List<string> suppressedErrors;
	}
}
