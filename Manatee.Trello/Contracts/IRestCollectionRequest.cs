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
 
	File Name:		IRestCollectionRequest.cs
	Namespace:		Manatee.Trello.Contracts
	Class Name:		IRestCollectionRequest<T>
	Purpose:		Defines a request used to retrieve a collection of objects.

***************************************************************************************/
namespace Manatee.Trello.Contracts
{
	/// <summary>
	/// Defines a request used to retrieve a collection of objects.
	/// </summary>
	/// <typeparam name="T">The type of object expected in the response.</typeparam>
	public interface IRestCollectionRequest<T> : IRestRequest<T> where T : new()
	{
	}
}