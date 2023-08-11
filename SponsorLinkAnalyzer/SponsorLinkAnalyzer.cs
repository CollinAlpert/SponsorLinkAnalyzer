using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SponsorLinkAnalyzer
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class SponsorLinkAnalyzer : DiagnosticAnalyzer
	{
		internal static readonly DiagnosticDescriptor DiagnosticDescriptor = new DiagnosticDescriptor(
			"PRIVACY001",
			"Remove SponsorLink dependency",
			"Do not depend on SponsorLink",
			"Privacy",
			DiagnosticSeverity.Error,
			true,
			"SponsorLink may log e-mail addresses in a closed infrastructure. It is recommended to remove this dependency."
		);

		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();
			context.RegisterCompilationAction(ctx =>
			{
				if(ctx.Compilation.ReferencedAssemblyNames.Any(a => a.Name == "SponsorLink"))
				{
					ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor, null));
				}
			});
		}

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptor);
	}
}