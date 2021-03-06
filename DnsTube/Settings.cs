﻿using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace DnsTube
{
	public class Settings : SettingsDTO
	{
		public Settings()
		{
			if (File.Exists(GetSettingsFilePath()))
			{
				string json = File.ReadAllText(GetSettingsFilePath());
				var settings = JsonConvert.DeserializeObject<SettingsDTO>(json);
				EmailAddress = settings.EmailAddress;
				IsUsingToken = settings.IsUsingToken;
				ApiKey = settings.ApiKey;
				ApiToken = settings.ApiToken;
				UpdateIntervalMinutes = settings.UpdateIntervalMinutes;
				SelectedDomains = settings.SelectedDomains;
				StartMinimized = settings.StartMinimized;
				ProtocolSupport = settings.ProtocolSupport;
				SkipCheckForNewReleases = settings.SkipCheckForNewReleases;
				ZoneIDs = settings.ZoneIDs ?? "";
			}
			else
			{
				UpdateIntervalMinutes = 30;
				SelectedDomains = new List<SelectedDomain>();
				IsUsingToken = true;
				ProtocolSupport = IpSupport.IPv4;
				ZoneIDs = "";
			}
		}

		public void Save()
		{
			string json = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(GetSettingsFilePath(), json);
		}

		string _getSettingsFilePath;
		public string GetSettingsFilePath()
		{
			if (_getSettingsFilePath == null)
			{
				string localApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				string appDirectory = Path.Combine(localApplicationDataDirectory, "DnsTube");
				Directory.CreateDirectory(appDirectory);
				_getSettingsFilePath = Path.Combine(appDirectory, "config.json");
			}
			return _getSettingsFilePath;
		}
	}

	public class SelectedDomain
	{
		public string ZoneName { get; set; }
		public string DnsName { get; set; }
	}

	public class SettingsDTO
	{
		public string EmailAddress { get; set; }
		public bool IsUsingToken { get; set; }
		public string ApiKey { get; set; }
		public string ApiToken { get; set; }
		public int UpdateIntervalMinutes { get; set; }
		public string PublicIpv4Address { get; set; }
		public string PublicIpv6Address { get; set; }
		public List<SelectedDomain> SelectedDomains { get; set; }
		public bool StartMinimized { get; set; }
		public bool SkipCheckForNewReleases { get; set; }
		public IpSupport ProtocolSupport { get; set; }
		public string ZoneIDs { get; set; }
	}

	public enum IpSupport
	{
		IPv4, IPv6, IPv4AndIPv6
	}
}
