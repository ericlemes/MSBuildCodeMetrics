﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildCodeMetrics.Core
{
	public interface ILoggableCodeMetricsProvider
	{
		void SetLogger(ILogger logger);
	}
}
