﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Json;
using Manatee.Trello.Rest;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of <see cref="ISticker"/>s.
	/// </summary>
	public class MemberStickerCollection : ReadOnlyStickerCollection, IMemberStickerCollection
	{
		internal MemberStickerCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) { }

		/// <summary>
		/// Adds a <see cref="ISticker"/> to a <see cref="IMember"/>'s custom sticker set by uploading data.
		/// </summary>
		/// <param name="data">The byte data of the file to attach.</param>
		/// <param name="name">A name for the attachment.</param>
		/// <returns>The attachment generated by Trello.</returns>
		public ISticker Add(byte[] data, string name)
		{
			var parameters = new Dictionary<string, object> { { RestFile.ParameterKey, new RestFile { ContentBytes = data, FileName = name } } };
			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_AddAttachment, new Dictionary<string, object> { { "_id", OwnerId } });
			var newData = JsonRepository.Execute<IJsonSticker>(Auth, endpoint, parameters);

			return new Sticker(newData, OwnerId, Auth);
		}
	}
}