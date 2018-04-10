﻿using System.Collections.Generic;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Json;

namespace Manatee.Trello.Internal.Synchronization
{
	internal class NotificationDataContext : LinkedSynchronizationContext<IJsonNotificationData>
	{
		static NotificationDataContext()
		{
			Properties = new Dictionary<string, Property<IJsonNotificationData>>
				{
					{
						nameof(NotificationData.Attachment),
						new Property<IJsonNotificationData, Attachment>((d, a) => d.Attachment == null
							                                                          ? null
							                                                          : new Attachment(d.Attachment, d.Card.Id, a),
						                                                (d, o) =>
							                                                {
								                                                if (o != null) d.Attachment = o.Json;
							                                                })
					},
					{
						nameof(NotificationData.Board),
						new Property<IJsonNotificationData, Board>((d, a) => d.Board == null
							                                                     ? null
							                                                     : new Board(d.Board, a),
						                                           (d, o) =>
							                                           {
								                                           if (o != null) d.Board = o.Json;
							                                           })
					},
					{
						nameof(NotificationData.BoardSource),
						new Property<IJsonNotificationData, Board>((d, a) => d.BoardSource == null
							                                                     ? null
							                                                     : new Board(d.BoardSource, a),
						                                           (d, o) =>
							                                           {
								                                           if (o != null) d.BoardSource = o.Json;
							                                           })
					},
					{
						nameof(NotificationData.BoardTarget),
						new Property<IJsonNotificationData, Board>((d, a) => d.BoardTarget == null
							                                                     ? null
							                                                     : new Board(d.BoardTarget, a),
						                                           (d, o) =>
							                                           {
								                                           if (o != null) d.BoardTarget = o.Json;
							                                           })
					},
					{
						nameof(NotificationData.Card),
						new Property<IJsonNotificationData, Card>((d, a) => d.Card == null
							                                                    ? null
							                                                    : new Card(d.Card, a),
						                                          (d, o) =>
							                                          {
								                                          if (o != null) d.Card = o.Json;
							                                          })
					},
					{
						nameof(NotificationData.CardSource),
						new Property<IJsonNotificationData, Card>((d, a) => d.CardSource == null
							                                                    ? null
							                                                    : new Card(d.CardSource, a),
						                                          (d, o) =>
							                                          {
								                                          if (o != null) d.CardSource = o.Json;
							                                          })
					},
					{
						nameof(NotificationData.CheckItem),
						new Property<IJsonNotificationData, CheckItem>((d, a) => d.CheckItem == null || d.CheckList == null
							                                                         ? null
							                                                         : new CheckItem(d.CheckItem, d.CheckList.Id),
						                                               (d, o) =>
							                                               {
								                                               if (o != null) d.CheckItem = o.Json;
							                                               })
					},
					{
						nameof(NotificationData.CheckList),
						new Property<IJsonNotificationData, CheckList>((d, a) => d.CheckList == null
							                                                         ? null
							                                                         : new CheckList(d.CheckList, a),
						                                               (d, o) =>
							                                               {
								                                               if (o != null) d.CheckList = o.Json;
							                                               })
					},
					{
						nameof(NotificationData.List),
						new Property<IJsonNotificationData, List>((d, a) => d.List == null
							                                                    ? null
							                                                    : new List(d.List, a),
						                                          (d, o) =>
							                                          {
								                                          if (o != null) d.List = o.Json;
							                                          })
					},
					{
						nameof(NotificationData.ListAfter),
						new Property<IJsonNotificationData, List>((d, a) => d.ListAfter == null
							                                                    ? null
							                                                    : new List(d.ListAfter, a),
						                                          (d, o) =>
							                                          {
								                                          if (o != null) d.ListAfter = o.Json;
							                                          })
					},
					{
						nameof(NotificationData.ListBefore),
						new Property<IJsonNotificationData, List>((d, a) => d.ListBefore == null
							                                                    ? null
							                                                    : new List(d.ListBefore, a),
						                                          (d, o) =>
							                                          {
								                                          if (o != null) d.ListBefore = o.Json;
							                                          })
					},
					{
						nameof(NotificationData.Member),
						new Property<IJsonNotificationData, Member>((d, a) => d.Member?.GetFromCache<Member>(a),
						                                            (d, o) =>
							                                            {
								                                            if (o != null) d.Member = o.Json;
							                                            })
					},
					{
						nameof(NotificationData.WasArchived),
						new Property<IJsonNotificationData, bool?>((d, a) => d.Old?.Closed, (d, o) =>
							{
								if (d.Old != null && o != null) d.Old.Closed = o;
							})
					},
					{
						nameof(NotificationData.OldDescription),
						new Property<IJsonNotificationData, string>((d, a) => d.Old?.Desc, (d, o) =>
							{
								if (d.Old != null && o != null) d.Old.Desc = o;
							})
					},
					{
						nameof(NotificationData.OldList),
						new Property<IJsonNotificationData, List>((d, a) => d.Old?.List == null
							                                                    ? null
							                                                    : new List(d.Old.List, a),
						                                          (d, o) =>
							                                          {
								                                          if (d.Old != null) d.Old.List = o.Json;
							                                          })
					},
					{
						nameof(NotificationData.OldPosition),
						new Property<IJsonNotificationData, Position>((d, a) => d.Old?.Pos, (d, o) =>
							{
								if (d.Old != null) d.Old.Pos = o.Value;
							})
					},
					{
						nameof(NotificationData.OldText),
						new Property<IJsonNotificationData, string>((d, a) => d.Old?.Text, (d, o) =>
							{
								if (d.Old != null) d.Old.Text = o;
							})
					},
					{
						nameof(NotificationData.Organization),
						new Property<IJsonNotificationData, Organization>((d, a) => d.Org == null
							                                                            ? null
							                                                            : new Organization(d.Org, a),
						                                                  (d, o) =>
							                                                  {
								                                                  if (o != null) d.Org = o.Json;
							                                                  })
					},
					{
						nameof(NotificationData.Text),
						new Property<IJsonNotificationData, string>((d, a) => d.Text, (d, o) => d.Text = o)
					},
				};
		}
		public NotificationDataContext(TrelloAuthorization auth)
			: base(auth) {}
	}
}