﻿using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Feature.Services.ParameterInfo;
using JetBrains.UI.RichText;
using JetBrains.Util;
using Microsoft.FSharp.Compiler.SourceCodeServices;

namespace JetBrains.ReSharper.Feature.Services.FSharp.ParameterInfo
{
  public class FSharpTypeArgumentCandidate : FSharpParameterInfoCandidateBase
  {
    [NotNull] private readonly string myTypeName;
    private readonly FSharpMethodGroupItem myCandidate;

    public FSharpTypeArgumentCandidate([NotNull] string typeName, [NotNull] FSharpMethodGroupItem candidate)
    {
      myTypeName = typeName;
      myCandidate = candidate;
    }

    public override RichText GetSignature(string[] namedArguments, AnnotationsDisplayKind showAnnotations,
      out TextRange[] parameterRanges,
      out int[] mapToOriginalOrder, out ExtensionMethodInfo extensionMethodInfo)
    {
      var parameters = myCandidate.StaticParameters;
      var text = new StringBuilder(myTypeName + "<");
      var newParameterRanges = new TextRange[parameters.Length];
      var originalOrder = new int[parameters.Length];
      for (var i = 0; i < parameters.Length; i++)
      {
        var paramRangeStart = text.Length;
        text.Append(parameters[i].Display);
        var paramRangeEnd = text.Length;

        newParameterRanges[i] = new TextRange(paramRangeStart, paramRangeEnd);
        if (i < parameters.Length - 1)
          text.Append(", ");
      }

      text.Append(">");

      extensionMethodInfo = ExtensionMethodInfo.NoExtension;
      parameterRanges = newParameterRanges;
      mapToOriginalOrder = originalOrder;
      return text.ToString();
    }

    public override int PositionalParameterCount => myCandidate.StaticParameters.Length;
  }
}