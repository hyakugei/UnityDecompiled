using System;
using UnityEngine;

namespace UnityEditor.PackageManager.Requests
{
	public abstract class Request
	{
		[SerializeField]
		private bool m_ErrorFetched;

		[SerializeField]
		private Error m_Error;

		[SerializeField]
		private NativeStatusCode m_Status = NativeStatusCode.NotFound;

		[SerializeField]
		private long m_Id;

		private NativeStatusCode NativeStatus
		{
			get
			{
				if (this.m_Status <= NativeStatusCode.InProgress)
				{
					this.m_Status = NativeClient.GetOperationStatus(this.Id);
				}
				return this.m_Status;
			}
		}

		protected long Id
		{
			get
			{
				return this.m_Id;
			}
		}

		public StatusCode Status
		{
			get
			{
				StatusCode result;
				switch (this.NativeStatus)
				{
				case NativeStatusCode.InQueue:
				case NativeStatusCode.InProgress:
					result = StatusCode.InProgress;
					break;
				case NativeStatusCode.Done:
					result = StatusCode.Success;
					break;
				case NativeStatusCode.Error:
				case NativeStatusCode.NotFound:
					result = StatusCode.Failure;
					break;
				default:
					throw new NotSupportedException(string.Format("Unknown native status code {0}", this.NativeStatus));
				}
				return result;
			}
		}

		public bool IsCompleted
		{
			get
			{
				return this.Status > StatusCode.InProgress;
			}
		}

		public Error Error
		{
			get
			{
				if (!this.m_ErrorFetched && this.Status == StatusCode.Failure)
				{
					this.m_ErrorFetched = true;
					this.m_Error = NativeClient.GetOperationError(this.Id);
					if (this.m_Error == null)
					{
						if (this.NativeStatus == NativeStatusCode.NotFound)
						{
							this.m_Error = new Error(ErrorCode.NotFound, "Operation not found");
						}
						else
						{
							this.m_Error = new Error(ErrorCode.Unknown, "Unknown error");
						}
					}
				}
				return this.m_Error;
			}
		}

		internal Request()
		{
		}

		internal Request(long operationId, NativeStatusCode initialStatus)
		{
			this.m_Id = operationId;
			this.m_Status = initialStatus;
		}
	}
	public abstract class Request<T> : Request
	{
		[SerializeField]
		private bool m_ResultFetched = false;

		[SerializeField]
		private T m_Result = default(T);

		public T Result
		{
			get
			{
				if (!this.m_ResultFetched && base.Status == StatusCode.Success)
				{
					this.m_ResultFetched = true;
					this.m_Result = this.GetResult();
				}
				return this.m_Result;
			}
		}

		internal Request()
		{
		}

		internal Request(long operationId, NativeStatusCode initialStatus) : base(operationId, initialStatus)
		{
		}

		protected abstract T GetResult();
	}
}
