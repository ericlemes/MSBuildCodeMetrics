using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core.Providers
{
	internal class FileLOCCount
	{
		private int _commentLineCount;
		public int CommentLineCount
		{
			get { return _commentLineCount; }
			set { _commentLineCount = value; }
		}

		private int _emptyLineCount;
		public int EmptyLineCount
		{
			get { return _emptyLineCount; }
			set { _emptyLineCount = value; }
		}

		private int _totalLineCount;
		public int TotalLineCount
		{
			get { return _totalLineCount; }
			set { _totalLineCount = 0; }
		}

		public int CodeLineCount
		{
			get { return _totalLineCount - _emptyLineCount - _commentLineCount; }
		}

		public FileLOCCount(int commentLineCount, int emptyLineCount, int codeLineCount)
		{
			_commentLineCount = commentLineCount;
			_emptyLineCount = emptyLineCount;
			_totalLineCount = codeLineCount;
		}

		public void Sum(FileLOCCount locCount)
		{
			_commentLineCount += locCount.CommentLineCount;
			_emptyLineCount += locCount.EmptyLineCount;
			_totalLineCount += locCount.TotalLineCount;
		}
	}
}
