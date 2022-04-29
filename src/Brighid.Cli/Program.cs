using System;
using System.Diagnostics;

using Amazon;
using Amazon.RDS.Util;

using DnsClient;

#pragma warning disable SA1516

var lookupClient = new LookupClient();
var entry = lookupClient.GetHostEntry(args[0]);

var url = entry.Aliases[1];
var token = RDSAuthTokenGenerator.GenerateAuthToken(RegionEndpoint.USEast1, url, 3306, "federated");
Environment.SetEnvironmentVariable("MYSQL_PWD", token);
var process = Process.Start("mysql", $"--host {url} --user federated --port 3306 --enable-cleartext-plugin --ssl-mode require");

await process.WaitForExitAsync();
