using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Grpc.Streams;
using Google.Protobuf;

namespace EventStore.Grpc {
	public partial class EventStoreGrpcClient {
		public Task<DeleteResult> SoftDeleteAsync(
			string streamName,
			StreamRevision expectedRevision,
			UserCredentials userCredentials = default,
			CancellationToken cancellationToken = default) =>
			DeleteInternal(new DeleteReq {
				Options = new DeleteReq.Types.Options {
					Soft = new DeleteReq.Types.Options.Types.SoftDeleteOptions(),
					RequestId = ByteString.CopyFrom(Uuid.NewUuid().ToSpan()),
					StreamName = streamName,
					Revision = expectedRevision
				}
			}, userCredentials, cancellationToken);

		public Task<DeleteResult> SoftDeleteAsync(
			string streamName,
			AnyStreamRevision expectedRevision,
			UserCredentials userCredentials = default,
			CancellationToken cancellationToken = default) =>
			DeleteInternal(new DeleteReq {
				Options = new DeleteReq.Types.Options {
					Soft = new DeleteReq.Types.Options.Types.SoftDeleteOptions(),
					RequestId = ByteString.CopyFrom(Uuid.NewUuid().ToSpan()),
					StreamName = streamName,
					AnyRevision = expectedRevision
				}
			}, userCredentials, cancellationToken);

		public Task<DeleteResult> HardDeleteAsync(
			string streamName,
			StreamRevision expectedRevision,
			UserCredentials userCredentials = default,
			CancellationToken cancellationToken = default) =>
			DeleteInternal(new DeleteReq {
				Options = new DeleteReq.Types.Options {
					Hard = new DeleteReq.Types.Options.Types.HardDeleteOptions(),
					RequestId = ByteString.CopyFrom(Uuid.NewUuid().ToSpan()),
					StreamName = streamName,
					Revision = expectedRevision
				}
			}, userCredentials, cancellationToken);

		public Task<DeleteResult> HardDeleteAsync(
			string streamName,
			AnyStreamRevision expectedRevision,
			UserCredentials userCredentials = default,
			CancellationToken cancellationToken = default) =>
			DeleteInternal(new DeleteReq {
				Options = new DeleteReq.Types.Options {
					Hard = new DeleteReq.Types.Options.Types.HardDeleteOptions(),
					RequestId = ByteString.CopyFrom(Uuid.NewUuid().ToSpan()),
					StreamName = streamName,
					AnyRevision = expectedRevision
				}
			}, userCredentials, cancellationToken);

		private async Task<DeleteResult> DeleteInternal(DeleteReq request, UserCredentials userCredentials,
			CancellationToken cancellationToken) {
			var result = await _client.DeleteAsync(request, GetRequestMetadata(userCredentials),
				cancellationToken: cancellationToken);

			return new DeleteResult(new Position(result.Position.CommitPosition, result.Position.PreparePosition));
		}
	}
}
