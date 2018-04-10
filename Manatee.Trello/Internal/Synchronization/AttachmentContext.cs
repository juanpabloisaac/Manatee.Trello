﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Json;

namespace Manatee.Trello.Internal.Synchronization
{
	internal class AttachmentContext : SynchronizationContext<IJsonAttachment>
	{
		private readonly string _ownerId;
		private bool _deleted;

		static AttachmentContext()
		{
			Properties = new Dictionary<string, Property<IJsonAttachment>>
				{
					{
						nameof(Attachment.Bytes),
						new Property<IJsonAttachment, int?>((d, a) => d.Bytes, (d, o) => d.Bytes = o)
					},
					{
						nameof(Attachment.Date),
						new Property<IJsonAttachment, DateTime?>((d, a) => d.Date, (d, o) => d.Date = o)
					},
					{
						nameof(Attachment.Member),
						new Property<IJsonAttachment, Member>((d, a) => d.Member?.GetFromCache<Member>(a),
						                                      (d, o) => d.Member = o?.Json)
					},
					{
						nameof(Attachment.Id),
						new Property<IJsonAttachment, string>((d, a) => d.Id, (d, o) => d.Id = o)
					},
					{
						nameof(Attachment.IsUpload),
						new Property<IJsonAttachment, bool?>((d, a) => d.IsUpload, (d, o) => d.IsUpload = o)
					},
					{
						nameof(Attachment.MimeType),
						new Property<IJsonAttachment, string>((d, a) => d.MimeType, (d, o) => d.MimeType = o)
					},
					{
						nameof(Attachment.Name),
						new Property<IJsonAttachment, string>((d, a) => d.Name, (d, o) => d.Name = o)
					},
					{
						nameof(Attachment.Url),
						new Property<IJsonAttachment, string>((d, a) => d.Url, (d, o) => d.Url = o)
					},
					{
						nameof(Attachment.Position),
						new Property<IJsonAttachment, Position>((d, a) => Position.GetPosition(d.Pos),
						                                        (d, o) => d.Pos = Position.GetJson(o))
					},
					{
						nameof(Attachment.EdgeColor),
						new Property<IJsonAttachment, WebColor>(
							(d, a) => d.EdgeColor.IsNullOrWhiteSpace() ? null : new WebColor(d.EdgeColor),
							(d, o) => d.EdgeColor = o?.ToString())
					},
				};
		}

		public AttachmentContext(string id, string ownerId, TrelloAuthorization auth)
			: base(auth)
		{
			_ownerId = ownerId;
			Data.Id = id;
		}

		protected override async Task SubmitData(IJsonAttachment json, CancellationToken ct)
		{
			// This may make a call to get the card, but it can't be avoided.  We need its ID.
			var endpoint = EndpointFactory.Build(EntityRequestType.Attachment_Write_Update,
			                                     new Dictionary<string, object>
				                                     {
					                                     {"_cardId", _ownerId},
					                                     {"_id", Data.Id},
				                                     });
			var newData = await JsonRepository.Execute(Auth, endpoint, json, ct);
			Merge(newData);
		}

		public async Task Delete(CancellationToken ct)
		{
			if (_deleted) return;
			CancelUpdate();

			var endpoint = EndpointFactory.Build(EntityRequestType.Attachment_Write_Delete,
			                                     new Dictionary<string, object>
				                                     {
					                                     {"_cardId", _ownerId},
					                                     {"_id", Data.Id}
				                                     });
			await JsonRepository.Execute(Auth, endpoint, ct);

			_deleted = true;
		}

		protected override bool CanUpdate()
		{
			return !_deleted;
		}
	}
}