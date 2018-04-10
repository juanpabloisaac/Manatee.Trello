﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Json;

namespace Manatee.Trello.Internal.Synchronization
{
	internal class NotificationContext : SynchronizationContext<IJsonNotification>
	{
		public NotificationDataContext NotificationDataContext { get; }

		static NotificationContext()
		{
			Properties = new Dictionary<string, Property<IJsonNotification>>
				{
					{
						nameof(Notification.Creator),
						new Property<IJsonNotification, Member>((d, a) => d.MemberCreator.GetFromCache<Member>(a),
						                                        (d, o) =>
							                                        {
								                                        if (o != null) d.MemberCreator = o.Json;
							                                        })
					},
					{
						nameof(Notification.Date),
						new Property<IJsonNotification, DateTime?>((d, a) => d.Date, (d, o) => d.Date = o)
					},
					{
						nameof(Notification.Id),
						new Property<IJsonNotification, string>((d, a) => d.Id, (d, o) => d.Id = o)
					},
					{
						nameof(Notification.IsUnread),
						new Property<IJsonNotification, bool?>((d, a) => d.Unread, (d, o) => d.Unread = o)
					},
					{
						nameof(Notification.Type),
						new Property<IJsonNotification, NotificationType?>((d, a) => d.Type, (d, o) => d.Type = o)
					},
				};
		}
		public NotificationContext(string id, TrelloAuthorization auth)
			: base(auth)
		{
			Data.Id = id;
			NotificationDataContext = new NotificationDataContext(Auth);
			NotificationDataContext.SynchronizeRequested += ct => Synchronize(ct);
			Data.Data = NotificationDataContext.Data;
		}

		public override async Task Expire(CancellationToken ct)
		{
			await base.Expire(ct);
		}

		protected override async Task<IJsonNotification> GetData(CancellationToken ct)
		{
			var endpoint = EndpointFactory.Build(EntityRequestType.Notification_Read_Refresh,
			                                     new Dictionary<string, object> {{"_id", Data.Id}});
			var newData = await JsonRepository.Execute<IJsonNotification>(Auth, endpoint, ct);

			return newData;
		}
		protected override async Task SubmitData(IJsonNotification json, CancellationToken ct)
		{
			var endpoint = EndpointFactory.Build(EntityRequestType.Notification_Write_Update,
			                                     new Dictionary<string, object> {{"_id", Data.Id}});
			var newData = await JsonRepository.Execute(Auth, endpoint, json, ct);
			Merge(newData);
		}
		protected override IEnumerable<string> MergeDependencies(IJsonNotification json)
		{
			return NotificationDataContext.Merge(json.Data);
		}
	}
}