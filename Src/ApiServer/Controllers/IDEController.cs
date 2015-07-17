using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using OpenFiddle.Models;
using OpenFiddle.Models.Ide;
using OpenFiddle.Models.Shared;
using OpenFiddle.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenFiddle.Controllers
{
    public class IDEController : ApiController
    {

        [HttpPost]
        public SyntaxErrors CheckSyntax(CodeInput input)
        {
            var workspace = new AdhocWorkspace();
            SyntaxTree tree = input.Language == Language.CSharp ? CSharpSyntaxTree.ParseText(input.Code) : VisualBasicSyntaxTree.ParseText(input.Code);
            SyntaxErrors errors = new SyntaxErrors();

            foreach (var obj in tree.GetDiagnostics())
            {
                errors.Add(new SyntaxError
                    {        
                        Description = obj.Descriptor.Description.ToString(),
                        Warning = obj.WarningLevel.ToString(),
                        Severity = obj.Severity.ToString(),
                        Location = obj.Location.Kind.ToString(),
                        CharacterAt = obj.Location.GetLineSpan().StartLinePosition.Character.ToString(),
                        OnLine = obj.Location.GetLineSpan().StartLinePosition.Line.ToString()                    
                    });
            }
            return errors;
        }

        [HttpPost]
        public AutoCompleteSuggestions AutoComplete(AutoCompleteInput input)
        {
            //TODO: waiting for Exposure of Completion API https://github.com/dotnet/roslyn/issues/3538
            //    var workspace = new AdhocWorkspace();

            //    string projName = "Console";
            //    var projectId = ProjectId.CreateNewId();
            //    var versionStamp = VersionStamp.Create();
            //    var projectInfo = ProjectInfo.Create(projectId, versionStamp, projName, projName, input.Language== Language.CSharp? LanguageNames.CSharp: LanguageNames.VisualBasic);
            //    var newProject = workspace.AddProject(projectInfo);
            //    var sourceText = SourceText.From(input.Code);
            //    var newDocument = workspace.AddDocument(newProject.Id, "Main." + (input.Language == Language.CSharp ? ".cs" : ".vb"), sourceText);

            //   var completionService = newDocument.GetLanguageService<ICompletionService>();

            //    foreach (var symbol in symbols.Where(s => s.Name.IsValidCompletionFor(wordToComplete)))
            //    {
            //        if (request.WantSnippet)
            //        {
            //            foreach (var completion in MakeSnippetedResponses(request, symbol))
            //            {
            //                _completions.Add(completion);
            //            }
            //        }
            //        else
            //        {
            //            _completions.Add(MakeAutoCompleteResponse(request, symbol));
            //        }
            //    }
            //}

            //return _completions
            //    .OrderByDescending(c => c.CompletionText.IsValidCompletionStartsWithExactCase(wordToComplete))
            //    .ThenByDescending(c => c.CompletionText.IsValidCompletionStartsWithIgnoreCase(wordToComplete))
            //    .ThenByDescending(c => c.CompletionText.IsCamelCaseMatch(wordToComplete))
            //    .ThenByDescending(c => c.CompletionText.IsSubsequenceMatch(wordToComplete))
            //    .ThenBy(c => c.CompletionText);

            throw new NotImplementedException();
        }

        [HttpPost]
        public string Format(CodeInput input)
        {
            var workspace = new AdhocWorkspace();

            if (input.Language == Language.CSharp)
            {
                var tree = CSharpSyntaxTree.ParseText(input.Code);
                return Formatter.Format(tree.GetRoot(), workspace).ToString();
            }
            else if (input.Language == Language.VbNet)
            {
                var tree = VisualBasicSyntaxTree.ParseText(input.Code);
                return Formatter.Format(tree.GetRoot(), workspace).ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [HttpPost]
        public string Convert(CodeInput input)
        {
            //TOOD Write a conversion tool that utilises Roslyn's SyntaxTrees.
            switch (input.Language)
            {
                case Language.CSharp:
                    if (input.Code == CodeSamples.HelloWorldConsoleVBNet)
                        return CodeSamples.HelloWorldConsoleCSharp;
                    else if (input.Code == CodeSamples.HelloWorldScriptVBNet)
                        return CodeSamples.HelloWorldScriptCSharp;
                    else
                        throw new NotImplementedException();
                case Language.VbNet:
                    if (input.Code == CodeSamples.HelloWorldConsoleCSharp)
                        return CodeSamples.HelloWorldConsoleVBNet;
                    else if (input.Code == CodeSamples.HelloWorldScriptCSharp)
                        return CodeSamples.HelloWorldScriptVBNet;
                    else
                        throw new NotImplementedException();
                default:
                    return string.Empty;
            }
        }

        [HttpGet]
        public Guid GUID()
        {
            return Guid.NewGuid();
        }

    }
}
