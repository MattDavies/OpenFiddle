using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace csfiddle.Helpers
{
    public class Sandbox : MarshalByRefObject
    {
        const string BaseDirectory = "Untrusted";
        const string DomainName = "Sandbox";

        public static Sandbox Create()
        {
            var setup = new AppDomainSetup()
            {
                ApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseDirectory),
                ApplicationName = DomainName,
                DisallowBindingRedirects = true,
                DisallowCodeDownload = true,
                DisallowPublisherPolicy = true
            };

            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            var domain = AppDomain.CreateDomain(DomainName, null, setup, permissions, typeof(Sandbox).Assembly.Evidence.GetHostEvidence<StrongName>());

            return (Sandbox)Activator.CreateInstanceFrom(domain, typeof(Sandbox).Assembly.ManifestModule.FullyQualifiedName, typeof(Sandbox).FullName).Unwrap();
        }

        public string Execute(string assemblyPath)
        {
            try
            {
                //todo: do this in a new process, not just a new thread
                
                new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery,
                    assemblyPath).Assert();
                var assembly = Assembly.LoadFile(assemblyPath);
                CodeAccessPermission.RevertAssert();

                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
                var stringWriter = new StringWriter();
                Console.SetOut(stringWriter);
                CodeAccessPermission.RevertAssert();

                var module = assembly.GetModules()[0];
                var mainMethod = module.GetTypes().FirstOrDefault(t => t.GetMethods().Any(m => m.Name == "Main"));

                if (mainMethod == null) return "Compile error: Couldn't find a valid Main() method to execute.";

                new WaitFor<object>(TimeSpan.FromSeconds(10)).Run(
                    () => mainMethod.GetMethod("Main").Invoke(null, new object[] {}));
                return "<pre>" + WebUtility.HtmlEncode(stringWriter.ToString()) + "</pre>";
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is SecurityException)
                {
                    return "While in BETA, C#Fiddle runs code under very limited security permissions.<br />Your code failed requesting the following permission:<br /><br />"
                           + ex.InnerException.Message;
                }
                return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            catch (TimeoutException)
            {
                return "While in BETA, C#Fiddle requires that your code run to completion in less than 10 seconds.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}