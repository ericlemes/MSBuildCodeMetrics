using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace MSBuildCodeMetrics.Core.UnitTests.Mock
{
	public class TaskItemMock : ITaskItem
	{		
		private readonly Dictionary<string, string> _metadatas = new Dictionary<string, string>();

		public System.Collections.IDictionary CloneCustomMetadata()
		{
			Dictionary<string, string> clone = new Dictionary<string,string>();
			foreach(KeyValuePair<string, string> pair in _metadatas)			
			{
				clone.Add(pair.Key, pair.Value);
			}
			return clone;
		}

		public void CopyMetadataTo(ITaskItem destinationItem)
		{
			throw new NotImplementedException();
		}

		public string GetMetadata(string metadataName)
		{
			if (!_metadatas.ContainsKey(metadataName))
				return null;
			return _metadatas[metadataName];
		}

		public string ItemSpec { get; set; }		

		public int MetadataCount
		{
			get { return _metadatas.Count;}			
		}

		public System.Collections.ICollection MetadataNames
		{
			get { return _metadatas.Keys; }
		}

		public void RemoveMetadata(string metadataName)
		{
			_metadatas.Remove(metadataName);
		}

		public void SetMetadata(string metadataName, string metadataValue)
		{
			if (_metadatas.ContainsKey(metadataName))
				_metadatas[metadataName] = metadataValue;
			else
				_metadatas.Add(metadataName, metadataValue);			
		}

		public TaskItemMock AddMetadata(string metadataName, string metadataValue)
		{
			SetMetadata(metadataName, metadataValue);
			return this;
		}

		public TaskItemMock(string itemSpec)
		{
			ItemSpec = itemSpec;
		}
	}
}
