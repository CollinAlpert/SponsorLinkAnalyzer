using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace MoqPrivacyAnalyzer.Tests;

public class MoqPrivacyAnalyzerTests
{
	[Fact]
	public Task WhenMoqIsNotInstalledNoDiagnostic()
	{
		return new MoqPrivacyCodeFixTest
		{
			TestCode = "public class Test {}",
			ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(NoMoq())
		}.RunAsync();
	}

	[Theory]
	[InlineData("4.18.4")]
	[InlineData("4.18.3")]
	[InlineData("4.17.2")]
	public Task WhenMoqInLowerVersionIsInstalled_NoDiagnostic(string version)
	{
		return new MoqPrivacyCodeFixTest
		{
			TestCode = "public class Test {}",
			ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(MoqWithVersion(version))
		}.RunAsync();
	}

	[Theory]
	[InlineData("4.20")]
	[InlineData("4.20.2")]
	public Task WhenMoqInHigherVersionIsInstalled_Diagnostic(string version)
	{
		return new MoqPrivacyCodeFixTest
		{
			TestCode = "public class Test {}",
			ExpectedDiagnostics = { new DiagnosticResult(MoqPrivacyAnalyzer.DiagnosticDescriptor) },
			ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(MoqWithVersion(version))
		}.RunAsync();
	}

	private static ImmutableArray<PackageIdentity> NoMoq()
	{
		return ImmutableArray<PackageIdentity>.Empty;
	}

	private static ImmutableArray<PackageIdentity> MoqWithVersion(string version)
	{
		return ImmutableArray.Create(new PackageIdentity("Moq", version));
	}
}