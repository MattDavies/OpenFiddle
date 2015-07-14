using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.CSharp;
using OpenFiddle.Models.Shared;
using Microsoft.VisualBasic;

namespace OpenFiddle.Helpers
{
    public class CompileHelper
    {
        public static string CompileAndRun(string code, Language language)
        {
            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            string[] references = { "System.dll", "System.Linq.dll", "System.Core.dll" };
            compilerParams.ReferencedAssemblies.AddRange(references);

            CodeDomProvider provider = language == Language.CSharp ? (CodeDomProvider)new CSharpCodeProvider() : (CodeDomProvider)new VBCodeProvider();
            var compile = provider.CompileAssemblyFromSource(compilerParams, code);

            if (compile.Errors.HasErrors)
            {
                return compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ",
                    (current, ce) => current
                        + string.Format("Line: {0}\r\nColumn: {1}\r\nError Code: {2}\r\nError Text: {3}\r\n",
                            ce.Line, ce.Column, ce.ErrorNumber, ce.ErrorText));
            }

            var sandbox = Sandbox.Create();
            return sandbox.Execute(compile.PathToAssembly);
        }
    }
}