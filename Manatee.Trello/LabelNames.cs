﻿/***************************************************************************************

	Copyright 2013 Little Crab Solutions

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		LabelNames.cs
	Namespace:		Manatee.Trello
	Class Name:		LabelNames
	Purpose:		Defines a set of labels for a board on Trello.com.

***************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using Manatee.Trello.Contracts;
using Manatee.Trello.Internal;
using Manatee.Trello.Internal.Json;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	//   "labelNames":{
	//      "red":"",
	//      "orange":"",
	//      "yellow":"",
	//      "green":"",
	//      "blue":"",
	//      "purple":""
	//   }
	/// <summary>
	/// Defines a set of labels for a board.
	/// </summary>
	public class LabelNames : ExpiringObject, IEnumerable<Label>
	{
		private IJsonLabelNames _jsonLabelNames;

		/// <summary>
		/// Gets or sets the name of the red label.
		/// </summary>
		public string Red
		{
			get
			{
				VerifyNotExpired();
				return (_jsonLabelNames == null) ? null : _jsonLabelNames.Red;
			}
			set
			{
				Validate.Writable(Svc);
				if (_jsonLabelNames == null) return;
				if (_jsonLabelNames.Red == value) return;
				_jsonLabelNames.Red = value ?? string.Empty;
				Parameters.Add("value", _jsonLabelNames.Red);
				Put("red");
			}
		}
		/// <summary>
		/// Gets or sets the name of the orange label.
		/// </summary>
		public string Orange
		{
			get
			{
				VerifyNotExpired();
				return (_jsonLabelNames == null) ? null : _jsonLabelNames.Orange;
			}
			set
			{
				Validate.Writable(Svc);
				if (_jsonLabelNames == null) return;
				if (_jsonLabelNames.Orange == value) return;
				_jsonLabelNames.Orange = value ?? string.Empty;
				Parameters.Add("value", _jsonLabelNames.Orange);
				Put("orange");
			}
		}
		/// <summary>
		/// Gets or sets the name of the yellow label.
		/// </summary>
		public string Yellow
		{
			get
			{
				VerifyNotExpired();
				return (_jsonLabelNames == null) ? null : _jsonLabelNames.Yellow;
			}
			set
			{
				Validate.Writable(Svc);
				if (_jsonLabelNames == null) return;
				if (_jsonLabelNames.Yellow == value) return;
				_jsonLabelNames.Yellow = value ?? string.Empty;
				Parameters.Add("value", _jsonLabelNames.Yellow);
				Put("yellow");
			}
		}
		/// <summary>
		/// Gets or sets the name of the green label.
		/// </summary>
		public string Green
		{
			get
			{
				VerifyNotExpired();
				return (_jsonLabelNames == null) ? null : _jsonLabelNames.Green;
			}
			set
			{
				Validate.Writable(Svc);
				if (_jsonLabelNames == null) return;
				if (_jsonLabelNames.Green == value) return;
				_jsonLabelNames.Green = value ?? string.Empty;
				Parameters.Add("value", _jsonLabelNames.Green);
				Put("green");
			}
		}
		/// <summary>
		/// Gets or sets the name of the blue label.
		/// </summary>
		public string Blue
		{
			get
			{
				VerifyNotExpired();
				return (_jsonLabelNames == null) ? null : _jsonLabelNames.Blue;
			}
			set
			{
				Validate.Writable(Svc);
				if (_jsonLabelNames == null) return;
				if (_jsonLabelNames.Blue == value) return;
				_jsonLabelNames.Blue = value ?? string.Empty;
				Parameters.Add("value", _jsonLabelNames.Blue);
				Put("blue");
			}
		}
		/// <summary>
		/// Gets or sets the name of the purple label.
		/// </summary>
		public string Purple
		{
			get
			{
				VerifyNotExpired();
				return (_jsonLabelNames == null) ? null : _jsonLabelNames.Purple;
			}
			set
			{
				Validate.Writable(Svc);
				if (_jsonLabelNames == null) return;
				if (_jsonLabelNames.Purple == value) return;
				_jsonLabelNames.Purple = value ?? string.Empty;
				Parameters.Add("value", _jsonLabelNames.Purple);
				Put("purple");
			}
		}

		internal static string TypeKey { get { return "labelNames"; } }
		internal override string Key { get { return TypeKey; } }

		/// <summary>
		/// Creates a new instance of the LabelNames class.
		/// </summary>
		public LabelNames()
		{
			_jsonLabelNames = new InnerJsonLabelNames();
		}
		internal LabelNames(Board owner)
			: this()
		{
			Owner = owner;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<Label> GetEnumerator()
		{
			return new List<Label>
			       	{
			       		new Label {Color = LabelColor.Red, Name = Red},
			       		new Label {Color = LabelColor.Orange, Name = Orange},
			       		new Label {Color = LabelColor.Yellow, Name = Yellow},
			       		new Label {Color = LabelColor.Green, Name = Green},
			       		new Label {Color = LabelColor.Blue, Name = Blue},
			       		new Label {Color = LabelColor.Purple, Name = Purple},
			       	}.GetEnumerator();
		}
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Retrieves updated data from the service instance and refreshes the object.
		/// </summary>
		protected override void Refresh()
		{
			var endpoint = EndpointGenerator.Default.Generate(Owner, this);
			var request = Api.RequestProvider.Create(endpoint.ToString());
			ApplyJson(Api.Get<IJsonLabelNames>(request));
		}
		/// <summary>
		/// Propigates the service instance to the object's owned objects.
		/// </summary>
		protected override void PropigateService() {}

		internal override void ApplyJson(object obj)
		{
			_jsonLabelNames = (IJsonLabelNames) obj;
		}

		private void Put(string extension)
		{
			if (Svc == null)
			{
				Parameters.Clear();
				return;
			}
			var endpoint = EndpointGenerator.Default.Generate(Owner, this);
			endpoint.Append(extension);
			var request = Api.RequestProvider.Create(endpoint.ToString());
			foreach (var parameter in Parameters)
			{
				request.AddParameter(parameter.Key, parameter.Value);
			}
			Api.Put<IJsonLabelNames>(request);
		}
	}
}
