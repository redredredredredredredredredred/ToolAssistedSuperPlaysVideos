﻿using System.ComponentModel.DataAnnotations;

namespace TASVideos.Pages.Forum.Topics.Models;

public class MoveTopicModel
{
	[Display(Name = "New Forum")]
	public int ForumId { get; init; }

	[Display(Name = "Topic")]
	public string TopicTitle { get; init; } = "";

	[Display(Name = "Current Forum")]
	public string ForumName { get; init; } = "";
}
