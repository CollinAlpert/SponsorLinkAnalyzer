using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace SponsorLinkAnalyzer.Tests;

public class SponsorLinkAnalyzerTests
{
	[Fact]
	public Task WhenSponsorLinkIsNotInstalledNoDiagnostic()
	{
		return new SponsorLinkCodeFixTest
		{
			TestCode = "public class Test {}",
			ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(NoSponsorLink())
		}.RunAsync();
	}

	[Fact]
	public Task WhenSponsorLinkIsInstalled_Diagnostic()
	{
		return new SponsorLinkCodeFixTest
		{
			TestCode = "public class Test {}",
			ExpectedDiagnostics = { new DiagnosticResult(SponsorLinkAnalyzer.DiagnosticDescriptor) },
			ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(SponsorLink())
		}.RunAsync();
	}

	private static ImmutableArray<PackageIdentity> NoSponsorLink()
	{
		return ImmutableArray<PackageIdentity>.Empty;
	}

	private static ImmutableArray<PackageIdentity> SponsorLink()
	{
		return ImmutableArray.Create(new PackageIdentity("Devlooped.SponsorLink", "1.1.0"));
	}
}