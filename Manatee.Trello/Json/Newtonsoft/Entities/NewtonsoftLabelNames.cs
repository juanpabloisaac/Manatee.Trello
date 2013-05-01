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
 
	File Name:		NewtonsoftLabelNames.cs
	Namespace:		Manatee.Trello.Json.Newtonsoft.Entities
	Class Name:		NewtonsoftLabelNames
	Purpose:		Implements IJsonLabelNames for Newtonsoft's Json.Net.

***************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Manatee.Trello.Json.Newtonsoft.Entities
{
	internal class NewtonsoftLabelNames : IJsonLabelNames
	{
		[JsonProperty("red")]
		public string Red { get; set; }
		[JsonProperty("orange")]
		public string Orange { get; set; }
		[JsonProperty("yellow")]
		public string Yellow { get; set; }
		[JsonProperty("green")]
		public string Green { get; set; }
		[JsonProperty("blue")]
		public string Blue { get; set; }
		[JsonProperty("purple")]
		public string Purple { get; set; }
	}
}
