﻿/***************************************************************************************

	Copyright 2015 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		LabelCollection.cs
	Namespace:		Manatee.Trello
	Class Name:		ReadOnlyLabelCollection, LabelCollection
	Purpose:		Collection objects for labels.

***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello.Exceptions;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Synchronization;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of labels for cards.
	/// </summary>
	public class CardLabelCollection : ReadOnlyCollection<Label>
	{
		private readonly CardContext _context;

		internal CardLabelCollection(CardContext context, TrelloAuthorization auth)
			: base(() => context.Data.Id, auth)
		{
			_context = context;
		}

		/// <summary>
		/// Adds a label to the collection.
		/// </summary>
		/// <param name="label">The label to add.</param>
		public void Add(Label label)
		{
			var error = NotNullRule<Label>.Instance.Validate(null, label);
			if (error != null)
				throw new ValidationException<Label>(label, new[] {error});

			var json = TrelloConfiguration.JsonFactory.Create<IJsonParameter>();
			json.String = label.Id;

			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_AddLabel, new Dictionary<string, object> {{"_id", OwnerId}});
			JsonRepository.Execute(Auth, endpoint, json);

			Items.Add(label);
			_context.Expire();
		}
		/// <summary>
		/// Removes a label from the collection.
		/// </summary>
		/// <param name="label">The label to add.</param>
		public void Remove(Label label)
		{
			var error = NotNullRule<Label>.Instance.Validate(null, label);
			if (error != null)
				throw new ValidationException<Label>(label, new[] {error});

			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_RemoveLabel, new Dictionary<string, object> {{"_id", OwnerId}, {"_labelId", label.Id}});
			JsonRepository.Execute(Auth, endpoint);

			Items.Remove(label);
			_context.Expire();
		}

		/// <summary>
		/// Implement to provide data to the collection.
		/// </summary>
		protected sealed override void Update()
		{
			_context.Synchronize();
			if (_context.Data.Labels == null) return;

			Items.Clear();
			Items.AddRange(_context.Data.Labels.Select(jl =>
				{
					var label = jl.GetFromCache<Label>(Auth);
					label.Json = jl;
					return label;
				}));
		}
	}

	/// <summary>
	/// A collection of labels for boards.
	/// </summary>
	public class BoardLabelCollection : ReadOnlyCollection<Label>
	{
		internal BoardLabelCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) {}

		/// <summary>
		/// Adds a label to the collection.
		/// </summary>
		/// <param name="name">The name of the label.</param>
		/// <param name="color">The color of the label to add.</param>
		/// <returns>The <see cref="Label"/> generated by Trello.</returns>
		public Label Add(string name, LabelColor? color)
		{
			var json = TrelloConfiguration.JsonFactory.Create<IJsonLabel>();
			json.Name = name ?? string.Empty;
			json.Color = color;
			json.ForceNullColor = !color.HasValue;
			json.Board = TrelloConfiguration.JsonFactory.Create<IJsonBoard>();
			json.Board.Id = OwnerId;

			var endpoint = EndpointFactory.Build(EntityRequestType.Board_Write_AddLabel);
			var newData = JsonRepository.Execute(Auth, endpoint, json);

			return new Label(newData, Auth);
		}

		/// <summary>
		/// Implement to provide data to the collection.
		/// </summary>
		protected sealed override void Update()
		{
			var endpoint = EndpointFactory.Build(EntityRequestType.Board_Read_Labels, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = JsonRepository.Execute<List<IJsonLabel>>(Auth, endpoint);

			Items.Clear();
			Items.AddRange(newData.Select(jb =>
				{
					var board = jb.GetFromCache<Label>(Auth);
					board.Json = jb;
					return board;
				}));
		}
	}
}