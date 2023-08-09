using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MoqPrivacyAnalyzer
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class MoqPrivacyAnalyzer : DiagnosticAnalyzer
	{
		internal static readonly DiagnosticDescriptor DiagnosticDescriptor = new DiagnosticDescriptor(
			"MOQ1",
			"Do not use Moq v4.20",
			"Do not use Moq v4.20 or later due to privacy concerns",
			"Privacy",
			DiagnosticSeverity.Error,
			true,
			"Moq v4.20 and later may log e-mail addresses in a closed infrastructure. It is recommended to use an earlier version."
		);

		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();
			context.RegisterCompilationAction(ctx =>
			{
				if(ctx.Compilation.ReferencedAssemblyNames.Any(a => a.Name == "Moq" && a.Version >= new Version(4, 20)))
				{
					ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor, null));
				}
			});
		}

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptor);
	}
}