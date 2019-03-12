﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using TASVideos.Data;
using TASVideos.Data.Entity;
using TASVideos.Extensions;
using TASVideos.Pages.Submissions.Models;

namespace TASVideos.Pages.Submissions
{
	[AllowAnonymous]
	public class IndexModel : BasePageModel
	{
		private static readonly IEnumerable<SelectListItem> Statuses = Enum.GetValues(typeof(SubmissionStatus))
			.Cast<SubmissionStatus>()
			.Select(s => new SelectListItem
			{
				Text = s.EnumDisplayName(),
				Value = ((int)s).ToString()
			})
			.ToList();

		private readonly ApplicationDbContext _db;

		public IndexModel(ApplicationDbContext db)
		{
			_db = db;
		}

		[FromQuery]
		public SubmissionSearchRequest Search { get; set; } = new SubmissionSearchRequest();

		public IEnumerable<SubmissionListEntry> Submissions { get; set; } = new List<SubmissionListEntry>();

		[Display(Name = "Statuses")]
		public IEnumerable<SelectListItem> AvailableStatuses => Statuses;

		public async Task OnGet()
		{
			// Defaults
			if (!Search.StatusFilter.Any())
			{
				Search.StatusFilter = !string.IsNullOrWhiteSpace(Search.User)
					? SubmissionSearchRequest.All
					: SubmissionSearchRequest.Default;
			}

			Submissions = await _db.Submissions
				.FilterBy(Search)
				.PersistToSubListEntry();
		}
	}
}
