﻿using System;
using System.Collections.Generic;
using System.Linq;

using TASVideos.Data.Entity;

namespace TASVideos.Api.Requests
{
	/// <summary>
	/// Represents the filtering criteria for the submissions endpoint
	/// </summary>
	public class SubmissionsRequest : ApiRequest, ISubmissionFilter
	{
		/// <summary>
		/// Gets or sets the statuses to filter by
		/// </summary>
		public string Statuses { get; set; }

		/// <summary>
		/// Gets or sets user's name to filter by
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// Gets or sets the start year to filter by
		/// </summary>
		public int? StartYear { get; set; }

		DateTime? ISubmissionFilter.StartDate => StartYear.HasValue
			? DateTime.UtcNow.AddYears(0 - (DateTime.UtcNow.Year - StartYear.Value))
			: (DateTime?)null;

		IEnumerable<SubmissionStatus> ISubmissionFilter.StatusFilter => !string.IsNullOrWhiteSpace(Statuses)
			? Statuses
				.Split(
					new[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries)
				.Where(s => Enum.TryParse(s, out SubmissionStatus x))
				.Select(s =>
					{
						Enum.TryParse(s, out SubmissionStatus x);
						return x;
					})
			: Enumerable.Empty<SubmissionStatus>();

	}
}
