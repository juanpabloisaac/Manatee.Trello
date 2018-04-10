﻿using System;
using System.Collections.Generic;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Json;

namespace Manatee.Trello.Internal.Synchronization
{
	internal class ActionDataContext : LinkedSynchronizationContext<IJsonActionData>
	{
		static ActionDataContext()
		{
			Properties = new Dictionary<string, Property<IJsonActionData>>
				{
					{
						nameof(ActionData.Attachment),
						new Property<IJsonActionData, Attachment>((d, a) => d.Attachment == null
							                                                    ? null
							                                                    : new Attachment(d.Attachment, d.Card.Id, a),
						                                          (d, o) =>
							                                          {
								                                          if (o != null) d.Attachment = o.Json;
							                                          })
					},
					{
						nameof(ActionData.Board),
						new Property<IJsonActionData, Board>((d, a) => d.Board == null ? null : new Board(d.Board, a),
						                                     (d, o) =>
							                                     {
								                                     if (o != null) d.Board = o.Json;
							                                     })
					},
					{
						nameof(ActionData.BoardSource),
						new Property<IJsonActionData, Board>((d, a) => d.BoardSource == null ? null : new Board(d.BoardSource, a),
						                                     (d, o) =>
							                                     {
								                                     if (o != null) d.BoardSource = o.Json;
							                                     })
					},
					{
						nameof(ActionData.BoardTarget),
						new Property<IJsonActionData, Board>((d, a) => d.BoardTarget == null ? null : new Board(d.BoardTarget, a),
						                                     (d, o) =>
							                                     {
								                                     if (o != null) d.BoardTarget = o.Json;
							                                     })
					},
					{
						nameof(ActionData.Card),
						new Property<IJsonActionData, Card>((d, a) => d.Card == null ? null : new Card(d.Card, a),
						                                    (d, o) =>
							                                    {
								                                    if (o != null) d.Card = o.Json;
							                                    })
					},
					{
						nameof(ActionData.CardSource),
						new Property<IJsonActionData, Card>((d, a) => d.CardSource == null ? null : new Card(d.CardSource, a),
						                                    (d, o) =>
							                                    {
								                                    if (o != null) d.CardSource = o.Json;
							                                    })
					},
					{
						nameof(ActionData.CheckItem),
						new Property<IJsonActionData, CheckItem>((d, a) => d.CheckItem == null || d.CheckList == null
							                                                   ? null
							                                                   : new CheckItem(d.CheckItem, d.CheckList.Id),
						                                         (d, o) =>
							                                         {
								                                         if (o != null) d.CheckItem = o.Json;
							                                         })
					},
					{
						nameof(ActionData.CheckList),
						new Property<IJsonActionData, CheckList>((d, a) => d.CheckList == null ? null : new CheckList(d.CheckList, a),
						                                         (d, o) =>
							                                         {
								                                         if (o != null) d.CheckList = o.Json;
							                                         })
					},
					{
						nameof(ActionData.Label),
						new Property<IJsonActionData, Label>((d, a) => d.Label == null ? null : new Label(d.Label, a),
						                                     (d, o) =>
							                                     {
								                                     if (o != null) d.Label = o.Json;
							                                     })
					},
					{
						nameof(ActionData.LastEdited),
						new Property<IJsonActionData, DateTime?>((d, a) => d.DateLastEdited, (d, o) =>
							{
								if (o != null) d.DateLastEdited = o;
							})
					},
					{
						nameof(ActionData.List),
						new Property<IJsonActionData, List>((d, a) => d.List == null ? null : new List(d.List, a),
						                                    (d, o) =>
							                                    {
								                                    if (o != null) d.List = o.Json;
							                                    })
					},
					{
						nameof(ActionData.ListAfter),
						new Property<IJsonActionData, List>((d, a) => d.ListAfter == null ? null : new List(d.ListAfter, a),
						                                    (d, o) =>
							                                    {
								                                    if (o != null) d.ListAfter = o.Json;
							                                    })
					},
					{
						nameof(ActionData.ListBefore),
						new Property<IJsonActionData, List>((d, a) => d.ListBefore == null ? null : new List(d.ListBefore, a),
						                                    (d, o) =>
							                                    {
								                                    if (o != null) d.ListBefore = o.Json;
							                                    })
					},
					{
						nameof(ActionData.Member),
						new Property<IJsonActionData, Member>((d, a) => d.Member?.GetFromCache<Member>(a),
						                                      (d, o) =>
							                                      {
								                                      if (o != null) d.Member = o.Json;
							                                      })
					},
					{
						nameof(ActionData.WasArchived),
						new Property<IJsonActionData, bool?>((d, a) => d.Old?.Closed, (d, o) =>
							{
								if (d.Old != null && o != null) d.Old.Closed = o;
							})
					},
					{
						nameof(ActionData.OldDescription),
						new Property<IJsonActionData, string>((d, a) => d.Old?.Desc, (d, o) =>
							{
								if (d.Old != null && o != null) d.Old.Desc = o;
							})
					},
					{
						nameof(ActionData.OldList),
						new Property<IJsonActionData, List>((d, a) => d.Old?.List == null ? null : new List(d.Old.List, a),
						                                    (d, o) =>
							                                    {
								                                    if (d.Old != null) d.Old.List = o.Json;
							                                    })
					},
					{
						nameof(ActionData.OldPosition),
						new Property<IJsonActionData, Position>((d, a) => d.Old?.Pos, (d, o) =>
							{
								if (d.Old != null) d.Old.Pos = o.Value;
							})
					},
					{
						nameof(ActionData.OldText),
						new Property<IJsonActionData, string>((d, a) => d.Old?.Text, (d, o) =>
							{
								if (d.Old != null) d.Old.Text = o;
							})
					},
					{
						nameof(ActionData.Organization),
						new Property<IJsonActionData, Organization>(
							(d, a) => d.Org == null ? null : new Organization(d.Org, a),
							(d, o) =>
								{
									if (o != null) d.Org = o.Json;
								})
					},
					{
						nameof(ActionData.PowerUp),
						new Property<IJsonActionData, PowerUpBase>((d, a) => (PowerUpBase) d.Plugin?.GetFromCache<IPowerUp>(a),
						                                           (d, o) =>
							                                           {
								                                           if (o != null) d.Plugin = o.Json;
							                                           })
					},
					{
						nameof(ActionData.Text),
						new Property<IJsonActionData, string>((d, a) => d.Text, (d, o) => d.Text = o)
					},
					{
						nameof(ActionData.Value),
						new Property<IJsonActionData, string>((d, a) => d.Value, (d, o) => d.Value = o)
					},
				};
		}
		public ActionDataContext(TrelloAuthorization auth)
			: base(auth) {}
	}
}