using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class CSharpCodeHelper
    {
        public static string GetClassInFileBySyntaxTree(string filePath)
        {
            string code = File.ReadAllText(filePath);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var root = syntaxTree.GetCompilationUnitRoot();

            var result = new List<MemberDeclarationSyntax>();

            FillClassDeclarations(result, root.Members);

            var fullTypeName = string.Empty;

            var classDeclaration = result.OfType<ClassDeclarationSyntax>().FirstOrDefault();

            if (classDeclaration != null)
            {
                fullTypeName = classDeclaration.Identifier.ValueText;

                if (classDeclaration.Parent is NamespaceDeclarationSyntax parentNamespace)
                {
                    var namespaceText = GetNamespaceName(parentNamespace);

                    if (!string.IsNullOrEmpty(namespaceText))
                    {
                        fullTypeName = string.Format($"{namespaceText}.{fullTypeName}");
                    }
                }
            }

            if (!string.IsNullOrEmpty(fullTypeName))
            {
                return fullTypeName;
            }

            return Path.GetFileNameWithoutExtension(filePath);
        }

        private static string GetNamespaceName(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            var namespaceText = namespaceDeclaration.Name.ToString();

            if (namespaceDeclaration.Parent is NamespaceDeclarationSyntax parentNamespace)
            {
                var nameParent = GetNamespaceName(parentNamespace);

                if (!string.IsNullOrEmpty(nameParent))
                {
                    return $"{nameParent}.{namespaceText}";
                }
            }

            return namespaceText;
        }

        private static void FillClassDeclarations(List<MemberDeclarationSyntax> result, SyntaxList<MemberDeclarationSyntax> members)
        {
            result.AddRange(members.Where(m => m.Kind() == SyntaxKind.ClassDeclaration));

            foreach (var item in members.Where(m => m.Kind() == SyntaxKind.NamespaceDeclaration))
            {
                if (item is NamespaceDeclarationSyntax namespaceDeclaration)
                {
                    FillClassDeclarations(result, namespaceDeclaration.Members);
                }
            }
        }
    }
}