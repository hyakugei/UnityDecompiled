using System;

namespace Unity.Jobs
{
	public interface IJobParallelFor
	{
		void Execute(int index);
	}
}
