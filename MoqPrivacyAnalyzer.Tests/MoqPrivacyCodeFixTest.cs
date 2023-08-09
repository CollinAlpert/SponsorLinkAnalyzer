using System.Collections.Immutable;
using System.Net;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace MoqPrivacyAnalyzer.Tests;

public sealed class MoqPrivacyCodeFixTest : CSharpCodeFixTest<MoqPrivacyAnalyzer, EmptyCodeFixProvider, XUnitVerifier>
{
	static MoqPrivacyCodeFixTest()
	{
		// If we have outdated defaults from the host unit test application targeting an older .NET Framework, use more
		// reasonable TLS protocol version for outgoing connections.
#pragma warning disable CA5364 // Do Not Use Deprecated Security Protocols
#pragma warning disable CS0618 // Type or member is obsolete
		if (ServicePointManager.SecurityProtocol == (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls))
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CA5364 // Do Not Use Deprecated Security Protocols
		{
#pragma warning disable CA5386 // Avoid hardcoding SecurityProtocolType value
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#pragma warning restore CA5386 // Avoid hardcoding SecurityProtocolType value
		}
	}

	internal static readonly ImmutableDictionary<string, ReportDiagnostic> NullableWarnings = GetNullableWarningsFromCompiler();

	public MoqPrivacyCodeFixTest()
	{
		ReferenceAssemblies = ReferenceAssemblies.Default;
	}

	private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
	{
		string[] args = { "/warnaserror:nullable" };
		var commandLineArguments = CSharpCommandLineParser.Default.Parse(args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
		var nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

		return nullableWarnings;
	}

	public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.CSharp7_3;

	protected override CompilationOptions CreateCompilationOptions()
	{
		var compilationOptions = base.CreateCompilationOptions();

		return compilationOptions.WithSpecificDiagnosticOptions(
			compilationOptions.SpecificDiagnosticOptions.SetItems(NullableWarnings));
	}

	protected override ParseOptions CreateParseOptions()
	{
		return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
	}
}