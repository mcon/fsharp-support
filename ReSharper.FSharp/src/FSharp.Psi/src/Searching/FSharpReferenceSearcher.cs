﻿using System.Collections.Generic;
using System.Linq;
using FSharp.Compiler.SourceCodeServices;
using JetBrains.ReSharper.Plugins.FSharp.Psi.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Plugins.FSharp.Psi.Searching
{
  public class FSharpReferenceSearcher : IDomainSpecificSearcher
  {
    private readonly IDeclaredElementsSet myElements;
    private readonly IList<FSharpSymbol> myFSharpSymbols;
    private readonly bool myFindCandidates;
    private readonly ICollection<string> myElementNames;

    public FSharpReferenceSearcher(IDeclaredElementsSet elements, bool findCandidates)
    {
      myElements = new DeclaredElementsSet(elements.Where(e => !(e is IFSharpSymbolElement)));
      myFSharpSymbols = elements.OfType<IFSharpSymbolElement>().Select(e => e.Symbol).ToIList();
      myFindCandidates = findCandidates;
      myElementNames = new HashSet<string>();

      foreach (var element in elements)
        myElementNames.AddRange(FSharpNamesUtil.GetPossibleSourceNames(element));
    }

    public bool ProcessProjectItem<TResult>(IPsiSourceFile sourceFile, IFindResultConsumer<TResult> consumer)
    {
      return sourceFile.GetPsiFiles<FSharpLanguage>().Any(file => ProcessElement(file, consumer));
    }

    public bool ProcessElement<TResult>(ITreeNode element, IFindResultConsumer<TResult> consumer)
    {
      var result = new FSharpReferenceSearchProcessor<TResult>(element, myFindCandidates, consumer, myElements,
        myFSharpSymbols, myElementNames).Run();
      return result == FindExecution.Stop;
    }
  }
}