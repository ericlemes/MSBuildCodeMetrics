namespace MSBuildCodeMetrics.Core.Providers
{
	internal class FileLOCCount
	{
	    public int CommentLineCount { get; set; }

	    public int EmptyLineCount { get; set; }

	    private int _totalLineCount;
		public int TotalLineCount
		{
			get { return _totalLineCount; }			
		}

		public int CodeLineCount
		{
			get { return _totalLineCount - EmptyLineCount - CommentLineCount; }
		}

		public FileLOCCount(int commentLineCount, int emptyLineCount, int codeLineCount)
		{
			CommentLineCount = commentLineCount;
			EmptyLineCount = emptyLineCount;
			_totalLineCount = codeLineCount;
		}

		public void Sum(FileLOCCount locCount)
		{
			CommentLineCount += locCount.CommentLineCount;
			EmptyLineCount += locCount.EmptyLineCount;
			_totalLineCount += locCount.TotalLineCount;
		}
	}
}
