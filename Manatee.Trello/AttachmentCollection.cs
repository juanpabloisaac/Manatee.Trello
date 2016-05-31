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
 
	File Name:		AttachmentCollection.cs
	Namespace:		Manatee.Trello
	Class Name:		ReadOnlyAttachmentCollection, AttachmentCollection
	Purpose:		Collection objects for attachments.

***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello.Exceptions;
using Manatee.Trello.Internal;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;
using Manatee.Trello.Rest;

namespace Manatee.Trello
{
	/// <summary>
	/// A read-only collection of attachments.
	/// </summary>
	public class ReadOnlyAttachmentCollection : ReadOnlyCollection<Attachment>
	{
		internal ReadOnlyAttachmentCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) {}

		/// <summary>
		/// Implement to provide data to the collection.
		/// </summary>
		protected sealed override void Update()
		{
			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Read_Attachments, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = JsonRepository.Execute<List<IJsonAttachment>>(Auth, endpoint);

			Items.Clear();
			Items.AddRange(newData.Select(ja =>
				{
					var attachment = TrelloConfiguration.Cache.Find<Attachment>(a => a.Id == ja.Id) ?? new Attachment(ja, OwnerId, Auth);
					attachment.Json = ja;
					return attachment;
				}));
		}
	}

	/// <summary>
	/// A collection of attachments.
	/// </summary>
	public class AttachmentCollection : ReadOnlyAttachmentCollection
	{
		internal AttachmentCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) {}

		/// <summary>
		/// Adds an attachment to a card using the URL of the attachment.
		/// </summary>
		/// <param name="url">A URL to the data to attach.  Must be a valid URL that begins with 'http://' or 'https://'.</param>
		/// <param name="name">An optional name for the attachment.  The linked file name will be used by default if not specified.</param>
		/// <returns>The attachment generated by Trello.</returns>
		public Attachment Add(string url, string name = null)
		{
			var errors = new List<string>
				{
					NotNullOrWhiteSpaceRule.Instance.Validate(null, url),
					UriRule.Instance.Validate(null, url)
				};
			if (errors.Any())
				throw new ValidationException<string>(url, errors);

			var parameters = new Dictionary<string, object>
				{
					{"url", url},
				};
			if (!name.IsNullOrWhiteSpace())
				parameters.Add("name", name);
			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_AddAttachment, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = JsonRepository.Execute<IJsonAttachment>(Auth, endpoint, parameters);

			return new Attachment(newData, OwnerId, Auth);
		}
		/// <summary>
		/// Adds an attachment to a card by uploading data.
		/// </summary>
		/// <param name="data">The byte data of the file to attach.</param>
		/// <param name="name">A name for the attachment.</param>
		/// <returns>The attachment generated by Trello.</returns>
		public Attachment Add(byte[] data, string name)
		{
			var parameters = new Dictionary<string, object> {{RestFile.ParameterKey, new RestFile {ContentBytes = data, FileName = name}}};
			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_AddAttachment, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = JsonRepository.Execute<IJsonAttachment>(Auth, endpoint, parameters);

			return new Attachment(newData, OwnerId, Auth);
		}
	}
}