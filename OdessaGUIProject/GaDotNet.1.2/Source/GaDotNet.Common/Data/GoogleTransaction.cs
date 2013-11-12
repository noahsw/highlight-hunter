/*
 * Facebook Google Analytics Tracker
 * Copyright 2010 Doug Rathbone
 * http://www.diaryofaninja.com
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace GaDotNet.Common.Data
{
	public class GoogleTransaction
	{
		public string ProductName { get; set; }

		public string ProductSku { get; set; }

		public string ProductVariant { get; set; }

		public decimal UnitPrice { get; set; }

		public int Quantity { get; set; }
		
		public string OrderId { get; set; }

		public string Affiliation { get; set; }

		public decimal TotalCost { get; set; }

		public decimal TaxCost { get; set; }

		public decimal ShippingCost { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Country { get; set; }



		/// <summary>
		/// Validates this instance.
		/// </summary>
		public void Validate()
		{
			if (String.IsNullOrEmpty(ProductName))
			{
				throw new ArgumentException("'ProductName' is a required field", "ProductName");
			}
			if (String.IsNullOrEmpty(ProductSku))
			{
				throw new ArgumentException("'ProductSku' is a required field", "ProductSku");
			}
			if (String.IsNullOrEmpty(ProductVariant))
			{
				throw new ArgumentException("'ProductVariant' is a required field", "ProductVariant");
			}
			if (String.IsNullOrEmpty(OrderId))
			{
				throw new ArgumentException("'OrderID' is a required field","OrderID");
			}
			if (String.IsNullOrEmpty(Affiliation))
			{
				throw new ArgumentException("'Affiliation' is a required field","Affiliation");
			}
			if (String.IsNullOrEmpty(City))
			{
				throw new ArgumentException("'City' is a required field","City");
			}
			if (String.IsNullOrEmpty(State))
			{
				throw new ArgumentException("'State' is a required field","State");
			}
			if (String.IsNullOrEmpty(Country))
			{
				throw new ArgumentException("'Country' is a required field","Country");
			}
		}
	}
}
