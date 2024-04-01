﻿using TASVideos.Core.Services.ExternalMediaPublisher;
using TASVideos.WikiEngine;

namespace TASVideos.WikiModules;

[WikiModule(ModuleNames.MediaPosts)]
public class MediaPosts(ApplicationDbContext db) : WikiViewComponent
{
	public List<MediaPost> Posts { get; set; } = [];

	public async Task<IViewComponentResult> InvokeAsync(int? days, int? limit)
	{
		var startDate = DateTime.UtcNow.AddDays(-(days ?? 7));

		var canSeeRestricted = UserClaimsPrincipal.Has(PermissionTo.SeeRestrictedForums);

		Posts = await db.MediaPosts
			.Since(startDate)
			.Where(m => canSeeRestricted || m.Type != PostType.Critical.ToString())
			.Where(m => canSeeRestricted || m.Type != PostType.Administrative.ToString())
			.Where(m => canSeeRestricted || m.Type != PostType.Log.ToString())
			.ByMostRecent()
			.Take(limit ?? 50)
			.ToListAsync();

		return View();
	}
}
